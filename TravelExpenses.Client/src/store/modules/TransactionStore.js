import AxiosService from '@/services/AxiosService.js'

function initialState() {
  return {
    saveTransactionBusy: false,
    transactionsBusy: false,
    transactions: [],
    selectedTransaction: {},
    page: 1,
    pageSize: 25,
    totalRecords: 0
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
    SET_PAGE_SIZE(state, size) {
      state.pageSize = size
    },
    SET_TOTAL_RECORDS(state, total) {
      state.totalRecords = total
    },
    SET_SAVE_TRANSACTION_BUSY(state, busy) {
      state.saveTransactionBusy = busy
    },
    SET_TRANSACTIONS_BUSY(state, busy) {
      state.transactionsBusy = busy
    },
    SET_TRANSACTIONS(state, transactions) {
      state.transactions = transactions
    },
    CLEAR_TRANSACTIONS(state) {
      state.transactions = []
    },
    CLEAR_SELECTED_TRANSACTION(state) {
      state.selectedTransaction = {}
    },
    SET_SELECTED_TRANSACTION(state, transaction) {
      state.selectedTransaction = transaction
    },
    SET_PAGE(state, page) {
      state.page = page
    }
  },
  actions: {
    saveTransaction({ dispatch }, transaction) {
      return dispatch('innerSaveTransaction', {
        transaction: transaction,
        editing: false
      })
    },
    editTransaction({ dispatch }, transaction) {
      return dispatch('innerSaveTransaction', {
        transaction: transaction,
        editing: true
      })
    },
    innerSaveTransaction({ dispatch, commit, state }, data) {
      commit('SET_SAVE_TRANSACTION_BUSY', true)

      let axiosOp = data.editing
        ? AxiosService.editTransaction
        : AxiosService.createTransaction

      return axiosOp(data.transaction)
        .then(() => {
          dispatch('getTransactions', state.page)
          dispatch(
            'showSaveMessage',
            `${data.transaction.title} has been ${
              data.editing ? 'changed' : 'saved'
            }`,
            {
              root: true
            }
          )
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_SAVE_TRANSACTION_BUSY', false)
        })
    },
    getTransactions({ state, commit, dispatch, rootState }, page) {
      commit('CLEAR_TRANSACTIONS')
      commit('CLEAR_SELECTED_TRANSACTION')
      commit('SET_PAGE', page || 1)

      let skip = 0
      if (page > 1) {
        skip = (page - 1) * state.pageSize
      }

      commit('SET_TRANSACTIONS_BUSY', true)

      return AxiosService.getTransactions(skip)
        .then(response => {
          commit('SET_PAGE_SIZE', +response.headers['page-size'])
          commit('SET_TOTAL_RECORDS', +response.headers['x-total-count'])
          commit('SET_TRANSACTIONS', response.data)
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_TRANSACTIONS_BUSY', false)
        })
    },
    setSelectedTransaction({ state, commit }, transaction) {
      if (state.selectedTransaction == transaction) {
        commit('SET_SELECTED_TRANSACTION', {})
      } else {
        commit('SET_SELECTED_TRANSACTION', transaction)
      }
    },
    deleteSelectedTransaction({ state, commit, dispatch }, completed) {
      if (state.selectedTransaction) {
        commit('SET_SAVE_TRANSACTION_BUSY', true)

        return AxiosService.deleteTransaction(state.selectedTransaction.id)
          .then(() => {
            commit('SET_SELECTED_TRANSACTION', {})
            dispatch('getTransactions')
          })
          .catch(error => {
            dispatch('showAxiosErrorMessage', error, { root: true })
          })
          .then(() => {
            commit('SET_SAVE_TRANSACTION_BUSY', false)
            if (completed) {
              completed()
            }
          })
      }
    }
  },
  getters: {
    pageCount: state => {
      return Math.ceil(state.totalRecords / state.pageSize) || 1
    }
  }
}
