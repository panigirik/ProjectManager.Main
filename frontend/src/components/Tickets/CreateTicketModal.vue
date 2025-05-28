<template>
  <div class="modal-overlay">
    <div class="modal">
      <h2>Add New Ticket</h2>

      <input v-model="title" placeholder="Title" class="input" />
      <textarea v-model="description" placeholder="Description" class="textarea"></textarea>

      <select v-model="assignedUserName" class="input">
        <option disabled value="">Select user</option>
        <option v-for="user in users" :key="user.id" :value="user.userName">
          {{ user.userName }}
        </option>
      </select>

      <input type="file" multiple @change="handleFileUpload" />

      <div class="buttons">
        <button @click="createTicket" class="save">Create</button>
        <button @click="$emit('close')" class="cancel">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'CreateTicketModal',
  props: {
    columnId: { type: String, required: true }
  },
  data() {
    return {
      title: '',
      description: '',
      assignedUserName: '',
      attachments: [],
      users: []
    };
  },
  methods: {
    handleFileUpload(event) {
      this.attachments = Array.from(event.target.files);
    },
    async fetchUsers() {
      const token = localStorage.getItem('accessToken');
      try {
        const response = await axios.get('http://localhost:5258/api/Users/user', {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });
        this.users = response.data;
      } catch (error) {
        console.error('Failed to load users:', error);
        alert('Failed to load users');
      }
    },
    async createTicket() {
      if (!this.title.trim()) {
        alert("Title is required");
        return;
      }

      const formData = new FormData();
      formData.append('Title', this.title);
      formData.append('Description', this.description);
      formData.append('AssignedUserName', this.assignedUserName);
      formData.append('ColumnId', this.columnId);

      for (let file of this.attachments) {
        formData.append('Attachments', file);
      }

      const token = localStorage.getItem('accessToken');

      try {
        await axios.post('http://localhost:5258/api/Tickets/ticket', formData, {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'multipart/form-data'
          }
        });

        this.$emit('ticketCreated');
        this.$emit('close');
      } catch (error) {
        console.error('Error creating ticket:', error);
        alert('Failed to create ticket');
      }
    }
  },
  mounted() {
    this.fetchUsers();
  }
};
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  width: 400px;
}

.input, .textarea, select {
  width: 100%;
  padding: 8px;
  margin-top: 10px;
  margin-bottom: 10px;
}

.textarea {
  resize: vertical;
  height: 80px;
}

.buttons {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.save {
  background-color: #5aac44;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
}

.cancel {
  background-color: #ccc;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
}
</style>
