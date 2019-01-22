/* eslint-disable no-console */
import * as HomeViews from '@/common/constants/HomeViews.js'

let redirectFlag = false

let nextDesination = (redirect, destination) => {
  redirectFlag = true
  redirect({ name: destination })
}

export let resetRedirectFlag = () => {
  redirectFlag = false
}

export let routerGuard = (
  to,
  redirect,
  proceed,
  getToken,
  getMissingSetupData
) => {
  if (redirectFlag) {
    redirectFlag = false
    proceed()
  }

  let destination = to.name

  getToken(token => {
    if (token) {
      if (to.name === HomeViews.Authentication) {
        destination = HomeViews.Transactions
      }

      getMissingSetupData(isMissingData => {
        if (isMissingData) {
          nextDesination(redirect, HomeViews.InitialSetup)
        } else {
          if (destination === HomeViews.InitialSetup) {
            nextDesination(redirect, HomeViews.Transactions)
          } else {
            proceed()
          }
        }
      })
    } else {
      nextDesination(redirect, HomeViews.Authentication)
    }
  })
}
