import React from 'react'

import { Employee } from '../../../types/Employee'
import { labelFor } from '../../../helpers/labelHelper'
import CLText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import FormattedDate from '../../DisplayHelpers/FormattedDate'

interface Props {
  employee: Employee
}

const DATE_FORMAT = 'ddd MMM DD, YYYY'

const EmployeeSurveyDates = ({ employee: e }: Props): JSX.Element => {
  const closedDate = e.deadlineDate
    ? new Date(e.deadlineDate.getTime())
    : undefined

  // Add one day.
  closedDate?.setDate(closedDate.getDate() + 1)

  return (
    <div className="row">
      <CLText columnClass="col" label={labelFor('importDate')}>
        <FormattedDate date={e.createdTs} customFormat={DATE_FORMAT} />
      </CLText>
      <CLText columnClass="col" label={labelFor('inviteDate')}>
        <FormattedDate date={e.inviteDate} customFormat={DATE_FORMAT} />
      </CLText>
      <CLText columnClass="col" label={labelFor('reminder1Date')}>
        <FormattedDate date={e.reminder1Date} customFormat={DATE_FORMAT} />
      </CLText>
      <CLText columnClass="col" label={labelFor('reminder2Date')}>
        <FormattedDate date={e.reminder2Date} customFormat={DATE_FORMAT} />
      </CLText>
      <CLText columnClass="col" label={labelFor('deadlineDate')}>
        <FormattedDate date={e.deadlineDate} customFormat={DATE_FORMAT} />
      </CLText>
      <CLText columnClass="col" label={labelFor('closedDate')}>
        {e.deadlineDate && (
          <FormattedDate date={closedDate} customFormat={DATE_FORMAT} />
        )}
      </CLText>
    </div>
  )
}

export default EmployeeSurveyDates
