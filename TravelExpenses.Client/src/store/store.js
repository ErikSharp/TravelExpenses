import Vue from 'vue'
import Vuex from 'vuex'
import router from '@/router'
import contact from '@/store/modules/user.js'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    contact
  },
  state: {
    authStep: 1,
    homeView: 'transactions'
  },
  mutations: {
    SET_AUTH_STEP(state, step) {
      state.authStep = step
    },
    SET_HOME_VIEW(state, view) {
      state.homeView = view
    }
  },
  actions: {
    setAuthStep({ commit }, step) {
      commit('SET_AUTH_STEP', step)
    },
    setHomeView({ commit }, view) {
      commit('SET_HOME_VIEW', view)
      router.push({ name: view })
    }
  }
})
