/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { Link, useParams } from 'react-router'
import { plainToClass } from 'class-transformer'
import React, { type JSX, useEffect, useState } from 'react';

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

const EmployeeDetail = (): JSX.Element => {
  const { employeeId } = useParams<{ employeeId: string }>()
  const [employee, setEmployee] = useState<Employee | null>(null)

  const fetchEmployee = async (): Promise<void> => {
    try {
      const response = await requestJSONWithErrorHandler(
        `api/employees/${employeeId}`,
        'get',
        null,
        'EMPLOYEE_NOT_FOUND',
        (responseJSON: string): void => {
          plainToClass(Employee, responseJSON);
        }
      )
      const employeeData = plainToClass(Employee, response)
      setEmployee(employeeData)
    } catch (error) {
      console.error('Failed to fetch employee data:', error)
    }
  };

  useEffect(() => {
    fetchEmployee();
  }, [employeeId]);

  return (
    <div>
      {employee === null ? (
        <p>
          <em>Loading...</em>
        </p>
      ) : (
        <>
          <div className="mb-3">
            <Link to="/employees">&larr; Back to employees list</Link>
          </div>
          <div className="row">
            <div className="col-lg-6 col-xl-2">
              <h3 className="text-muted">Employee</h3>
              <h2>
                {employee.preferredFirstName} {employee.lastName}
              </h2>
            </div>
            <ColumnarLabelledText
              label={'Current status'}
              columnClass="col-lg-6 col-xl-3"
              extraClasses="mt-3"
            >
              <h3 className="mt-1">
                <EditableDropdown
                  modelDatabaseId={employee.id!}
                  fieldName="currentEmployeeStatusCode"
                  fieldValue={employee.currentEmployeeStatusCode!.code}
                  refreshDataCallback={fetchEmployee}
                  options={EmployeeStatus.toOptions()}
                  valueToDisplayAccessor={(value: string): string =>
                    EmployeeStatus.fromKey(value as EmployeeStatusEnum)
                      .displayName
                  }
                />
              </h3>
            </ColumnarLabelledText>
            <div className="col-md-12 col-lg-12 col-xl-7 mt-3">
              <EmployeeSurveyDates employee={employee} />
            </div>
          </div>
          <hr />
          <div className="row">
            <div className="col-8">
              <EmployeeIds employee={employee} />
              <hr />
              <EmployeePersonalInfo
                employee={employee}
                populateData={fetchEmployee}
              />
              <hr />
              <EmployeeContact employee={employee} populateData={fetchEmployee} />
              <hr />
              <EmployeeHireInfo employee={employee} />
              <hr />
              <div className="row">
                <div className="col">
                  <EmployeePriorJobInfo employee={employee} />
                </div>
                <div className="col">
                  <EmployeeCurrentJobInfo employee={employee} />
                </div>
              </div>
              <hr />
              <EmployeeExtraDetails employee={employee} />
              <hr />
              <EmployeeMetadataFooter employee={employee} />
            </div>
            <div className="col-4">
              <h3>Timeline</h3>
              <AddComment
                modelDatabaseId={employee.id!}
                employeeStatusCode={employee.currentEmployeeStatusCode!.code}
                refreshDataCallback={fetchEmployee}
              />
              {employee.timelineEntries && (
                <TimelineEntryList timelineEntries={employee.timelineEntries} />
              )}
            </div>
          </div>
        </>
      )}
    </div>
  )
}

export default EmployeeDetail
