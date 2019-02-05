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
    checkLocalStorageForToken({ commit }) {
      let token = LocalStorage.getToken()
      if (token) {
        commit('SET_TOKEN', token)
      }
    },
    async login({ dispatch, state, commit }, details) {
      try {
        commit('SET_BUSY', true)
        let response = await Axios.login(details)
        if (state.persistToken) {
          LocalStorage.saveToken(response.data.token)
        }

        commit('SET_TOKEN', response.data.token)
        Router.push({ name: 'transactions' })
      } catch (error) {
        if (error.response) {
          dispatch(
            'showSnackbar',
            {
              message:
                'The user credentials are incorrect or the user is disabled',
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
    async registerUser({ state, commit }, details) {
      try {
        commit('SET_BUSY', true)
        let response = await Axios.register(details)
        if (state.persistToken) {
          LocalStorage.saveToken(response.data.token)
        }

        commit('SET_TOKEN', response.data.token)
        Router.push({ name: 'transactions' })
      } catch (error) {
        // if (error.response) {
        // } else if (error.request) {
        // } else {
        // }
      } finally {
        commit('SET_BUSY', false)
      }
    },
    logout({ commit, dispatch }) {
      commit('SET_TOKEN', null)
      LocalStorage.clearToken()
      dispatch('InitialSetup/reset', null, { root: true })
      Router.push({ name: 'authentication' })
    }
  },
  getters: {
    userId: state => {
      if (state.authToken) {
        let base64Url = state.authToken.split('.')[1]
        let base64 = base64Url.replace('-', '+').replace('_', '/')
        return +JSON.parse(window.atob(base64)).userId
      } else {
        return 0
      }
    }
  }
}
