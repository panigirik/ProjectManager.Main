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
import { ElMessage } from "element-plus"; // Если используешь Element Plus

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
        title: "",
        description: "",
        assignedUserName: "",
      },
      newFiles: [],
      ticketImageLinks: [],
      nullselectedImage: null // Ссылки на изображения из Dropbox
    };
  },
  methods: {
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

        this.ticket = response.data;
        this.form.title = response.data.title;
        this.form.description = response.data.description;
        this.form.assignedUserName = response.data.assignedUserName;

        if (response.data.attachments && response.data.attachments.length) {
        const token = localStorage.getItem("accessToken");
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
        console.error("Ошибка при загрузке тикета:", err);
        this.error = "Не удалось загрузить данные тикета.";
      } finally {
        this.loading = false;
      }
    },

    handleFileChange(event) {
      // Фильтруем только изображения
      this.newFiles = Array.from(event.target.files).filter((file) =>
        file.type.startsWith("image/")
      );
    },

    openImage(link) {
      this.selectedImage = link;
    },


    async submitForm() {
      const formData = new FormData();
      formData.append("TicketId", this.ticketId);
      formData.append("Title", this.form.title);
      formData.append("Description", this.form.description);
      formData.append("AssignedUserName", this.form.assignedUserName);
      formData.append("ColumnId", this.ticket.columnId);

      this.newFiles.forEach((file) => {
        formData.append("Attachments", file);
      });

      try {
        const token = localStorage.getItem("accessToken");
        const response = await axios.put(
          "http://localhost:5258/api/Tickets/ticket",
          formData,
          {
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "multipart/form-data",
            },
          }
        );

        // Обновляем локальный тикет и ссылки после успешного обновления
        this.ticket = response.data;

        if (this.ticket.attachments && this.ticket.attachments.length) {
          const imageLinks = await Promise.all(
            this.ticket.attachments.map((path) =>
              getDropboxTemporaryLink(path)
            )
          );
          this.ticketImageLinks = imageLinks.filter((link) => !!link);
        } else {
          this.ticketImageLinks = [];
        }

        this.newFiles = [];
        this.$emit("close");
      } catch (err) {
        console.error("Ошибка при сохранении тикета:", err);
        alert("Не удалось сохранить изменения.");
      }
    },

    async deleteTicket() {
        try {
          const token = localStorage.getItem("accessToken");
          await axios.delete(`http://localhost:5258/api/Tickets/ticket/${this.ticketId}`, {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });

          ElMessage.success("Тикет успешно удалён.");
          this.$emit("deleted", this.ticketId); // уведомляем родителя, что тикет удалён
          this.$emit("close"); // закрываем модалку
        } catch (err) {
          console.error("Ошибка при удалении тикета:", err);
          ElMessage.error("Не удалось удалить тикет.");
        }
      },

      confirmDeleteTicket() {
        if (confirm("Вы уверены, что хотите удалить этот тикет?")) {
          this.deleteTicket();
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


  