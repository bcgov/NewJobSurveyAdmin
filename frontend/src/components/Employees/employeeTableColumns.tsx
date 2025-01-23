import { CellContext, ColumnDef } from '@tanstack/react-table'
import { Link } from 'react-router'
import React, { type JSX } from 'react';

import { Employee } from '../../types/Employee'
import { EmployeeStatus } from '../../types/EmployeeStatus'
import FormattedDate from '../DisplayHelpers/FormattedDate'
import { FixTypeLater } from '../../types/FixTypeLater'

type EmployeeCellProps = CellContext<Employee, FixTypeLater>

export const employeeTableColumns = (): ColumnDef<Employee>[] => [
  {
    header: 'Telkey',
    cell: (props: EmployeeCellProps): JSX.Element => (
      <Link to={`/employees/${props.cell.row.original.id}`}>{props.getValue()}</Link>
    ),
    accessorKey: 'telkey'
  },
  {
    header: 'Employee ID',
    accessorKey: 'governmentEmployeeId'
  },
  {
    header: 'Preferred first name',
    accessorKey: 'preferredFirstName'
  },
  {
    header: 'Last name',
    accessorKey: 'lastName'
  },
  {
    header: 'Preferred email',
    accessorKey: 'preferredEmail'
  },
  {
    header: 'Hiring reason',
    accessorKey: 'staffingReason'
  },
  {
    header: 'New hire or internal staffing',
    accessorKey: 'newHireOrInternalStaffing'
  },
  {
    header: 'Hire effective date',
    cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        noWrap
        showLocalTimezone
        date={(props.getValue() as unknown) as Date}
      />
    ),
    accessorKey: 'effectiveDate'
  },
  {
    header: 'Import date',
    cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        noWrap
        showLocalTimezone
        date={(props.getValue() as unknown) as Date}
      />
    ),
    accessorKey: 'createdTs'
  },
  {
    header: 'Invite date',
    cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        noWrap
        showLocalTimezone
        date={(props.getValue() as unknown) as Date}
      />
    ),
    accessorKey: 'inviteDate'
  },
  {
    header: 'Deadline date',
    cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        noWrap
        showLocalTimezone
        date={(props.getValue() as unknown) as Date}
      />
    ),
    accessorKey: 'deadlineDate'
  },
  {
    header: 'Status',
    cell: (props: EmployeeCellProps): JSX.Element => (
      <>{((props.getValue() as unknown) as EmployeeStatus).displayName}</>
    ),
    accessorKey: 'currentEmployeeStatusCode'
  },
  {
    header: 'Last modified time',
    cell: (props: EmployeeCellProps): JSX.Element => (
      <FormattedDate
        date={(props.getValue() as unknown) as Date}
        showTime
        showLocalTimezone
      />
    ),
    accessorKey: 'modifiedTs'
  },
  {
    header: 'Timeline entries',
    accessorKey: 'timelineEntryCount'
  }
]
