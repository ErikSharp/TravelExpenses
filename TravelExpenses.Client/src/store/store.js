import Vue from 'vue'
import Vuex from 'vuex'
import contact from '@/store/modules/user.js'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    contact
  },
  state: {
    error: '',
    busy: false
  },
  mutations: {
    SET_ERROR(state, error) {
      state.error = error
    },
    SET_BUSY(state, busy) {
      state.busy = busy
    }
  },
  actions: {
    setError({ commit }, error) {
      commit('SET_ERROR', error)
    },
    setBusy({ commit }, busy) {
      commit('SET_BUSY', busy)
    }
  }
})
