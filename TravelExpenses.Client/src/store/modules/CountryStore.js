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
    setCountries({ commit }, countries) {
      commit('SET_COUNTRIES', countries)
    },
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
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
    }
  },
  getters: {
    findCountry: state => id => {
      return state.countries.find(c => c.id === id)
    }
  }
}
