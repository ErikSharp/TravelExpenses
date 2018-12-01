import Axios from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    values: [],
    busy: false
  },
  mutations: {
    SET_VALUES(state, data) {
      state.values = data
    },
    SET_BUSY(state, busy) {
      state.busy = busy
    }
  },
  actions: {
    async retrieveValues({ commit }) {
      commit('SET_BUSY', true)
      commit('SET_VALUES', [])
      let response = await Axios.getValues()
      if (response) {
        commit('SET_VALUES', response.data)
      }
      commit('SET_BUSY', false)
    }
  }
}
