import { plainToClass } from 'class-transformer'
import moment from 'moment'
import React from 'react'

import { AdminSetting } from '../../types/AdminSetting'
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
  plusDaysToNext?: number
  updateAdminSetting: (adminSetting: AdminSetting) => void
  startDay?: number
}

const DatePreviewDate = ({
  adminSetting,
  basePullDate,
  color,
  dayNum,
  eventName,
  icon,
  plusDaysToNext,
  updateAdminSetting
}: Props): JSX.Element => {
  const date = basePullDate?.clone().add(dayNum, 'days')

  return (
    <div className="DatePreviewDate col-12">
      <div className="row align-items-center">
        <div className="col-8">
          <div
            className={`Dates border bg-${color}-light border-${color} p-3 shadow d-flex align-items-center`}
          >
            <div className={`mr-3 text-${color}`} style={{ maxWidth: '50px' }}>
              <i className={`fas fa-lg ${icon}`} />
            </div>
            <div>
              <h4 className="mb-2 text-muted">{eventName}</h4>
              <h3 className="mb-0">{date?.format('dddd, MMM D')}</h3>
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
