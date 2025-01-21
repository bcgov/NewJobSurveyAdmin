import { plainToClass } from 'class-transformer'
import React from 'react'

import { AdminSetting } from '../../types/AdminSetting'
import { FixTypeLater } from '../../types/FixTypeLater'
import { requestJSONWithErrorHandler } from '../../helpers/requestHelpers'
import DatePreview from './DatePreview'

import AdminInterfaceHelp from './AdminInterfaceHelp'
import AdminActionButtons from './AdminActionButtons'

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
      <div className="col-md-12 col-lg-10 offset-lg-1">
        <h1>Admin interface</h1>
        <div className="row">
          <div className="col-5">
            {adminSettings.length > 0 && (
              <>
                <DatePreview
                  adminSettings={adminSettings}
                  setAdminSettings={setAdminSettings}
                />
              </>
            )}
          </div>
          <div className="col-6 offset-1">
            <AdminActionButtons />
            <AdminInterfaceHelp />
          </div>
        </div>
      </div>
    </div>
  )
}

export default AdminInterface
