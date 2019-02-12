import AxiosService from '@/services/AxiosService.js'

function initialState() {
  return {
    saveCashWithdrawalBusy: false,
    recentCashWithdrawalsBusy: false,
    recentCashWithdrawals: [],
    recentCashWithdrawalsStale: false,
    noMoreCashWithdrawals: false,
    selectedCashWithdrawal: {}
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
    SET_SAVE_CASH_WITHDRAWAL_BUSY(state, busy) {
      state.saveCashWithdrawalBusy = busy
    },
    SET_RECENT_CASH_WITHDRAWALS_BUSY(state, busy) {
      state.recentCashWithdrawalsBusy = busy
    },
    APPEND_TO_RECENT_CASH_WITHDRAWALS(state, cashWithdrawals) {
      state.recentCashWithdrawals = state.recentCashWithdrawals.concat(
        cashWithdrawals
      )
    },
    CLEAR_RECENT_CASH_WITHDRAWALS(state) {
      state.recentCashWithdrawals = []
      state.noMoreCashWithdrawals = false
      state.recentCashWithdrawalsStale = false
    },
    SET_NO_MORE_CASH_WITHDRAWALS(state) {
      state.noMoreCashWithdrawals = true
    },
    SET_RECENT_CASH_WITHDRAWALS_STALE(state) {
      state.recentCashWithdrawalsStale = true
    },
    SET_SELECTED_CASH_WITHDRAWAL(state, cashWithdrawal) {
      state.selectedCashWithdrawal = cashWithdrawal
    }
  },
  actions: {
    saveCashWithdrawal({ dispatch }, data) {
      dispatch('innerSaveCashWithdrawal', {
        cashWithdrawal: data.cashWithdrawal,
        complete: data.complete,
        editing: false
      })
    },
    editCashWithdrawal({ dispatch }, data) {
      dispatch('innerSaveCashWithdrawal', {
        cashWithdrawal: data.cashWithdrawal,
        complete: data.complete,
        editing: true
      })
    },
    innerSaveCashWithdrawal({ dispatch, commit }, data) {
      commit('SET_SAVE_CASH_WITHDRAWAL_BUSY', true)

      let axiosOp = data.editing
        ? AxiosService.editCashWithdrawal
        : AxiosService.createCashWithdrawal

      return axiosOp(data.cashWithdrawal)
        .then(() => {
          commit('SET_RECENT_CASH_WITHDRAWALS_STALE')
          dispatch(
            'showSaveMessage',
            `${data.cashWithdrawal.title} has been ${
              data.editing ? 'changed' : 'saved'
            }`,
            {
              root: true
            }
          )
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_SAVE_CASH_WITHDRAWAL_BUSY', false)
          data.complete()
        })
    },
    getRecentCashWithdrawals({ commit, dispatch }, skip) {
      if (!skip) {
        skip = 0
      }

      commit('SET_RECENT_CASH_WITHDRAWALS_BUSY', true)

      return AxiosService.getRecentCashWithdrawals(skip)
        .then(response => {
          if (response.data.length) {
            commit('APPEND_TO_RECENT_CASH_WITHDRAWALS', response.data)
          } else {
            commit('SET_NO_MORE_CASH_WITHDRAWALS')
          }
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_RECENT_CASH_WITHDRAWALS_BUSY', false)
        })
    },
    getNextCashWithdrawals({ state, dispatch }) {
      let length = state.recentCashWithdrawals.length
      dispatch('getRecentCashWithdrawals', length)
    },
    reloadRecentCashWithdrawals({ commit, dispatch }) {
      commit('CLEAR_RECENT_CASH_WITHDRAWALS')
      dispatch('getRecentCashWithdrawals')
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
            dispatch('reloadRecentCashWithdrawals')
          })
          .catch(error => {
            dispatch('showErrorMessage', error, { root: true })
          })
          .then(() => {
            commit('SET_SAVE_CASH_WITHDRAWAL_BUSY', false)
            if (completed) {
              completed()
            }
          })
      }
    }
  }
}
