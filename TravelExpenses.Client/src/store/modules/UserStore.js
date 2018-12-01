import AxiosService from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    contacts: []
  },
  mutations: {
    SET_CONTACTS(state, contacts) {
      state.contacts = contacts
    }
  },
  actions: {
    getContacts({ dispatch, commit }) {
      dispatch('setBusy', true, { root: true })

      return AxiosService.getContacts()
        .then(response => {
          dispatch('setBusy', false, { root: true })
          commit('SET_CONTACTS', response.data)
        })
        .catch(error => {
          dispatch('setBusy', false, { root: true })
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
    }
  }
}
