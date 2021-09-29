import React from 'react'

import { Employee } from '../../../types/Employee'
import { labelFor } from '../../../helpers/labelHelper'
import CLText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import Date from '../../DisplayHelpers/FormattedDate'

interface Props {
  employee: Employee
}

const BASE_COL_CLASS = 'col-12'

const EmployeePriorJobInfo = ({ employee: e }: Props): JSX.Element => {
  const hasPriorJobInfo =
    e.priorPositionTitle && e.priorPositionTitle.length > 0
  return (
    <div
      className="text-secondary bg-light border border-secondary p-3"
      style={{ zIndex: 500, marginTop: '8px', minHeight: '500px' }}
    >
      <div className="row">
        <div className={BASE_COL_CLASS}>
          <h2>Prior job info</h2>
        </div>
        {!hasPriorJobInfo && (
          <div className="col">No prior job information.</div>
        )}
        {hasPriorJobInfo && (
          <>
            <CLText
              columnClass={BASE_COL_CLASS}
              label={labelFor('effectiveDate')}
            >
              <Date date={e.priorEffectiveDate} />
            </CLText>
            <CLText
              columnClass={BASE_COL_CLASS}
              label={labelFor('appointmentStatus')}
            >
              {e.priorAppointmentStatus?.code}
            </CLText>
            <div className="col">
              <hr />
            </div>
            <CLText
              columnClass={BASE_COL_CLASS}
              label={labelFor('positionTitle')}
            >
              {e.priorPositionTitle}
            </CLText>
            <CLText
              columnClass={BASE_COL_CLASS}
              label={labelFor('positionCode')}
            >
              {e.priorPositionCode}
            </CLText>
            <CLText columnClass={BASE_COL_CLASS} label={labelFor('ministry')}>
              {e.priorOrganization}
            </CLText>
            <CLText
              columnClass={BASE_COL_CLASS}
              label={labelFor('departmentId')}
            >
              {e.priorDepartmentId}
            </CLText>
            <CLText
              columnClass={BASE_COL_CLASS}
              label={labelFor('classification')}
            >
              {e.priorClassification}
            </CLText>
            <CLText columnClass={BASE_COL_CLASS} label={labelFor('jobCode')}>
              {e.priorJobCode}
            </CLText>
          </>
        )}
      </div>
    </div>
  )
}

export default EmployeePriorJobInfo
