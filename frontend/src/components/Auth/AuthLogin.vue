<template>
    <el-form @submit.prevent="handleLogin">
      <el-input v-model="email" placeholder="Email" />
      <el-input v-model="password" type="password" placeholder="Password" />
      <el-button type="primary" native-type="submit">Login</el-button>
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
  