import moment from 'moment'
import React from 'react'

import { AdminSetting } from '../../types/AdminSetting'
import EditableAdminVariable from './EditableAdminVariable'

interface Props {
  adminSetting?: AdminSetting
  basePullDate?: moment.Moment
  dayNum: number
  eventName: string
  plusDaysToNext?: number
  setAdminSettings?: (adminSettings: AdminSetting[]) => void
  startDay?: number
}

const DatePreviewDate = ({
  adminSetting,
  basePullDate,
  dayNum,
  eventName,
  plusDaysToNext,
  setAdminSettings
}: Props): JSX.Element => {
  const date = basePullDate?.clone().add(dayNum, 'days')

  return (
    <div className="DatePreviewDate my-3">
      <div className="row">
        <div className="col-6">
          <h3 className="mb-1">
            <span className="text-muted">{eventName}</span>
            <br />
            {date?.format('dddd, MMM D')}
          </h3>
          <p className="mb-0">
            <span className="text-muted">(day {dayNum})</span>
          </p>
          {plusDaysToNext && (
            <p>
              + <strong>{plusDaysToNext}</strong> days
            </p>
          )}
        </div>
        {adminSetting && setAdminSettings && (
          <EditableAdminVariable
            adminSetting={adminSetting}
            setAdminSettings={setAdminSettings}
          />
        )}
      </div>
    </div>
  )
}

export default DatePreviewDate
