<template>
  <div class="auth-container">
    <h2>Login</h2>
    <el-form @submit.prevent="handleLogin" class="auth-form">
      <el-form-item label="Email">
        <el-input v-model="email" placeholder="Enter your email" />
      </el-form-item>
      <el-form-item label="Password">
        <el-input v-model="password" type="password" placeholder="Enter your password" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" native-type="submit" :loading="loading">Login</el-button>
      </el-form-item>
    </el-form>

    <div class="register-link">
      <p>Don't have an account?</p>
      <el-button type="text" @click="goToRegister">Register here</el-button>
    </div>
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
      loading: false,
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
          password: this.password,
        });

        const { accessToken, refreshToken } = response.data;
        localStorage.setItem('accessToken', accessToken);
        localStorage.setItem('refreshToken', refreshToken);

        this.$message.success('Login successful');
        this.$router.push('/dashboard');
      } catch (error) {
        this.$message.error('Login failed: ' + (error.response?.data?.message || 'Unknown error'));
      } finally {
        this.loading = false;
      }
    },
    goToRegister() {
      this.$router.push('/register');
    }
  },
};
</script>

<style scoped>
.auth-container {
  max-width: 400px;
  margin: 100px auto;
  padding: 2rem;
  border: 1px solid #ddd;
  border-radius: 8px;
  background-color: #fff;
}

.register-link {
  margin-top: 1rem;
  text-align: center;
}

.register-link p {
  margin-bottom: 0.5rem;
  font-size: 14px;
}

.register-link .el-button {
  padding: 0;
  font-size: 14px;
  color: #409EFF;
}
</style>
