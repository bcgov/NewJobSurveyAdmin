import { dateOrUndefined } from '../helpers/dateHelper'
import { EmployeeTimelineEntry } from './EmployeeTimelineEntry'
import { EmployeeStatus, EmployeeStatusEnum } from './EmployeeStatus'
import { AppointmentStatus, AppointmentStatusEnum } from './AppointmentStatus'
import { Transform, Type } from 'class-transformer'

export class Employee {
  public age?: string
  public chipsEmail?: string
  public classification?: string
  public currentEmployeeStatus?: string
  public departmentId?: string
  public departmentIdDescription?: string
  public developmentRegion?: string
  public firstName?: string
  public fullName?: string
  public gender?: string
  public governmentEmail?: string
  public governmentEmployeeId?: string
  public id?: string
  public jobClassificationGroup?: string
  public jobCode?: string
  public lastName?: string
  public locationCity?: string
  public locationGroup?: string
  public middleName?: string
  public newHireOrInternalStaffing?: string
  public nocCode?: string
  public nocDescription?: string
  public organization?: string
  public organizationCount?: string
  public positionCode?: string
  public positionTitle?: string
  public preferredEmail?: string
  public preferredEmailFlag?: string
  public preferredFirstName?: string
  public preferredFirstNameFlag?: boolean
  public priorClassification?: string
  public priorDepartmentId?: string
  public priorDepartmentIdDescription?: string
  public priorEmployeeStatus?: string
  public priorJobClassificationGroup?: string
  public priorJobCode?: string
  public priorNocCode?: string
  public priorNocDescription?: string
  public priorOrganization?: string
  public priorPositionCode?: string
  public priorPositionTitle?: string
  public priorUnionCode?: string
  public recordCount?: string
  public regionalDistrict?: string
  public serviceYears?: string
  public staffingAction?: string
  public staffingReason?: string
  public taToPermanent?: string
  public telkey?: string
  public triedToUpdateInFinalState?: boolean
  public unionCode?: string

  // Dates
  @Transform((date: string) => dateOrUndefined(date))
  public birthDate?: Date
  @Transform((date: string) => dateOrUndefined(date))
  public effectiveDate?: Date
  @Transform((date: string) => dateOrUndefined(date))
  public priorEffectiveDate?: Date

  // UTC Datetimes
  @Transform((date: string) => dateOrUndefined(date, true))
  public createdTs?: Date
  @Transform((date: string) => dateOrUndefined(date, true))
  public modifiedTs?: Date

  // Fields requiring custom transformation annotations

  @Transform((k: AppointmentStatusEnum) => AppointmentStatus.fromKey(k))
  public appointmentStatus?: AppointmentStatus
  @Transform((k: AppointmentStatusEnum) => AppointmentStatus.fromKey(k))
  public priorAppointmentStatus?: AppointmentStatus

  @Transform((k: EmployeeStatusEnum) => EmployeeStatus.fromKey(k))
  public currentEmployeeStatusCode?: EmployeeStatus

  @Type(() => EmployeeTimelineEntry)
  public timelineEntries!: EmployeeTimelineEntry[]

  get timelineEntryCount(): number {
    return this.timelineEntries.length
  }
}
