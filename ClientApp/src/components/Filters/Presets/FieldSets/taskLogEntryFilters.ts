import { Filter } from '../../FilterClasses/FilterTypes'
import DateFilter from '../../FilterClasses/DateFilter'
import EnumFilter from '../../FilterClasses/EnumFilter'
import TextFilter from '../../FilterClasses/TextFilter'

export const taskLogEntryFilters: Filter[] = [
  new DateFilter('logDate'),
  new EnumFilter('taskOutcomeCode'),
  new TextFilter('taskCode'),
  new TextFilter('comment')
]
