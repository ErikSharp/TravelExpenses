/* eslint-disable no-console */
import * as HomeViews from '@/common/constants/HomeViews.js'
import Store from '@/store/store.js'

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
  // console.log(
  //   `routerGuard enter (redirectFlag: ${redirectFlag}, to.name: ${to.name})`
  // )
  if (redirectFlag) {
    redirectFlag = false
    proceed()
    return
  }

  let destination = to.name

  getToken(token => {
    // console.log(`getToken: ${token ? 'have token' : 'no token'}`)
    if (token) {
      // console.log(`to.name: ${to.name}`)
      if (to.name === HomeViews.Authentication) {
        destination = HomeViews.Transactions
      }

      getMissingSetupData(isMissingData => {
        // console.log(`getMissingSetupData: isMissingData: ${isMissingData}`)
        if (isMissingData) {
          nextDesination(redirect, HomeViews.InitialSetup)
        } else {
          // console.log(`destination: ${destination}`)
          if (destination === HomeViews.InitialSetup) {
            nextDesination(redirect, HomeViews.Transactions)
          } else {
            if (destination == to.name) {
              Store.dispatch('setHomeView', to.name, { root: true })
              proceed()
            } else {
              nextDesination(redirect, destination)
            }
          }
        }
      })
    } else {
      nextDesination(redirect, HomeViews.Authentication)
    }
  })
}
