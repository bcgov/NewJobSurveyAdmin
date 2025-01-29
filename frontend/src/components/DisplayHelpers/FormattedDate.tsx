import React, { type JSX } from 'react';
import timezone from 'dayjs/plugin/timezone';
import advancedFormat from 'dayjs/plugin/advancedFormat';
import utc from 'dayjs/plugin/utc';
import relativeTime from 'dayjs/plugin/relativeTime';
import dayjs from 'dayjs';
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

  dayjs.extend(utc);
  dayjs.extend(timezone);
  dayjs.extend(relativeTime);
  dayjs.extend(advancedFormat);

  let dayjsDate = dayjs(date);

  if (showLocalTimezone) {
    dayjsDate = dayjsDate.tz(TIMEZONE);
  }

  let displayDate = ''

  if (customFormat) {
    displayDate = dayjsDate.format(customFormat)
  } else if (nice) {
    displayDate = dayjsDate.fromNow()
  } else {
    displayDate = showTime
      ? dayjsDate.format(defaultNiceDatetimeFormat)
      : dayjsDate.format(defaultDateFormat)
  }

  return (
    <span className={`Date ${noWrap && 'text-nowrap'}`}>{displayDate}</span>
  )
}

export default FormattedDate
