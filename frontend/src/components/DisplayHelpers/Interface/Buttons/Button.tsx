import React, { type JSX } from 'react';

export interface CommonButtonProps {
  className?: string
  colorType?: string
  disabled?: boolean
  marginClasses?: string
  onClick?: () => void
  reset?: boolean
  size?: string
  submit?: boolean
}

interface Props extends CommonButtonProps {
  children: React.ReactNode
  icon?: string
}

class Button extends React.Component<Props> {
  public render(): JSX.Element {
    const { onClick, children, submit, reset } = this.props
    const className = this.props.className || ''
    const colorType = this.props.colorType || 'primary'
    const marginClasses = this.props.marginClasses || ''
    const size = this.props.size ? `btn-${this.props.size}` : ''
    return (
      <button
        className={`btn ${size} btn-${colorType} ${marginClasses} ${className}`}
        onClick={onClick}
        type={submit ? 'submit' : reset ? 'reset' : 'button'}
        disabled={this.props.disabled}
      >
        {children}
      </button>
    )
  }
}

export default Button
