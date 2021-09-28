import React from 'react'

import LabelledText from './LabelledText'

import './LabelledText.scss'

interface Props {
  children: React.ReactNode
  columnClass?: string
  helperText?: string
  label: React.ReactNode
}

const ColumnarLabelledText = ({
  columnClass = 'col-4',
  ...other
}: Props): JSX.Element => {
  return (
    <div className={`ColumnarLabelledText ${columnClass}`}>
      <LabelledText {...other} />
    </div>
  )
}

export default ColumnarLabelledText
