import React from 'react'

import { AppointmentStatus } from '../types/AppointmentStatus'
import { Employee } from '../types/Employee'
import { EmployeeStatus } from '../types/EmployeeStatus'
import { SelectOption } from '../components/DisplayHelpers/Interface/EditableFields/EditableSelect'
import { TaskOutcome } from '../types/TaskOutcome'
import FAIcon from '../components/DisplayHelpers/Interface/Icons/FAIcon'
import { NewHireOrInternalStaffing } from '../types/NewHireOrInternalStaffing'

const fieldLabels: { [key: string]: string } = {
  age: 'Age',
  ageGroup: 'Age group',
  appointmentStatus: 'Appointment status',
  backDated: 'Back dated',
  birthDate: 'Birth date',
  blankEmail: 'Preferred email',
  chipsCity: 'PSA extract city',
  chipsEmail: 'PSA extract email',
  chipsFirstName: 'PSA extract first name',
  chipsLastName: 'PSA extract last name',
  classification: 'Classification',
  classificationGroup: 'Classification group',
  closedDate: 'Closed date',
  comment: 'Comment',
  createdTs: 'Created date',
  currentEmployeeStatusCode: 'Current status',
  deadlineDate: 'Deadline date',
  departmentId: 'Department ID',
  effectiveDate: 'Hire effective date',
  exitCount: 'Exit count',
  firstName: 'First name',
  gender: 'Gender',
  governmentEmail: 'Email',
  governmentEmployeeId: 'Employee ID',
  id: 'Database ID',
  importDate: 'Import date',
  inviteDate: 'Invite date',
  jobCode: 'Job code',
  jobFunctionCode: 'Job function code',
  lastDayWorkedDate: 'Last day worked date',
  lastName: 'Last name',
  ldapCity: 'LDAP city',
  ldapEmail: 'LDAP email',
  ldapFirstName: 'LDAP first name',
  ldapLastName: 'LDAP last name',
  ldapOrganization: 'LDAP organization',
  leaveDate: 'Leave date',
  locationCity: 'Location city',
  locationGroup: 'Location group',
  ministry: 'Ministry',
  modifiedTs: 'Last modified date',
  newHireOrInternalStaffing: 'New hire or internal staffing',
  originalHireDate: 'Original hire date',
  organization: 'Organization',
  positionCode: 'Position code',
  positionTitle: 'Position title',
  preferredEmail: 'Preferred email',
  preferredFirstName: 'Preferred first name',
  recordCount: 'Record count',
  reminder1Date: 'Reminder 1',
  reminder2Date: 'Reminder 2',
  serviceGroup: 'Service group',
  serviceYears: 'Service years',
  staffingReason: 'Hiring reason',
  taskCode: 'Task',
  taskOutcomeCode: 'Status',
  telkey: 'Telkey',
  timelineEntries: '',
  triedToUpdateInFinalState: 'Tried to update in final state'
}

const optionsForEnum: { [key: string]: () => SelectOption[] } = {
  currentEmployeeStatusCode: EmployeeStatus.toOptions,
  taskOutcomeCode: TaskOutcome.toOptions,
  appointmentStatus: AppointmentStatus.toOptions,
  newHireOrInternalStaffing: NewHireOrInternalStaffing.toOptions,
  staffingReason: () => []
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
