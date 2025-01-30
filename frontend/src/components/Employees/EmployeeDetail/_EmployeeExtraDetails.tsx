import React, { type JSX } from 'react';

import { Employee } from '../../../types/Employee'
import { labelFor } from '../../../helpers/labelHelper'
import CLText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'

interface Props {
  employee: Employee
}

const EmployeeExtraDetails = ({ employee: e }: Props): JSX.Element => {
  return (
    <div className="EmployeeExtraDetails row">
      <div className="col-12">
        <h3>Data by source</h3>
      </div>
      <CLText label={labelFor('chipsFirstName')}>{e.chipsFirstName}</CLText>
      <CLText label={labelFor('ldapFirstName')}>{e.ldapFirstName}</CLText>
      <CLText label={labelFor('firstName')}>{e.firstName}</CLText>
      <CLText label={labelFor('chipsLastName')}>{e.chipsLastName}</CLText>
      <CLText label={labelFor('ldapLastName')}>{e.ldapLastName}</CLText>
      <CLText label={labelFor('lastName')}>{e.lastName}</CLText>
      <CLText label={labelFor('chipsEmail')}>{e.chipsEmail}</CLText>
      <CLText label={labelFor('ldapEmail')}>{e.ldapEmail}</CLText>
      <CLText label={labelFor('governmentEmail')}>{e.governmentEmail}</CLText>
      <CLText label={labelFor('chipsCity')}>{e.chipsCity}</CLText>
      <CLText label={labelFor('ldapCity')}>{e.ldapCity}</CLText>
      <CLText label={labelFor('locationCity')}>{e.locationCity}</CLText>
      <CLText label={labelFor('organization')}>{e.organization}</CLText>
      <CLText label={labelFor('ldapOrganization')}>{e.ldapOrganization}</CLText>
    </div>
  )
}

export default EmployeeExtraDetails
