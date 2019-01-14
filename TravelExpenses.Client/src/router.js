import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/views/Home.vue'
import NotFound from '@/views/NotFound.vue'
import Authentication from '@/views/Authentication.vue'
import InitialSetup from '@/views/InitialSetup.vue'
import Transactions from '@/components/Transactions.vue'
import Reconcile from '@/components/reconcile/Reconcile.vue'
import CashWithdrawals from '@/components/CashWithdrawals.vue'
import Queries from '@/components/Queries.vue'
import Setup from '@/components/setup/Setup.vue'
import Store from '@/store/store.js'
import * as HomeViews from '@/common/constants/HomeViews.js'

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
          name: HomeViews.Transactions,
          path: '',
          component: Transactions
        }
      ]
    },
    {
      path: '/authentication',
      name: HomeViews.Authentication,
      component: Authentication
    },
    {
      path: '/cash-withdrawals',
      component: Home,
      children: [
        {
          name: HomeViews.CashWithdrawals,
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
          name: HomeViews.Reconcile,
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
          name: HomeViews.Queries,
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
          name: HomeViews.Setup,
          path: '',
          component: Setup
        }
      ]
    },
    {
      path: '/initial-setup',
      name: HomeViews.InitialSetup,
      component: InitialSetup
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
