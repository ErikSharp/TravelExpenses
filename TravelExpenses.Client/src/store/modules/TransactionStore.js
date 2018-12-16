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
          dispatch(
            'showSnackbar',
            {
              message: `${transaction.title} has been saved`,
              color: 'primary darken-3'
            },
            { root: true }
          )
        })
        .catch(error => {
          if (error.response) {
            dispatch(
              'setError',
              'The request was made and the server responded with a status code that falls out of the range of 2xx'
            )
          } else if (error.request) {
            dispatch(
              'setError',
              'The request was made but no response was received',
              { root: true }
            )
            // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
            // http.ClientRequest in node.js
          } else {
            dispatch(
              'setError',
              'Something happened in setting up the request that triggered an Error',
              { root: true }
            )
          }
        })
        .then(() => {
          commit('SET_BUSY', false)
        })
    }
  }
}
