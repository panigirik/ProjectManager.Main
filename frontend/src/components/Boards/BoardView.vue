<template>
    <div class="dashboard">
      <h1>Your Boards</h1>
      <div v-if="loading">Loading boards...</div>
      <div v-else>
        <div v-if="boards.length > 0" class="board-list">
          <el-card
            v-for="board in boards"
            :key="board.boardId"
            class="board-item"
            @click="openBoard(board.boardId)"
          >
            <strong>{{ board.boardName }}</strong><br />
            <span>{{ board.columnIds.length }} columns</span>
          </el-card>
        </div>
        <div v-else>
          <p>You don't have any boards yet.</p>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: 'MainDashboard',
    data() {
      return {
        boards: [],
        loading: true
      };
    },
    async created() {
      const token = localStorage.getItem('accessToken');
  
      if (!token) {
        this.$message.error('You must be logged in.');
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
        this.$message.error('Failed to load boards');
      } finally {
        this.loading = false;
      }
    },
    methods: {
        openBoard(boardId) {
        this.$router.push(`/boards/${boardId}`);
      }
    }
  };
  </script>
  
  <style scoped>
  .dashboard {
    padding: 2rem;
  }
  
  .board-list {
    list-style: none;
    padding: 0;
  }
  
  .board-item {
    background: #f0f0f0;
    padding: 1rem;
    margin-bottom: 1rem;
    border-radius: 6px;
    cursor: pointer;
  }
  </style>
  