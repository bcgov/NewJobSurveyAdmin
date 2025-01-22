import { Routes, Route } from 'react-router'
import React, { useEffect } from 'react'

import { windowLocation } from '../helpers/envHelper'
import AdminInterface from './Admin/AdminInterface'
import AuthenticatedRoute from './Wrappers/AuthenticatedRoute'
import EmployeeDetail from './Employees/EmployeeDetail/EmployeeDetail'
import EmployeeListing from './Employees/EmployeeListing'
import HealthStatus from './HealthStatus/HealthStatus'
import Home from './Home'
import Layout from './Wrappers/Layout'
import LogoutPage from './Login/LogoutPage'
import TaskLogEntryListing from './TaskLogEntries/TaskLogEntryListing'

import '../custom.css'

const App = () => {
  // If we get redirected here from a Keycloak logon, redirect the user to
  // the location we had saved for them before being redirected.
  useEffect(() => {
    const href = windowLocation.get()
    if (href) {
      window.location.href = href
    }
  }, [])

  return (
    <Layout>
      <Routes>
        <Route path="/" element={<AuthenticatedRoute component={Home} />} />
        <Route path="/logout" element={<LogoutPage />} />
        <Route path="/status" element={<HealthStatus />} />
        <Route path="/employees/:employeeId" element={<AuthenticatedRoute component={EmployeeDetail} />} />
        <Route path="/employees" element={<AuthenticatedRoute component={EmployeeListing} />} />
        <Route path="/task-log-entries" element={<AuthenticatedRoute component={TaskLogEntryListing} />} />
        <Route path="/admin" element={<AuthenticatedRoute component={AdminInterface} />} />
      </Routes>
    </Layout>
  )
}

export default App
