<template>
  <div>
    <h1>Board Details</h1>

    <div v-if="loading">Loading...</div>

    <div v-else-if="board">
      <h2>{{ board.boardName }}</h2>

      <div class="columns-container">
        <div
          v-for="column in columns"
          :key="column.columnId"
          class="column"
        >
          <h3>{{ column.columnName }}</h3>

          <div v-if="!column.tickets || column.tickets.length === 0">
            <p>No tickets</p>
          </div>

          <div
            v-else
            v-for="ticket in column.tickets"
            :key="ticket.ticketId"
            class="ticket"
          >
            <h4>{{ ticket.title }}</h4>
            <p>{{ ticket.description }}</p>
            <small>Assigned to: {{ ticket.assignedUserName || 'Unassigned' }}</small>
          </div>
        </div>
      </div>
    </div>

    <div v-else>
      <p>Error: Board not found or failed to load.</p>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'BoardDetailsPage',
  data() {
    return {
      board: null,
      columns: [],
      loading: true,
    };
  },
  async created() {
    const token = localStorage.getItem('accessToken');
    const boardId = this.$route.params.id;

    try {
      // Загружаем доску (можно убрать, если boardName не нужен)
      this.board = {
        boardId,
        boardName: 'Board', // или получай название, если нужно
      };

      // Загружаем все колонки по boardId
      const columnsResponse = await axios.get(`http://localhost:5258/api/Columns/columns/${boardId}`, {
        headers: { Authorization: `Bearer ${token}` }
      });

      const columns = columnsResponse.data;

      // Для каждой колонки загружаем тикеты напрямую
      this.columns = await Promise.all(columns.map(async (column) => {
        const ticketsResponse = await axios.get(
          `http://localhost:5258/api/Tickets/column/${column.columnId}`,
          { headers: { Authorization: `Bearer ${token}` } }
        );

        return {
          ...column,
          tickets: ticketsResponse.data
        };
      }));
    } catch (error) {
      console.error("Ошибка при загрузке доски или колонок:", error);
    } finally {
      this.loading = false;
    }
  }
};
</script>


<style scoped>
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
</style>
