export default {
  namespaced: true,
  state: {
    reconcileWindowId: 0
  },
  mutations: {
    SET_RECONCILE_WINDOW_ID(state, id) {
      state.reconcileWindowId = id
    }
  },
  actions: {
    setReconcileWindowId({ commit }, id) {
      commit('SET_RECONCILE_WINDOW_ID', id)
    }
  }
}
