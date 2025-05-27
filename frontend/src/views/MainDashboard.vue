<template>
  <div class="dashboard">
    <div class="header">
      <h1>Your Boards</h1>
      <el-button type="primary" @click="showAddBoardDialog = true">
        <el-icon><Plus /></el-icon>
        Add New Board
      </el-button>
    </div>

    <!-- Загрузка -->
    <el-skeleton v-if="loading" animated :rows="4" />

    <!-- Список досок -->
    <el-row
      v-else-if="boards.length > 0"
      :gutter="20"
      class="board-list"
      justify="start"
    >
      <el-col
        v-for="board in boards"
        :key="board.boardId"
        :span="6"
        class="board-col"
      >
      <el-card
        shadow="hover"
        class="board-item"
        @click="openBoard(board.boardId)"
      >
        <template #header>
          <div class="card-header">
            <el-icon><FolderOpened /></el-icon>
            <span class="board-title">{{ board.boardName }}</span>

            <el-button
              type="danger"
              :icon="Delete"
              circle
              size="small"
              class="delete-btn"
              @click.stop="confirmDeleteBoard(board.boardId)"
            />
          </div>
        </template>
        <div class="card-content">
          <el-tag type="info">{{ board.columnIds?.length || 0 }} columns</el-tag>
        </div>
      </el-card>

      </el-col>
    </el-row>

    <!-- Если досок нет -->
    <div v-else class="no-boards">
      <el-empty description="You don't have any boards yet." />
    </div>

    <!-- Диалог добавления доски -->
    <el-dialog v-model="showAddBoardDialog" title="Create New Board" width="30%">
      <el-form :model="newBoard" label-position="top">
        <el-form-item label="Board Name">
          <el-input v-model="newBoard.boardName" placeholder="Enter board name" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showAddBoardDialog = false">Cancel</el-button>
        <el-button type="primary" @click="createBoard">Create</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script>
import axios from 'axios';
import { ElMessage } from 'element-plus';
import { FolderOpened, Plus } from '@element-plus/icons-vue';

export default {
  name: 'MainDashboard',
  components: {
    FolderOpened,
    Plus
  },
  data() {
    return {
      boards: [],
      loading: true,
      showAddBoardDialog: false,
      newBoard: {
        boardName: ''
      }
    };
  },
  async created() {
    await this.loadBoards();
  },
  methods: {
    async loadBoards() {
      const token = localStorage.getItem('accessToken');
      if (!token) {
        ElMessage.error('You must be logged in.');
        this.$router.push('/login');
        return;
      }

      try {
        const response = await axios.get('http://localhost:5258/api/Boards/column', {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        this.boards = response.data;
      } catch (error) {
        ElMessage.error('Failed to load boards');
      } finally {
        this.loading = false;
      }
    },

    openBoard(boardId) {
      this.$router.push(`/boards/${boardId}`);
    },

    async createBoard() {
      const token = localStorage.getItem('accessToken');
      if (!this.newBoard.boardName.trim()) {
        ElMessage.warning('Board name is required');
        return;
      }

      try {
        const response = await axios.post(
          'http://localhost:5258/api/Boards',
          {
            boardName: this.newBoard.boardName
          },
          {
            headers: {
              Authorization: `Bearer ${token}`
            }
          }
        );
        this.boards.push(response.data);
        ElMessage.success('Board created successfully');
        this.showAddBoardDialog = false;
        this.newBoard.boardName = '';
      } catch (error) {
        ElMessage.error('Failed to create board');
      }
    },

        confirmDeleteBoard(boardId) {
        this.$confirm(
          'Are you sure you want to delete this board? All data will be lost.',
          'Warning',
          {
            confirmButtonText: 'Yes, delete it',
            cancelButtonText: 'Cancel',
            type: 'warning'
          }
        )
          .then(() => this.deleteBoard(boardId))
          .catch(() => {
            ElMessage.info('Deletion canceled');
          });
      },

      async deleteBoard(boardId) {
        const token = localStorage.getItem('accessToken');
        try {
          await axios.delete(`http://localhost:5258/api/Boards/column/${boardId}`, {
            headers: {
              Authorization: `Bearer ${token}`
            }
          });
          this.boards = this.boards.filter(board => board.boardId !== boardId);
          ElMessage.success('Board deleted');
        } catch (error) {
          ElMessage.error('Failed to delete board');
        }
      },

    generateGuid() {
      // Генерация GUID (UUID v4) простым способом
      return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        const r = (Math.random() * 16) | 0,
          v = c === 'x' ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      });
    },

    getUserIdFromToken(token) {
      // Распарсить JWT токен и получить userId (обычно в payload.sub или payload.userId)
      if (!token) return null;

      try {
        const payloadBase64 = token.split('.')[1];
        if (!payloadBase64) return null;

        // Base64 декодирование
        const decodedPayload = atob(payloadBase64.replace(/-/g, '+').replace(/_/g, '/'));

        const payload = JSON.parse(decodedPayload);

        // Предположим, userId хранится в поле 'sub' (Subject)
        return payload.sub || payload.userId || null;
      } catch (e) {
        return null;
      }
    }
  }
};
</script>

<style scoped>
.dashboard {
  background-color: #1e1e1e;
  min-height: 100vh;
  padding: 2rem;
  color: #f0f0f0;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  color: #ffffff;
}

.board-list {
  margin-top: 1rem;
}

.board-col {
  transition: transform 0.2s;
}

.board-col:hover {
  transform: translateY(-5px);
}

.board-item {
  background-color: #2a2a2a;
  color: #ffffff;
  border: 1px solid #3a3a3a;
  cursor: pointer;
  transition: box-shadow 0.3s;
}

.card-header {
  display: flex;
  align-items: center;
  font-weight: bold;
}

.board-title {
  margin-left: 0.5rem;
  font-size: 1.1rem;
}

.card-content {
  margin-top: 0.5rem;
}

.no-boards {
  margin-top: 2rem;
  text-align: center;
}

.card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.delete-btn {
  background-color: rgba(255, 0, 0, 0.1); /* полупрозрачный красный */
  border: none;
}

.delete-btn:hover {
  background-color: rgba(255, 0, 0, 0.3);
}


</style>
