import React, { type JSX } from 'react';

import LabelledText from './LabelledText'

import './LabelledText.scss'

interface Props {
  children: React.ReactNode
  columnClass?: string
  extraClasses?: string
  helperText?: string
  label: React.ReactNode
}

const ColumnarLabelledText = ({
  columnClass = 'col-4',
  extraClasses,
  ...other
}: Props): JSX.Element => {
  return (
    <div className={`ColumnarLabelledText ${columnClass} ${extraClasses}`}>
      <LabelledText {...other} />
    </div>
  )
}

export default ColumnarLabelledText
