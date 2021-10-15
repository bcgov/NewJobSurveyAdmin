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
  new EnumFilter('newHireOrInternalStaffing'),
  new EnumFilter('currentEmployeeStatusCode'),
  new TextFilter('staffingReason'),
  new CustomFilter('blankEmail')
]
