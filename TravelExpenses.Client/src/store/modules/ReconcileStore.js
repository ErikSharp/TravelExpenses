import AxiosService from '@/services/AxiosService.js'

function initialState() {
  return {
    reconcileWindowId: 0,
    cashOnHand: 0,
    reconcileBusy: false,
    currency: {},
    location: {},
    reconcileSummary: {}
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
    SET_RECONCILE_WINDOW_ID(state, id) {
      state.reconcileWindowId = id
    },
    SET_CASH_ON_HAND(state, amount) {
      state.cashOnHand = amount
    },
    SET_RECONCILE_BUSY(state, busy) {
      state.reconcileBusy = busy
    },
    SET_CURRENCY(state, currency) {
      state.currency = currency
    },
    SET_LOCATION(state, location) {
      state.location = location
    },
    SET_RECONCILE_SUMMARY(state, summary) {
      state.reconcileSummary = summary
    }
  },
  actions: {
    setReconcileWindowId({ commit }, id) {
      commit('SET_RECONCILE_WINDOW_ID', id)
    },
    setCurrency({ commit }, currency) {
      commit('SET_CURRENCY', currency)
    },
    setLocation({ commit }, location) {
      commit('SET_LOCATION', location)
    },
    setCashOnHand({ commit }, amount) {
      commit('SET_CASH_ON_HAND', amount)
    },
    getReconcileSummary({ dispatch, commit, state }, callback) {
      commit('SET_RECONCILE_BUSY', true)

      return AxiosService.getReconcileSummary(
        state.location.id,
        state.currency.id
      )
        .then(response => {
          commit('SET_RECONCILE_SUMMARY', response.data)

          //only go forward if we got data back
          callback()
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_RECONCILE_BUSY', false)
        })
    }
  }
}
