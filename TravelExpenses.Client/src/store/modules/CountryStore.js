import AxiosService from '@/services/AxiosService.js'

function initialState() {
  return {
    countries: []
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
    SET_COUNTRIES(state, countries) {
      state.countries = countries
    }
  },
  actions: {
    load({ state, dispatch, commit }) {
      if (state.countries.length) {
        return
      }

      commit('SET_COUNTRIES', [])

      return AxiosService.getCountries()
        .then(response => {
          commit('SET_COUNTRIES', response.data)
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
    }
  }
}
