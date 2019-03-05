import Axios from '@/services/AxiosService.js'

function initialState() {
  return {
    user: {},
    preferences: {}
  }
}

export default {
  namespaced: true,
  state: initialState,
  mutations: {
    SET_USER(state, user) {
      state.user = user
    },
    SET_PREFERENCES(state, preferences) {
      state.preferences = preferences
    },
    SET_SHOW_RECONCILE_INSTRUCTIONS(state, show) {
      state.preferences.ShowReconcileInstructions = show
    }
  },
  actions: {
    getUser({ commit, dispatch }) {
      return Axios.getUser()
        .then(response => {
          commit('SET_USER', response.data)

          if (response.data.preferences) {
            commit('SET_PREFERENCES', JSON.parse(response.data.preferences))
          }
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
    },
    savePreferences({ state, commit, dispatch }) {
      return Axios.savePreferences(state.preferences)
        .then(response => {
          commit('SET_USER', response.data)

          if (response.data.preferences) {
            commit('SET_PREFERENCES', JSON.parse(response.data.preferences))
          }
        })
        .catch(error => {
          console.log(error)
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
    },
    setShowReconcileInstructions({ commit, dispatch }, show) {
      commit('SET_SHOW_RECONCILE_INSTRUCTIONS', show)
      dispatch('savePreferences')
    }
  },
  getters: {
    showReconcileInstructions: state => {
      return state.user.preferences.ShowReconcileInstructions
    }
  }
}
