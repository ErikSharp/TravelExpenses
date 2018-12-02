import Vue from 'vue'
import Vuex from 'vuex'
import Router from '@/router'
import User from '@/store/modules/UserStore.js'
import Authentication from '@/store/modules/AuthenticationStore.js'
import Values from '@/store/modules/ValuesStore.js'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    User,
    Authentication,
    Values
  },
  state: {
    authStep: 1,
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
    SET_AUTH_STEP(state, step) {
      state.authStep = step
    },
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
    setAuthStep({ commit }, step) {
      commit('SET_AUTH_STEP', step)
    },
    setHomeView({ commit }, view) {
      commit('SET_HOME_VIEW', view)
      Router.push({ name: view })
    },
    showSnackbar({ commit }, snackbar) {
      commit('SET_SNACKBAR_DETAILS', snackbar)
    }
  }
})
