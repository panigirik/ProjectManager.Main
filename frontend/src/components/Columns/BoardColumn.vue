<template>
  <el-card shadow="always" style="position: relative; padding-top: 40px;">
    <!-- Dropdown в правом верхнем углу -->
    <el-dropdown 
      trigger="click" 
      @command="cmd => handleColumnCommand(cmd, column)"
      style="position: absolute; top: 10px; right: 10px;"
    >
      <span class="dropdown-icon">
        <el-icon><MoreFilled /></el-icon>
      </span>
      <template #dropdown>
        <el-dropdown-menu>
          <el-dropdown-item command="delete">Delete Column</el-dropdown-item>
          <el-dropdown-item command="sort">Sort Tickets by Date</el-dropdown-item>
        </el-dropdown-menu>
      </template>
    </el-dropdown>

    <!-- Название колонки -->
    <h3>{{ column.name }}</h3>

    <!-- Список тикетов с перетаскиванием -->
    <draggable v-model="tickets" @end="onDragEnd" item-key="id">
      <template #item="{ element }">
        <el-card class="mb-2" shadow="hover">
          <div>{{ element.title }}</div>
        </el-card>
      </template>
    </draggable>
  </el-card>
</template>

<script>
import draggable from 'vuedraggable';
import { ElMessage } from 'element-plus';
import { MoreFilled } from '@element-plus/icons-vue';

export default {
  components: {
    draggable,
    MoreFilled
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
      try {
        const response = await this.$axios.get(`/api/Tickets/ticket?columnId=${this.column.id}`);
        this.tickets = response.data;
      } catch (error) {
        ElMessage.error('Failed to load tickets');
      }
    },
    async onDragEnd(event) {
      const movedTicket = this.tickets[event.newIndex];
      try {
        await this.$axios.put('/api/Tickets/move-ticket', {
          ticketId: movedTicket.id,
          newColumnId: this.column.id
        });
        ElMessage.success('Ticket moved');
      } catch (error) {
        ElMessage.error('Failed to move ticket');
      }
    },
    handleColumnCommand(command, column) {
      if (command === 'delete') {
        this.$emit('delete-column', column.id);
      } else if (command === 'sort') {
        this.tickets.sort((a, b) => new Date(a.date) - new Date(b.date));
      }
    }
  }
};
</script>

<style scoped>
.mb-2 {
  margin-bottom: 8px;
}
</style>
