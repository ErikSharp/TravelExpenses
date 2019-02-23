import AxiosService from '@/services/AxiosService.js'
import { LossGain } from '@/common/constants/StringConstants.js'
import remove from 'lodash/remove'

function initialState() {
  return {
    busy: false,
    addCategoryBusy: false,
    editCategoryBusy: false,
    categories: [],
    lossGainCategory: {},
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
      state.lossGainCategory = categories.find(c => c.categoryName === LossGain)

      remove(categories, c => {
        if (c) {
          return c.categoryName === LossGain
        } else {
          return false
        }
      })

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
  },
  getters: {
    findCategory: state => id => {
      return state.categories.find(c => c.id === id)
    }
  }
}
