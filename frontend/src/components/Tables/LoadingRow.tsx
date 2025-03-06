import React, { type JSX } from 'react';

interface Props {
  loading: boolean;
  pageIndex: number;
  pageSize: number;
  recordCount: number;
  colSpan: number;
}

const LoadingRow = ({
  loading,
  pageSize,
  pageIndex,
  recordCount,
  colSpan = 10000
}: Props): JSX.Element => {
  const rangeMin = recordCount === 0 ? 0 : pageIndex * pageSize + 1
  const rangeMax = Math.min((pageIndex + 1) * pageSize, recordCount)
  return (
    (<tr>
      {loading ? (
        // Use our custom loading state to show a loading indicator
        (<td colSpan={colSpan}>Loading...</td>)
      ) : (
        <td colSpan={colSpan}>
          Showing {rangeMin} to {rangeMax} of {recordCount} results
        </td>
      )}
    </tr>)
  );
}

export default LoadingRow
