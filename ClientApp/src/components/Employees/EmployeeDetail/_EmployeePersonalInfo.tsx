import React from 'react'

import { Employee } from '../../../types/Employee'
import { labelFor, labelForWithFlag } from '../../../helpers/labelHelper'
import CLText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import Date from '../../DisplayHelpers/FormattedDate'
import EditableStringField from '../EditableStringField'

interface Props {
  employee: Employee
  populateData: () => void
}

const EmployeePersonalInfo = ({ employee: e, populateData }: Props): JSX.Element => {
  return (
    <div className="row">
      <CLText label={labelFor('firstName')}>{e.firstName}</CLText>
      <CLText label={labelForWithFlag('preferredFirstName', e)}>
        <EditableStringField
          employeeDatabaseId={e.id!}
          fieldName={'preferredFirstName'}
          fieldValue={e.preferredFirstName!}
          refreshDataCallback={populateData}
        />
      </CLText>
      <CLText label={labelFor('lastName')}>{e.lastName}</CLText>
      <CLText label={labelFor('gender')}>{e.gender}</CLText>
      <CLText label={labelFor('birthDate')}>
        <Date date={e.birthDate} />
      </CLText>
      <CLText label={labelFor('age')}>{e.age}</CLText>
    </div>
  )
}

export default EmployeePersonalInfo
