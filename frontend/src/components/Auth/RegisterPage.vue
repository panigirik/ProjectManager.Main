<template>
  <div class="auth-container">
    <h2>Register</h2>
    <el-form @submit.prevent="handleRegister" class="auth-form">
      <el-form-item label="Username">
        <el-input v-model="userName" placeholder="Enter username" />
      </el-form-item>
      <el-form-item label="Email">
        <el-input v-model="email" placeholder="Enter email" />
      </el-form-item>
      <el-form-item label="Password">
        <el-input v-model="password" type="password" placeholder="Enter password" />
      </el-form-item>
      <el-form-item label="Confirm Password">
        <el-input v-model="confirmPassword" type="password" placeholder="Confirm password" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" native-type="submit">Register</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import axios from 'axios';
import { v4 as uuidv4 } from 'uuid';

export default {
  name: 'RegisterPage',
  data() {
    return {
      userName: '',
      email: '',
      password: '',
      confirmPassword: '',
    };
  },
  methods: {
    async handleRegister() {
      if (!this.userName || !this.email || !this.password || !this.confirmPassword) {
        this.$message.error('All fields are required');
        return;
      }

      if (this.password !== this.confirmPassword) {
        this.$message.error('Passwords do not match');
        return;
      }

      const userId = uuidv4();

      try {
        const response = await axios.post('http://localhost:5258/api/Register/register', {
          userId,
          userName: this.userName,
          email: this.email,
          role: 1,
          password: this.password,
          boardIds: [],
        });

        // проверка, успешен ли ответ
        if (response.status >= 200 && response.status < 300) {
          this.$message.success('Registration successful');
          await this.$router.push('/dashboard');
        } else {
          // если статус не 2xx — это ошибка
          this.$message.error('Registration failed: unexpected server response');
        }
      } catch (error) {
        this.$message.error(
          'Registration failed: ' +
            (error.response?.data?.message || error.message || 'Unknown error')
        );
      }
    },
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
</style>
