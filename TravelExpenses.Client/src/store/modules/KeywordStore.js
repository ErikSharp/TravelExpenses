import AxiosService from '@/services/AxiosService.js'

function initialState() {
  return {
    busy: false,
    addKeywordBusy: false,
    editKeywordBusy: false,
    keywords: [],
    sampleKeywords: [
      'Expensive',
      'Cheap',
      'Coffee',
      'Alcohol',
      'Lunch',
      'Dinner',
      'Breakfast',
      'Uber',
      'Doctor',
      'Dentist'
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
    setKeywords({ commit }, keywords) {
      commit('SET_KEYWORDS', keywords)
    },
    load({ dispatch, commit }) {
      commit('SET_KEYWORDS', [])
      commit('SET_BUSY', true)

      return AxiosService.getKeywords()
        .then(response => {
          commit('SET_KEYWORDS', response.data)
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_BUSY', false)
        })
    },
    addKeywords({ dispatch, commit }, newKeywords) {
      commit('SET_ADD_KEYWORD_BUSY', true)

      return AxiosService.addKeyword(newKeywords)
        .then(response => {
          commit('SET_KEYWORDS', response.data)
          dispatch(
            'showSaveMessage',
            `${
              newKeywords.length === 1
                ? newKeywords[0] + ' has'
                : newKeywords.length + ' keywords have'
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
          commit('SET_ADD_KEYWORD_BUSY', false)
        })
    },
    editKeyword({ dispatch, commit }, keyword) {
      commit('SET_EDIT_KEYWORD_BUSY', true)

      return AxiosService.editKeyword(keyword)
        .then(response => {
          commit('SET_KEYWORDS', response.data)
          dispatch(
            'showSaveMessage',
            `${keyword.keywordName} has been updated`,
            {
              root: true
            }
          )
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_EDIT_KEYWORD_BUSY', false)
        })
    }
  },
  getters: {
    findKeyword: state => id => {
      return state.keywords.find(c => c.id === id)
    }
  }
}
