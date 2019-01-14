//import AxiosService from '@/services/AxiosService.js'
import Windows from '@/common/enums/InitialSetupWindows.js'
import Axios from '@/services/AxiosService.js'
import Router from '@/router'
import * as HomeViews from '@/common/constants/HomeViews.js'

export default {
  namespaced: true,
  state: {
    title: 'Manage Your Moo-lah',
    window: Windows.introduction,
    loaded: false,
    hasLocation: false,
    hasCurrency: false,
    hasCategory: false,
    hasKeyword: false
  },
  mutations: {
    SET_WINDOW(state, window) {
      state.window = window
    },
    SET_HAS_LOCATION(state, hasLocation) {
      state.hasLocation = hasLocation
    },
    SET_LOADED(state) {
      state.loaded = true
    }
  },
  actions: {
    checkBaseRequirements({ state, commit }) {
      if (!state.loaded) {
        let requests = []
        requests.push(
          Axios.getLocations().then(response => {
            commit('SET_HAS_LOCATION', !!response.data)
          })
        )

        Axios.all(requests).then(() => {
          console.log('They have all completed')
          commit('SET_LOADED')
        })
      }

      let missingBaseData =
        state.hasLocation &&
        state.hasCurrency &&
        state.hasCategory &&
        state.hasKeyword

      if (missingBaseData) {
        commit('SET_WINDOW', Windows.introduction)
      }
    },
    setWindow({ commit }, window) {
      if (window === Windows.transactions) {
        Router.push({ name: HomeViews.Transactions })
      } else {
        commit('SET_WINDOW', window)
      }
    }
  }
}
