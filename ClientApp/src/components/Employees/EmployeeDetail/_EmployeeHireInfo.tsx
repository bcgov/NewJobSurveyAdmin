import React from 'react'

import { Employee } from '../../../types/Employee'
import { labelFor } from '../../../helpers/labelHelper'
import CLText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'

interface Props {
  employee: Employee
}

const EmployeeHireInfo = ({ employee: e }: Props): JSX.Element => {
  return (
    <div className="EmployeeHireInfo row">
      <CLText label={labelFor('newHireOrInternalStaffing')}>
        {e.newHireOrInternalStaffing}
      </CLText>
      <CLText label={labelFor('staffingReason')}>{e.staffingReason}</CLText>
      <CLText label={labelFor('recordCount')}>{e.recordCount}</CLText>
      <CLText label={labelFor('locationCity')}>{e.locationCity}</CLText>
      <CLText label={labelFor('serviceYears')}>{e.serviceYears}</CLText>
    </div>
  )
}

export default EmployeeHireInfo
