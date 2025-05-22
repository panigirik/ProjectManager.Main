<template>
    <div class="modal-backdrop" @click.self="$emit('close')">
      <div class="modal-window">
        <header class="modal-header">
          <h3>Редактирование тикета</h3>
          <button class="close-btn" @click="$emit('close')">×</button>
        </header>
  
        <section class="modal-body">
          <div v-if="loading">Загрузка тикета...</div>
          <div v-else-if="error">{{ error }}</div>
          <div v-else-if="ticket">
            <form @submit.prevent="submitForm">
              <label>
                Название:
                <input v-model="form.title" required />
              </label>
  
              <label>
                Описание:
                <textarea v-model="form.description" required></textarea>
              </label>
  
              <label>
                Назначено:
                <input v-model="form.assignedUserName" />
              </label>
  
              <label>
                Вложения:
                <input type="file" multiple @change="handleFileChange" />
              </label>
  
              <div v-if="ticket.attachments && ticket.attachments.length">
                <p><strong>Текущие вложения:</strong></p>
                <ul>
                  <li v-for="(a, idx) in ticket.attachments" :key="idx">{{ a }}</li>
                </ul>
              </div>
  
              <footer class="modal-footer">
                <button type="submit">Сохранить</button>
                <button type="button" @click="$emit('close')">Отмена</button>
              </footer>
            </form>
          </div>
        </section>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: "TicketDetailModal",
    props: {
      ticketId: {
        type: String,
        required: true,
      },
    },
    data() {
      return {
        ticket: null,
        loading: false,
        error: null,
        form: {
          title: '',
          description: '',
          assignedUserName: '',
          attachments: [],
        },
        newFiles: [],
      };
    },
    methods: {
      async fetchTicket() {
        this.loading = true;
        try {
          const token = localStorage.getItem('accessToken');
          const response = await axios.get(`http://localhost:5258/api/Tickets/ticket/${this.ticketId}`, {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
          this.ticket = response.data;
          this.form.title = response.data.title;
          this.form.description = response.data.description;
          this.form.assignedUserName = response.data.assignedUserName;
        } catch (err) {
          console.error("Ошибка при загрузке тикета:", err);
          this.error = "Не удалось загрузить данные тикета.";
        } finally {
          this.loading = false;
        }
      },
      handleFileChange(event) {
        this.newFiles = Array.from(event.target.files);
      },
      async submitForm() {
        const formData = new FormData();
        formData.append('TicketId', this.ticketId);
        formData.append('Title', this.form.title);
        formData.append('Description', this.form.description);
        formData.append('AssignedUserName', this.form.assignedUserName);
        formData.append('ColumnId', this.ticket.columnId);
  
        this.newFiles.forEach(file => {
          formData.append('Attachments', file);
        });
  
        try {
          const token = localStorage.getItem('accessToken');
          await axios.put('http://localhost:5258/api/Tickets/ticket', formData, {
            headers: {
              Authorization: `Bearer ${token}`,
              'Content-Type': 'multipart/form-data',
            },
          });
          this.$emit('close');
        } catch (err) {
          console.error("Ошибка при сохранении тикета:", err);
          alert("Не удалось сохранить изменения.");
        }
      },
    },
    mounted() {
      this.fetchTicket();
    },
  };
  </script>
  
  <style scoped>
  .modal-backdrop {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0,0,0,0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
  }
  .modal-window {
    background: white;
    border-radius: 8px;
    width: 400px;
    max-width: 90%;
    box-shadow: 0 2px 10px rgba(0,0,0,0.3);
    overflow: hidden;
  }
  .modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 12px 16px;
    background: #f5f5f5;
    border-bottom: 1px solid #ddd;
  }
  .close-btn {
    background: transparent;
    border: none;
    font-size: 20px;
    cursor: pointer;
  }
  .modal-body {
    padding: 16px;
  }
  .modal-body .description {
    white-space: pre-wrap;
    margin: 8px 0;
  }
  .modal-footer {
    padding: 12px 16px;
    text-align: right;
    background: #f5f5f5;
    border-top: 1px solid #ddd;
  }
  .modal-footer button {
    padding: 6px 12px;
  }
  </style>
  