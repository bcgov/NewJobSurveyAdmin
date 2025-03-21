import timezone from 'dayjs/plugin/timezone';
import dayjs from 'dayjs';
import React, { useContext, type JSX } from 'react';

import { FilterDispatch } from '../../FilterForm'
import { FixTypeLater } from '../../../../types/FixTypeLater'
import DateFilter from '../../FilterClasses/DateFilter'
import IconButton from '../../../DisplayHelpers/Interface/Buttons/IconButton'

export const getPreviousFiscalYearFilter = (): DateFilter => {
  dayjs.extend(timezone);
  let startDate = dayjs();
  // Jan - 0
  // Feb - 1
  // Mar - 2
  // Apr - 3
  const currentYearApril = dayjs().month(3).startOf('month');

  if (startDate.isBefore(currentYearApril)) {
    startDate = startDate.subtract(1, 'year');
  }
  startDate = startDate.subtract(1, 'year').month(3).startOf('month');
  const endDate = dayjs(startDate).add(1, 'year').month(2).endOf('month');

  return new DateFilter('effectiveDate', startDate.toDate(), endDate.toDate());
}

interface Props {
  submitId: number
  setSubmitId: (submitId: number) => void
}

const SetPreviousFiscalYear = ({
  submitId,
  setSubmitId,
}: Props): JSX.Element => {
  const dispatch = useContext(FilterDispatch) as FixTypeLater

  const setPreviousFiscalYear = React.useCallback((): void => {
    dispatch({
      type: 'setFilter',
      filter: getPreviousFiscalYearFilter(),
    })
    setSubmitId(submitId + 1)
  }, [dispatch, submitId, setSubmitId])

  return (
    <FilterDispatch.Provider value={dispatch}>
      <IconButton
        label="Previous fiscal year"
        iconName="calendar-alt"
        colorType="outline-primary"
        marginClasses="me-2"
        iconMarginClasses="me-2"
        buttonClasses="btn-sm"
        onClick={setPreviousFiscalYear}
      />
    </FilterDispatch.Provider>
  )
}

export default SetPreviousFiscalYear
