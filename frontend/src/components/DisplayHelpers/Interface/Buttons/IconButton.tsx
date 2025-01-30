import React, { type JSX } from 'react';

import Button, { CommonButtonProps } from './Button'
import FAIcon from '../Icons/FAIcon'

import './IconButton.scss'

interface Props extends CommonButtonProps {
  buttonClasses?: string
  disabled?: boolean
  iconClasses?: string
  iconMarginClasses?: string
  iconName: string
  iconRight?: boolean
  iconType?: string
  label?: React.ReactNode
  onClick?: () => void
  reset?: boolean
  submit?: boolean
}

class IconButton extends React.Component<Props> {
  public render(): JSX.Element {
    const {
      buttonClasses,
      iconClasses,
      iconMarginClasses,
      iconName,
      iconRight,
      iconType,
      label
    } = this.props

    const icon = (
      <FAIcon
        name={iconName}
        type={iconType}
        classes={iconClasses}
        marginClasses={iconMarginClasses}
      />
    )

    return (
      <Button {...this.props} className={buttonClasses}>
        {!iconRight && icon}
        {label}
        {iconRight && icon}
      </Button>
    )
  }
}

export default IconButton
