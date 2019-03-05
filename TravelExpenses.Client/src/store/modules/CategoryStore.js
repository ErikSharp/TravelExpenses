import AxiosService from '@/services/AxiosService.js'
import { LossGain } from '@/common/constants/StringConstants.js'
import remove from 'lodash/remove'
import clone from 'lodash/clone'

const cornflowerBlue = 0x6495ed

function initialState() {
  return {
    busy: false,
    addCategoryBusy: false,
    editCategoryBusy: false,
    categories: [],
    lossGainCategory: null,
    editCategory: null,
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
    SET_EDIT_CATEGORY(state, category) {
      state.editCategory = category
    },
    CLEAR_EDIT_CATEGORY(state) {
      state.editCategory = null
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
    setEditCategory({ commit }, category) {
      commit('SET_EDIT_CATEGORY', category)
    },
    clearEditCategory({ commit }) {
      commit('CLEAR_EDIT_CATEGORY')
    },
    addCategory({ dispatch, state }) {
      dispatch('addCategories', [state.editCategory])
    },
    addCategories({ dispatch, commit }, newCategories) {
      commit('SET_ADD_CATEGORY_BUSY', true)

      return AxiosService.addCategories(newCategories)
        .then(response => {
          commit('CLEAR_EDIT_CATEGORY')
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
    editCategory({ dispatch, commit, state }) {
      commit('SET_EDIT_CATEGORY_BUSY', true)

      return AxiosService.editCategory(state.editCategory)
        .then(response => {
          let name = state.editCategory.categoryName
          commit('CLEAR_EDIT_CATEGORY')
          commit('SET_CATEGORIES', response.data)

          dispatch('showSaveMessage', `${name} has been updated`, {
            root: true
          })
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_EDIT_CATEGORY_BUSY', false)
        })
    },
    setName({ state, dispatch }, name) {
      let editCategory = {}

      if (!state.editCategory) {
        editCategory = {
          categoryName: name,
          color: cornflowerBlue,
          icon: ''
        }
      } else {
        editCategory = clone(state.editCategory)
        editCategory.categoryName = name
      }
      dispatch('setEditCategory', editCategory)
    },
    setRed({ state, dispatch }, red) {
      let editCategory = {}

      if (!state.editCategory) {
        editCategory = {
          categoryName: '',
          color: (red << 16) | cornflowerBlue,
          icon: ''
        }
      } else {
        editCategory = clone(state.editCategory)
        editCategory.color = editCategory.color & 0x00ffff
        editCategory.color = (red << 16) | editCategory.color
      }
      dispatch('setEditCategory', editCategory)
    },
    setGreen({ state, dispatch }, green) {
      let editCategory = {}

      if (!state.editCategory) {
        editCategory = {
          categoryName: '',
          color: (green << 8) | cornflowerBlue,
          icon: ''
        }
      } else {
        editCategory = clone(state.editCategory)
        editCategory.color = editCategory.color & 0xff00ff
        editCategory.color = (green << 8) | editCategory.color
      }
      dispatch('setEditCategory', editCategory)
    },
    setBlue({ state, dispatch }, blue) {
      let editCategory = {}

      if (!state.editCategory) {
        editCategory = {
          categoryName: '',
          color: blue | cornflowerBlue,
          icon: ''
        }
      } else {
        editCategory = clone(state.editCategory)
        editCategory.color = editCategory.color & 0xffff00
        editCategory.color = blue | editCategory.color
      }
      dispatch('setEditCategory', editCategory)
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
    },
    editHexColor: state => {
      if (state.editCategory) {
        const hex = state.editCategory.color.toString(16)
        return '#000000'.substring(0, 7 - hex.length) + hex
      } else {
        return '#' + cornflowerBlue.toString(16)
      }
    },
    red: state => {
      if (state.editCategory) {
        return (state.editCategory.color & 0xff0000) >> 16
      } else {
        return (cornflowerBlue & 0xff0000) >> 16
      }
    },
    green: state => {
      if (state.editCategory) {
        return (state.editCategory.color & 0x00ff00) >> 8
      } else {
        return (cornflowerBlue & 0x00ff00) >> 8
      }
    },
    blue: state => {
      if (state.editCategory) {
        return state.editCategory.color & 0x0000ff
      } else {
        return cornflowerBlue & 0x0000ff
      }
    }
  }
}
