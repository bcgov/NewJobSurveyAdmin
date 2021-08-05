import {  RouteComponentProps } from 'react-router-dom'
import React from 'react'

import { Employee } from '../../types/Employee'
import { requestJSONWithoutAuth } from '../../helpers/requestHelpers'
import ContentWrapper from '../Wrappers/ContentWrapper'
import { plainToClass } from 'class-transformer'
import { FixTypeLater } from '../../types/FixTypeLater'

interface IParams {
  employeeId: string
}

interface IOwnProps extends RouteComponentProps<IParams> {}

interface IStateProps {}

interface IDispatchProps {}

interface IProps extends IOwnProps, IStateProps, IDispatchProps {}

interface IState {
  status?: string
}

class HealthStatus extends React.Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = { status: undefined }

    this.populateData = this.populateData.bind(this)
  }

  componentDidMount(): void {
    this.populateData()
  }

  renderEmployee(e: Employee): JSX.Element {
    return (
      <div>
        <div className="row">
          <div className="col">
            <h3 className="text-muted">Health Status</h3>
          </div>
        </div>
      </div>
    )
  }

  render(): JSX.Element {
    const contents =
      this.state.status === undefined ? (
        <p>
          <em>Loading...</em>
        </p>
      ) : (
        <p>{this.state.status}</p>
      )

    return <ContentWrapper><h1>Health status</h1>{contents}</ContentWrapper>
  }

  async populateData(): Promise<void> {
    await requestJSONWithoutAuth(
      `api/healthstatus/status`,
      'get',
      null,
      'HEALTH_STATUS_FAILED',
      (responseJSON: FixTypeLater): void => {
        console.log('responseJSON', responseJSON)
        this.setState({ status: responseJSON.msg })
      }
    )
  }
}

export default HealthStatus
