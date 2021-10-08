import { plainToClass } from 'class-transformer'
import React from 'react'

import { AdminSetting } from '../../types/AdminSetting'
import { FixTypeLater } from '../../types/FixTypeLater'
import { requestJSONWithErrorHandler } from '../../helpers/requestHelpers'
import DatePreview from './DatePreview'
import RefreshStatusButton from './RefreshStatusButton'

const AdminInterface = (): JSX.Element => {
  const [adminSettings, setAdminSettings] = React.useState<AdminSetting[]>([])

  React.useEffect((): void => {
    requestJSONWithErrorHandler(
      `api/adminSettings`,
      'get',
      null,
      'ADMIN_SETTINGS_NOT_FOUND',
      (responseJSON: FixTypeLater[]): void => {
        setAdminSettings(responseJSON.map(s => plainToClass(AdminSetting, s)))
      }
    )
  }, [])

  return (
    <div className="Centered AdminInterface row">
      <div className="col-md-10 offset-md-1 col-lg-8 offset-lg-2 col-xl-6 offset-xl-3">
        <h1>Admin interface</h1>
        {adminSettings.length > 0 && (
          <>
            <DatePreview
              adminSettings={adminSettings}
              setAdminSettings={setAdminSettings}
            />
            <div className="row mt-4">
              <RefreshStatusButton />
            </div>
          </>
        )}
      </div>
    </div>
  )
}

export default AdminInterface
