import timezone from 'dayjs/plugin/timezone';
import dayjs from 'dayjs';
import React, { useContext, type JSX } from 'react';

import { FilterDispatch } from '../../FilterForm'
import { FixTypeLater } from '../../../../types/FixTypeLater'
import DateFilter from '../../FilterClasses/DateFilter'
import IconButton from '../../../DisplayHelpers/Interface/Buttons/IconButton'

export const getPreviousMonthFilter = (): DateFilter => {
  dayjs.extend(timezone);
  const startDate = dayjs().subtract(1, 'month').startOf('month');
  const endDate = dayjs(startDate).endOf('month');
  return new DateFilter('effectiveDate', startDate.toDate(), endDate.toDate());
}

interface Props {
  submitId: number
  setSubmitId: (submitId: number) => void
}

const SetPreviousMonth = ({ submitId, setSubmitId }: Props): JSX.Element => {
  const dispatch = useContext(FilterDispatch) as FixTypeLater

  const setPreviousMonth = React.useCallback((): void => {
    dispatch({
      type: 'setFilter',
      filter: getPreviousMonthFilter(),
    })
    setSubmitId(submitId + 1)
  }, [dispatch, submitId, setSubmitId])

  return (
    <FilterDispatch.Provider value={dispatch}>
      <IconButton
        label="Previous month"
        iconName="calendar-minus"
        colorType="outline-primary"
        marginClasses="me-2"
        iconMarginClasses="me-2"
        buttonClasses="btn-sm"
        onClick={setPreviousMonth}
      />
    </FilterDispatch.Provider>
  )
}

export default SetPreviousMonth
