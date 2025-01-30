import React, { type JSX } from 'react';

import { requestJSONWithErrorHandler } from '../../helpers/requestHelpers'
import ColumnarLabelledText from '../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import SuccessMessage from '../Employees/SuccessMessage'

const FullDataPull = (): JSX.Element => {
  const [successTime, setSuccessTime] = React.useState(0)
  const [successMessage, setSuccessMessage] = React.useState<
    string | undefined
  >(undefined)
  const [refreshButtonActive, setRefreshButtonActive] = React.useState(false)

  const reconcileEmployees = React.useCallback(() => {
    setRefreshButtonActive(true)
    requestJSONWithErrorHandler(
      `api/Employees/ScheduledLoadAndUpdate`,
      'post',
      null,
      'DATA_PULL_FAILED',
      (): void => {
        setRefreshButtonActive(false)
        setSuccessTime(Date.now())
        setSuccessMessage('The scheduled task finished.')
      }
    )
  }, [])

  return (
    <ColumnarLabelledText
      helperText="This will trigger an execution of the daily scheduled task, as outlined in steps 1&ndash;3 below."
      label="Run scheduled task"
      columnClass="col-6"
    >
      <button
        className="btn btn-primary mt-2"
        onClick={reconcileEmployees}
        disabled={refreshButtonActive}
      >
        {refreshButtonActive ? 'Running...' : 'Run scheduled task'}
      </button>
      <SuccessMessage
        className="mt-2"
        successTime={successTime}
        successMessage={successMessage}
      />
    </ColumnarLabelledText>
  )
}

export default FullDataPull
