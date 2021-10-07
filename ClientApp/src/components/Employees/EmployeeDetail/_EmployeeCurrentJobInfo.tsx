import React from 'react'

import { Employee } from '../../../types/Employee'
import { labelFor } from '../../../helpers/labelHelper'
import CLText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import Date from '../../DisplayHelpers/FormattedDate'

interface Props {
  employee: Employee
}

const EmployeeCurrentJobInfo = ({ employee: e }: Props): JSX.Element => {
  // console.log('e', e)
  return (
    <div
      className="bg-white border border-secondary p-4 shadow-lg"
      style={{ marginLeft: '-60px', zIndex: 1000 }}
    >
      <div className="row">
        <div className="col-12">
          <h2>Current job info</h2>
        </div>
        <CLText columnClass="col-12" label={labelFor('effectiveDate')}>
          <Date date={e.effectiveDate} />
        </CLText>
        <CLText columnClass="col-12" label={labelFor('appointmentStatus')}>
          {e.appointmentStatus!.code}
        </CLText>
      </div>
      <hr />
      <div className="row">
        <CLText columnClass="col-12" label={labelFor('positionTitle')}>
          {e.positionTitle}
        </CLText>
        <CLText columnClass="col-12" label={labelFor('positionCode')}>
          {e.positionCode}
        </CLText>
        <CLText columnClass="col-12" label={labelFor('ministry')}>
          {e.organization}
        </CLText>
        <CLText columnClass="col-12" label={labelFor('departmentId')}>
          {e.departmentId}
        </CLText>
        <CLText columnClass="col-12" label={labelFor('classification')}>
          {e.classification}
        </CLText>
        <CLText columnClass="col-12" label={labelFor('jobCode')}>
          {e.jobCode}
        </CLText>
      </div>
    </div>
  )
}

export default EmployeeCurrentJobInfo
