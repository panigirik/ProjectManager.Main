<template>
    <div
      class="ticket-list"
      @dragover.prevent
      @drop="onDrop"
    >
      <div
        class="ticket"
        v-for="ticket in localTickets"
        :key="ticket.ticketId"
        :draggable="true"
        @dragstart="onDragStart($event, ticket)"
      >
        <h4>{{ ticket.title }}</h4>
        <p>{{ ticket.description }}</p>
        <small>Assigned to: {{ ticket.assignedUserName || 'Unassigned' }}</small>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  import { jwtDecode } from 'jwt-decode';
  
  export default {
    name: 'MoveTicketHtml5',
    props: {
      column: Object,
      tickets: Array,
    },
    data() {
      return {
        localTickets: [...this.tickets],
        draggedTicket: null,
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
        const userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        const newColumnId = this.column.columnId;
  
        try {
          await axios.put(
            'http://localhost:5258/api/Tickets/move-ticket',
            {
              ticketId: ticketId,
              newColumnId: newColumnId,
              userId: userId,
              commitLink: '', // если нужно, можно заменить на реальное значение
            },
            {
              headers: {
                Authorization: `Bearer ${token}`,
                'Content-Type': 'application/json',
              },
            }
          );
  
          // Эмитим событие наверх, чтобы обновить тикеты в колонке
          this.$emit('ticketMoved');
        } catch (error) {
          console.error('Error moving ticket:', error);
        }
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
  