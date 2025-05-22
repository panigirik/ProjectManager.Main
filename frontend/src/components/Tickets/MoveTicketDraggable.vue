<template>
  <div class="ticket-list" @dragover.prevent @drop="onDrop">
    <div
      class="ticket"
      v-for="ticket in localTickets"
      :key="ticket.ticketId"
      :draggable="true"
      @dragstart="onDragStart($event, ticket)"
      @click="openTicketDetail(ticket.ticketId)"
    >
      <h4>{{ ticket.title }}</h4>
      <p>{{ ticket.description }}</p>
      <small>Assigned to: {{ ticket.assignedUserName || 'Unassigned' }}</small>
    </div>

    <TicketDetailModal
      v-if="showModal"
      :ticketId="selectedTicketId"
      @close="closeModal"
    />
  </div>
</template>

<script>
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import TicketDetailModal from './TicketDetailModal.vue';

export default {
  name: 'MoveTicketDraggable',
  components: {
    TicketDetailModal,
  },
  props: {
    column: Object,
    tickets: Array,
  },
  data() {
    return {
      localTickets: [...this.tickets],
      draggedTicket: null,
      showModal: false,
      selectedTicketId: null,
    };
  },
  watch: {
    tickets(newTickets) {
      this.localTickets = [...newTickets];
    },
  },
  methods: {
    onDragStart(event, ticket) {
      event.dataTransfer.setData('ticketId', ticket.ticketId);
    },

    async onDrop(event) {
      const ticketId = event.dataTransfer.getData('ticketId');
      if (!ticketId) return;

      const token = localStorage.getItem('accessToken');
      const decoded = jwtDecode(token);
      const userId =
        decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      const newColumnId = this.column.columnId;

      try {
        await axios.put(
          'http://localhost:5258/api/Tickets/move-ticket',
          {
            ticketId: ticketId,
            newColumnId: newColumnId,
            userId: userId,
          },
          {
            headers: {
              Authorization: `Bearer ${token}`,
              'Content-Type': 'application/json',
            },
          }
        );
        this.$emit('ticketMoved');
      } catch (error) {
        console.error('Error moving ticket:', error);
        let message = 'Unknown error occurred while moving ticket.';
        if (error.response && error.response.data) {
          message = error.response.data;
        }
        alert(`Ticket move failed: ${message}`);
      }
    },

    openTicketDetail(ticketId) {
      this.selectedTicketId = ticketId;
      this.showModal = true;
    },

    closeModal() {
      this.selectedTicketId = null;
      this.showModal = false;
    },
  },
};
</script>

<style scoped>
.ticket-list {
  border: 1px solid #ccc;
  min-height: 200px;
  padding: 10px;
}
.ticket {
  background-color: #f4f4f4;
  padding: 10px;
  margin-bottom: 8px;
  cursor: grab;
}
</style>
