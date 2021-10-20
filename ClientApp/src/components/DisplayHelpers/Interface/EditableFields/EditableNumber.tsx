import React from 'react'

import { FixTypeLater } from '../../../../types/FixTypeLater'
import { requestJSONWithErrorHandler } from '../../../../helpers/requestHelpers'
import { userNameFromState } from '../../../../helpers/userHelper'
import SuccessMessage from '../../../Employees/SuccessMessage'

import './EditableField.scss'

export interface SelectOption {
  name: string
  value: string
}

interface Props {
  modelDatabaseId: string
  fieldName: string
  fieldValue: string
  inline?: boolean
  max?: number
  min?: number
  modelPath?: string
  refreshDataCallback?: (response: FixTypeLater) => void
  step?: number
  valueToDisplayAccessor?: (value: string) => string
}

const EditableNumber = ({
  modelDatabaseId,
  fieldName,
  fieldValue,
  inline,
  max,
  min,
  modelPath,
  refreshDataCallback,
  step,
  valueToDisplayAccessor
}: Props): JSX.Element => {
  const [newValue, setNewValue] = React.useState(fieldValue || '')
  const [isEditable, setIsEditable] = React.useState(false)
  const [successTime, setSuccessTime] = React.useState(0)

  const toggleEditable = (): void => {
    setIsEditable(!isEditable)
  }

  const submitEdit = (event: React.FormEvent<HTMLFormElement>): void => {
    event.preventDefault()
    requestJSONWithErrorHandler(
      `api/${modelPath || 'employees'}/${modelDatabaseId}`,
      'patch',
      {
        [fieldName]: newValue,
        AdminUserName: userNameFromState()
      },
      'CANNOT_EDIT_EMPLOYEE',
      (response): void => {
        toggleEditable()
        if (refreshDataCallback) {
          refreshDataCallback(response)
        }
        setSuccessTime(Date.now())
      }
    )
  }

  return (
    <div className={`EditableField EditableNumber ${inline && 'd-inline'}`}>
      {isEditable ? (
        <form onSubmit={submitEdit} className={`${inline && 'form-inline'}`}>
          <input
            className="form-control form-control-sm"
            type="number"
            min={min}
            max={max}
            step={step}
            value={newValue}
            onChange={(e): void => setNewValue(e.target.value)}
          />
          <input
            type="button"
            value="Cancel"
            className={`btn btn-sm btn-outline-danger ${!inline &&
              'mt-2'} ${inline && 'ml-2'} mr-2`}
            onClick={toggleEditable}
          />
          <input
            type="submit"
            value="Save"
            className={`btn btn-sm btn-primary ${!inline && 'mt-2'}`}
          />
        </form>
      ) : (
        <span className="Editable" onClick={toggleEditable}>
          {valueToDisplayAccessor
            ? valueToDisplayAccessor(fieldValue)
            : fieldValue}
        </span>
      )}
      <SuccessMessage
        className={`pt-1 ${!inline && 'mt-2'} ${inline && 'ml-2'}`}
        successTime={successTime}
        inline={inline}
      />
    </div>
  )
}

export default EditableNumber
