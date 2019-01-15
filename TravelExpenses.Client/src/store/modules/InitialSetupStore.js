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
    baseData: {
      hasLocation: false,
      hasCategory: false,
      hasKeyword: false
    }
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
    SET_BASE_DATA(state, baseData) {
      state.baseData = baseData
    },
    SET_LOADED(state) {
      state.loaded = true
    }
  },
  actions: {
    checkBaseRequirements({ state, commit }, callback) {
      let missingBaseData =
        state.hasLocation && state.hasCategory && state.hasKeyword

      if (!state.loaded || missingBaseData) {
        AxiosService.getBaseRequirements().then(response => {
          commit('SET_BASE_DATA', response.data)
          commit('SET_LOADED')
          callback()
        })
      } else {
        callback()
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
  },
  getters: {
    missingBaseData: state => {
      return !state.hasLocation || !state.hasCategory || !state.hasKeyword
    }
  }
}
