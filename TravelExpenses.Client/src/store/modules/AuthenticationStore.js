import Router from '@/router'
import LocalStorage from '@/services/LocalStorageService.js'

export default {
  namespaced: true,
  state: {
    jsonWebToken: ''
  },
  mutations: {
    SET_TOKEN(state, token) {
      if (token) {
        state.jsonWebToken = token
      }
    }
  },
  actions: {
    setToken({ commit }, token) {
      commit('SET_TOKEN', token)
      Router.push({ name: 'transactions' })
    },
    getToken({ dispatch }) {
      let token = LocalStorage.getToken()

      if (token) {
        dispatch('setToken', token)
      } else {
        Router.push({ name: 'authentication' })
      }
    }
  },
  getters: {
    isAuthenticated: state => {
      return !!state.jsonWebToken
    }
  }
}
