import { Route } from 'react-router-dom'
import React from 'react'

import { FixTypeLater } from '../../types/FixTypeLater'
import AuthWrapper from './AuthWrapper'

const AuthenticatedRoute = ({
  component: Component,
  ...rest
}: FixTypeLater): JSX.Element => (
  <Route
    {...rest}
    render={(props): JSX.Element => (
      <AuthWrapper>
        <Component {...props} />
      </AuthWrapper>
    )}
  />
)

export default AuthenticatedRoute
