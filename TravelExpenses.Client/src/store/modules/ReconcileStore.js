function initialState() {
  return {
    reconcileWindowId: 0
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
    }
  },
  actions: {
    setReconcileWindowId({ commit }, id) {
      commit('SET_RECONCILE_WINDOW_ID', id)
    }
  }
}
