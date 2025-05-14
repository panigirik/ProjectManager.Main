<template>
    <div>
      <h3>{{ column.name }}</h3>
      <draggable v-model="tickets" @end="onDragEnd">
        <div v-for="ticket in tickets" :key="ticket.id">
          {{ ticket.title }}
        </div>
      </draggable>
    </div>
  </template>
  
  <script>
  import draggable from 'vuedraggable';
  
  export default {
    components: {
      draggable
    },
    props: ['column'],
    data() {
      return {
        tickets: []
      };
    },
    created() {
      this.fetchTickets();
    },
    methods: {
      async fetchTickets() {
        const response = await this.$axios.get(`/api/Tickets/ticket?columnId=${this.column.id}`);
        this.tickets = response.data;
      },
      onDragEnd(event) {
        const movedTicket = this.tickets[event.newIndex];
        this.$axios.put('/api/Tickets/move-ticket', {
          ticketId: movedTicket.id,
          newColumnId: this.column.id
        });
      }
    }
  };
  </script>
  