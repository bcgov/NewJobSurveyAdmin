import React from 'react'
import moment from 'moment-timezone'
import {
  defaultDateFormat,
  defaultNiceDatetimeFormat
} from '../../helpers/dateHelper'

interface Props {
  date?: Date
  showTime?: boolean
  showLocalTimezone?: boolean
  nice?: boolean
  customFormat?: string
  noWrap?: boolean
}

const TIMEZONE = 'America/Vancouver'

const FormattedDate = ({
  customFormat,
  date,
  nice,
  showLocalTimezone,
  showTime,
  noWrap
}: Props): JSX.Element => {
  if (!date) {
    return <>—</>
  }

  let momentDate = moment(date)

  if (showLocalTimezone) {
    momentDate = momentDate.tz(TIMEZONE)
  }

  let displayDate = ''

  if (customFormat) {
    displayDate = momentDate.format(customFormat)
  } else if (nice) {
    displayDate = momentDate.fromNow()
  } else {
    displayDate = showTime
      ? momentDate.format(defaultNiceDatetimeFormat)
      : momentDate.format(defaultDateFormat)
  }

  return (
    <span className={`Date ${noWrap && 'text-nowrap'}`}>{displayDate}</span>
  )
}

export default FormattedDate
