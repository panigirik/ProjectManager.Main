<template>
    <div class="modal-overlay" @click.self="$emit('close')">
      <div class="modal">
        <h3>Configure Transition Rule</h3>
  
        <label>Ticket:</label>
        <select v-model="ticketId">
          <option v-for="ticket in allTickets" :key="ticket.ticketId" :value="ticket.ticketId">
            {{ ticket.title }}
          </option>
        </select>
  
        <label>From Column:</label>
        <select v-model="fromColumnId">
          <option v-for="col in columns" :key="col.columnId" :value="col.columnId">
            {{ col.columnName }}
          </option>
        </select>
  
        <label>To Column:</label>
        <select v-model="toColumnId">
          <option v-for="col in columns" :key="col.columnId" :value="col.columnId">
            {{ col.columnName }}
          </option>
        </select>
  
        <label>Is Allowed:</label>
        <input type="checkbox" v-model="isAllowed" />
  
        <label>Required Validations:</label>
        <input v-model.number="requiredValidations" type="number" min="0" />
  
        <div class="actions">
          <button @click="submitRule">Submit Rule</button>
          <button @click="$emit('close')">Cancel</button>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  import { v4 as uuidv4 } from 'uuid';
  import { jwtDecode } from 'jwt-decode';
  
  export default {
    props: {
      boardId: String
    },
    data() {
      return {
        columns: [],
        allTickets: [],
        ticketId: null,
        fromColumnId: null,
        toColumnId: null,
        isAllowed: true,
        requiredValidations: 0
      };
    },
    async mounted() {
      await this.loadColumnsAndTickets();
    },
    methods: {
      async loadColumnsAndTickets() {
        try {
          const token = localStorage.getItem('accessToken');
  
          // 1. Загрузим все колонки доски
          const colResp = await axios.get(`http://localhost:5258/api/Columns/columns/${this.boardId}`, {
            headers: {
              Authorization: `Bearer ${token}`
            }
          });
          this.columns = colResp.data;
  
          // 2. Загрузим все тикеты из каждой колонки
          const allTickets = [];
  
          for (const col of this.columns) {
            const ticketResp = await axios.get(`http://localhost:5258/api/Tickets/column/${col.columnId}`, {
              headers: {
                Authorization: `Bearer ${token}`
              }
            });
            allTickets.push(...ticketResp.data);
          }
  
          this.allTickets = allTickets;
        } catch (err) {
          console.error("Error loading columns/tickets:", err);
          alert("Failed to load board data.");
        }
      },
  
      async submitRule() {
        try {
          const token = localStorage.getItem('accessToken');
          const decoded = jwtDecode(token);
          const userId = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
  
          const payload = {
            ticketTransitionRuleId: uuidv4(),
            ticketId: this.ticketId,
            fromColumnId: this.fromColumnId,
            toColumnId: this.toColumnId,
            isAllowed: this.isAllowed,
            requiredValidations: this.requiredValidations,
            userId: userId
          };
  
          await axios.post("http://localhost:5258/api/TicketTransition/rule", payload, {
            headers: {
              Authorization: `Bearer ${token}`,
              'Content-Type': 'application/json'
            }
          });
  
          alert("Rule saved!");
          this.$emit('close');
        } catch (err) {
          console.error(err);
          alert("Failed to save rule.");
        }
      }
    }
  };
  </script>
  
  <style scoped>
  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.4);
    display: flex;
    align-items: center;
    justify-content: center;
  }
  
  .modal {
    background: white;
    padding: 20px;
    border-radius: 8px;
    min-width: 300px;
  }
  
  .actions {
    margin-top: 15px;
    display: flex;
    justify-content: space-between;
  }
  </style>
  