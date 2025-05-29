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
            <div v-if="form.assignedUserId">Выбран: {{ selectedAssignedUserName }}</div>
            <select v-model="form.assignedUserId">
              <option value="">-- Не назначено --</option>
              <option v-for="user in users" :key="user.userId" :value="user.userId">
                {{ user.userName }}
              </option>
            </select>
          </label>



            <label>
              Вложения:
              <input type="file" multiple @change="handleFileChange" />
            </label>

            <!-- модалка для полного изображения -->
            <div v-if="selectedImage" class="image-modal" @click="selectedImage = null">
              <img :src="selectedImage" class="full-image" @click.stop />
              <button class="close-full-image" @click="selectedImage = null">×</button>
            </div>


            <div v-if="ticket.attachments && ticket.attachments.length">
              <p><strong>Вложения (изображения):</strong></p>
              <div class="thumbnail-list">
                <img
                  v-for="(link, idx) in ticketImageLinks"
                  :key="idx"
                  :src="link"
                  class="thumbnail"
                  @click="openImage(link)"
                />
              </div>
            </div>

            <footer class="modal-footer">
            <button type="submit">Сохранить</button>
            <button type="button" @click="$emit('close')">Отмена</button>
            <button
              type="button"
              class="delete-circle-inside"
              @click="confirmDeleteTicket"
              title="Удалить тикет"
            >
              Удалить
            </button>
          </footer>
          </form>
        </div>
      </section>
    </div>
  </div>
</template>


<script>
import axios from "axios";
import { getDropboxTemporaryLink } from "@/dropboxClient";

export default {
  name: "TicketDetailModal",
  props: {
    ticketId: {
      type: String,
      required: true,
    },
    columns: {
    type: Array,
    required: true,
  }
  },
  data() {
    return {
      ticket: null,
      loading: false,
      error: null,
      form: {
        title: "",
        description: "",
        assignedUserId: "",
        assignedUserName: "",
        columnId: "",
      },
      users: [],
      newFiles: [],
      ticketImageLinks: [],
      selectedImage: null,
    };
  },

  computed: {
    selectedAssignedUserName() {
      const user = this.users.find(u => u.userId === this.form.assignedUserId);
      return user ? user.userName : "";
    },
  },

  methods: {
    async fetchUsers() {
      try {
        const token = localStorage.getItem("accessToken");
        const response = await axios.get("http://localhost:5258/api/Users/user", {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        this.users = response.data;
      } catch (err) {
        console.error("Ошибка при загрузке пользователей:", err);
        this.users = [];
      }
    },

    async fetchTicket() {
      this.loading = true;
      this.error = null;

      try {
        const token = localStorage.getItem("accessToken");
        const response = await axios.get(
          `http://localhost:5258/api/Tickets/ticket/${this.ticketId}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );

        const ticketData = response.data;
        this.ticket = ticketData;

        this.form.title = ticketData.title;
        this.form.description = ticketData.description;
        this.form.assignedUserId = ticketData.assignedUserId;
        this.form.columnId = ticketData.columnId || "";

        const assignedUser = this.users.find(u => u.userId === ticketData.assignedUserId);
        this.form.assignedUserName = assignedUser ? assignedUser.userName : "";

        if (ticketData.attachments && ticketData.attachments.length) {
          try {
            const linksResponse = await axios.get(
              `http://localhost:5258/api/Tickets/ticket/${this.ticketId}/attachments/links`,
              {
                headers: { Authorization: `Bearer ${token}` },
              }
            );
            this.ticketImageLinks = linksResponse.data;
          } catch (err) {
            console.error("Ошибка при получении ссылок на вложения:", err);
            this.ticketImageLinks = [];
          }
        } else {
          this.ticketImageLinks = [];
        }
      } catch (err) {
        console.error("Ошибка при загрузке тикета:", err.response?.data || err.message);
        alert("Не удалось загрузить тикет.");
      } finally {
        this.loading = false;
      }
    },

    handleFileChange(event) {
      this.newFiles = Array.from(event.target.files).filter(file =>
        file.type.startsWith("image/")
      );
    },

    openImage(link) {
      this.selectedImage = link;
    },

    async submitForm() {
      const assignedUserName = this.selectedAssignedUserName;

      if (!assignedUserName) {
        alert("Поле AssignedUserName обязательно для заполнения.");
        return;
      }

      try {
        const formData = new FormData();

        formData.append("TicketId", this.ticketId);
        formData.append("Title", this.form.title || "");
        formData.append("Description", this.form.description || "");
        formData.append("AssignedUserName", assignedUserName);

        if (this.form.columnId) {
          formData.append("ColumnId", this.form.columnId);
        }

        if (this.newFiles.length > 0) {
          this.newFiles.forEach(file => {
            formData.append("Attachments", file);
          });
        }

        const token = localStorage.getItem("accessToken");

        await axios.put("http://localhost:5258/api/Tickets/ticket", formData, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        this.ticket.title = this.form.title;
        this.ticket.description = this.form.description;
        this.ticket.assignedUserName = assignedUserName;
        this.ticket.columnId = this.form.columnId;

        if (this.ticket.attachments && this.ticket.attachments.length) {
          const imageLinks = await Promise.all(
            this.ticket.attachments.map(path => getDropboxTemporaryLink(path))
          );
          this.ticketImageLinks = imageLinks.filter(link => !!link);
        } else {
          this.ticketImageLinks = [];
        }

        this.newFiles = [];
        this.$emit("close");
      } catch (err) {
        console.error("Ошибка при сохранении тикета:", err.response?.data || err.message);
        alert("Не удалось сохранить изменения.");
      }
    },

    handleTicketDeleted(ticketId) {
          // Пройтись по колонкам и удалить тикет
          for (const column of this.columns) {
            const index = column.tickets.findIndex(t => t.ticketId === ticketId);
            if (index !== -1) {
              column.tickets.splice(index, 1); // удалить тикет локально
              break;
            }
          }
        },

        async deleteTicket(ticketId) {
          const token = localStorage.getItem('accessToken');
          try {
            await axios.delete(`http://localhost:5258/api/Tickets/ticket/${ticketId}`, {
              headers: { Authorization: `Bearer ${token}` }
            });

            this.$emit('ticketDeleted', this.ticketId, this.columnId);
            this.$emit('close');
          } catch (error) {
            console.error("Error deleting ticket:", error);
          }
        },


    confirmDeleteTicket() {
      if (confirm("Вы уверены, что хотите удалить этот тикет?")) {
        this.deleteTicket(this.ticketId);
        this.$emit('close'); // закрыть модалку после удаления
      }
    },

  },

  mounted() {
    this.fetchUsers().then(() => {
      this.fetchTicket();
    });
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
    background-color: rgba(198, 224, 211, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
  }

    .modal-window {
    background: white;
    border-radius: 12px;
    width: 90%;
    height: 90%;
    max-width: 1200px;
    max-height: 800px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
    overflow: auto;
    display: flex;
    flex-direction: column;
  }

  .delete-circle-inside {
    margin-left: 16px;
    background-color: #ff4d4f;
    color: white;
    border: none;
    padding: 6px 12px;
    border-radius: 4px;
    cursor: pointer;
    transition: background-color 0.3s;
  }

  .delete-circle-inside:hover {
    background-color: #d9363e;
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

    .modal-body label {
  display: block;
  margin-bottom: 12px;
}

.modal-body input,
.modal-body textarea {
  width: 100%;
  padding: 8px;
  margin-top: 4px;
  box-sizing: border-box;
  font-size: 14px;
}

.modal-body textarea {
  min-height: 120px;
  resize: vertical;
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


  .thumbnail-list {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  margin-top: 10px;
}

.thumbnail {
  width: 80px;
  height: 80px;
  object-fit: cover;
  border: 2px solid #ccc;
  border-radius: 4px;
  cursor: pointer;
  transition: transform 0.2s;
}

.thumbnail:hover {
  transform: scale(1.1);
}

.thumbnail-list {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-top: 8px;
}

.thumbnail {
  width: 150px;
  height: 150px;
  object-fit: cover;
  border-radius: 8px;
  cursor: pointer;
  transition: transform 0.2s ease-in-out;
}
.thumbnail:hover {
  transform: scale(1.05);
}

.image-modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.8);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.full-image {
  max-width: 90%;
  max-height: 90%;
  border-radius: 8px;
  box-shadow: 0 0 20px #000;
}

.close-full-image {
  position: fixed;
  top: 30px;
  right: 30px;
  font-size: 32px;
  background: transparent;
  border: none;
  color: white;
  cursor: pointer;
}
</style>


  