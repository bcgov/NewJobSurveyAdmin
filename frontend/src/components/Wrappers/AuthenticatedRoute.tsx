import { FixTypeLater } from '../../types/FixTypeLater'
import AuthWrapper from './AuthWrapper'

import type { JSX } from "react";

const AuthenticatedRoute = ({
  component: Component
}: FixTypeLater): JSX.Element => (
  <AuthWrapper>
    <Component />
  </AuthWrapper>
)

export default AuthenticatedRoute
