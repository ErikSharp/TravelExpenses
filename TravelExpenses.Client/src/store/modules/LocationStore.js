import AxiosService from '@/services/AxiosService.js'

export default {
  namespaced: true,
  state: {
    busy: false,
    addLocationBusy: false,
    editLocationBusy: false,
    locations: []
  },
  mutations: {
    SET_BUSY(state, busy) {
      state.busy = busy
    },
    SET_ADD_KEYWORD_BUSY(state, busy) {
      state.addLocationBusy = busy
    },
    SET_EDIT_KEYWORD_BUSY(state, busy) {
      state.editLocationBusy = busy
    },
    SET_KEYWORDS(state, locations) {
      state.locations = locations
    }
  },
  actions: {
    load({ dispatch, commit }) {
      commit('SET_KEYWORDS', [])
      commit('SET_BUSY', true)

      return AxiosService.getLocations()
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
    addLocation({ dispatch, commit }, newLocation) {
      commit('SET_ADD_KEYWORD_BUSY', true)

      return AxiosService.addLocation(newLocation)
        .then(() => {
          dispatch(
            'showSaveMessage',
            `${newLocation.locationName} has been saved`,
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
          commit('SET_ADD_KEYWORD_BUSY', false)
        })
    },
    editLocation({ dispatch, commit }, location) {
      commit('SET_EDIT_KEYWORD_BUSY', true)

      return AxiosService.editLocation(location)
        .then(() => {
          dispatch(
            'showSaveMessage',
            `${location.locationName} has been updated`,
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
