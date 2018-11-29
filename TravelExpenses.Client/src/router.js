import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/views/Home.vue'
import NotFound from '@/views/NotFound.vue'
import Authentication from '@/views/Authentication.vue'

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home
    },
    {
      path: '/authentication',
      name: 'authentication',
      component: Authentication
    },
    {
      path: '/cash-withdrawals',
      name: 'cashWithdrawals',
      component: Home
    },
    {
      path: '/reconcile',
      name: 'reconcile',
      component: Home
    },
    {
      path: '/queries',
      name: 'queries',
      component: Home
    },
    {
      path: '/settings',
      name: 'settings',
      component: Home
    },
    {
      path: '*',
      component: NotFound
    }
  ]
})
