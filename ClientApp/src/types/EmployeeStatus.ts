/* globals Map */

import { SelectOption } from '../components/DisplayHelpers/Interface/EditableFields/EditableSelect'

export enum EmployeeStatusEnum {
  Active = 'Active',
  SurveyComplete = 'SurveyComplete',
  OutOfScope = 'OutOfScope',
  Declined = 'Declined',
  Expired = 'Expired'
}

export enum EmployeeStatusStateEnum {
  Active = 'Active',
  Final = 'Final'
}

export class EmployeeStatus {
  code: EmployeeStatusEnum
  state: EmployeeStatusStateEnum
  displayName: string
  description?: string

  constructor(
    statusCode: EmployeeStatusEnum,
    statusState: EmployeeStatusStateEnum,
    displayName: string,
    description: string
  ) {
    this.code = statusCode
    this.state = statusState
    this.displayName = displayName
    this.description = description
  }

  static ACTIVE: EmployeeStatus = new EmployeeStatus(
    EmployeeStatusEnum.Active,
    EmployeeStatusStateEnum.Active,
    'Active',
    'Employee is active.'
  )
  static SURVEY_COMPLETE: EmployeeStatus = new EmployeeStatus(
    EmployeeStatusEnum.SurveyComplete,
    EmployeeStatusStateEnum.Final,
    'Survey: Complete',
    'Survey has been finished.'
  )
  static INELIGIBLE_OTHER: EmployeeStatus = new EmployeeStatus(
    EmployeeStatusEnum.OutOfScope,
    EmployeeStatusStateEnum.Final,
    'Out of scope',
    'Other ineligibility reason.'
  )
  static DECLINED: EmployeeStatus = new EmployeeStatus(
    EmployeeStatusEnum.Declined,
    EmployeeStatusStateEnum.Active,
    'Survey: Do not remind / declined',
    'The employee has asked not to complete the survey.'
  )
  static EXPIRED: EmployeeStatus = new EmployeeStatus(
    EmployeeStatusEnum.Expired,
    EmployeeStatusStateEnum.Final,
    'Survey: Expired',
    "The employee's effective date has passed without completing the survey."
  )

  static array = (): EmployeeStatus[] => [
    EmployeeStatus.ACTIVE,
    EmployeeStatus.SURVEY_COMPLETE,
    EmployeeStatus.INELIGIBLE_OTHER,
    EmployeeStatus.DECLINED,
    EmployeeStatus.EXPIRED
  ]

  static map = (): Map<EmployeeStatusEnum, EmployeeStatus> => {
    return new Map(EmployeeStatus.array().map(s => [s.code, s]))
  }

  static fromKey = (key: EmployeeStatusEnum): EmployeeStatus => {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    return EmployeeStatus.map().get(key)!
  }

  static toOptions = (): SelectOption[] => {
    return EmployeeStatus.array()
      .map(status => ({
        name: status.displayName,
        value: status.code
      }))
      .sort((a, b) => a.name.localeCompare(b.name))
  }
}
