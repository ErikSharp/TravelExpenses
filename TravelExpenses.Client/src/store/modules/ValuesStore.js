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
    async retrieveValues({ commit, dispatch }) {
      commit('SET_BUSY', true)
      commit('SET_VALUES', [])

      try {
        let response = await Axios.getValues()
        commit('SET_VALUES', response.data)
        dispatch(
          'showSnackbar',
          { message: 'Whatsup my ninjas', color: 'primary darken-3' },
          { root: true }
        )
      } catch (error) {
        if (error.response) {
          dispatch(
            'showSnackbar',
            {
              message:
                'The request was made and the server responded with a status code that falls out of the range of 2xx',
              mode: 'multi-line',
              color: 'error'
            },
            { root: true }
          )
        } else if (error.request) {
          dispatch(
            'showSnackbar',
            {
              message: 'The request was made but no response was received',
              color: 'error'
            },
            { root: true }
          )
          // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
          // http.ClientRequest in node.js
        } else {
          dispatch(
            'showSnackbar',
            {
              message:
                'Something happened in setting up the request that triggered an Error',
              color: 'error'
            },
            { root: true }
          )
        }
      } finally {
        commit('SET_BUSY', false)
      }
    }
  }
}
