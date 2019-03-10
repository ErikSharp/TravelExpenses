import AxiosService from '@/services/AxiosService.js'

function initialState() {
  return {
    busy: false,
    addLocationBusy: false,
    editLocationBusy: false,
    locations: []
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
    SET_ADD_LOCATION_BUSY(state, busy) {
      state.addLocationBusy = busy
    },
    SET_EDIT_LOCATION_BUSY(state, busy) {
      state.editLocationBusy = busy
    },
    SET_LOCATIONS(state, locations) {
      state.locations = locations
    }
  },
  actions: {
    setLocations({ commit }, locations) {
      commit('SET_LOCATIONS', locations)
    },
    load({ dispatch, commit }) {
      commit('SET_LOCATIONS', [])
      commit('SET_BUSY', true)

      return AxiosService.getLocations()
        .then(response => {
          commit('SET_LOCATIONS', response.data)
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_BUSY', false)
        })
    },
    addLocation({ dispatch, commit }, newLocation) {
      commit('SET_ADD_LOCATION_BUSY', true)

      return AxiosService.addLocation(newLocation)
        .then(response => {
          commit('SET_LOCATIONS', response.data)
          dispatch(
            'showSaveMessage',
            `${newLocation.locationName} has been saved`,
            {
              root: true
            }
          )
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_ADD_LOCATION_BUSY', false)
        })
    },
    editLocation({ dispatch, commit }, location) {
      commit('SET_EDIT_LOCATION_BUSY', true)

      return AxiosService.editLocation(location)
        .then(response => {
          commit('SET_LOCATIONS', response.data)
          dispatch(
            'showSaveMessage',
            `${location.locationName} has been updated`,
            {
              root: true
            }
          )
        })
        .catch(error => {
          dispatch('showAxiosErrorMessage', error, { root: true })
        })
        .then(() => {
          commit('SET_EDIT_LOCATION_BUSY', false)
        })
    }
  },
  getters: {
    findLocation: state => id => {
      return state.locations.find(l => l.id === id)
    }
  }
}
