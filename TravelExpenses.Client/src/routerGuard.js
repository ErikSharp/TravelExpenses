import * as HomeViews from '@/common/constants/HomeViews.js'

let redirectFlag = false

let nextDesination = (next, destination) => {
  redirectFlag = true
  next({ name: destination })
}

let routerGuard = (to, next, getToken, getMissingSetupData) => {
  if (redirectFlag) {
    redirectFlag = false
    next()
  }

  let destination = to.name

  getToken(token => {
    if (token) {
      if (to.name === HomeViews.Authentication) {
        destination = HomeViews.Transactions
      }

      getMissingSetupData(isMissingData => {
        if (isMissingData) {
          nextDesination(next, HomeViews.InitialSetup)
        } else {
          if (destination === HomeViews.InitialSetup) {
            nextDesination(next, HomeViews.Transactions)
          } else {
            nextDesination(next, destination)
          }
        }
      })
    } else {
      nextDesination(next, HomeViews.Authentication)
    }
  })
}

export default routerGuard
