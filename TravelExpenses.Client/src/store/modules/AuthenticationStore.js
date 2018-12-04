/* eslint-disable no-console */
import LocalStorage from '@/services/LocalStorageService.js'
import Axios from '@/services/AxiosService.js'
import Router from '@/router'

export default {
  namespaced: true,
  state: {
    authToken: '',
    persistToken: true
  },
  mutations: {
    SET_TOKEN(state, token) {
      state.authToken = token
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
    async login({ dispatch, state }, details) {
      try {
        console.log(details)
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
          console.log(error.response.data)
          console.log(error.response.status)
          console.log(error.response.headers)
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
          console.log(error.request)
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

          console.log('Error', error.message)
        }
      }
    },
    // eslint-disable-next-line no-unused-vars
    async registerUser({ dispatch, state }, details) {
      try {
        let response = await Axios.register(details)
        if (state.persistToken) {
          LocalStorage.saveToken(response.data.token)
        }

        dispatch('setToken', response.data.token)
      } catch (error) {
        if (error.response) {
          console.log('server says')
          console.log(error.response)
        } else if (error.request) {
          console.log('no response from the server')
        } else {
          console.log('error setting up the request')
        }
      }
    },
    logout({ commit }) {
      commit('SET_TOKEN', null)
      LocalStorage.clearToken()
      Router.push({ name: 'authentication' })
    }
  }
}
