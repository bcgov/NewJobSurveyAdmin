import { dateOrUndefined } from '../helpers/dateHelper'
import { EmployeeTimelineEntry } from './EmployeeTimelineEntry'
import { EmployeeStatus, EmployeeStatusEnum } from './EmployeeStatus'
import { AppointmentStatus, AppointmentStatusEnum } from './AppointmentStatus'
import { Transform, Type } from 'class-transformer'

export class Employee {
  public age?: string
  public chipsCity?: string
  public chipsEmail?: string
  public chipsFirstName?: string
  public chipsLastName?: string
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
  public ldapCity?: string
  public ldapEmail?: string
  public ldapFirstName?: string
  public ldapLastName?: string
  public ldapOrganization?: string
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
  @Transform(({ value }: { value: string }) => dateOrUndefined(value))
  public birthDate?: Date
  @Transform(({ value }: { value: string }) => dateOrUndefined(value))
  public effectiveDate?: Date
  @Transform(({ value }: { value: string }) => dateOrUndefined(value))
  public priorEffectiveDate?: Date
  @Transform(({ value }: { value: string }) => dateOrUndefined(value))
  public inviteDate?: Date
  @Transform(({ value }: { value: string }) => dateOrUndefined(value))
  public reminder1Date?: Date
  @Transform(({ value }: { value: string }) => dateOrUndefined(value))
  public reminder2Date?: Date
  @Transform(({ value }: { value: string }) => dateOrUndefined(value))
  public deadlineDate?: Date

  // UTC Datetimes
  @Transform(({ value }: { value: string }) => dateOrUndefined(value, true))
  public createdTs?: Date
  @Transform(({ value }: { value: string }) => dateOrUndefined(value, true))
  public modifiedTs?: Date

  // Fields requiring custom transformation annotations
  @Transform(({ value }: { value: AppointmentStatusEnum }) =>
    AppointmentStatus.fromKey(value)
  )
  public appointmentStatus?: AppointmentStatus
  @Transform(({ value }: { value: AppointmentStatusEnum }) =>
    AppointmentStatus.fromKey(value)
  )
  public priorAppointmentStatus?: AppointmentStatus

  @Transform(({ value }: { value: EmployeeStatusEnum }) =>
    EmployeeStatus.fromKey(value)
  )
  public currentEmployeeStatusCode?: EmployeeStatus

  @Type(() => EmployeeTimelineEntry)
  public timelineEntries!: EmployeeTimelineEntry[]

  get timelineEntryCount(): number {
    return this.timelineEntries.length
  }
}
