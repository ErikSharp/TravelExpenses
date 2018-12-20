import AxiosService from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    busy: false
  },
  mutations: {
    SET_BUSY(state, busy) {
      state.busy = busy
    }
  },
  actions: {
    saveTransaction({ dispatch, commit }, transaction) {
      commit('SET_BUSY', true)

      return AxiosService.createTransaction(transaction)
        .then(() => {
          dispatch('showSaveMessage', `${transaction.title} has been saved`, {
            root: true
          })
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_BUSY', false)
        })
    }
  }
}