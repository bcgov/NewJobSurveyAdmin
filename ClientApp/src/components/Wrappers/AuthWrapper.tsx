import React from 'react'

import { authRole } from '../../helpers/envHelper'
import KeycloakService from '../Login/KeycloakService'
import Unauthenticated from '../Login/Unauthenticated'
import Unauthorized from '../Login/Unauthorized'

interface AuthWrapperProps {
  children: React.ReactNode
}

const AuthWrapper = (props: AuthWrapperProps): JSX.Element => {
  const isLoggedIn = KeycloakService.isLoggedIn()
  const hasCorrectRole = KeycloakService.hasRole([authRole()])

  if (!isLoggedIn) return <Unauthenticated />
  if (!hasCorrectRole) return <Unauthorized />

  return <>{props.children}</>
}

export default AuthWrapper
