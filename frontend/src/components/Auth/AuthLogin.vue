<template>
  <el-form @submit.prevent="handleLogin" class="login-form">
    <el-input v-model="email" placeholder="Email" />
    <el-input v-model="password" type="password" placeholder="Password" />
    <el-button type="primary" native-type="submit">Login</el-button>
    <div class="register-link">
      <router-link to="/register">Зарегистрироваться</router-link>
    </div>
  </el-form>
</template>

<script>
export default {
  data() {
    return {
      email: '',
      password: ''
    };
  },
  methods: {
    async handleLogin() {
      try {
        const response = await this.$axios.post('/login', {
          email: this.email,
          password: this.password
        });
        this.$store.commit('setToken', response.data.token);
        this.$router.push('/dashboard');
      } catch (error) {
        this.$message.error('Login failed');
      }
    }
  }
};
</script>

<style scoped>
.register-link {
  margin-top: 10px;
  text-align: center;
}
.register-link a {
  color: #409EFF; /* Синий цвет */
  font-size: 14px;
  text-decoration: none;
}
.register-link a:hover {
  text-decoration: underline;
}
</style>
