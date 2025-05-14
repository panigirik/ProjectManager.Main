import { createRouter, createWebHistory } from 'vue-router';
import Dashboard from '../views/MainDashboard.vue';
import LoginPage from '../views/LoginPage.vue';
import RegisterPage from '../components/Auth/RegisterPage.vue';
import BoardDetailsPage from '../components/Boards/BoardDetailsPage.vue';  

const routes = [
  { path: '/', name: 'Login', component: LoginPage },
  { path: '/register', name: 'RegisterPage', component: RegisterPage },
  { path: '/dashboard', name: 'MainDashboard', component: Dashboard },
  
  // Добавляем маршрут для отображения доски с параметром id
  { path: '/boards/:id', name: 'BoardDetails', component: BoardDetailsPage, props: true },

  // Добавьте другие маршруты по необходимости
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
