import React, { type JSX } from 'react';
import FullDataPull from './FullDataPullButton'

import RefreshStatusButton from './RefreshStatusButton'

const AdminActionButtons = (): JSX.Element => {
  return (
    <div className="AdminActionButtons row">
      <RefreshStatusButton />
      <FullDataPull />
    </div>
  )
}

export default AdminActionButtons
