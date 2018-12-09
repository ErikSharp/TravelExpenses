export default {
  namespaced: true,
  state: {
    busy: false
  },
  mutations: {
    SET_BUSY(state, busy) {
      state.busy = busy
    }
  },
  actions: {
    saveTransaction({ dispatch, commit }, transaction) {
      commit('SET_BUSY', true)

      setTimeout(() => {
        commit('SET_BUSY', false)
        dispatch(
          'showSnackbar',
          {
            message: `${transaction.title} has been saved`,
            color: 'primary darken-3'
          },
          { root: true }
        )
      }, 1000)
    }
  }
}
