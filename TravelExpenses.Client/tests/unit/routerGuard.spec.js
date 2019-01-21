import { expect } from 'chai'
import routerGuard from '@/routerGuard.js'
import * as HomeViews from '@/common/constants/HomeViews.js'

describe('routerGuard', () => {
  it('redirects to authentication when no token', () => {
    let to = { name: HomeViews.Transactions }
    let next = to => {
      expect(to).to.equal(HomeViews.Authentication)
    }
    routerGuard(
      to,
      next,
      () => {
        return null
      },
      null
    )
  })
})
