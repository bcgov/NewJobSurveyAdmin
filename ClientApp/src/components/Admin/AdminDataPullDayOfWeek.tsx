import { plainToClass } from 'class-transformer'
import moment from 'moment'
import React from 'react'

import { AdminSetting } from '../../types/AdminSetting'
import { AnyJson } from '../../types/JsonType'
import { NameValuePair } from '../DisplayHelpers/Interface/Selects/CollectionSelect'
import ColumnarLabelledText from '../DisplayHelpers/Interface/LabelledItems/ColumnarLabelledText'
import EditableSelect from '../DisplayHelpers/Interface/EditableFields/EditableSelect'

interface Props {
  dataPullSetting: AdminSetting
  updateAdminSetting: (adminSetting: AdminSetting) => void
}

/**
 * Given an ISO day of the week in numeric or string format, return the full
 * name of that day of the week as a string. So `1` (or `"1"`) will return
 * `"Monday"`, `2` `"Tuesday"`, etc.
 * @param i The ISO day of the week.
 * @throws RangeError if i is not between 1 and 7 inclusive
 */
const mapIsoWeekdayToOption = (i: number | string): NameValuePair => {
  if (isNaN(+i) || +i < 1 || +i > 7) {
    throw new RangeError('isoWeekDay must be between 1 and 7, inclusive')
  }
  return {
    name: moment()
      .isoWeekday(+i)
      .format('dddd'),
    value: `${i}`
  }
}

const ISO_DAYS_OF_WEEK_SELECT_ITEMS = [1, 2, 3, 4, 5, 6, 7].map(
  mapIsoWeekdayToOption
)

const AdminDataPullDayOfWeek = ({
  dataPullSetting,
  updateAdminSetting
}: Props): JSX.Element => {
  if (!dataPullSetting) return <></>

  const { id, displayName, value } = dataPullSetting

  return (
    <ColumnarLabelledText
      key={id}
      label={displayName}
      columnClass="col"
      helperText="The day of the week new employee information will be pulled from the PSA API. CallWeb status will still be updated on other days."
    >
      <h3 className="mt-2">
        <EditableSelect
          modelDatabaseId={id!}
          fieldName="value"
          fieldValue={value!}
          options={ISO_DAYS_OF_WEEK_SELECT_ITEMS}
          modelPath={'adminSettings'}
          valueToDisplayAccessor={(value: string): string => {
            return mapIsoWeekdayToOption(value).name
          }}
          refreshDataCallback={(responseJson: AnyJson): void => {
            updateAdminSetting(plainToClass(AdminSetting, responseJson))
          }}
        />
      </h3>
    </ColumnarLabelledText>
  )
}

export default AdminDataPullDayOfWeek
