import axios from 'axios'

// This is the one instance that everyone will use
const apiClient = axios.create({
  baseURL: process.env.VUE_APP_API_URL,
  withCredentials: false,
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json',
    Authorization:
      'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJuYmYiOjE1NDI0NjgyODIsImV4cCI6MTU0MzA3MzA4MiwiaWF0IjoxNTQyNDY4MjgyfQ.Fyo4okV_SRRY3t4ReqMrjTqqzGUhbOvKvM0rf3LKaE0'
  }
})

export default {
  getContacts() {
    return apiClient.get('/contacts')
  }
}
