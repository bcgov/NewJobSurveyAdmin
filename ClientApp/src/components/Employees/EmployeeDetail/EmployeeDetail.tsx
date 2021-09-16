/* eslint-disable @typescript-eslint/no-non-null-assertion */

import React from 'react'
import { Link, RouteComponentProps } from 'react-router-dom'
import { plainToClass } from 'class-transformer'

import { Employee } from '../../../types/Employee'
import { EmployeeStatus, EmployeeStatusEnum } from '../../../types/EmployeeStatus'
import { requestJSONWithErrorHandler } from '../../../helpers/requestHelpers'
import AddComment from '../AddComment'
import ContentWrapper from '../../Wrappers/ContentWrapper'
import EditableDropdown from '../EditableSelect'
import EmployeeContact from './_EmployeeContact'
import EmployeeCurrentJobInfo from './_EmployeeCurrentJobInfo'
import EmployeePriorJobInfo from './_EmployeePriorJobInfo'
import EmployeeIds from './_EmployeeIds'
import EmployeeMetadataFooter from './_EmployeeMetadataFooter'
import EmployeePersonalInfo from './_EmployeePersonalInfo'
import LabelledText from '../../DisplayHelpers/Interface/LabelledItems/LabelledText'
import TimelineEntryList from '../TimelineEntryList'

interface IParams {
  employeeId: string
}

interface IOwnProps extends RouteComponentProps<IParams> {}

interface IStateProps {}

interface IDispatchProps {}

interface IProps extends IOwnProps, IStateProps, IDispatchProps {}

interface IState {
  employee?: Employee
}

class EmployeeDetail extends React.Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = { employee: undefined }

    this.populateData = this.populateData.bind(this)
  }

  componentDidMount(): void {
    this.populateData()
  }

  renderEmployee(e: Employee): JSX.Element {
    return (
      <div>
        <div className="mb-3">
          <Link to="/employees">&larr; Back to exiting employees list</Link>
        </div>
        <div className="row">
          <div className="col">
            <h3 className="text-muted">Employee</h3>
            <h2>
              {e.preferredFirstName} {e.lastName}
            </h2>
          </div>
          <div className="col">
            <LabelledText label={'Current status'}>
              <h3 className="mt-1">
                <EditableDropdown
                  employeeDatabaseId={e.id!}
                  fieldName="currentEmployeeStatusCode"
                  fieldValue={e.currentEmployeeStatusCode!.code}
                  refreshDataCallback={this.populateData}
                  options={EmployeeStatus.toOptions()}
                  valueToDisplayAccessor={(value: string): string =>
                    EmployeeStatus.fromKey(value as EmployeeStatusEnum)
                      .displayName
                  }
                />
              </h3>
            </LabelledText>
          </div>
        </div>
        <hr />
        <div className="row">
          <div className="col-8">
            <EmployeeIds employee={e} />
            <hr />
            <EmployeePersonalInfo employee={e} populateData={this.populateData} />
            <hr />
            <EmployeeContact employee={e} populateData={this.populateData} />
            <hr />
            <div className='row'>
              <div className='col'><EmployeeCurrentJobInfo employee={e} /></div>
              <div className='col'><EmployeePriorJobInfo employee={e} /></div>
            </div>
            <hr />
            <EmployeeMetadataFooter employee={e} />
          </div>
          <div className="col-4">
            <h3>Timeline</h3>
            <AddComment
              employeeDatabaseId={e.id!}
              employeeStatusCode={e.currentEmployeeStatusCode!.code}
              refreshDataCallback={this.populateData}
            />
            {e.timelineEntries && (
              <TimelineEntryList timelineEntries={e.timelineEntries} />
            )}
          </div>
        </div>
      </div>
    )
  }

  render(): JSX.Element {
    const contents =
      this.state.employee === undefined ? (
        <p>
          <em>Loading...</em>
        </p>
      ) : (
        this.renderEmployee(this.state.employee)
      )

    return <ContentWrapper>{contents}</ContentWrapper>
  }

  async populateData(): Promise<void> {
    await requestJSONWithErrorHandler(
      `api/employees/${this.props.match.params.employeeId}`,
      'get',
      null,
      'EMPLOYEE_NOT_FOUND',
      (responseJSON: string): void => {
        console.log('responseJSON', responseJSON)
        this.setState({ employee: plainToClass(Employee, responseJSON) })
      }
    )
  }
}

export default EmployeeDetail
