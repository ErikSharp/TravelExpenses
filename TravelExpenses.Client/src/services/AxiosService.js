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
  async getValues() {
    try {
      const response = await apiClient.get('/api/values')
      console.log(response.data)
      console.log(response.status)
      console.log(response.statusText)
      console.log(response.headers)
      console.log(response.config)
      return response
    } catch (error) {
      if (error.response) {
        console.log(
          'The request was made and the server responded with a status code that falls out of the range of 2xx'
        )
        console.log(error.response.data)
        console.log(error.response.status)
        console.log(error.response.headers)
      } else if (error.request) {
        console.log('The request was made but no response was received')
        // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
        // http.ClientRequest in node.js
        console.log(error.request)
      } else {
        console.log(
          'Something happened in setting up the request that triggered an Error'
        )
        console.log('Error', error.message)
      }
      console.log(error.config)
    }
    console.log('This always get executed')
  }
}
