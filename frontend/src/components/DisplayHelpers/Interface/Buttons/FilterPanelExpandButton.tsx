import React, { type JSX } from 'react';

import IconButton from './IconButton'

interface Props {
  toggleExpanded: () => void
}

const FilterPanelExpandButton = ({ toggleExpanded }: Props): JSX.Element => {
  return (
    <div className="FilterPanelExpandButton">
      <IconButton
        iconType="fas"
        iconName={'caret-down'}
        label={`Hide filters`}
        iconRight
        iconMarginClasses="ms-2"
        colorType="secondary"
        buttonClasses="NoOutline NoBackground Faded"
        iconClasses="fa-lg"
        onClick={(): void => toggleExpanded()}
      />
    </div>
  )
}

export default FilterPanelExpandButton
