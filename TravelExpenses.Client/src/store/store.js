import Vue from 'vue'
import Vuex from 'vuex'
import contact from '@/store/modules/user.js'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    contact
  },
  state: {
    authStep: 1
  },
  mutations: {
    SET_AUTH_STEP(state, step) {
      state.authStep = step
    }
  },
  actions: {
    setAuthStep({ commit }, step) {
      commit('SET_AUTH_STEP', step)
    }
  }
})
