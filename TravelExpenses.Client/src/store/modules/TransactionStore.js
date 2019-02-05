import AxiosService from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    saveTransactionBusy: false,
    recentTransactionsBusy: false,
    recentTransactions: [],
    recentTransactionsStale: false,
    noMoreTransactions: false,
    selectedTransaction: {}
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
      state.noMoreTransactions = false
      state.recentTransactionsStale = false
    },
    SET_NO_MORE_TRANSACTIONS(state) {
      state.noMoreTransactions = true
    },
    SET_RECENT_TRANSACTIONS_STALE(state) {
      state.recentTransactionsStale = true
    },
    SET_SELECTED_TRANSACTION(state, transaction) {
      state.selectedTransaction = transaction
    }
  },
  actions: {
    saveTransaction({ dispatch, commit }, transaction) {
      commit('SET_SAVE_TRANSACTION_BUSY', true)

      return AxiosService.createTransaction(transaction)
        .then(() => {
          commit('SET_RECENT_TRANSACTIONS_STALE')
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
          if (response.data.length) {
            commit('APPEND_TO_RECENT_TRANSACTIONS', response.data)
          } else {
            commit('SET_NO_MORE_TRANSACTIONS')
          }
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_RECENT_TRANSACTIONS_BUSY', false)
        })
    },
    getNextTransactions({ state, dispatch }) {
      let length = state.recentTransactions.length
      dispatch('getRecentTransactions', length)
    },
    reloadRecentTransactions({ commit, dispatch }) {
      commit('CLEAR_RECENT_TRANSACTIONS')
      dispatch('getRecentTransactions')
    },
    setSelectedTransaction({ state, commit }, transaction) {
      if (state.selectedTransaction == transaction) {
        commit('SET_SELECTED_TRANSACTION', {})
      } else {
        commit('SET_SELECTED_TRANSACTION', transaction)
      }
    }
  }
}
