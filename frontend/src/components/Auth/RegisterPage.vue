<template>
    <div class="register-page">
      <h2>Register</h2>
      <form @submit.prevent="register">
        <label for="email">Email:</label>
        <input id="email" v-model="email" type="email" required />
  
        <label for="password">Password:</label>
        <input id="password" v-model="password" type="password" required />
  
        <button type="submit">Register</button>
      </form>
    </div>
  </template>
  
  <script>
  export default {
    name: 'RegisterPage',
    data() {
      return {
        email: '',
        password: ''
      };
    },
    methods: {
      register() {
        // Добавь сюда логику регистрации
        console.log('Registering', this.email, this.password);
      }
    }
  };
  </script>
  
  <style scoped>
  .register-page {
    max-width: 400px;
    margin: auto;
    padding: 2rem;
  }
  </style>
  