import { plainToClass } from 'class-transformer'
import React, { type JSX } from 'react';

import { FixTypeLater } from '../../types/FixTypeLater'
import { TaskLogEntry } from '../../types/TaskLogEntry'
import { taskLogEntryFilters } from '../Filters/Presets/FieldSets/taskLogEntryFilters'
import { taskLogEntryTableColumns } from './taskLogEntryTableColumns'
import GenericListing from '../Listings/GenericListing'

const TaskLogEntryListing = (): JSX.Element => {
  const dataMapperCallback = React.useCallback(
    (responseJSON: FixTypeLater[]): TaskLogEntry[] =>
      responseJSON.map(t => plainToClass(TaskLogEntry, t)),
    []
  )

  const exportedDataMapperCallback = React.useCallback(
    (responseJSON: FixTypeLater[]): FixTypeLater[] =>
      responseJSON.map(t => {
        delete t.task
        delete t.taskOutcome
        return t
      }),
    []
  )

  const taskLogEntryTableCols = taskLogEntryTableColumns()

  return (
    <GenericListing
      modelName="task log entries"
      filterableFields={taskLogEntryFilters}
      columns={taskLogEntryTableCols}
      listingPath="taskLogEntries"
      dataMapper={dataMapperCallback}
      sortProp={`&sorts=-createdTs`} // By default, sort reverse chronologically
      exportedDataMapper={exportedDataMapperCallback}
    />
  )
}

export default TaskLogEntryListing
