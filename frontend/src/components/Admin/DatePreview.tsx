/* eslint-disable @typescript-eslint/no-non-null-assertion */
import React, { type JSX } from 'react';
import timezone from 'dayjs/plugin/timezone';
import utc from 'dayjs/plugin/utc';
import dayjs from 'dayjs';

import { AdminSetting, AdminSettingKeyEnum } from '../../types/AdminSetting'
import AdminDataPullDayOfWeek from './AdminDataPullDayOfWeek'
import DatePreviewDate from './DatePreviewDate'

import './DatePreview.scss'
import AdminSetBlackoutPeriod from './AdminSetBlackoutPeriod'

interface Props {
  adminSettings: AdminSetting[]
  setAdminSettings: (adminSettings: AdminSetting[]) => void
}

const DatePreview = ({
  adminSettings,
  setAdminSettings,
}: Props): JSX.Element => {
  const blackoutPeriodSetting = adminSettings.find(
    (as) => as.key === AdminSettingKeyEnum.IsBlackoutPeriod
  )!
  const isBlackoutPeriodActive = blackoutPeriodSetting.value === 'True'

  // NB: We are using ISO DAY OF WEEK, where 1 is Monday and 7 is Sunday
  // So subtract 1 when adding to startOf('isoWeek'), which is a Monday
  const dataPullSetting = adminSettings.find(
    (as) => as.key === AdminSettingKeyEnum.DataPullDayOfWeek
  )!
  const dataPullDayOfWeek = +dataPullSetting!.value! - 1

  const inviteDaysSetting = adminSettings.find(
    (as) => as.key === AdminSettingKeyEnum.InviteDays
  )
  const inviteDays = +inviteDaysSetting!.value!

  const reminder1DaysSetting = adminSettings.find(
    (as) => as.key === AdminSettingKeyEnum.Reminder1Days
  )
  const reminder1Days = +reminder1DaysSetting!.value!

  const reminder2DaysSetting = adminSettings.find(
    (as) => as.key === AdminSettingKeyEnum.Reminder2Days
  )
  const reminder2Days = +reminder2DaysSetting!.value!

  const apparentCloseDaysSetting = adminSettings.find(
    (as) => as.key === AdminSettingKeyEnum.CloseDays
  )
  const apparentCloseDays = +apparentCloseDaysSetting!.value!

  const realCloseDays = 1

  dayjs.extend(utc);
  dayjs.extend(timezone);

  const basePullDate = dayjs()
    .startOf('isoWeek')
    .add(dataPullDayOfWeek, 'days');
  const now = dayjs();

  if (now.unix() > dayjs(basePullDate).add(1, 'days').unix()) {
    basePullDate.add(1, 'week')
  }

  const updateAdminSetting = React.useCallback(
    (newSetting: AdminSetting) => {
      // Remove the old pull day setting
      const updatedAdminSettings = adminSettings.filter(
        (a) => a.key !== newSetting.key
      )
      updatedAdminSettings.push(newSetting)
      setAdminSettings(updatedAdminSettings)
    },
    [adminSettings, setAdminSettings]
  )

  return (
    <div className="DatePreview">
      <div className="row mb-3">
        <AdminDataPullDayOfWeek
          updateAdminSetting={updateAdminSetting}
          dataPullSetting={dataPullSetting}
        />
        <AdminSetBlackoutPeriod
          updateAdminSetting={updateAdminSetting}
          blackoutPeriodSetting={blackoutPeriodSetting}
        />
      </div>
      <div className="row DatePreviewDates">
        <DatePreviewDate
          isBlackoutPeriodActive={isBlackoutPeriodActive}
          adminSetting={inviteDaysSetting}
          updateAdminSetting={updateAdminSetting}
          dayNum={0}
          basePullDate={basePullDate}
          eventName={'Next data pull'}
          plusDaysToNext={inviteDays}
          icon={'fa-download'}
          color={'primary'}
        />
        <DatePreviewDate
          isBlackoutPeriodActive={isBlackoutPeriodActive}
          adminSetting={reminder1DaysSetting}
          updateAdminSetting={updateAdminSetting}
          dayNum={0 + inviteDays}
          basePullDate={basePullDate}
          eventName={'Invite'}
          plusDaysToNext={reminder1Days}
          icon={'fa-envelope-open-text'}
          color={'success'}
        />
        <DatePreviewDate
          isBlackoutPeriodActive={isBlackoutPeriodActive}
          adminSetting={reminder2DaysSetting}
          updateAdminSetting={updateAdminSetting}
          dayNum={0 + inviteDays + reminder1Days}
          basePullDate={basePullDate}
          eventName={'Reminder 1'}
          plusDaysToNext={reminder2Days}
          icon={'fa-envelope-open-text'}
          color={'warning'}
        />
        <DatePreviewDate
          isBlackoutPeriodActive={isBlackoutPeriodActive}
          adminSetting={apparentCloseDaysSetting}
          updateAdminSetting={updateAdminSetting}
          dayNum={0 + inviteDays + reminder1Days + reminder2Days}
          basePullDate={basePullDate}
          eventName={'Reminder 2'}
          plusDaysToNext={apparentCloseDays}
          icon={'fa-envelope-open-text'}
          color={'warning'}
        />
        <DatePreviewDate
          isBlackoutPeriodActive={isBlackoutPeriodActive}
          updateAdminSetting={updateAdminSetting}
          dayNum={
            0 + inviteDays + reminder1Days + reminder2Days + apparentCloseDays
          }
          basePullDate={basePullDate}
          eventName={'User told survey closed'}
          plusDaysToNext={realCloseDays}
          icon={'fa-envelope-open-text'}
          color={'danger'}
        />
        <DatePreviewDate
          isBlackoutPeriodActive={isBlackoutPeriodActive}
          updateAdminSetting={updateAdminSetting}
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
          icon={'fa-ban'}
          color={'primary'}
        />
      </div>
    </div>
  )
}

export default DatePreview
