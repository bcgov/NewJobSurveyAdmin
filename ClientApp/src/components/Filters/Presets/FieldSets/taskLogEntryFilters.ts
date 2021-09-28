import { Filter } from '../../FilterClasses/FilterTypes'
import DateFilter from '../../FilterClasses/DateFilter'
import EnumFilter from '../../FilterClasses/EnumFilter'
import TextFilter from '../../FilterClasses/TextFilter'

export const taskLogEntryFilters: Filter[] = [
  new DateFilter('createdTs'),
  new EnumFilter('taskOutcomeCode'),
  new TextFilter('comment')
]
