import moment from 'moment'
import React from 'react'
import AdminInterfaceHelpTopic from './AdminInterfaceHelpTopic'

// The scheduled task runs at 16:00 UTC.
const SCHEDULED_TASK_UTC_TIME = moment
  .utc()
  .hour(16)
  .minute(0)

// The task time will be shown in Pacific time.
const TASK_TIME = SCHEDULED_TASK_UTC_TIME.clone()
  .tz('America/Vancouver')
  .format('h:mm')

const AdminInterfaceHelp = (): JSX.Element => {
  return (
    <div className="AdminInterfaceHelp text-muted">
      <h2>
        <i className="fas fa-info-circle mr-2" /> Information
      </h2>
      <div className="row">
        <div className="col">
          <AdminInterfaceHelpTopic title="Data pulls">
            The scheduled task runs every day at {TASK_TIME} Pacific time. The
            task will update employee statuses from CallWeb every day, but new
            hire employee data will be pulled from the PSA API only on the date
            specified.
          </AdminInterfaceHelpTopic>

          <AdminInterfaceHelpTopic title="Days between invites">
            You can set the days between invitations. Remember to adjust the
            email template in CallWeb to reflect any changes made to the closing
            date.
          </AdminInterfaceHelpTopic>
        </div>
        <div className="col">
          <AdminInterfaceHelpTopic title="Blackout period">
            <p>
              The blackout period setting allows an admin to pause the pushing
              of survey invitation dates into CallWeb. When set to
              &ldquo;Yes,&rdquo; all invitation-related dates on imported
              employees will be set to January 1, 2099. Otherwise, employees
              will be imported as normal. When the blackout period is turned
              off, employees will have their invite dates reset as if they had
              been added to the system on the currently-set data pull day.
            </p>
          </AdminInterfaceHelpTopic>
        </div>
      </div>
    </div>
  )
}

export default AdminInterfaceHelp
