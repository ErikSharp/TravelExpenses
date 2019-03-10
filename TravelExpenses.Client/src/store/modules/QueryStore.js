import AxiosService from '@/services/AxiosService.js'

function initialState() {
  return {
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
    SET_QUERY_WINDOW(state, window) {
      state.queryWindow = window
    }
  },
  actions: {
    setQueryWindow({ commit }, window) {
      commit('SET_QUERY_WINDOW', window)
    }
  }
}
