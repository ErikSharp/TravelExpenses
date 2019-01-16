import AxiosService from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    countries: []
  },
  mutations: {
    SET_COUNTRIES(state, countries) {
      state.countries = countries
    }
  },
  actions: {
    load({ dispatch, commit }) {
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
