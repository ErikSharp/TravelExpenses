/* eslint-disable no-console */
import { expect } from 'chai'
import * as Guard from '@/routerGuard.js'
import * as HomeViews from '@/common/constants/HomeViews.js'

describe('routerGuard', () => {
  beforeEach(() => {
    Guard.resetRedirectFlag()
  }),
    it('redirects to authentication when no token', done => {
      let to = { name: HomeViews.Transactions }
      let redirect = to => {
        expect(to.name).to.equal(HomeViews.Authentication)
        done()
      }

      Guard.routerGuard(
        to,
        redirect,
        null,
        callback => {
          callback(null)
        },
        callback => callback(true)
      )
    }),
    it('redirects to initial setup when missing data', done => {
      let to = { name: HomeViews.Transactions }
      let redirect = to => {
        expect(to.name).to.equal(HomeViews.InitialSetup)
        done()
      }

      Guard.routerGuard(
        to,
        redirect,
        null,
        callback => {
          callback('token')
        },
        callback => callback(true)
      )
    }),
    it('redirects to transactions from initial setup when there is no missing data', done => {
      let to = { name: HomeViews.InitialSetup }
      let redirect = to => {
        expect(to.name).to.equal(HomeViews.Transactions)
        done()
      }

      Guard.routerGuard(
        to,
        redirect,
        null,
        callback => {
          callback('token')
        },
        callback => callback(false)
      )
    }),
    it('proceeds to the destination when it should', done => {
      let to = { name: HomeViews.Queries }
      let proceed = () => {
        done()
      }

      Guard.routerGuard(
        to,
        null,
        proceed,
        callback => {
          callback('token')
        },
        callback => callback(false)
      )
    })
})
