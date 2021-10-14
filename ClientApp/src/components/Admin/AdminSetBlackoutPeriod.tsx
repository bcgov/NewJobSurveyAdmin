import { plainToClass } from 'class-transformer'
import moment from 'moment'
import React from 'react'

import { AdminSetting } from '../../types/AdminSetting'
import { AnyJson } from '../../types/JsonType'
import ColumnarLabelledText from '../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import EditableSelect from '../DisplayHelpers/Interface/EditableFields/EditableSelect'

interface Props {
  blackoutPeriodSetting: AdminSetting
  updateAdminSetting: (adminSetting: AdminSetting) => void
}

const OPTIONS = [
  { name: 'Yes', value: '1' },
  { name: 'No', value: '0' }
]

const AdminSetBlackoutPeriod = ({
  blackoutPeriodSetting,
  updateAdminSetting
}: Props): JSX.Element => {
  if (!blackoutPeriodSetting) return <></>

  const { id, displayName, value } = blackoutPeriodSetting

  return (
    <ColumnarLabelledText
      key={id}
      label={displayName}
      columnClass="col-6"
      helperText={`If "Yes," all invitation-related dates on imported employees will be set to January 1, 9999. Otherwise, employees will be imported as normal.`}
    >
      <h3 className="mt-2">
        <EditableSelect
          modelDatabaseId={id!}
          fieldName="value"
          fieldValue={value!}
          options={OPTIONS}
          modelPath={'adminSettings'}
          valueToDisplayAccessor={(value: string): string => {
            return OPTIONS.find(o => o.value === value)!.name
          }}
          refreshDataCallback={(responseJson: AnyJson): void => {
            updateAdminSetting(plainToClass(AdminSetting, responseJson))
          }}
        />
      </h3>
    </ColumnarLabelledText>
  )
}

export default AdminSetBlackoutPeriod
