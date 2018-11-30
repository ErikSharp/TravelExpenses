import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/views/Home.vue'
import NotFound from '@/views/NotFound.vue'
import Authentication from '@/views/Authentication.vue'
import Transactions from '@/components/Transactions.vue'
import Reconcile from '@/components/Reconcile.vue'
import CashWithdrawals from '@/components/CashWithdrawals.vue'
import Queries from '@/components/Queries.vue'
import Setup from '@/components/Setup.vue'

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
      path: '/transactions',
      component: Home,
      children: [
        {
          name: 'transactions',
          path: '',
          component: Transactions
        }
      ]
    },
    {
      path: '/cash-withdrawals',
      component: Home,
      children: [
        {
          name: 'cashWithdrawals',
          path: '',
          component: CashWithdrawals
        }
      ]
    },
    {
      path: '/reconcile',
      component: Home,
      children: [
        {
          name: 'reconcile',
          path: '',
          component: Reconcile
        }
      ]
    },
    {
      path: '/queries',
      component: Home,
      children: [
        {
          name: 'queries',
          path: '',
          component: Queries
        }
      ]
    },
    {
      path: '/setup',
      component: Home,
      children: [
        {
          name: 'setup',
          path: '',
          component: Setup
        }
      ]
    },
    {
      path: '*',
      component: NotFound
    }
  ]
})
