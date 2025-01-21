import { Filter } from '../../FilterClasses/FilterTypes'
import CustomFilter from '../../FilterClasses/CustomFilter'
import DateFilter from '../../FilterClasses/DateFilter'
import EnumFilter from '../../FilterClasses/EnumFilter'
import TextFilter from '../../FilterClasses/TextFilter'

export const employeeFilters: Filter[] = [
  new TextFilter('telkey'),
  new TextFilter('governmentEmployeeId'),
  new TextFilter('preferredFirstName'),
  new TextFilter('lastName'),
  new TextFilter('preferredEmail'),
  new DateFilter('effectiveDate'),
  new DateFilter('importDate'),
  new DateFilter('lastModifiedDate'),
  new EnumFilter(
    'newHireOrInternalStaffing',
    undefined,
    'Employees/Values/NewHireOrInternalStaffing'
  ),
  new EnumFilter('currentEmployeeStatusCode'),
  new EnumFilter('hiringReason', undefined, 'Employees/Values/StaffingReason'),
  new CustomFilter('blankEmail'),
]
