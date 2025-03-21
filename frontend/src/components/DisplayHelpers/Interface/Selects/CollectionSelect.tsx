import React, { type JSX } from 'react';
import ReactSelect, { SingleValue, MultiValue } from 'react-select';

import { FixTypeLater } from '../../../../types/FixTypeLater'

import './CollectionSelect.scss'
import styles from '../../../../_common.module.scss'

const { baseColor, focusShadowColor, focusBorderColor } = styles

export interface ShimEvent {
  target: {
    value: string
  }
}

export interface NameValuePair {
  name: string
  value: string
}

export interface CollectionSelectValue {
  value: string
  label: string
  isDefault?: boolean
}

export type CollectionSelectReturnValue = string[] | null

export const singleValue = (
  values: CollectionSelectReturnValue
): string | null => {
  return values != null && Array.isArray(values) && values.length > 0
    ? values[0]
    : null
}

export interface CollectionSelectProps<T> {
  className?: string
  defaultValueKeys?: string[]
  id?: string
  isClearable?: boolean
  isMultiSelect?: boolean
  items?: T[]
  label?: string
  nameAccessor?: (item: T) => string
  onChangeCallback: (selectedValues: CollectionSelectReturnValue) => void
  placeholder?: React.ReactNode
  valueAccessor?: (item: T) => string
}

const customReactSelectStyles = {
  option: (provided: FixTypeLater, state: FixTypeLater): FixTypeLater => ({
    ...provided,
    borderRadius: '0px',
    backgroundColor: state.isSelected
      ? baseColor
      : state.isFocused
        ? focusShadowColor
        : 'white'
  }),
  menu: (provided: FixTypeLater): FixTypeLater => ({
    ...provided,
    borderRadius: '0px'
  }),
  control: (provided: FixTypeLater, state: FixTypeLater): FixTypeLater => {
    const styles = {
      ...provided,
      boxShadow: 'none',
      borderRadius: '0px',
      '&:focus': { borderRadius: '0px' }
    }
    if (state.menuIsOpen || state.isFocused) {
      styles['borderColor'] = focusBorderColor
      styles[':hover'] = { borderColor: focusBorderColor }
      styles['boxShadow'] = `0 0 0 0.2rem ${focusShadowColor}`
    }
    return styles
  }
}

type Props<T> = CollectionSelectProps<T>

class CollectionSelect<T> extends React.Component<Props<T>> {
  public constructor(props: Props<T>) {
    super(props)
    this.onChange = this.onChange.bind(this)
  }

  protected onChange(
    selectedItems: SingleValue<CollectionSelectValue> | MultiValue<CollectionSelectValue>
  ): void {
    if (selectedItems != null && !Array.isArray(selectedItems)) {
      // Selected items is not an array. But for simplicity, we want to return
      // it as such everywhere, even when multiselect is not enabled. So
      // make the selected item into an array.
      const value = (selectedItems as CollectionSelectValue).value
      if (value === '') {
        this.props.onChangeCallback(null)
      } else {
        this.props.onChangeCallback([value]) // Note we put the value in an array.
      }
    } else if (Array.isArray(selectedItems)) {
      // It's an array; just map and return
      const values = selectedItems.map(item => item.value)
      this.props.onChangeCallback(values)
    } else {
      // It's probably null.
      this.props.onChangeCallback(null)
    }
  }

  protected mapItems(items: T[]): CollectionSelectValue[] {
    return items.map(variable => {
      const value = this.props.valueAccessor
        ? this.props.valueAccessor(variable)
        : ''
      const label = this.props.nameAccessor
        ? this.props.nameAccessor(variable)
        : ''
      const isDefault = this.props.defaultValueKeys
        ? this.props.defaultValueKeys.includes(value)
        : false

      return { label, value, isDefault }
    })
  }

  public render(): JSX.Element {
    const items = this.props.items
    const options = items && items.length ? this.mapItems(items) : []
    const defaultOptions = this.props.defaultValueKeys
      ? options.filter(option => option.isDefault)
      : undefined

    const placeholder = this.props.placeholder

    return (
      <div className={'form-group ' + this.props.className}>
        {this.props.label && (
          <label htmlFor={this.props.id}>{this.props.label}</label>
        )}
        <ReactSelect
          className="ReactSelect form-control"
          defaultValue={defaultOptions}
          id={this.props.id}
          isClearable={
            this.props.isClearable === undefined || this.props.isClearable
          }
          isMulti={this.props.isMultiSelect}
          onChange={this.onChange}
          options={options}
          placeholder={placeholder}
          styles={customReactSelectStyles}
        />
      </div>
    )
  }
}

export default CollectionSelect
