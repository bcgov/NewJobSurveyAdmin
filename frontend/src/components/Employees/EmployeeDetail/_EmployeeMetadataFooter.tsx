import React from 'react'

import { Employee } from '../../../types/Employee'
import { labelFor } from '../../../helpers/labelHelper'
import CLText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import Date from '../../DisplayHelpers/FormattedDate'

interface Props {
  employee: Employee
}

const EmployeeIds = ({ employee: e }: Props): JSX.Element => {
  return (
    <div className="row text-muted">
      <CLText label={labelFor('createdTs')}>
        <small>
          <Date date={e.createdTs} showTime showLocalTimezone />
        </small>
      </CLText>
      <CLText label={labelFor('modifiedTs')}>
        <small>
          <Date date={e.modifiedTs} showTime showLocalTimezone />
        </small>
      </CLText>
      <CLText label={labelFor('triedToUpdateInFinalState')}>
        {e.triedToUpdateInFinalState ? 'True' : 'False'}
      </CLText>
    </div>
  )
}

export default EmployeeIds
