import Axios from '@/services/AxiosService.js'

function initialState() {
  return {
    user: {}
  }
}

export default {
  namespaced: true,
  state: initialState,
  mutations: {
    SET_USER(state, user) {
      state.user = user
    }
  },
  actions: {
    getUser({ commit, dispatch }) {
      return Axios.getUser()
        .then(response => {
          commit('SET_USER', response.data)
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error)
        })
    }
  }
}
