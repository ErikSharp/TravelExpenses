import AxiosService from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    currencies: []
  },
  mutations: {
    SET_CURRENCIES(state, currencies) {
      state.currencies = currencies
    }
  },
  actions: {
    load({ dispatch, commit }) {
      commit('SET_CURRENCIES', [])

      return AxiosService.getCurrencies()
        .then(response => {
          commit('SET_CURRENCIES', response.data)
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
    }
  }
}
