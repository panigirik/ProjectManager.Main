import { createRouter, createWebHistory } from 'vue-router';
import LoginPage from '../views/LoginPage.vue';
import RegisterPage from '../components/Auth/RegisterPage.vue';
import Dashboard from '../views/MainDashboard.vue';
import BoardDetailsPage from '../components/Boards/BoardDetailsPage.vue';
import BoardList from '../components/Boards/BoardList.vue';

const routes = [
  { path: '/', name: 'Login', component: LoginPage },
  { path: '/register', name: 'Register', component: RegisterPage },
  { path: '/dashboard', name: 'Dashboard', component: Dashboard },
  { path: '/boards/:id', name: 'BoardDetails', component: BoardDetailsPage, props: true },
  { path: '/boards', name: 'BoardList', component: BoardList }
  
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
