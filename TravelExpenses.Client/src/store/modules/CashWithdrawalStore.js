import AxiosService from '@/services/AxiosService.js'

function initialState() {
  return {
    saveCashWithdrawalBusy: false,
    cashWithdrawalsBusy: false,
    cashWithdrawals: [],
    selectedCashWithdrawal: {},
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
    SET_SAVE_CASH_WITHDRAWAL_BUSY(state, busy) {
      state.saveCashWithdrawalBusy = busy
    },
    SET_CASH_WITHDRAWALS_BUSY(state, busy) {
      state.cashWithdrawalsBusy = busy
    },
    SET_CASH_WITHDRAWALS(state, withdrawals) {
      state.cashWithdrawals = withdrawals
    },
    CLEAR_CASH_WITHDRAWALS(state) {
      state.cashWithdrawals = []
    },
    CLEAR_SELECTED_CASH_WITHDRAWAL(state) {
      state.selectedCashWithdrawal = {}
    },
    SET_SELECTED_CASH_WITHDRAWAL(state, cashWithdrawal) {
      state.selectedCashWithdrawal = cashWithdrawal
    },
    SET_PAGE(state, page) {
      state.page = page
    }
  },
  actions: {
    saveCashWithdrawal({ dispatch }, cashWithdrawal) {
      return dispatch('innerSaveCashWithdrawal', {
        cashWithdrawal: cashWithdrawal,
        editing: false
      })
    },
    editCashWithdrawal({ dispatch }, cashWithdrawal) {
      return dispatch('innerSaveCashWithdrawal', {
        cashWithdrawal: cashWithdrawal,
        editing: true
      })
    },
    innerSaveCashWithdrawal({ dispatch, commit, state }, data) {
      commit('SET_SAVE_CASH_WITHDRAWAL_BUSY', true)

      let axiosOp = data.editing
        ? AxiosService.editCashWithdrawal
        : AxiosService.createCashWithdrawal

      return axiosOp(data.cashWithdrawal)
        .then(() => {
          dispatch('getCashWithdrawals', state.page)
          dispatch(
            'showSaveMessage',
            `Cash withdrawal has been ${data.editing ? 'changed' : 'saved'}`,
            {
              root: true
            }
          )
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_SAVE_CASH_WITHDRAWAL_BUSY', false)
        })
    },
    getCashWithdrawals({ state, commit, dispatch, rootState }, page) {
      commit('CLEAR_CASH_WITHDRAWALS')
      commit('CLEAR_SELECTED_CASH_WITHDRAWAL')
      commit('SET_PAGE', page || 1)

      let skip = 0
      if (page > 1) {
        skip = (page - 1) * state.pageSize
      }

      commit('SET_CASH_WITHDRAWALS_BUSY', true)

      return AxiosService.getCashWithdrawals(
        skip,
        rootState.Location.selectedLocation.id
      )
        .then(response => {
          commit('SET_PAGE_SIZE', +response.headers['page-size'])
          commit('SET_TOTAL_RECORDS', +response.headers['x-total-count'])
          commit('SET_CASH_WITHDRAWALS', response.data)
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_CASH_WITHDRAWALS_BUSY', false)
        })
    },
    setSelectedCashWithdrawal({ state, commit }, cashWithdrawal) {
      if (state.selectedCashWithdrawal == cashWithdrawal) {
        commit('SET_SELECTED_CASH_WITHDRAWAL', {})
      } else {
        commit('SET_SELECTED_CASH_WITHDRAWAL', cashWithdrawal)
      }
    },
    deleteSelectedCashWithdrawal({ state, commit, dispatch }, completed) {
      if (state.selectedCashWithdrawal) {
        commit('SET_SAVE_CASH_WITHDRAWAL_BUSY', true)

        return AxiosService.deleteCashWithdrawal(
          state.selectedCashWithdrawal.id
        )
          .then(() => {
            commit('SET_SELECTED_CASH_WITHDRAWAL', {})
            dispatch('getCashWithdrawals')
          })
          .catch(error => {
            dispatch('showAxiosErrorMessage', error, { root: true })
          })
          .then(() => {
            commit('SET_SAVE_CASH_WITHDRAWAL_BUSY', false)
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
