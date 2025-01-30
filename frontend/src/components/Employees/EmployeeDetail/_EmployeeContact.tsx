import React, { type JSX } from 'react';
import * as EmailValidator from 'email-validator'

import { Employee } from '../../../types/Employee'
import { labelFor, labelForWithFlag } from '../../../helpers/labelHelper'
import CLText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import EditableStringField from '../../DisplayHelpers/Interface/EditableFields/EditableStringField'

interface Props {
  employee: Employee
  populateData: () => void
}

const EmployeeContact = ({ employee: e, populateData }: Props): JSX.Element => {
  return (
    <div className="row">
      <CLText label={labelFor('governmentEmail')}>{e.governmentEmail}</CLText>
      <CLText label={labelForWithFlag('preferredEmail', e)}>
        <EditableStringField
          validator={(email: string): boolean =>
            email.length === 0 || EmailValidator.validate(email)
          }
          modelDatabaseId={e.id!}
          fieldName={'preferredEmail'}
          fieldValue={e.preferredEmail!}
          refreshDataCallback={populateData}
        />
      </CLText>
    </div>
  )
}

export default EmployeeContact
