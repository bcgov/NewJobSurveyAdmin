import React from 'react'

import { Employee } from '../../../types/Employee'
import { labelFor } from '../../../helpers/labelHelper'
import CLText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'

interface Props {
  employee: Employee
}

const EmployeeIds = ({ employee: e }: Props): JSX.Element => {
  return (
    <div className="row">
      <CLText label={labelFor('id')}>{e.id}</CLText>
      <CLText label={labelFor('telkey')}>{e.telkey}</CLText>
      <CLText label={labelFor('governmentEmployeeId')}>
        {e.governmentEmployeeId}
      </CLText>
    </div>
  )
}

export default EmployeeIds
