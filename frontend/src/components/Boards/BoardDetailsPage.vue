<template>
  <div>
    <h1>Board Details</h1>

    <div v-if="loading">Loading...</div>

    <div v-else-if="board">
      <h2>{{ board.boardName }}</h2>

      <button v-if="isCreator" @click="showModal = true" class="add-column-btn">
        + Add Column
      </button>

      <CreateColumnModal
        v-if="showModal"
        :boardId="board.boardId"
        @close="showModal = false"
        @columnCreated="loadColumns"
      />

      <button v-if="isCreator" @click="showRulesModal = true" class="configure-rules-btn">
      + Configure Transition Rules
      </button>

      <TransitionRulesModal
        v-if="showRulesModal"
        :boardId="board.boardId"
        :columns="columns"
        :tickets="tickets"
        @close="showRulesModal = false"
      />




      <!-- ✅ ОБЁРТКА С FLEX-КОНТЕЙНЕРОМ -->
      <div class="columns-container">
        <div
          v-for="column in columns"
          :key="column.columnId"
          class="column"
        >
          <!-- 🔽 ВСТАВЛЕННЫЙ БЛОК С НАЗВАНИЕМ И МЕНЮ -->
          <div class="column-header">
            <h3>{{ column.columnName }}</h3>
            <div class="column-menu-wrapper">
              <button @click="toggleColumnMenu(column.columnId)" class="menu-button">⋮</button>
              <div v-if="openMenuId === column.columnId" class="menu-dropdown">
                <button @click="deleteColumn(column.columnId)">Delete Column</button>
                <button @click="sortTicketsByDate(column)">Sort Tickets by Date</button>
              </div>
            </div>
          </div>
          <!-- 🔼 КОНЕЦ БЛОКА -->

          <div v-if="!column.tickets || column.tickets.length === 0">
            <p>No tickets</p>
          </div>


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

          <TicketDetailModal
            v-if="showTicketDetailModal && selectedTicketId"
            :ticketId="selectedTicketId"
            @close="closeTicketDetailModal"
          />



          <button @click="openTicketModal(column.columnId)" class="add-ticket-btn">
            + Add Ticket
          </button>

          <CreateTicketModal 
            v-if="showTicketModal && selectedColumnId === column.columnId"
            :columnId="column.columnId"
            @close="showTicketModal = false"
            @ticketCreated="refreshColumn"
          />
        </div>
      </div>
    </div>
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

export default {
  name: 'BoardDetailsPage',
  components: {
    CreateColumnModal,
    CreateTicketModal,
    MoveTicketDraggable,
    TransitionRulesModal,
    TicketDetailModal
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
      showRulesModal: false,
      openTicketMenuId: null
    };
  },
  async created() {
    const token = localStorage.getItem('accessToken');
    const boardId = this.$route.params.id;

    try {
      const decoded = jwtDecode(token);
      this.isCreator = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === "creator";

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
        const columnsResponse = await axios.get(`http://localhost:5258/api/Columns/columns/${boardId}`, {
          headers: { Authorization: `Bearer ${token}` }
        });

        let columns = columnsResponse.data;

        if (!Array.isArray(columns)) {
          console.warn("Expected an array of columns but got:", columns);
          this.columns = [];
          return;
        }

        this.columns = await Promise.all(columns.map(async (column) => {
          const ticketsResponse = await axios.get(
            `http://localhost:5258/api/Tickets/column/${column.columnId}`,
            { headers: { Authorization: `Bearer ${token}` } }
          );

          return {
            ...column,
            tickets: Array.isArray(ticketsResponse.data) ? ticketsResponse.data : []
          };
        }));
      } catch (error) {
        console.error("Error loading columns:", error);
        this.columns = [];
      }
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
      } catch (err) {
        console.error(err);
      }
    },
    closeTicketDetailModal() {
      this.showTicketDetailModal = false;
      this.selectedTicket = null;
    },

    async fetchTickets() {
      const token = localStorage.getItem('accessToken');
      const boardId = this.$route.params.id;
      const response = await axios.get(`http://localhost:5258/api/Tickets/board/${boardId}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      this.tickets = response.data;
    },


    async deleteTicket(ticketId) {
  const token = localStorage.getItem('accessToken');
  try {
    await axios.delete(`http://localhost:5258/api/Tickets/ticket/${ticketId}`, {
      headers: { Authorization: `Bearer ${token}` }
    });
    await this.loadColumns(); // обновляем колонки после удаления
  } catch (error) {
    console.error('Error deleting ticket:', error);
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

  toggleTicketMenu(ticketId) {
    this.openTicketMenuId = this.openTicketMenuId === ticketId ? null : ticketId;
  },


  openTransitionRuleModal(ticketId) {
    this.selectedTicketId = ticketId;
    this.showRulesModal = true;
    this.openTicketMenuId = null; // закрываем меню
  },
  closeTransitionRuleModal() {
    this.showRulesModal = false;
    this.selectedTicketId = null;
  },


toggleColumnMenu(columnId) {
  this.openMenuId = this.openMenuId === columnId ? null : columnId;
},


    openTicketModal(columnId) {
      this.selectedColumnId = columnId;
      this.showTicketModal = true;
    },

    async refreshColumn() {
      await this.loadColumns();
      this.showTicketModal = false;
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


</style>