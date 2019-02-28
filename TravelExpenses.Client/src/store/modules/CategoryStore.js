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
    chosenIconAndColor: null,
    sampleCategories: [
      {
        categoryName: 'Transportation',
        icon: 'commute',
        color: 0x3f51b5
      },
      {
        categoryName: 'Dining',
        icon: 'fastfood',
        color: 0xff9800
      },
      {
        categoryName: 'Groceries',
        icon: 'local_grocery_store',
        color: 0x388e3c
      },
      {
        categoryName: 'Entertainment',
        icon: 'theaters',
        color: 0xf06292
      },
      {
        categoryName: 'Accommodations',
        icon: 'local_hotel',
        color: 0xba68c8
      },
      {
        categoryName: 'Utilities',
        icon: 'power',
        color: 0x66bb6a
      },
      {
        categoryName: 'Medical',
        icon: 'local_hospital',
        color: 0xe53935
      },
      {
        categoryName: 'Fees',
        icon: 'local_atm',
        color: 0x64b5f6
      },
      {
        categoryName: 'Deposit',
        icon: 'attach_money',
        color: 0x4dd0e1
      },
      {
        categoryName: 'Non-trip',
        icon: 'card_giftcard',
        color: 0xf57c00
      }
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
      let lossGains = remove(categories, c => {
        return c && c.categoryName === LossGain
      })

      state.lossGainCategory = lossGains[0]
      state.categories = categories
    },
    SET_CHOSEN_ICON_AND_COLOR(state, iconColor) {
      state.chosenIconAndColor = iconColor
    },
    CLEAR_CHOSEN_ICON_AND_COLOR(state) {
      state.chosenIconAndColor = null
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
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_BUSY', false)
        })
    },
    setChosenIconAndColor({ commit }, iconColor) {
      commit('SET_CHOSEN_ICON_AND_COLOR', iconColor)
    },
    clearChosenIconAndColor({ commit }) {
      commit('CLEAR_CHOSEN_ICON_AND_COLOR')
    },
    addCategories({ dispatch, commit }, newCategories) {
      commit('SET_ADD_CATEGORY_BUSY', true)

      return AxiosService.addCategories(newCategories)
        .then(response => {
          commit('CLEAR_CHOSEN_ICON_AND_COLOR')
          commit('SET_CATEGORIES', response.data)

          dispatch(
            'showSaveMessage',
            `${
              newCategories.length === 1
                ? newCategories[0].categoryName + ' has'
                : newCategories.length + ' categories have'
            } been saved`,
            {
              root: true
            }
          )
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_ADD_CATEGORY_BUSY', false)
        })
    },
    editCategory({ dispatch, commit }, category) {
      commit('SET_EDIT_CATEGORY_BUSY', true)

      return AxiosService.editCategory(category)
        .then(response => {
          commit('CLEAR_CHOSEN_ICON_AND_COLOR')
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
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_EDIT_CATEGORY_BUSY', false)
        })
    }
  },
  getters: {
    findCategory: state => id => {
      let category = state.categories.find(c => c.id === id)

      if (!category) {
        category =
          state.lossGainCategory.id === id ? state.lossGainCategory : null
      }

      return category
    }
  }
}
