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
  getValues() {
    return apiClient.get('/api/values')
  },
  login(details) {
    return apiClient.put('api/users/authenticate', details)
  },
  register(details) {
    return apiClient.post('api/users', details)
  }
}
