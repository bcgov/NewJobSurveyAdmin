/* eslint-disable @typescript-eslint/no-non-null-assertion */
import React from 'react'
import moment from 'moment'

import { AdminSetting, AdminSettingKeyEnum } from '../../types/AdminSetting'
import { FixTypeLater } from '../../types/FixTypeLater'
import { plainToClass } from 'class-transformer'
import { requestJSONWithErrorHandler } from '../../helpers/requestHelpers'
import ColumnarLabelledText from '../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import DatePreviewDate from './DatePreviewDate'
import EditableStringField from '../Employees/EditableStringField'

interface Props {
  adminSettings: AdminSetting[]
  setAdminSettings: (adminSettings: AdminSetting[]) => void
}

const DatePreview = ({
  adminSettings,
  setAdminSettings
}: Props): JSX.Element => {
  // NB: We are using ISO DAY OF WEEK, where 1 is Monday and 7 is Sunday
  // So subtract 1 when adding to startOf('isoWeek'), which is a Monday
  const dataPullSetting = adminSettings.find(
    as => as.key === AdminSettingKeyEnum.DataPullDayOfWeek
  )!
  const dataPullDayOfWeek = +dataPullSetting!.value! - 1

  const inviteDaysSetting = adminSettings.find(
    as => as.key === AdminSettingKeyEnum.InviteDays
  )
  const inviteDays = +inviteDaysSetting!.value!

  const reminder1DaysSetting = adminSettings.find(
    as => as.key === AdminSettingKeyEnum.Reminder1Days
  )
  const reminder1Days = +reminder1DaysSetting!.value!

  const reminder2DaysSetting = adminSettings.find(
    as => as.key === AdminSettingKeyEnum.Reminder2Days
  )
  const reminder2Days = +reminder2DaysSetting!.value!

  const apparentCloseDaysSetting = adminSettings.find(
    as => as.key === AdminSettingKeyEnum.CloseDays
  )
  const apparentCloseDays = +apparentCloseDaysSetting!.value!

  const realCloseDays = 1

  const basePullDate = moment()
    .startOf('isoWeek')
    .add(dataPullDayOfWeek, 'days')
  const now = moment()

  if (now.unix() > basePullDate.unix()) {
    basePullDate.add(1, 'week')
  }

  return (
    <div className="DatePreview">
      <div className="row">
        <ColumnarLabelledText
          key={dataPullSetting.id}
          label={dataPullSetting.displayName!}
          columnClass="col"
        >
          <EditableStringField
            modelPath={'adminSettings'}
            validator={(value: string): boolean => {
              return !isNaN(+value) && +value > 0
            }}
            employeeDatabaseId={dataPullSetting.id!}
            fieldName={'Value'}
            fieldValue={dataPullSetting.value!}
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
      </div>
      <DatePreviewDate
        adminSetting={inviteDaysSetting}
        setAdminSettings={setAdminSettings}
        dayNum={0}
        basePullDate={basePullDate}
        eventName={'Next data pull'}
        plusDaysToNext={inviteDays}
      />
      <DatePreviewDate
        adminSetting={reminder1DaysSetting}
        setAdminSettings={setAdminSettings}
        dayNum={0 + inviteDays}
        basePullDate={basePullDate}
        eventName={'Invite'}
        plusDaysToNext={reminder1Days}
      />
      <DatePreviewDate
        adminSetting={reminder2DaysSetting}
        setAdminSettings={setAdminSettings}
        dayNum={0 + inviteDays + reminder1Days}
        basePullDate={basePullDate}
        eventName={'Reminder 1'}
        plusDaysToNext={reminder2Days}
      />
      <DatePreviewDate
        adminSetting={apparentCloseDaysSetting}
        setAdminSettings={setAdminSettings}
        dayNum={0 + inviteDays + reminder1Days + reminder2Days}
        basePullDate={basePullDate}
        eventName={'Reminder 2'}
        plusDaysToNext={apparentCloseDays}
      />
      <DatePreviewDate
        setAdminSettings={setAdminSettings}
        dayNum={
          0 + inviteDays + reminder1Days + reminder2Days + apparentCloseDays
        }
        basePullDate={basePullDate}
        eventName={'User told survey closed'}
        plusDaysToNext={realCloseDays}
      />
      <DatePreviewDate
        setAdminSettings={setAdminSettings}
        dayNum={
          0 +
          inviteDays +
          reminder1Days +
          reminder2Days +
          apparentCloseDays +
          realCloseDays
        }
        basePullDate={basePullDate}
        eventName={'Survey actually closed'}
      />
    </div>
  )
}

export default DatePreview
