import React from 'react'

interface Props {
  children: JSX.Element | JSX.Element[]
}

class ContentWrapper extends React.Component<Props> {
  render(): JSX.Element {
    return <div>{this.props.children}</div>
  }
}

export default ContentWrapper
