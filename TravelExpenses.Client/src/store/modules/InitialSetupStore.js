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
    },
    RESET(state) {
      state.loaded = false
      state.title = baseTitle
      state.window = Windows.introduction
    }
  },
  actions: {
    reset({ commit }) {
      commit('RESET')
    },
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
    nextWindow({ commit, state, rootState }) {
      switch (state.window) {
        case Windows.introduction:
          if (!rootState.Location.locations.length) {
            commit('SET_WINDOW', Windows.location)
          } else if (!rootState.Category.categories.length) {
            commit('SET_WINDOW', Windows.categories1)
          } else {
            commit('SET_WINDOW', Windows.keywords1)
          }
          break
        case Windows.location:
          if (!rootState.Category.categories.length) {
            commit('SET_WINDOW', Windows.categories1)
          } else if (!rootState.Keyword.keywords.length) {
            commit('SET_WINDOW', Windows.keywords1)
          } else {
            commit('SET_WINDOW', Windows.finish)
          }
          break
        case Windows.categories1:
          commit('SET_WINDOW', Windows.categories2)
          break
        case Windows.categories2:
          if (!rootState.Keyword.keywords.length) {
            commit('SET_WINDOW', Windows.keywords1)
          } else {
            commit('SET_WINDOW', Windows.finish)
          }
          break
        case Windows.keywords1:
          commit('SET_WINDOW', Windows.keywords2)
          break
        case Windows.keywords2:
          commit('SET_WINDOW', Windows.finish)
          break
        case Windows.finish:
          Router.push({ name: HomeViews.Transactions })
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
