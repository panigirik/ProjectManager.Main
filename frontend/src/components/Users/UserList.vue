<template>
  <el-table :data="users">
    <el-table-column prop="name" label="Name" />
    <el-table-column prop="role" label="Role" />
    <el-table-column>
      <template #default="scope">
        <el-button @click="editUser(scope.row)">Edit</el-button>
        <el-button type="danger" @click="deleteUser(scope.row.id)">Delete</el-button>
      </template>
    </el-table-column>
  </el-table>
</template>

<script>
export default {
  data() {
    return {
      users: []
    };
  },
  created() {
    this.fetchUsers();
  },
  methods: {
    async fetchUsers() {
      const response = await this.$axios.get('/api/Users/user');
      this.users = response.data;
    },
    editUser(user) {
      // Пример использования параметра user
      console.log(user);  // Здесь можно передать данные пользователя в форму или открыть модальное окно
    },
    deleteUser(id) {
      this.$axios.delete(`/api/Users/user/${id}`);
      this.fetchUsers();
    }
  }
};
</script>
