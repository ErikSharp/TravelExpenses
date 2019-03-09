import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/views/Home.vue'
import NotFound from '@/views/NotFound.vue'
import Authentication from '@/views/Authentication.vue'
import InitialSetup from '@/views/InitialSetup.vue'
import Transactions from '@/components/transaction/Transactions.vue'
import Reconcile from '@/components/reconcile/Reconcile.vue'
import CashWithdrawals from '@/components/cashWithdrawal/CashWithdrawals.vue'
import Queries from '@/components/query/Queries.vue'
import Setup from '@/components/setup/Setup.vue'
import Store from '@/store/store.js'
import * as HomeViews from '@/common/constants/HomeViews.js'
import { routerGuard } from '@/routerGuard.js'

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

let getToken = callback => {
  let token = Store.state.Authentication.authToken

  if (token) {
    callback(token)
  } else {
    Store.dispatch('Authentication/checkLocalStorageForToken').then(() => {
      callback(Store.state.Authentication.authToken)
    })
  }
}

myRouter.beforeEach((to, from, next) => {
  routerGuard(to, to => next(to), () => next(), getToken, callback => {
    if (!Store.state.InitialSetup.loaded) {
      Store.dispatch('InitialSetup/checkBaseRequirements').then(() => {
        if (!Store.state.User.user) {
          Store.dispatch('Authentication/logout')
        } else {
          callback(Store.getters['InitialSetup/missingBaseData'])
        }
      })
    } else {
      callback(Store.getters['InitialSetup/missingBaseData'])
    }
  })
})

export default myRouter
