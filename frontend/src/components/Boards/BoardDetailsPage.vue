<template>
  <div>
    <el-page-header content="Board Details" />

    <el-skeleton :loading="loading" animated>
      <template #default>
        <el-card shadow="always" class="board-card">
          <div class="board-title" v-if="board">{{ board.boardName }}</div>

          <div class="board-header">
            <div class="actions">
              <el-button v-if="isCreator" type="primary" @click="showModal = true">+ Add Column</el-button>
              <el-button v-if="isCreator" type="info" @click="showRulesModal = true">+ Configure Transition Rules</el-button>
            </div>
          </div>
        </el-card>

        <div class="back-button" @click="goBack">
          <el-icon><ArrowLeft /></el-icon>
          <span>Back</span>
        </div>

        <CreateColumnModal
          v-if="showModal"
          :boardId="board.boardId"
          @close="showModal = false"
          @columnCreated="loadColumns"
        />

        <TransitionRulesModal
          v-if="showRulesModal"
          :boardId="board.boardId"
          :columns="columns"
          :tickets="tickets"
          @close="showRulesModal = false"
        />

        <div class="columns-container">
          <el-card
            v-for="column in columns"
            :key="column.columnId"
            class="column"
            shadow="hover"
          >
            <!-- Заголовок колонки с меню -->
            <div class="column-header" style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 10px;">
              <h3 style="margin: 0;">{{ column.columnName }}</h3>

              <el-dropdown trigger="click" @command="cmd => handleColumnCommand(cmd, column)">
                <span class="dropdown-icon" style="cursor: pointer;">
                  <el-icon><MoreFilled /></el-icon>
                </span>
                <template #dropdown>
                  <el-dropdown-menu>
                    <el-dropdown-item command="delete">Delete Column</el-dropdown-item>
                    <el-dropdown-item command="sort">Sort Tickets by Date</el-dropdown-item>
                  </el-dropdown-menu>
                </template>
              </el-dropdown>
            </div>

            <el-empty v-if="!column.tickets || column.tickets.length === 0" description="No tickets" />

            <MoveTicketDraggable
              :column="column"
              :tickets="column.tickets"
              @update:tickets="newTickets => column.tickets = newTickets"
              @ticketMoved="loadColumns"
            />

            <TicketCard
              v-for="ticket in column.tickets"
              :key="ticket.ticketId"
              :ticket="ticket"
              @openTicketDetail="openTicketDetail"
              @openDeleteConfirmation="deleteTicket"
            />

            <el-button type="success" @click="openTicketModal(column.columnId)" plain>
              <el-icon><Plus /></el-icon>
              Add Ticket
            </el-button>

            <!-- Модалка создания тикета -->
            <CreateTicketModal
              v-if="showTicketModal && selectedColumnId === column.columnId"
              :columnId="column.columnId"
              @close="showTicketModal = false"
              @ticketCreated="refreshColumn(column.columnId)"
            />
          </el-card>
        </div>

        <!-- Модалка деталей тикета -->
        <TicketDetailModal
          v-if="showTicketDetailModal && selectedTicketId"
          :ticketId="selectedTicketId"
          :columns="columns"
          @close="closeTicketDetailModal"
          @ticketDeleted="refreshColumnAfterDelete"
        />


      </template>
    </el-skeleton>
  </div>
</template>



<script>
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import CreateColumnModal from '../Columns/CreateColumnModal.vue';
import CreateTicketModal from '../Tickets/CreateTicketModal.vue';
import MoveTicketDraggable from '../Tickets/MoveTicketDraggable.vue';
import TransitionRulesModal from '../Tickets/TransitionRulesModal.vue';
import TicketDetailModal from '../Tickets/TicketDetailModal.vue';
import { MoreFilled } from '@element-plus/icons-vue';

export default {
  name: 'BoardDetailsPage',
  components: {
    CreateColumnModal,
    CreateTicketModal,
    MoveTicketDraggable,
    TransitionRulesModal,
    TicketDetailModal,
    MoreFilled
  },
  data() {
    return {
      board: null,
      columns: [],
      tickets: [],
      loading: true,
      isCreator: false,
      showModal: false,
      showTicketModal: false,
      selectedColumnId: null,
      openMenuId: null,
      selectedTicketId: null,
      selectedTicket: null,
      showTicketDetailModal: false,
      showRulesModal: false,
      openTicketMenuId: null
    };
  },
  async created() {
    const token = localStorage.getItem('accessToken');
    const boardId = this.$route.params.id;

    try {
      const decoded = jwtDecode(token);
      this.isCreator =
        decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === "creator";

      const boardResponse = await axios.get(`http://localhost:5258/api/Boards/column/${boardId}`, {
        headers: { Authorization: `Bearer ${token}` }
      });

      if (!boardResponse.data || Object.keys(boardResponse.data).length === 0) {
        console.error("Empty board response:", boardResponse.data);
        return;
      }

      this.board = boardResponse.data;
      await this.loadColumns();
      await this.fetchTickets();
    } catch (error) {
      console.error("Error loading board or columns:", error);
    } finally {
      this.loading = false;
    }
  },
  methods: {
    async loadColumns() {
      const token = localStorage.getItem('accessToken');
      const boardId = this.board.boardId;

      try {
        const columnsResponse = await axios.get(
          `http://localhost:5258/api/Columns/columns/${boardId}`,
          { headers: { Authorization: `Bearer ${token}` } }
        );

        let columns = columnsResponse.data;

        if (!Array.isArray(columns)) {
          console.warn("Expected an array of columns but got:", columns);
          this.columns = [];
          return;
        }

        this.columns = await Promise.all(
          columns.map(async (column) => {
            const ticketsResponse = await axios.get(
              `http://localhost:5258/api/Tickets/column/${column.columnId}`,
              { headers: { Authorization: `Bearer ${token}` } }
            );

            return {
              ...column,
              tickets: Array.isArray(ticketsResponse.data) ? ticketsResponse.data : []
            };
          })
        );
      } catch (error) {
        console.error("Error loading columns:", error);
        this.columns = [];
      }
    },

    async fetchTickets() {
      const token = localStorage.getItem('accessToken');
      const boardId = this.$route.params.id;
      try {
        const response = await axios.get(`http://localhost:5258/api/Tickets/board/${boardId}`, {
          headers: { Authorization: `Bearer ${token}` }
        });
        this.tickets = response.data;
      } catch (error) {
        console.error("Error fetching tickets:", error);
      }
    },

    handleColumnCommand(command, column) {
      if (command === 'delete') {
        this.deleteColumn(column.columnId);
      } else if (command === 'sort') {
        this.sortTicketsByDate(column);
      }
    },

    async deleteColumn(columnId) {
      const token = localStorage.getItem('accessToken');
      try {
        await axios.delete(`http://localhost:5258/api/Columns/column/${columnId}`, {
          headers: { Authorization: `Bearer ${token}` }
        });
        await this.loadColumns();
      } catch (error) {
        console.error("Error deleting column:", error);
      }
    },

    openTicketModal(columnId) {
      this.selectedColumnId = columnId;
      this.showTicketModal = true;
    },

    goBack() {
      this.$router.back();
    },

    async openTicketDetail(ticketId) {
      const token = localStorage.getItem('accessToken');
      try {
        const response = await axios.get(
          `http://localhost:5258/api/Tickets/ticket/${ticketId}`,
          { headers: { Authorization: `Bearer ${token}` } }
        );
        this.selectedTicket = response.data;
        this.showTicketDetailModal = true;
      } catch (error) {
        console.error("Error opening ticket detail:", error);
      }
    },

    closeTicketDetailModal() {
      this.showTicketDetailModal = false;
      this.selectedTicket = null;
    },


    toggleTicketMenu(ticketId) {
      this.openTicketMenuId = this.openTicketMenuId === ticketId ? null : ticketId;
    },

    openTransitionRuleModal(ticketId) {
      this.selectedTicketId = ticketId;
      this.showRulesModal = true;
      this.openTicketMenuId = null;
    },

    closeTransitionRuleModal() {
      this.showRulesModal = false;
      this.selectedTicketId = null;
    },

    toggleColumnMenu(columnId) {
      this.openMenuId = this.openMenuId === columnId ? null : columnId;
    },

    handleTicketDeleted(ticketId) {
      for (const column of this.columns) {
        const index = column.tickets.findIndex(t => t.ticketId === ticketId);
        if (index !== -1) {
          // Vue-реактивное обновление
          this.$set(column, 'tickets', [
            ...column.tickets.slice(0, index),
            ...column.tickets.slice(index + 1)
          ]);
          break;
        }
      }
    },

    async refreshColumnAfterDelete(ticketId, columnId) {
      await this.refreshColumn(columnId);
      this.closeTicketDetailModal();
    },

    async refreshColumn(columnId) {
      const token = localStorage.getItem('accessToken');
      try {
        const ticketsResponse = await axios.get(
          `http://localhost:5258/api/Tickets/column/${columnId}`,
          { headers: { Authorization: `Bearer ${token}` } }
        );

        const updatedTickets = Array.isArray(ticketsResponse.data) ? ticketsResponse.data : [];

        const columnIndex = this.columns.findIndex(col => col.columnId === columnId);
        if (columnIndex !== -1) {
          this.columns[columnIndex].tickets = updatedTickets;
        }

        this.showTicketModal = false;
      } catch (error) {
        console.error("Error refreshing column:", error);
      }
    },

    sortTicketsByDate(column) {
      if (!column.tickets || column.tickets.length === 0) return;

      column.tickets.sort((a, b) => new Date(a.createdAt) - new Date(b.createdAt));
    }
  }
};
</script>



<style scoped>
.add-column-btn {
  margin-top: 10px;
  padding: 8px 12px;
  background-color: #0079bf;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}

.columns-container {
  display: flex;
  flex-direction: row;
  gap: 20px;
  overflow-x: auto;
  margin-top: 20px;
}

.column {
  background-color: #f1f1f1;
  border-radius: 8px;
  padding: 10px;
  width: 300px;
  min-width: 250px;
}

.ticket {
  background-color: #fff;
  border: 1px solid #ccc;
  border-radius: 6px;
  padding: 8px;
  margin-top: 10px;
}

.column-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  position: relative;
}

.menu-button {
  background: none;
  border: none;
  font-size: 18px;
  cursor: pointer;
  padding: 0 6px;
}

.menu-dropdown {
  position: absolute;
  top: 30px;
  right: 0;
  background-color: white;
  border: 1px solid #ccc;
  border-radius: 6px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
  z-index: 10;
  display: flex;
  flex-direction: column;
}

.menu-dropdown button {
  padding: 8px 12px;
  background: none;
  border: none;
  text-align: left;
  cursor: pointer;
}

.menu-dropdown button:hover {
  background-color: #f0f0f0;
}

.configure-rules-btn {
  margin-left: 12px;
  padding: 8px 14px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}

.columns-wrapper {
  flex-wrap: nowrap;
  overflow-x: auto;
}

.column-card {
  min-width: 280px;
  max-width: 350px;
}

.column-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.dropdown-icon {
  position: absolute;
  top: 10px;
  right: 10px;
  cursor: pointer;
  z-index: 1;
  color: #909399;
  transition: color 0.3s;
}

.dropdown-icon:hover {
  color: #409EFF; /* основной цвет Element Plus */
}

.back-button {
  display: flex;
  align-items: center;
  gap: 6px;
  cursor: pointer;
  color: #0079bf;
  font-weight: 500;
  margin-bottom: 16px;
  user-select: none;
  transition: color 0.2s;
}



</style>