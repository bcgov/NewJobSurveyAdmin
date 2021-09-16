import React from 'react'
import { CellProps, Column } from 'react-table'
import { Link } from 'react-router-dom'

import { Employee } from '../../types/Employee'
import { EmployeeStatus } from '../../types/EmployeeStatus'
import { Reason } from '../../types/Reason'
import FormattedDate from '../DisplayHelpers/FormattedDate'
import { AppointmentStatus } from '../../types/AppointmentStatus'

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
    Header: 'Record count',
    accessor: 'recordCount'
  },
  {
    Header: 'Appointment status',
    Cell: (props: EmployeeCellProps): JSX.Element => {
      const appointmentStatus = (props.value as unknown) as AppointmentStatus
      return (
        <>
          {appointmentStatus
            ? appointmentStatus.code
            : '[Unknown Appointment Status]'}
        </>
      )
    },
    accessor: 'appointmentStatus'
  },
  {
    Header: 'Hire effective date',
    Cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        showLocalTimezone
        date={(props.value as unknown) as Date}
      />
    ),
    accessor: 'effectiveDate'
  },
  {
    Header: 'Hiring reason',
    Cell: (props: EmployeeCellProps): JSX.Element => {
      const staffingReason = (props.value as unknown) as Reason
      return <>{staffingReason ? staffingReason.reasonCode : '[Unknown Reason]'}</>
    },
    accessor: 'staffingReason'
  },
  {
    Header: 'Status',
    Cell: (props: EmployeeCellProps): JSX.Element => (
      <>{((props.value as unknown) as EmployeeStatus).displayName}</>
    ),
    accessor: 'currentEmployeeStatusCode'
  },
  {
    Header: 'Last modified date',
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
