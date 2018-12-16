import Vue from 'vue'
import Vuex from 'vuex'
import Router from '@/router'
import Authentication from '@/store/modules/AuthenticationStore.js'
import Values from '@/store/modules/ValuesStore.js'
import Reconcile from '@/store/modules/ReconcileStore.js'
import SetupData from '@/store/modules/SetupDataStore.js'
import Transaction from '@/store/modules/TransactionStore.js'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    Authentication,
    Values,
    Reconcile,
    SetupData,
    Transaction
  },
  state: {
    homeView: 'transactions',
    snackbar: {
      show: false,
      color: '',
      mode: '',
      timeout: 3000,
      message: ''
    }
  },
  mutations: {
    SET_HOME_VIEW(state, view) {
      state.homeView = view
    },
    SET_SNACKBAR_DETAILS(state, snackbar) {
      //'primary', success', 'info', 'error', 'cyan darken-2'
      state.snackbar.color = snackbar.color || 'primary'
      state.snackbar.message = snackbar.message || 'No message provided'
      state.snackbar.mode = snackbar.mode || ''
      state.snackbar.timeout = snackbar.timeout || 3000
      state.snackbar.show = true
    }
  },
  actions: {
    setHomeView({ commit }, view) {
      commit('SET_HOME_VIEW', view)
      Router.push({ name: view })
    },
    showSnackbar({ commit }, snackbar) {
      commit('SET_SNACKBAR_DETAILS', snackbar)
    }
  }
})
