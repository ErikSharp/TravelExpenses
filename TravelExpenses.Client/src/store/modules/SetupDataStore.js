import SetupWindow from '@/common/enums/SetupWindows.js'

export default {
  namespaced: true,
  state: {
    setupWindow: SetupWindow.navigation,
    currencies: [
      {
        id: 1,
        isoCode: 'GBP',
        currencyName: 'Pounds Sterling',
        isHomeCurrency: true,
        homeCurrencyRatio: 1
      },
      {
        id: 2,
        isoCode: 'USD',
        currencyName: 'US Dollar',
        isHomeCurrency: false,
        homeCurrencyRatio: 1.27282
      },
      {
        id: 3,
        isoCode: 'THB',
        currencyName: 'Thai Baht',
        isHomeCurrency: false,
        homeCurrencyRatio: 41.8183
      }
    ],
    categories: [
      {
        id: 1,
        categoryName: 'Dining'
      },
      {
        id: 2,
        categoryName: 'Groceries'
      },
      {
        id: 3,
        categoryName: 'Transportation'
      },
      {
        id: 4,
        categoryName: 'Fees'
      }
    ],
    locations: [
      {
        id: 1,
        locationName: 'Chiang Mai',
        currencyId: 3,
        countryId: 1
      },
      {
        id: 2,
        locationName: 'Bangkok',
        currencyId: 3,
        countryId: 1
      }
    ],
    countries: [
      {
        id: 1,
        countryName: 'Thailand'
      }
    ],
    keywords: [
      {
        id: 1,
        keywordName: 'Uber'
      },
      {
        id: 2,
        keywordName: 'Flight'
      },
      {
        id: 3,
        keywordName: 'Bus'
      },
      {
        id: 4,
        keywordName: 'Tuk Tuk'
      }
    ]
  },
  mutations: {
    SET_SETUP_WINDOW(state, window) {
      state.setupWindow = window
    }
  },
  actions: {
    setSetupWindow({ commit }, window) {
      commit('SET_SETUP_WINDOW', window)
    }
  }
}
