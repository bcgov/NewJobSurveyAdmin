import React, { type JSX } from 'react';

import { EmployeeTimelineEntry } from '../../types/EmployeeTimelineEntry'
import TimelineEntry from './TimelineEntry'

interface Props {
  timelineEntries: EmployeeTimelineEntry[]
}

class TimelineEntryList extends React.Component<Props> {
  render(): JSX.Element {
    const sortedEntries = this.props.timelineEntries.sort((a, b) => {
      if (
        !a ||
        !a.createdTs ||
        !b ||
        !b.createdTs ||
        a.createdTs === b.createdTs
      ) {
        return 0
      } else if (a.createdTs < b.createdTs) {
        return 1
      } else {
        return -1
      }
    })
    return (
      <div className="TimelineEntryList">
        {sortedEntries.map(tl => (
          <TimelineEntry key={tl.id} timelineEntry={tl} />
        ))}
      </div>
    )
  }
}
export default TimelineEntryList
