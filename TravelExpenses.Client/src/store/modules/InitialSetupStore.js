import Windows from '@/common/enums/InitialSetupWindows.js'
import Router from '@/router'
import * as HomeViews from '@/common/constants/HomeViews.js'
import { Promise } from 'bluebird'

const baseTitle = 'Manage Your Moo-lah'

export default {
  namespaced: true,
  state: {
    title: baseTitle,
    window: Windows.introduction,
    loaded: false
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
    SET_LOADED(state) {
      state.loaded = true
    }
  },
  actions: {
    checkBaseRequirements({ state, commit, dispatch }, callback) {
      if (!state.loaded) {
        let locations = dispatch('Location/load', null, { root: true })
        let keywords = dispatch('Keyword/load', null, { root: true })
        let categories = dispatch('Category/load', null, { root: true })

        Promise.all([locations, keywords, categories]).then(() => {
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
    missingBaseData: (state, getters, rootState) => {
      return (
        !rootState.Location.locations.length ||
        !rootState.Category.categories.length ||
        !rootState.Keyword.keywords.length
      )
    }
  }
}
