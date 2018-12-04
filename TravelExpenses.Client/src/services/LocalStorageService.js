const tokenKey = 'TravelExpenses'

export default {
  saveToken(token) {
    localStorage.setItem(tokenKey, token)
  },
  getToken() {
    return localStorage.getItem(tokenKey)
  },
  clearToken() {
    localStorage.removeItem(tokenKey)
  }
}
