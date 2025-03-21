import React, { type JSX } from 'react';
import { plainToClass } from 'class-transformer';
import timezone from 'dayjs/plugin/timezone';
import dayjs from 'dayjs';

import { Employee } from '../../types/Employee'
import { employeeFilters } from '../Filters/Presets/FieldSets/employeeFilters'
import { employeeTableColumns } from './employeeTableColumns'
import { FixTypeLater } from '../../types/FixTypeLater'
import EmployeePresets from '../Filters/Presets/EmployeePresets'
import GenericListing from '../Listings/GenericListing'
import { defaultDateFormat } from '../../helpers/dateHelper'

const EmployeeListing = (): JSX.Element => {
  const dataMapperCallback = React.useCallback(
    (responseJSON: FixTypeLater[]): Employee[] =>
      responseJSON.map((e) => plainToClass(Employee, e)),
    []
  )

  dayjs.extend(timezone);

  const exportedDataMapperCallback = React.useCallback(
    (responseJSON: FixTypeLater[]): FixTypeLater[] =>
      responseJSON.map((e) => {
        delete e.timelineEntries
        delete e.currentEmployeeStatus
        e.birthDate = dayjs(e.birthDate).format(defaultDateFormat)
        e.originalHireDate = dayjs(e.originalHireDate).format(
          defaultDateFormat
        )
        e.lastDayWorkedDate = dayjs(e.lastDayWorkedDate).format(
          defaultDateFormat
        )
        e.effectiveDate = dayjs(e.effectiveDate).format(defaultDateFormat)
        e.leaveDate = dayjs(e.leaveDate).format(defaultDateFormat)
        return e
      }),
    []
  )

  const cols = employeeTableColumns();

  return (
    <GenericListing
      sortProp={`&sorts=-modifiedTs`} // By default, sort by last modified
      modelName="employees"
      filterableFields={employeeFilters}
      columns={cols}
      presetComponent={EmployeePresets}
      listingPath="employees"
      dataMapper={dataMapperCallback}
      exportedDataMapper={exportedDataMapperCallback}
    />
  )
}

export default EmployeeListing
