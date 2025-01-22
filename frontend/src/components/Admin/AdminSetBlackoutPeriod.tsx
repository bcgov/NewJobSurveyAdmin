import { plainToClass } from 'class-transformer'
import moment from 'moment-timezone'
import React, { type JSX } from 'react';

import { AdminSetting } from '../../types/AdminSetting'
import { AnyJson } from '../../types/JsonType'
import ColumnarLabelledText from '../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import EditableSelect from '../DisplayHelpers/Interface/EditableFields/EditableSelect'

interface Props {
  blackoutPeriodSetting: AdminSetting
  updateAdminSetting: (adminSetting: AdminSetting) => void
}

const OPTIONS = [
  { name: 'Yes', value: 'True' },
  { name: 'No', value: 'False' },
]

const AdminSetBlackoutPeriod = ({
  blackoutPeriodSetting,
  updateAdminSetting,
}: Props): JSX.Element => {
  if (!blackoutPeriodSetting) return <></>

  const { id, displayName, value } = blackoutPeriodSetting

  return (
    <ColumnarLabelledText key={id} label={displayName} columnClass="col-6">
      <h3 className="mt-2">
        <EditableSelect
          modelDatabaseId={id!}
          fieldName="value"
          fieldValue={value!}
          options={OPTIONS}
          modelPath={'adminSettings'}
          valueToDisplayAccessor={(value: string): string => {
            return OPTIONS.find((o) => o.value === value)!.name
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
