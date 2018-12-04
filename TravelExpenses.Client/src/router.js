/* eslint-disable no-console */
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
import Store from '@/store/store.js'

Vue.use(Router)

let myRouter = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
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
      path: '/authentication',
      name: 'authentication',
      component: Authentication
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

myRouter.beforeEach((to, from, next) => {
  let token = Store.state.Authentication.authToken

  if (to.name === 'authentication') {
    if (token) {
      next({ name: 'transactions' })
    } else {
      Store.dispatch('Authentication/checkLocalStorageForToken')
      if (Store.state.Authentication.authToken) {
        next({ name: 'transactions' })
      } else {
        next()
      }
    }
  } else {
    if (token) {
      next()
    } else {
      Store.dispatch('Authentication/checkLocalStorageForToken')
      if (Store.state.Authentication.authToken) {
        next()
      } else {
        next({ name: 'authentication' })
      }
    }
  }
})

export default myRouter
