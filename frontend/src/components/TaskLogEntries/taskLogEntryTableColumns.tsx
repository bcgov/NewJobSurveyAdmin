import { CellProps, Column } from 'react-table'
import React, { type JSX } from 'react';

import { TaskLogEntry } from '../../types/TaskLogEntry'
import FormattedDate from '../DisplayHelpers/FormattedDate'
import TaskComment from './TaskComment'
import TaskOutcome from './TaskOutcome'
import { FixTypeLater } from '../../types/FixTypeLater'

type TaskLogEntryCellProps = React.PropsWithChildren<
  CellProps<TaskLogEntry, FixTypeLater>
>

export const taskLogEntryTableColumns = (): Column<TaskLogEntry>[] => [
  {
    Header: 'Date',
    Cell: (props: TaskLogEntryCellProps): JSX.Element => (
      <FormattedDate
        date={(props.value as unknown) as Date}
        showTime
        showLocalTimezone
      />
    ),
    accessor: 'createdTs'
  },
  {
    Header: 'Task',
    accessor: 'taskCode'
  },
  {
    Header: 'Status',
    Cell: (props: TaskLogEntryCellProps): JSX.Element => (
      <TaskOutcome taskOutcomeCode={props.value as string} />
    ),
    accessor: 'taskOutcomeCode'
  },
  {
    Header: 'Comment',
    Cell: (props: TaskLogEntryCellProps): JSX.Element => (
      <TaskComment comment={props.value as string} />
    ),
    accessor: 'comment'
  }
]
