import Windows from '@/common/enums/InitialSetupWindows.js'
import Router from '@/router'
import * as HomeViews from '@/common/constants/HomeViews.js'
import AxiosService from '@/services/AxiosService.js'

function initialState() {
  return {
    title: 'Manage Your Moo-lah',
    window: Windows.introduction,
    loaded: false
  }
}

export default {
  namespaced: true,
  state: initialState,
  mutations: {
    SET_WINDOW(state, window) {
      switch (window) {
        case Windows.introduction:
        case Windows.finish:
          state.title = initialState().title
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
      const s = initialState()
      Object.keys(s).forEach(key => {
        state[key] = s[key]
      })
    }
  },
  actions: {
    getBaseData({ state, commit, dispatch }) {
      if (!state.loaded) {
        return AxiosService.getBaseData()
          .then(response => {
            dispatch('Location/setLocations', response.data.locations, {
              root: true
            })
            dispatch('Keyword/setKeywords', response.data.keywords, {
              root: true
            })
            dispatch('Category/setCategories', response.data.categories, {
              root: true
            })
            dispatch('User/setUser', response.data.user, { root: true })
            dispatch('Country/setCountries', response.data.countries, {
              root: true
            })
            dispatch('Currency/setCurrencies', response.data.currencies, {
              root: true
            })

            commit('SET_LOADED')
          })
          .catch(error => {
            dispatch('showAxiosErrorMessage', error, { root: true })
          })
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
