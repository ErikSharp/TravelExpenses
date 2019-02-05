import SetupWindow from '@/common/enums/SetupWindows.js'
import { firstLetterUpper } from '@/common/StringUtilities.js'

function initialState() {
  return {
    setupWindow: SetupWindow.navigation
  }
}

export default {
  namespaced: true,
  state: initialState,
  mutations: {
    RESET(state) {
      const s = initialState()
      Object.keys(s).forEach(key => {
        state[key] = s[key]
      })
    },
    SET_SETUP_WINDOW(state, window) {
      state.setupWindow = window
    }
  },
  actions: {
    setSetupWindow({ dispatch, commit, rootState }, window) {
      switch (window) {
        case SetupWindow.locations:
          dispatch('setTitle', 'Locations', { root: true })
          dispatch('Location/load', null, { root: true })
          dispatch('Country/load', null, { root: true })
          break
        case SetupWindow.keywords:
          dispatch('setTitle', 'Keywords', { root: true })
          dispatch('Keyword/load', null, { root: true })
          break
        case SetupWindow.categories:
          dispatch('setTitle', 'Categories', { root: true })
          dispatch('Category/load', null, { root: true })
          break
        case SetupWindow.navigation:
          dispatch('setTitle', firstLetterUpper(rootState.homeView), {
            root: true
          })
          break
      }
      commit('SET_SETUP_WINDOW', window)
    }
  }
}
