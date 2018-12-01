export default {
  saveToken(token) {
    localStorage.setItem('TravelExpenses', token)
  },
  getToken() {
    return localStorage.getItem('TravelExpenses')
  }
}
