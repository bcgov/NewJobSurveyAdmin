import { plainToClass } from 'class-transformer'
import moment from 'moment'
import React from 'react'

import { AdminSetting, AdminSettingKeyEnum } from '../../types/AdminSetting'
import { AnyJson } from '../../types/JsonType'
import EditableNumber from '../DisplayHelpers/Interface/EditableFields/EditableNumber'

import './DatePreviewDate.scss'

interface Props {
  adminSetting?: AdminSetting
  basePullDate?: moment.Moment
  color: string
  dayNum: number
  eventName: string
  icon: string
  isBlackoutPeriodActive: boolean
  plusDaysToNext?: number
  startDay?: number
  updateAdminSetting: (adminSetting: AdminSetting) => void
}

const DatePreviewDate = ({
  adminSetting,
  basePullDate,
  color: propColor,
  dayNum,
  eventName,
  icon,
  isBlackoutPeriodActive,
  plusDaysToNext,
  updateAdminSetting
}: Props): JSX.Element => {
  const date = basePullDate?.clone().add(dayNum, 'days')

  const color = isBlackoutPeriodActive ? 'dark' : propColor

  return (
    <div className="DatePreviewDate col-12">
      <div className="row align-items-center">
        <div className="col-12">
          <div
            className={`Dates border bg-${color}-light border-${color} p-3 shadow d-flex align-items-center`}
          >
            <div className={`mr-3 text-${color}`} style={{ maxWidth: '50px' }}>
              <i className={`fas fa-lg ${icon}`} />
            </div>
            <div>
              <h4 className="mb-2 text-muted">{eventName}</h4>
              <h3 className="mb-0">
                {isBlackoutPeriodActive && dayNum !== 0
                  ? 'Blackout period active'
                  : date?.format('dddd, MMM D')}
              </h3>
            </div>
            <div className="ml-auto">
              <span className={`badge badge-pill badge-secondary`}>
                Day {dayNum}
              </span>
            </div>
          </div>
        </div>
        {plusDaysToNext && (
          <div className="PlusDaysToNext col-6 offset-1 d-flex align-items-center my-4">
            <div>+</div>
            <div>
              <strong>
                {adminSetting && updateAdminSetting ? (
                  <EditableNumber
                    modelPath={'adminSettings'}
                    modelDatabaseId={adminSetting.id!}
                    fieldName={'Value'}
                    fieldValue={adminSetting.value!}
                    inline
                    refreshDataCallback={(responseJson: AnyJson): void => {
                      updateAdminSetting(
                        plainToClass(AdminSetting, responseJson)
                      )
                    }}
                    min={0}
                    step={1}
                  />
                ) : (
                  plusDaysToNext
                )}
              </strong>
            </div>
            <div>&nbsp;day{plusDaysToNext > 1 && 's'}</div>
          </div>
        )}
      </div>
    </div>
  )
}

export default DatePreviewDate
