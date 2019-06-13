import axios from 'axios'
import Store from '@/store/store.js'
import { filter } from 'bluebird'

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
  getTransactions(skip) {
    let params = {
      skip: skip
    }

    return apiClient.get('api/transactions', {
      params: params
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
  getCashWithdrawals(skip) {
    let params = {
      skip: skip
    }

    return apiClient.get('api/cash-withdrawals', {
      params: params
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
  getReconcileSummary(currencyId) {
    return apiClient.get(`api/reconcile/currency-totals/${currencyId}`)
  },
  getUser() {
    return apiClient.get('api/users/me')
  },
  savePreferences(preferences) {
    return apiClient.put('api/users/preferences', preferences)
  },
  getBaseData() {
    return apiClient.get('api/base-data')
  }
}
