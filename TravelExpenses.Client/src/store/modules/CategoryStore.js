import AxiosService from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    busy: false,
    addCategoryBusy: false,
    editCategoryBusy: false,
    categories: [],
    sampleCategories: [
      'Transportation',
      'Dining',
      'Groceries',
      'Entertainment',
      'Accommodations',
      'Utilities',
      'Medical',
      'Fees',
      'Deposit',
      'Non-trip'
    ]
  },
  mutations: {
    SET_BUSY(state, busy) {
      state.busy = busy
    },
    SET_ADD_CATEGORY_BUSY(state, busy) {
      state.addCategoryBusy = busy
    },
    SET_EDIT_CATEGORY_BUSY(state, busy) {
      state.editCategoryBusy = busy
    },
    SET_CATEGORIES(state, categories) {
      state.categories = categories
    }
  },
  actions: {
    load({ dispatch, commit }) {
      commit('SET_CATEGORIES', [])
      commit('SET_BUSY', true)

      return AxiosService.getCategories()
        .then(response => {
          commit('SET_CATEGORIES', response.data)
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_BUSY', false)
        })
    },
    addCategories({ dispatch, commit }, newCategories) {
      commit('SET_ADD_CATEGORY_BUSY', true)

      return AxiosService.addCategories(newCategories)
        .then(response => {
          commit('SET_CATEGORIES', response.data)
          dispatch(
            'showSaveMessage',
            `${
              newCategories.length === 1
                ? newCategories[0] + ' has'
                : newCategories.length + ' categories have'
            } been saved`,
            {
              root: true
            }
          )
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_ADD_CATEGORY_BUSY', false)
        })
    },
    editCategory({ dispatch, commit }, category) {
      commit('SET_EDIT_CATEGORY_BUSY', true)

      return AxiosService.editCategory(category)
        .then(response => {
          commit('SET_CATEGORIES', response.data)
          dispatch(
            'showSaveMessage',
            `${category.categoryName} has been updated`,
            {
              root: true
            }
          )
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_EDIT_CATEGORY_BUSY', false)
        })
    }
  }
}
