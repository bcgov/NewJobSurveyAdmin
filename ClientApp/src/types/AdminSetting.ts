import { dateOrUndefined } from '../helpers/dateHelper'
import { Transform } from 'class-transformer'

export class AdminSetting {
  id?: string
  key?: string
  displayName?: string
  value?: string

  @Transform(({ value }: { value: string }) => dateOrUndefined(value, true))
  createdTs?: Date

  @Transform(({ value }: { value: string }) => dateOrUndefined(value, true))
  modifiedTs?: Date
}

export enum AdminSettingKeyEnum {
  IsBlackoutPeriod = 'IsBlackoutPeriod',
  EmployeeExpirationThreshold = 'EmployeeExpirationThreshold',
  DataPullDayOfWeek = 'DataPullDayOfWeek',
  InviteDays = 'InviteDays',
  Reminder1Days = 'Reminder1Days',
  Reminder2Days = 'Reminder2Days',
  CloseDays = 'CloseDays'
}
