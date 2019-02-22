import axios from 'axios'
import Store from '@/store/store.js'

// This is the one instance that everyone will use
const apiClient = axios.create({
  baseURL: process.env.VUE_APP_API_URL,
  withCredentials: false,
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  }
})

apiClient.interceptors.request.use(config => {
  if (Store.state.Authentication.authToken) {
    config.headers['Authorization'] = `Bearer ${
      Store.state.Authentication.authToken
    }`
  }

  return config
})

export default {
  login(details) {
    return apiClient.put('api/users/authenticate', details)
  },
  register(details) {
    return apiClient.post('api/users', details)
  },
  getRecentTransactions(skip) {
    return apiClient.get('api/transactions', {
      params: {
        skip: skip
      }
    })
  },
  createTransaction(transaction) {
    return apiClient.post('api/transactions', transaction)
  },
  editTransaction(transaction) {
    return apiClient.put('api/transactions', transaction)
  },
  deleteTransaction(transactionId) {
    return apiClient.delete(`api/transactions/${transactionId}`)
  },
  getRecentCashWithdrawals(skip) {
    return apiClient.get('api/cash-withdrawals', {
      params: {
        skip: skip
      }
    })
  },
  createCashWithdrawal(cashWithdrawal) {
    return apiClient.post('api/cash-withdrawals', cashWithdrawal)
  },
  editCashWithdrawal(cashWithdrawal) {
    return apiClient.put('api/cash-withdrawals', cashWithdrawal)
  },
  deleteCashWithdrawal(cashWithdrawalId) {
    return apiClient.delete(`api/cash-withdrawals/${cashWithdrawalId}`)
  },
  getCountries() {
    return apiClient.get('api/countries')
  },
  getCurrencies() {
    return apiClient.get('api/currencies')
  },
  getKeywords() {
    return apiClient.get('api/keywords')
  },
  addKeyword(newKeywords) {
    return apiClient.post('api/keywords', newKeywords)
  },
  editKeyword(keyword) {
    return apiClient.put('api/keywords', keyword)
  },
  getCategories() {
    return apiClient.get('api/categories')
  },
  addCategories(newCategories) {
    return apiClient.post('api/categories', newCategories)
  },
  editCategory(category) {
    return apiClient.put('api/categories', category)
  },
  getLocations() {
    return apiClient.get('api/locations')
  },
  addLocation(newLocation) {
    return apiClient.post('api/locations', newLocation)
  },
  editLocation(location) {
    return apiClient.put('api/locations', location)
  },
  getReconcileSummary(locationId, currencyId) {
    const payload = {
      locationId,
      currencyId
    }

    return apiClient.put('api/reconcile/currency-totals', payload)
  }
}
