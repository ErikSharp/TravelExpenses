import AxiosService from '@/services/AxiosService.js'
import round from 'lodash/round'
import { toLocaleStringWithEndingZero } from '@/common/StringUtilities.js'

function initialState() {
  return {
    reconcileWindowId: 0,
    cashOnHand: 0,
    reconcileBusy: false,
    currency: null,
    reconcileSummary: {
      totalSpent: 0,
      totalWithdrawn: 0,
      totalLossGain: 0,
      lastTransactionDay: ''
    }
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
    setCashOnHand({ commit }, amount) {
      commit('SET_CASH_ON_HAND', amount)
    },
    getReconcileSummary({ dispatch, commit, state }) {
      if (!state.currency) {
        return
      }

      commit('SET_RECONCILE_BUSY', true)

      return AxiosService.getReconcileSummary(state.currency.id)
        .then(response => {
          if (!response.data.totalWithdrawn) {
            dispatch(
              'showSnackbar',
              {
                message: `There are no cash withdrawal records for ${
                  state.currency.currencyName
                }`,
                color: 'red'
              },
              { root: true }
            )
          } else {
            commit('SET_RECONCILE_SUMMARY', response.data)
          }
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_RECONCILE_BUSY', false)
        })
    }
  },
  getters: {
    cashShouldBe: state => {
      return (
        state.reconcileSummary.totalWithdrawn -
        state.reconcileSummary.totalSpent
      )
    },
    difference: (state, getters) => {
      return round(state.cashOnHand - getters.cashShouldBe, 3)
    },
    amountOut: (state, getters) => {
      return Math.abs(getters.difference + state.reconcileSummary.totalLossGain)
    },
    amountOutString: (state, getters) => {
      return toLocaleStringWithEndingZero(getters.amountOut)
    },
    haveNetGain: (state, getters) => {
      return getters.difference + state.reconcileSummary.totalLossGain > 0
    },
    numbersMatch: (state, getters) => {
      return (
        round(getters.difference + state.reconcileSummary.totalLossGain, 3) ===
        0
      )
    },
    resultString: (state, getters) => {
      if (!state.currency) {
        return ''
      }

      return `You ${getters.haveNetGain ? 'have' : 'are'} ${
        getters.amountOutString
      } ${state.currency.isoCode} ${getters.haveNetGain ? 'too much' : 'short'}`
    }
  }
}
