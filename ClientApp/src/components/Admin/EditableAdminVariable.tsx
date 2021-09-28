/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { plainToClass } from 'class-transformer'
import React from 'react'

import { AdminSetting } from '../../types/AdminSetting'
import { FixTypeLater } from '../../types/FixTypeLater'
import { requestJSONWithErrorHandler } from '../../helpers/requestHelpers'
import ColumnarLabelledText from '../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import EditableStringField from '../Employees/EditableStringField'

interface Props {
  adminSetting: AdminSetting
  setAdminSettings: (adminSettings: AdminSetting[]) => void
}

const EditableAdminVariable = ({
  adminSetting,
  setAdminSettings
}: Props): JSX.Element => {
  return (
    <ColumnarLabelledText
      key={adminSetting.id}
      label={adminSetting.displayName!}
      columnClass="col-6"
    >
      <EditableStringField
        modelPath={'adminSettings'}
        validator={(value: string): boolean => {
          return !isNaN(+value) && +value > 0
        }}
        employeeDatabaseId={adminSetting.id!}
        fieldName={'Value'}
        fieldValue={adminSetting.value!}
        ignoreAdminUserName
        refreshDataCallback={(): void => {
          requestJSONWithErrorHandler(
            `api/adminSettings`,
            'get',
            null,
            'ADMIN_SETTINGS_NOT_FOUND',
            (responseJSON: FixTypeLater[]): void => {
              setAdminSettings(
                responseJSON.map(s => plainToClass(AdminSetting, s))
              )
            }
          )
        }}
      />
    </ColumnarLabelledText>
  )
}

export default EditableAdminVariable
