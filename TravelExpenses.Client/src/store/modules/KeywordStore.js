import AxiosService from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    busy: false,
    addKeywordBusy: false,
    editKeywordBusy: false,
    keywords: []
  },
  mutations: {
    SET_BUSY(state, busy) {
      state.busy = busy
    },
    SET_ADD_KEYWORD_BUSY(state, busy) {
      state.addKeywordBusy = busy
    },
    SET_EDIT_KEYWORD_BUSY(state, busy) {
      state.editKeywordBusy = busy
    },
    SET_KEYWORDS(state, keywords) {
      state.keywords = keywords
    }
  },
  actions: {
    load({ dispatch, commit }) {
      commit('SET_KEYWORDS', [])
      commit('SET_BUSY', true)

      return AxiosService.getKeywords()
        .then(response => {
          commit('SET_KEYWORDS', response.data)
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_BUSY', false)
        })
    },
    addKeyword({ dispatch, commit }, newKeyword) {
      commit('SET_ADD_KEYWORD_BUSY', true)

      return AxiosService.addKeyword(newKeyword)
        .then(() => {
          dispatch('showSaveMessage', `${newKeyword} has been saved`, {
            root: true
          })
          dispatch('load')
        })
        .catch(error => {
          dispatch('showErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_ADD_KEYWORD_BUSY', false)
        })
    },
    editKeyword({ dispatch, commit }, keyword) {
      commit('SET_EDIT_KEYWORD_BUSY', true)

      return AxiosService.editKeyword(keyword)
        .then(() => {
          dispatch(
            'showSaveMessage',
            `${keyword.keywordName} has been updated`,
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
          commit('SET_EDIT_KEYWORD_BUSY', false)
        })
    }
  }
}
