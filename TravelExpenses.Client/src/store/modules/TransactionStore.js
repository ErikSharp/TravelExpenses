import AxiosService from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    saveTransactionBusy: false,
    recentTransactionsBusy: false,
    recentTransactions: []
  },
  mutations: {
    SET_SAVE_TRANSACTION_BUSY(state, busy) {
      state.saveTransactionBusy = busy
    },
    SET_RECENT_TRANSACTIONS_BUSY(state, busy) {
      state.recentTransactionsBusy = busy
    },
    APPEND_TO_RECENT_TRANSACTIONS(state, transactions) {
      state.recentTransactions = state.recentTransactions.concat(transactions)
    },
    CLEAR_RECENT_TRANSACTIONS(state) {
      state.recentTransactions = []
    }
  },
  actions: {
    saveTransaction({ dispatch, commit }, transaction) {
      commit('SET_SAVE_TRANSACTION_BUSY', true)

      return AxiosService.createTransaction(transaction)
        .then(() => {
          dispatch('showSaveMessage', `${transaction.title} has been saved`, {
            root: true
          })
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_SAVE_TRANSACTION_BUSY', false)
        })
    },
    getRecentTransactions({ commit, dispatch }, skip) {
      if (!skip) {
        skip = 0
      }

      commit('SET_RECENT_TRANSACTIONS_BUSY', true)

      return AxiosService.getRecentTransactions(skip)
        .then(response => {
          commit('APPEND_TO_RECENT_TRANSACTIONS', response.data)
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_RECENT_TRANSACTIONS_BUSY', false)
        })
    },
    reloadRecentTransactions({ commit, dispatch }) {
      commit('CLEAR_RECENT_TRANSACTIONS')
      dispatch('getRecentTransactions')
    }
  }
}
