import AxiosService from '@/services/AxiosService.js'

function initialState() {
  return {
    currency: null,
    queryWindow: 0
  }
}

export default {
  namespaced: true,
  state: initialState,
  mutations: {
    RESET(state) {
      const s = initialState()
      Object.keys(s).forEach(key => {
        state[key] = s[key]
      })
    },
    SET_CURRENCY(state, currency) {
      state.currency = currency
    },
    SET_QUERY_WINDOW(state, window) {
      state.queryWindow = window
    }
  },
  actions: {
    setCurrency({ commit }, currency) {
      commit('SET_CURRENCY', currency)
    },
    setQueryWindow({ commit }, window) {
      commit('SET_QUERY_WINDOW', window)
    }
  }
}
