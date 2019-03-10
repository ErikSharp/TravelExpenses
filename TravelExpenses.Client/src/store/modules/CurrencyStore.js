import AxiosService from '@/services/AxiosService.js'

function initialState() {
  return {
    currencies: []
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
    SET_CURRENCIES(state, currencies) {
      state.currencies = currencies
    }
  },
  actions: {
    setCurrencies({ commit }, currencies) {
      commit('SET_CURRENCIES', currencies)
    },
    load({ dispatch, commit }) {
      commit('SET_CURRENCIES', [])

      return AxiosService.getCurrencies()
        .then(response => {
          commit('SET_CURRENCIES', response.data)
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
    }
  },
  getters: {
    findCurrency: state => id => {
      return state.currencies.find(c => c.id === id)
    }
  }
}
