import React from 'react'

import { Employee } from '../../../types/Employee'
import { labelFor } from '../../../helpers/labelHelper'
import CLText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import Date from '../../DisplayHelpers/FormattedDate'

interface Props {
  employee: Employee
}

const EmployeeCurrentJobInfo = ({ employee: e }: Props): JSX.Element => {
  return (
    <>
      <div className="row">
        <CLText label={labelFor('effectiveDate')}>
          <Date date={e.effectiveDate} />
        </CLText>
        <CLText label={labelFor('reason')}>
          {e.staffingReason ? e.staffingReason.reasonCode : '[Unknown Reason]'}
        </CLText>
        <CLText label={labelFor('recordCount')}>{e.recordCount}</CLText>
      </div>
      <hr />
      <div className="row">
        <CLText label={labelFor('ministry')}>{e.organization}</CLText>
        <CLText label={labelFor('departmentId')}>{e.departmentId}</CLText>
        <CLText label={labelFor('appointmentStatus')}>
          {e.appointmentStatus!.code}
        </CLText>
        <CLText label={labelFor('classification')}>
          {e.classification}
        </CLText>
        <CLText label={labelFor('jobCode')}>{e.jobCode}</CLText>
        <CLText label={labelFor('locationCity')}>{e.locationCity}</CLText>
        <CLText label={labelFor('serviceYears')}>{e.serviceYears}</CLText>
        <CLText label={labelFor('positionTitle')}>
          {e.positionTitle}
        </CLText>
        <CLText label={labelFor('positionCode')}>{e.positionCode}</CLText>
      </div>
    </>
  )
}

export default EmployeeCurrentJobInfo
