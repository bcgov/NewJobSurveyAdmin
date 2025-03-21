import React, { type JSX } from 'react';

import './LabelledText.scss'

interface Props {
  children: React.ReactNode
  helperText?: string
  label: React.ReactNode
}

const LabelledText = (props: Props): JSX.Element => {
  return (
    <div className="LabelledText">
      <span className="Label">{props.label}</span>
      {props.helperText && <div className="HelperText">{props.helperText}</div>}
      <span className="Text">{props.children}</span>
    </div>
  )
}

export default LabelledText
