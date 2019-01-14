import Windows from '@/common/enums/InitialSetupWindows.js'
import AxiosService from '@/services/AxiosService.js'
import Router from '@/router'
import * as HomeViews from '@/common/constants/HomeViews.js'

const baseTitle = 'Manage Your Moo-lah'

export default {
  namespaced: true,
  state: {
    title: baseTitle,
    window: Windows.introduction,
    loaded: false,
    hasLocation: false,
    hasCategory: false,
    hasKeyword: false
  },
  mutations: {
    SET_WINDOW(state, window) {
      switch (window) {
        case Windows.introduction:
        case Windows.finish:
          state.title = baseTitle
          break
        default:
          state.title = 'Initial Setup'
          break
      }

      state.window = window
    },
    SET_HAS_LOCATION(state, hasLocation) {
      state.hasLocation = hasLocation
    },
    SET_HAS_CATEGORY(state, hasCategory) {
      state.hasCategory = hasCategory
    },
    SET_HAS_KEYWORD(state, hasKeyword) {
      state.hasKeyword = hasKeyword
    },
    SET_LOADED(state) {
      state.loaded = true
    },
    SET_TITLE(state, title) {
      state.title = title
    }
  },
  actions: {
    checkBaseRequirements({ state, commit }) {
      let missingBaseData =
        state.hasLocation && state.hasCategory && state.hasKeyword

      if (!state.loaded || missingBaseData) {
        let requests = []
        requests.push(
          AxiosService.getBaseRequirements().then(response => {
            commit('SET_HAS_LOCATION', response.data.hasLocation)
            commit('SET_HAS_CATEGORY', response.data.hasCategory)
            commit('SET_HAS_KEYWORD', response.data.hasKeyword)
          })
        )
      }
    },
    setWindow({ commit }, window) {
      switch (window) {
        case Windows.transactions:
          Router.push({ name: HomeViews.Transactions })
          break
        default:
          commit('SET_WINDOW', window)
          break
      }
    }
  }
}
