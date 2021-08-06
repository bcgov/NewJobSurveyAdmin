import React from 'react'
import { Route } from 'react-router-dom'

import AuthenticatedRoute from './Wrappers/AuthenticatedRoute'
import CallbackHandler from './Login/CallbackHandler'
import EmployeeDetail from './Employees/EmployeeDetail'
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
        <Route exact path="/" component={Home} />
        <Route path="/callback" component={CallbackHandler} />
        <Route path="/status" component={HealthStatus} />
        <Route
          exact
          path="/employees/:employeeId"
          component={EmployeeDetail}
        />
        <Route
          exact
          path="/employees"
          component={EmployeeListing}
        />
        <Route
          exact
          path="/task-log-entries"
          component={TaskLogEntryListing}
        />
        <Route exact path="/admin" component={AdminInterface} />
      </Layout>
    )
  }
}
