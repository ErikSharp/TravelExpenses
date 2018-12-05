import LocalStorage from '@/services/LocalStorageService.js'
import Axios from '@/services/AxiosService.js'
import Router from '@/router'

export default {
  namespaced: true,
  state: {
    busy: false,
    authToken: '',
    persistToken: true
  },
  mutations: {
    SET_TOKEN(state, token) {
      state.authToken = token
    },
    SET_BUSY(state, busy) {
      state.busy = busy
    }
  },
  actions: {
    setToken({ commit }, token) {
      commit('SET_TOKEN', token)
      Router.push({ name: 'transactions' })
    },
    checkLocalStorageForToken({ dispatch }) {
      let token = LocalStorage.getToken()
      if (token) {
        dispatch('setToken', token)
      }
    },
    async login({ dispatch, state, commit }, details) {
      try {
        commit('SET_BUSY', true)
        let response = await Axios.login(details)
        if (state.persistToken) {
          LocalStorage.saveToken(response.data.token)
        }

        dispatch('setToken', response.data.token)
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
    },
    async registerUser({ dispatch, state, commit }, details) {
      try {
        commit('SET_BUSY', true)
        let response = await Axios.register(details)
        if (state.persistToken) {
          LocalStorage.saveToken(response.data.token)
        }

        dispatch('setToken', response.data.token)
      } catch (error) {
        // if (error.response) {
        // } else if (error.request) {
        // } else {
        // }
      } finally {
        commit('SET_BUSY', false)
      }
    },
    logout({ commit }) {
      commit('SET_TOKEN', null)
      LocalStorage.clearToken()
      Router.push({ name: 'authentication' })
    }
  }
}
