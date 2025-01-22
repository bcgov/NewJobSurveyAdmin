import { Route } from 'react-router'

import { FixTypeLater } from '../../types/FixTypeLater'
import AuthWrapper from './AuthWrapper'

import type { JSX } from "react";

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
