import { CellContext, ColumnDef } from '@tanstack/react-table';
import React, { type JSX } from 'react';

import { TaskLogEntry } from '../../types/TaskLogEntry';
import FormattedDate from '../DisplayHelpers/FormattedDate';
import TaskComment from './TaskComment';
import TaskOutcome from './TaskOutcome';
import { FixTypeLater } from '../../types/FixTypeLater';

type TaskLogEntryCellProps = CellContext<TaskLogEntry, FixTypeLater>;

export const taskLogEntryTableColumns = (): ColumnDef<TaskLogEntry>[] => [
  {
    header: 'Date',
    cell: (props: TaskLogEntryCellProps): JSX.Element => (
      <FormattedDate
        date={props.getValue() as Date}
        showTime
        showLocalTimezone
      />
    ),
    accessorKey: 'createdTs'
  },
  {
    header: 'Task',
    accessorKey: 'taskCode'
  },
  {
    header: 'Status',
    cell: (props: TaskLogEntryCellProps): JSX.Element => (
      <TaskOutcome taskOutcomeCode={props.getValue() as string} />
    ),
    accessorKey: 'taskOutcomeCode'
  },
  {
    header: 'Comment',
    cell: (props: TaskLogEntryCellProps): JSX.Element => (
      <TaskComment comment={props.getValue() as string} />
    ),
    accessorKey: 'comment'
  }
];
