import Vue from 'vue'
import Vuex from 'vuex'
import Router from '@/router'
import Authentication from '@/store/modules/AuthenticationStore.js'
import Reconcile from '@/store/modules/ReconcileStore.js'
import SetupData from '@/store/modules/SetupDataStore.js'
import Transaction from '@/store/modules/TransactionStore.js'
import Country from '@/store/modules/CountryStore.js'
import Category from '@/store/modules/CategoryStore.js'
import Keyword from '@/store/modules/KeywordStore.js'
import * as HomeViews from '@/common/constants/HomeViews.js'
import SetupWindows from '@/common/enums/SetupWindows.js'
import { firstLetterUpper } from '@/common/StringUtilities.js'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    Authentication,
    Reconcile,
    SetupData,
    Transaction,
    Country,
    Category,
    Keyword
  },
  state: {
    homeView: HomeViews.Transactions,
    title: '',
    snackbar: {
      show: false,
      color: '',
      mode: '',
      timeout: 5000,
      message: ''
    }
  },
  mutations: {
    SET_HOME_VIEW(state, view) {
      state.homeView = view
    },
    SET_SNACKBAR_DETAILS(state, snackbar) {
      //'primary', success', 'info', 'error', 'cyan darken-2'
      state.snackbar.color = snackbar.color || 'primary'
      state.snackbar.message = snackbar.message || 'No message provided'
      state.snackbar.mode = snackbar.mode || ''
      state.snackbar.timeout = snackbar.timeout || 3000
      state.snackbar.show = true
    },
    SET_TITLE(state, title) {
      state.title = title
    }
  },
  actions: {
    setHomeView({ dispatch, commit }, view) {
      commit('SET_HOME_VIEW', view)
      switch (view) {
        case HomeViews.Transactions:
          dispatch('setTitle', 'Recent Transactions')
          dispatch('SetupData/setSetupWindow', SetupWindows.navigation)
          break
        default:
          dispatch('setTitle', firstLetterUpper(view))
          break
      }
      Router.push({ name: view })
    },
    setTitle({ commit }, title) {
      commit('SET_TITLE', title)
    },
    showSnackbar({ commit }, snackbar) {
      commit('SET_SNACKBAR_DETAILS', snackbar)
    },
    showErrorMessage({ dispatch }, error) {
      if (error.response) {
        dispatch('showSnackbar', {
          message:
            'The request was made and the server responded with a status code that falls out of the range of 2xx',
          mode: 'multi-line',
          color: 'error'
        })
      } else if (error.request) {
        dispatch('showSnackbar', {
          message: 'The request was made but no response was received',
          color: 'error'
        })
        // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
        // http.ClientRequest in node.js
      } else {
        dispatch('showSnackbar', {
          message:
            'Something happened in setting up the request that triggered an Error',
          color: 'error'
        })
      }
    },
    showSaveMessage({ dispatch }, message) {
      dispatch('showSnackbar', {
        message: message,
        color: 'primary darken-3'
      })
    }
  }
})
