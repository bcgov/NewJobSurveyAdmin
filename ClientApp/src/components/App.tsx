import { Route } from 'react-router-dom'
import React from 'react'

import AuthenticatedRoute from './Wrappers/AuthenticatedRoute'
import CallbackHandler from './Login/CallbackHandler'
import EmployeeDetail from './Employees/EmployeeDetail/EmployeeDetail'
import EmployeeListing from './Employees/EmployeeListing'
import Home from './Home'
import Layout from './Wrappers/Layout'
import TaskLogEntryListing from './TaskLogEntries/TaskLogEntryListing'
import AdminInterface from './Admin/AdminInterface'
import HealthStatus from './HealthStatus/HealthStatus'

import '../custom.css'

export default class App extends React.Component {
  static displayName = App.name

  render(): JSX.Element {
    return (
      <Layout>
        <AuthenticatedRoute exact path="/" component={Home} />
        <Route path="/callback" component={CallbackHandler} />
        <Route path="/status" component={HealthStatus} />
        <AuthenticatedRoute
          exact
          path="/employees/:employeeId"
          component={EmployeeDetail}
        />
        <AuthenticatedRoute
          exact
          path="/employees"
          component={EmployeeListing}
        />
        <AuthenticatedRoute
          exact
          path="/task-log-entries"
          component={TaskLogEntryListing}
        />
        <AuthenticatedRoute exact path="/admin" component={AdminInterface} />
      </Layout>
    )
  }
}
