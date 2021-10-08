import React from 'react'

import { AppointmentStatus } from '../types/AppointmentStatus'
import { Employee } from '../types/Employee'
import { EmployeeStatus } from '../types/EmployeeStatus'
import { SelectOption } from '../components/DisplayHelpers/Interface/EditableFields/EditableSelect'
import { TaskOutcome } from '../types/TaskOutcome'
import FAIcon from '../components/DisplayHelpers/Interface/Icons/FAIcon'

const fieldLabels: { [key: string]: string } = {
  id: 'Database ID',
  telkey: 'Telkey',
  recordCount: 'Record count',
  governmentEmployeeId: 'Employee ID',
  chipsFirstName: 'CHIPS First Name',
  firstName: 'First name',
  preferredFirstName: 'Preferred first name',
  chipsLastName: 'CHIPS Last Name',
  lastName: 'Last name',
  birthDate: 'Birth date',
  gender: 'Gender',
  chipsEmail: 'CHIPS email',
  governmentEmail: 'Email',
  preferredEmail: 'Preferred email',
  classification: 'Classification',
  ministry: 'Ministry',
  departmentId: 'Department ID',
  jobFunctionCode: 'Job function code',
  locationCity: 'Location city',
  originalHireDate: 'Original hire date',
  lastDayWorkedDate: 'Last day worked date',
  effectiveDate: 'Hire effective date',
  staffingReason: 'Hiring reason',
  appointmentStatus: 'Appointment status',
  positionCode: 'Position code',
  positionTitle: 'Position title',
  age: 'Age',
  leaveDate: 'Leave date',
  serviceYears: 'Service years',
  jobCode: 'Job code',
  backDated: 'Back dated',
  exitCount: 'Exit count',
  ageGroup: 'Age group',
  classificationGroup: 'Classification group',
  serviceGroup: 'Service group',
  locationGroup: 'Location group',
  currentEmployeeStatusCode: 'Current status',
  timelineEntries: '',
  createdTs: 'Created date',
  modifiedTs: 'Last modified date',
  taskOutcomeCode: 'Status',
  comment: 'Comment',
  blankEmail: 'Preferred email',
  triedToUpdateInFinalState: 'Tried to update in final state'
}

const optionsForEnum: { [key: string]: () => SelectOption[] } = {
  currentEmployeeStatusCode: EmployeeStatus.toOptions,
  taskOutcomeCode: TaskOutcome.toOptions,
  appointmentStatus: AppointmentStatus.toOptions
}

export const labelFor = (fieldName: string): string => fieldLabels[fieldName]

export const labelForWithFlag = (
  fieldName: string,
  employee: Employee,
  flagTest?: (e: Employee) => boolean
): JSX.Element => {
  const flagIsSet = flagTest
    ? flagTest(employee)
    : employee[`${fieldName}Flag` as keyof Employee]
  return (
    <span
      title={flagIsSet ? 'This field has been edited by an admin' : undefined}
    >
      {fieldLabels[fieldName]}{' '}
      {flagIsSet && <FAIcon name="flag" marginClasses="ml-1" />}
    </span>
  )
}

export const optionsFor = (fieldName: string): SelectOption[] => {
  const options = optionsForEnum[fieldName]().sort((a, b) =>
    a.name.localeCompare(b.name)
  )
  return options
}
