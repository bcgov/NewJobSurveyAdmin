/* globals Map */

import { SelectOption } from '../components/DisplayHelpers/Interface/EditableFields/EditableSelect'

export enum NewHireOrInternalStaffingEnum {
  NewHire = 'New Hires', // Yes, with an 's'
  InternalStaffing = 'Internal Staffing'
}

export class NewHireOrInternalStaffing {
  code: NewHireOrInternalStaffingEnum

  constructor(NewHireOrInternalStaffing: NewHireOrInternalStaffingEnum) {
    this.code = NewHireOrInternalStaffing
  }

  static NEW_HIRE: NewHireOrInternalStaffing = new NewHireOrInternalStaffing(
    NewHireOrInternalStaffingEnum.NewHire
  )
  static INTERNAL_STAFFING: NewHireOrInternalStaffing = new NewHireOrInternalStaffing(
    NewHireOrInternalStaffingEnum.InternalStaffing
  )

  static array = (): NewHireOrInternalStaffing[] => [
    NewHireOrInternalStaffing.NEW_HIRE,
    NewHireOrInternalStaffing.INTERNAL_STAFFING
  ]

  static map = (): Map<
    NewHireOrInternalStaffingEnum,
    NewHireOrInternalStaffing
  > => {
    return new Map(NewHireOrInternalStaffing.array().map(s => [s.code, s]))
  }

  static fromKey = (
    key: NewHireOrInternalStaffingEnum
  ): NewHireOrInternalStaffing => {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    return NewHireOrInternalStaffing.map().get(key)!
  }

  static toOptions = (): SelectOption[] => {
    return NewHireOrInternalStaffing.array().map(status => ({
      name: status.code,
      value: status.code
    }))
  }
}
