<template>
    <div>
      <el-button @click="createBoard">New Board</el-button>
      <el-card v-for="board in boards" :key="board.id" @click="openBoard(board.id)">
        {{ board.name }}
      </el-card>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        boards: []
      };
    },
    created() {
      this.fetchBoards();
    },
    methods: {
      async fetchBoards() {
        const response = await this.$axios.get('/api/Boards/column');
        this.boards = response.data;
      },
      createBoard() {
        // Logic to create a new board
      },
      openBoard(id) {
        this.$router.push(`/boards/${id}`);
      }
    }
  };
  </script>
  