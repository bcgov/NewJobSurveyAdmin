import { CellProps, Column } from 'react-table'
import { Link } from 'react-router-dom'
import React from 'react'

import { AppointmentStatus } from '../../types/AppointmentStatus'
import { Employee } from '../../types/Employee'
import { EmployeeStatus } from '../../types/EmployeeStatus'
import FormattedDate from '../DisplayHelpers/FormattedDate'

type EmployeeCellProps = React.PropsWithChildren<
  CellProps<Employee, string | undefined>
>

export const employeeTableColumns = (): Column<Employee>[] => [
  {
    Header: 'Telkey',
    Cell: (props: EmployeeCellProps): JSX.Element => (
      <Link to={`/employees/${props.cell.row.original.id}`}>{props.value}</Link>
    ),
    accessor: 'telkey'
  },
  {
    Header: 'Employee ID',
    accessor: 'governmentEmployeeId'
  },
  {
    Header: 'Preferred first name',
    accessor: 'preferredFirstName'
  },
  {
    Header: 'Last name',
    accessor: 'lastName'
  },
  {
    Header: 'Preferred email',
    accessor: 'preferredEmail'
  },
  {
    Header: 'Hiring reason',
    accessor: 'staffingReason'
  },
  {
    Header: 'New hire or internal staffing',
    accessor: 'newHireOrInternalStaffing'
  },
  {
    Header: 'Hire effective date',
    Cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        noWrap
        showLocalTimezone
        date={(props.value as unknown) as Date}
      />
    ),
    accessor: 'effectiveDate'
  },
  {
    Header: 'Import date',
    Cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        noWrap
        showLocalTimezone
        date={(props.value as unknown) as Date}
      />
    ),
    accessor: 'createdTs'
  },
  {
    Header: 'Invite date',
    Cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        noWrap
        showLocalTimezone
        date={(props.value as unknown) as Date}
      />
    ),
    accessor: 'inviteDate'
  },
  {
    Header: 'Reminder 1 date',
    Cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        noWrap
        showLocalTimezone
        date={(props.value as unknown) as Date}
      />
    ),
    accessor: 'reminder1Date'
  },
  {
    Header: 'Reminder 2 date',
    Cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        noWrap
        showLocalTimezone
        date={(props.value as unknown) as Date}
      />
    ),
    accessor: 'reminder2Date'
  },
  {
    Header: 'Deadline date',
    Cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        noWrap
        showLocalTimezone
        date={(props.value as unknown) as Date}
      />
    ),
    accessor: 'deadlineDate'
  },
  {
    Header: 'Status',
    Cell: (props: EmployeeCellProps): JSX.Element => (
      <>{((props.value as unknown) as EmployeeStatus).displayName}</>
    ),
    accessor: 'currentEmployeeStatusCode'
  },
  {
    Header: 'Last modified time',
    Cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        date={(props.value as unknown) as Date}
        showTime
        showLocalTimezone
      />
    ),
    accessor: 'modifiedTs'
  },
  {
    Header: 'Timeline entries',
    accessor: 'timelineEntryCount'
  }
]
