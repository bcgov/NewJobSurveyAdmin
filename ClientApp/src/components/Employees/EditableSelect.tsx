import React from 'react'

import { requestJSONWithErrorHandler } from '../../helpers/requestHelpers'
import { userNameFromState } from '../../helpers/userHelper'
import SuccessMessage from './SuccessMessage'

import './EditableField.scss'

export interface SelectOption {
  name: string
  value: string
}

interface Props {
  employeeDatabaseId: string
  fieldName: string
  fieldValue: string
  options: SelectOption[]
  refreshDataCallback: () => void
  valueToDisplayAccessor?: (value: string) => string
}

const EditableSelect = (props: Props): JSX.Element => {
  const {
    employeeDatabaseId,
    fieldName,
    fieldValue,
    options,
    valueToDisplayAccessor
  } = props

  const [newValue, setNewValue] = React.useState(fieldValue || '')
  const [isEditable, setIsEditable] = React.useState(false)
  const [successTime, setSuccessTime] = React.useState(0)

  const toggleEditable = (): void => {
    setIsEditable(!isEditable)
  }

  const submitEdit = (event: React.FormEvent<HTMLFormElement>): void => {
    event.preventDefault()
    requestJSONWithErrorHandler(
      `api/employees/${employeeDatabaseId}`,
      'patch',
      {
        [fieldName]: newValue,
        AdminUserName: userNameFromState()
      },
      'CANNOT_EDIT_EMPLOYEE',
      (): void => {
        toggleEditable()
        props.refreshDataCallback()
        setSuccessTime(Date.now())
      }
    )
  }

  return (
    <div className="EditableField EditableDropdown">
      {isEditable ? (
        <form onSubmit={submitEdit}>
          <select
            className="form-control form-control-sm"
            value={newValue}
            onChange={(e): void => setNewValue(e.target.value)}
          >
            {options.map(
              (option): JSX.Element => {
                return (
                  <option key={option.value} value={option.value}>
                    {option.name}
                  </option>
                )
              }
            )}
          </select>
          <input
            type="button"
            value="Cancel"
            className="btn btn-sm btn-outline-danger mt-2 mr-2"
            onClick={toggleEditable}
          />
          <input
            type="submit"
            value="Save"
            className="btn btn-sm btn-primary mt-2"
          />
        </form>
      ) : (
        <span className="Editable" onClick={toggleEditable}>
          {valueToDisplayAccessor
            ? valueToDisplayAccessor(fieldValue)
            : fieldValue}
        </span>
      )}
      <SuccessMessage className="pt-1 mt-2" successTime={successTime} />
    </div>
  )
}

export default EditableSelect
