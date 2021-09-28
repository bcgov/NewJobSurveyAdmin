import { dateOrUndefined } from '../helpers/dateHelper'
import { Transform } from 'class-transformer'

export class AdminSetting {
  id?: string
  key?: string
  displayName?: string
  value?: string

  @Transform((date: string) => dateOrUndefined(date, true))
  createdTs?: Date

  @Transform((date: string) => dateOrUndefined(date, true))
  modifiedTs?: Date
}

export enum AdminSettingKeyEnum {
  EmployeeExpirationThreshold = "EmployeeExpirationThreshold",
  DataPullDayOfWeek = "DataPullDayOfWeek",
  InviteDays = "InviteDays",
  Reminder1Days = "Reminder1Days",
  Reminder2Days = "Reminder2Days",
  CloseDays = "CloseDays",
}