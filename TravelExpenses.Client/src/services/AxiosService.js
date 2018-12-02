/* eslint-disable no-console */
import axios from 'axios'
import { Promise } from 'bluebird'

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

// eslint-disable-next-line no-unused-vars
let requestInterceptor = apiClient.interceptors.request.use(
  config => {
    console.log('Intercepting a request')
    console.log(config)
    console.log('End request interception')
    return config
  },
  error => {
    console.log('We have an error in the request interceptor:')
    console.log(error)
    return Promise.reject(error)
  }
)

// eslint-disable-next-line no-unused-vars
let responseInterceptor = apiClient.interceptors.response.use(
  response => {
    console.log('Intercepting a response')
    console.log(response)
    console.log('End response interception')
    return response
  },
  error => {
    console.log('We have an error in the response interceptor:')
    console.log(error)
    return Promise.reject(error)
  }
)

export default {
  getValues() {
    return apiClient.get('/api/values')
  }
}
