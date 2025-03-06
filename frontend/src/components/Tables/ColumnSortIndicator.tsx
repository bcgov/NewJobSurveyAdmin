import React, { type JSX } from 'react';
import { Column } from '@tanstack/react-table';
import FAIcon from '../DisplayHelpers/Interface/Icons/FAIcon'

interface Props<T> {
  column: Column<T, unknown>
}

const ColumnSortIndicator = <T,>(props: Props<T>): JSX.Element => {
  const { column } = props
  return (
    <span>
      {column.getIsSorted() ? (
        column.getIsSorted() === 'desc' ? (
          <FAIcon name="caret-up" marginClasses="ms-1" />
        ) : (
          <FAIcon name="caret-down" marginClasses="ms-1" />
        )
      ) : (
        ''
      )}
    </span>
  );
};

export default ColumnSortIndicator;
