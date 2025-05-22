<template>
  <div class="ticket-card" @click="$emit('openTicketDetail', ticket.id)">
    <div class="ticket-content">
      <h4>{{ ticket.title }}</h4>
      <p>{{ ticket.description }}</p>
    </div>

    <!-- Для кнопки удаления добавляем остановку всплытия клика,
         чтобы клик по кнопке не запускал открытие окна деталей -->
    <button
      class="delete-icon-btn"
      aria-label="Delete ticket"
      @click.stop="$emit('openDeleteConfirmation', ticket.id)"
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="16"
        height="16"
        fill="white"
        viewBox="0 0 24 24"
        stroke="white"
        stroke-width="2"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="M3 6h18M9 6v12a2 2 0 0 0 4 0V6m-4 0h4"
        />
      </svg>
    </button>
  </div>
</template>

  
  <script>
  export default {
    name: "TicketCard",
    props: {
      ticket: {
        type: Object,
        required: true,
      },
    },
  };
  </script>
  
  <style scoped>
  .ticket-card {
    position: relative;
    background-color: #fefefe;
    border-radius: 12px;
    box-shadow: 0 2px 10px rgb(0 0 0 / 0.1);
    padding: 18px 24px;
    margin-bottom: 20px;
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    user-select: none;
  }
  
  .ticket-content h4 {
    margin: 0 0 6px 0;
    font-size: 18px;
    font-weight: 700;
    color: #222;
  }
  
  .ticket-content p {
    margin: 0;
    font-size: 14px;
    color: #444;
    line-height: 1.3;
  }
  
  /* Кнопка удаления — маленький круг в левом нижнем углу */
  .delete-icon-btn {
    position: absolute;
    bottom: 12px;
    left: 12px;
    background-color: #d9534f;
    border: none;
    border-radius: 50%;
    width: 32px;
    height: 32px;
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: background-color 0.25s ease;
    box-shadow: 0 0 6px rgb(217 83 79 / 0.6);
  }
  
  .delete-icon-btn:hover {
    background-color: #c9302c;
  }
  
  .delete-icon-btn svg {
    pointer-events: none;
  }
  </style>
  