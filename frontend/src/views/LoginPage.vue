<template>
  <div class="login-container">
    <h2>Login Page</h2>
    <el-form @submit.prevent="handleLogin" label-position="top" class="login-form">
      <el-form-item label="Email">
        <el-input v-model="email" placeholder="Enter your email" />
      </el-form-item>
      <el-form-item label="Password">
        <el-input v-model="password" type="password" placeholder="Enter your password" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="handleLogin" :loading="loading">Login</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'LoginPage',
  data() {
    return {
      email: '',
      password: '',
      loading: false
    };
  },
  methods: {
    async handleLogin() {
      if (!this.email || !this.password) {
        this.$message.error('Email and password are required');
        return;
      }

      try {
        this.loading = true;

        const response = await axios.post('http://localhost:5258/login', {
          email: this.email,
          password: this.password
        });

        const { accessToken, refreshToken } = response.data;

        localStorage.setItem('accessToken', accessToken);
        localStorage.setItem('refreshToken', refreshToken);

        this.$message.success('Login successful');
        this.$router.push('/dashboard');

      } catch (error) {
        if (error.response && error.response.data) {
          this.$message.error(`Login failed: ${error.response.data.message || error.response.statusText}`);
        } else {
          this.$message.error('Login failed: Network or server error');
        }
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style scoped>
.login-container {
  max-width: 400px;
  margin: 100px auto;
  padding: 2rem;
  border: 1px solid #ddd;
  border-radius: 8px;
  background-color: #fff;
}

.login-form {
  display: flex;
  flex-direction: column;
}
</style>
