import { timeout } from 'q'

function initialState() {
  return {
    reconcileWindowId: 0,
    cashOnHand: 0,
    reconcileBusy: false,
    currency: {},
    location: {}
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
    getReconcileSummary({ commit }, callback) {
      commit('SET_RECONCILE_BUSY', true)
      setTimeout(() => {
        callback()
      }, 3000)
    }
  }
}
