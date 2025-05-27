import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import { createPinia } from 'pinia';

import ElementPlus from 'element-plus';
import 'element-plus/dist/index.css'; // базовые стили Element Plus
import 'element-plus/theme-chalk/dark/css-vars.css'; // тёмная тема Element Plus

import * as ElementPlusIconsVue from '@element-plus/icons-vue';
import './assets/global.css'; // твои кастомные стили (создай этот файл сам)

import axios from 'axios';

// Создание Vue-приложения
const app = createApp(App);

// Подключение библиотек
app.use(createPinia());
app.use(router);
app.use(ElementPlus);

// Регистрация иконок Element Plus глобально
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
  app.component(key, component);
}

// Axios interceptor для автоматической подстановки токена
axios.interceptors.request.use(config => {
  const token = localStorage.getItem('accessToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, error => {
  return Promise.reject(error);
});

// Монтирование
app.mount('#app');
