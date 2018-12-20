import AxiosService from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    busy: false,
    addCountryBusy: false,
    countries: []
  },
  mutations: {
    SET_BUSY(state, busy) {
      state.busy = busy
    },
    SET_ADD_COUNTRY_BUSY(state, busy) {
      state.addCountryBusy = busy
    },
    SET_COUNTRIES(state, countries) {
      state.countries = countries
    }
  },
  actions: {
    initialize({ dispatch, commit }) {
      commit('SET_COUNTRIES', [])
      commit('SET_BUSY', true)

      return AxiosService.getCountries()
        .then(response => {
          commit('SET_COUNTRIES', response.data)
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_BUSY', false)
        })
    },
    addCountry({ dispatch, commit }, newCountry) {
      commit('SET_ADD_COUNTRY_BUSY', true)

      return AxiosService.addCountry(newCountry)
        .then(() => {
          dispatch('showSaveMessage', `${newCountry} has been saved`, {
            root: true
          })
          dispatch('initialize')
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_ADD_COUNTRY_BUSY', false)
        })
    }
  }
}
