import { CellProps, Column } from 'react-table'
import { Link } from 'react-router'
import React, { type JSX } from 'react';

import { Employee } from '../../types/Employee'
import { EmployeeStatus } from '../../types/EmployeeStatus'
import FormattedDate from '../DisplayHelpers/FormattedDate'
import { FixTypeLater } from '../../types/FixTypeLater'

type EmployeeCellProps = React.PropsWithChildren<
  CellProps<Employee, FixTypeLater>
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
