/* eslint-disable @typescript-eslint/no-non-null-assertion */
import React, { useMemo, type JSX } from 'react';

import { Filter, FilterType } from './FilterClasses/FilterTypes'
import { PresetProps } from './Presets/PresetProps'
import DateFilter from './FilterClasses/DateFilter'
import DateFilterInput from './Inputs/DateFilterInput'
import EnumFilter from './FilterClasses/EnumFilter'
import EnumFilterInput from './Inputs/EnumFilterInput'
import IconButton from '../DisplayHelpers/Interface/Buttons/IconButton'
import TextFilter from './FilterClasses/TextFilter'
import TextFilterInput from './Inputs/TextFilterInput'

interface Props {
  addFilters: (filters: Filter[]) => void
  filterableFields: Filter[]
  presetComponent?: React.FC<PresetProps>
  resetFilters: () => void
}

export type FilterMapAction = {
  filter?: Filter
  type: 'setFilter' | 'reset'
}

type FilterMap = { [key: string]: Filter }

function reducer(state: FilterMap, action: FilterMapAction): FilterMap {
  const { type, filter } = action
  const filterMapClone = { ...state }
  switch (type) {
    case 'setFilter':
      //eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      filterMapClone[filter!.fieldName] = filter!
      return filterMapClone
    case 'reset':
      return {}
  }
}

export const FilterDispatch = React.createContext({})

const FilterForm = ({
  addFilters,
  resetFilters,
  filterableFields,
  presetComponent,
}: Props): JSX.Element => {
  const [filterMap, dispatch] = React.useReducer(reducer, {})
  const [resetTimestamp, setResetTimestamp] = React.useState<number>(0)
  const [submitId, setSubmitId] = React.useState<number>(0)

  const formRef = React.useRef<HTMLFormElement>(null)

  const submitForm = React.useCallback(
    (event: React.FormEvent<HTMLFormElement>): void => {
      event.preventDefault()
      setSubmitId(submitId + 1)
    },
    [submitId]
  )

  const reset = React.useCallback((): void => {
    dispatch({ type: 'reset' })
    resetFilters()
    setResetTimestamp(Date.now())
  }, [resetFilters, dispatch])

  React.useEffect((): void => {
    addFilters(Object.values(filterMap))
    dispatch({ type: 'reset' })
    formRef.current!.reset()
    setResetTimestamp(Date.now())
    // Note: we only care about submitId here.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [submitId])

  const inputs = useMemo(() => {
    // Ignore Custom fields
    return filterableFields
      .filter((f) => f.type !== FilterType.Custom)
      .map((filter): JSX.Element => {
        let filterComponent
        let colWidth = 2
        switch (filter.type) {
          case FilterType.Date:
            filterComponent = (
              <DateFilterInput
                filter={filter as DateFilter}
                resetTimestamp={resetTimestamp}
              />
            )
            colWidth = 2
            break
          case FilterType.Enum:
            filterComponent = (
              <EnumFilterInput
                filter={filter as EnumFilter}
                resetTimestamp={resetTimestamp}
              />
            )
            break
          case FilterType.String:
          default:
            filterComponent = <TextFilterInput filter={filter as TextFilter} />
        }
        return (
          <div key={filter.fieldName} className={`col-${colWidth}`}>
            {filterComponent}
          </div>
        )
      })
  }, [resetTimestamp, filterableFields])

  const PresetComponent = presetComponent

  return (
    <FilterDispatch.Provider value={dispatch}>
      <div className="FilterForm">
        <form onSubmit={submitForm} ref={formRef}>
          <div className="row">{inputs}</div>
          <div className="row align-items-center mt-2">
            <div className="col-8 form-group mb-0">
              {PresetComponent && (
                <PresetComponent
                  submitId={submitId}
                  setSubmitId={setSubmitId}
                />
              )}
            </div>
            <div className="col-4 form-group mb-0">
              <div className="text-end">
                <IconButton
                  label="Set filters"
                  iconName="check"
                  marginClasses="me-3"
                  iconMarginClasses="me-2"
                  submit
                />
                <IconButton
                  label="Reset all filters"
                  iconName="undo"
                  colorType="secondary"
                  iconMarginClasses="me-2"
                  onClick={reset}
                  reset
                />
              </div>
            </div>
          </div>
        </form>
      </div>
    </FilterDispatch.Provider>
  )
}

export default FilterForm
