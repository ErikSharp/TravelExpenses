import AxiosService from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    busy: false,
    addCategoryBusy: false,
    editCategoryBusy: false,
    categories: []
  },
  mutations: {
    SET_BUSY(state, busy) {
      state.busy = busy
    },
    SET_ADD_COUNTRY_BUSY(state, busy) {
      state.addCategoryBusy = busy
    },
    SET_EDIT_COUNTRY_BUSY(state, busy) {
      state.editCategoryBusy = busy
    },
    SET_COUNTRIES(state, categories) {
      state.categories = categories
    }
  },
  actions: {
    load({ dispatch, commit }) {
      commit('SET_COUNTRIES', [])
      commit('SET_BUSY', true)

      return AxiosService.getCategories()
        .then(response => {
          commit('SET_COUNTRIES', response.data)
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_BUSY', false)
        })
    },
    addCategory({ dispatch, commit }, newCategory) {
      commit('SET_ADD_COUNTRY_BUSY', true)

      return AxiosService.addCategory(newCategory)
        .then(() => {
          dispatch('showSaveMessage', `${newCategory} has been saved`, {
            root: true
          })
          dispatch('load')
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_ADD_COUNTRY_BUSY', false)
        })
    },
    editCategory({ dispatch, commit }, category) {
      commit('SET_EDIT_COUNTRY_BUSY', true)

      return AxiosService.editCategory(category)
        .then(() => {
          dispatch(
            'showSaveMessage',
            `${category.categoryName} has been updated`,
            {
              root: true
            }
          )
          dispatch('load')
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_EDIT_COUNTRY_BUSY', false)
        })
    }
  }
}
