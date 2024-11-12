/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { Link, RouteComponentProps } from 'react-router-dom'
import { plainToClass } from 'class-transformer'
import React from 'react'

import { Employee } from '../../../types/Employee'
import {
  EmployeeStatus,
  EmployeeStatusEnum,
} from '../../../types/EmployeeStatus'
import { requestJSONWithErrorHandler } from '../../../helpers/requestHelpers'
import AddComment from '../AddComment'
import ColumnarLabelledText from '../../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import EditableDropdown from '../../DisplayHelpers/Interface/EditableFields/EditableSelect'
import EmployeeContact from './_EmployeeContact'
import EmployeeCurrentJobInfo from './_EmployeeCurrentJobInfo'
import EmployeeExtraDetails from './_EmployeeExtraDetails'
import EmployeeHireInfo from './_EmployeeHireInfo'
import EmployeeIds from './_EmployeeIds'
import EmployeeMetadataFooter from './_EmployeeMetadataFooter'
import EmployeePersonalInfo from './_EmployeePersonalInfo'
import EmployeePriorJobInfo from './_EmployeePriorJobInfo'
import EmployeeSurveyDates from './_EmployeeSurveyDates'
import TimelineEntryList from '../TimelineEntryList'

interface Params {
  employeeId: string
}

type IOwnProps = RouteComponentProps<Params>

type Props = IOwnProps

interface State {
  employee?: Employee
}

class EmployeeDetail extends React.Component<Props, State> {
  constructor(props: Props) {
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
          <Link to="/employees">&larr; Back to employees list</Link>
        </div>
        <div className="row">
          <div className="col-lg-6 col-xl-2">
            <h3 className="text-muted">Employee</h3>
            <h2>
              {e.preferredFirstName} {e.lastName}
            </h2>
          </div>
          <ColumnarLabelledText
            label={'Current status'}
            columnClass="col-lg-6 col-xl-3"
            extraClasses="mt-3"
          >
            <h3 className="mt-1">
              <EditableDropdown
                modelDatabaseId={e.id!}
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
          </ColumnarLabelledText>
          <div className="col-md-12 col-lg-12 col-xl-7 mt-3">
            <EmployeeSurveyDates employee={e} />
          </div>
        </div>
        <hr />
        <div className="row">
          <div className="col-8">
            <EmployeeIds employee={e} />
            <hr />
            <EmployeePersonalInfo
              employee={e}
              populateData={this.populateData}
            />
            <hr />
            <EmployeeContact employee={e} populateData={this.populateData} />
            <hr />
            <EmployeeHireInfo employee={e} />
            <hr />
            <div className="row">
              <div className="col">
                <EmployeePriorJobInfo employee={e} />
              </div>
              <div className="col">
                <EmployeeCurrentJobInfo employee={e} />
              </div>
            </div>
            <hr />
            <EmployeeExtraDetails employee={e} />
            <hr />
            <EmployeeMetadataFooter employee={e} />
          </div>
          <div className="col-4">
            <h3>Timeline</h3>
            <AddComment
              modelDatabaseId={e.id!}
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

    return <>{contents}</>
  }

  async populateData(): Promise<void> {
    await requestJSONWithErrorHandler(
      `api/employees/${this.props.match.params.employeeId}`,
      'get',
      null,
      'EMPLOYEE_NOT_FOUND',
      (responseJSON: string): void => {
        this.setState({ employee: plainToClass(Employee, responseJSON) })
      }
    )
  }
}

export default EmployeeDetail
