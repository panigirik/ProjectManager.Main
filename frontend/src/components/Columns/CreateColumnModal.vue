<template>
  <div class="modal-overlay">
    <div class="modal">
      <h2>Add New Column</h2>
      <input
        v-model="columnName"
        placeholder="Enter column name"
        class="input"
      />
      <div class="buttons">
        <button @click="createColumn" class="save">Save</button>
        <button @click="$emit('close')" class="cancel">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'CreateColumnModal',
  props: {
    boardId: { type: String, required: true }
  },
  data() {
    return {
      columnName: ''
    };
  },
  methods: {
    async createColumn() {
      if (!this.columnName.trim()) {
        alert("Column name cannot be empty");
        return;
      }

      const token = localStorage.getItem('accessToken');

      try {
        await axios.post('http://localhost:5258/api/Columns/column', {
          columnName: this.columnName,
          boardId: this.boardId
        }, {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });

        this.$emit('columnCreated');
        this.$emit('close');
      } catch (error) {
        console.error("Error creating column:", error);
        alert("Failed to create column");
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
  width: 300px;
}

.input {
  width: 100%;
  padding: 8px;
  margin-top: 10px;
  margin-bottom: 20px;
}

.buttons {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.save {
  background-color: #0079bf;
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