import React from 'react'

import { Employee } from '../../../types/Employee'
import { labelFor } from '../../../helpers/labelHelper'
import CLText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import Date from '../../DisplayHelpers/FormattedDate'

interface Props {
  employee: Employee
}

const EmployeePriorJobInfo = ({ employee: e }: Props): JSX.Element => {
  return (
    <div className='bg-light'>
      <div className="row">
        <CLText label={labelFor('effectiveDate')}>
          <Date date={e.priorEffectiveDate} />
        </CLText>
      </div>
      <hr />
      <div className="row">
        <CLText label={labelFor('ministry')}>{e.priorOrganization}</CLText>
        <CLText label={labelFor('departmentId')}>{e.priorDepartmentId}</CLText>
        <CLText label={labelFor('appointmentStatus')}>
          {e.priorAppointmentStatus!.code}
        </CLText>
        <CLText label={labelFor('classification')}>
          {e.priorClassification}
        </CLText>
        <CLText label={labelFor('jobCode')}>{e.priorJobCode}</CLText>
        <CLText label={labelFor('positionTitle')}>
          {e.priorPositionTitle}
        </CLText>
        <CLText label={labelFor('positionCode')}>{e.priorPositionCode}</CLText>
      </div>
    </div>
  )
}

export default EmployeePriorJobInfo
