/* eslint react/jsx-key: "off" */
// Turn off the jsx-key warning; it goes off because the spread operators used
// by react-table are not obviously supplying the key (but it actually does get
// supplied when rendering in browser).

import React, { type JSX } from 'react';
import { useReactTable, getCoreRowModel, getSortedRowModel, getPaginationRowModel, ColumnDef, flexRender } from '@tanstack/react-table';
import { FixTypeLater } from '../../types/FixTypeLater';
import ColumnSortIndicator from './ColumnSortIndicator';
import LoadingRow from './LoadingRow';
import Pagination from './Pagination';

interface Props<T extends object> {
  data: T[];
  columns: ColumnDef<T>[];
  fetchData: (options: FixTypeLater) => FixTypeLater;
  loading: boolean;
  controlledPageCount: number;
  controlledPageIndex: number;
  recordCount: number;
  pageSize: number;
}

const GenericTable = <T extends object>(props: Props<T>): JSX.Element => {
  const {
    data,
    columns,
    fetchData,
    loading,
    controlledPageCount,
    controlledPageIndex,
    recordCount,
    pageSize
  } = props;

  const table = useReactTable({
    data,
    columns,
    pageCount: controlledPageCount,
    state: {
      pagination: {
        pageIndex: controlledPageIndex,
        pageSize
      }
    },
    manualPagination: true,
    manualSorting: true,
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    onPaginationChange: (updater) => {
      const newState = typeof updater === 'function' ? updater({ pageIndex: controlledPageIndex, pageSize }) : updater;
      fetchData({ pageIndex: newState.pageIndex, pageSize: newState.pageSize });
    },
    onSortingChange: (updater) => {
      const newState = typeof updater === 'function' ? updater([]) : updater;
      fetchData({ pageIndex: controlledPageIndex, pageSize, sorting: newState });
    }
  });

  return (
    <div>
      <table>
        <thead>
          {table.getHeaderGroups().map(headerGroup => (
            <tr key={headerGroup.id}>
              {headerGroup.headers.map(header => (
                <th key={header.id}>
                  {header.isPlaceholder ? null : (
                    <div {...{
                      onClick: header.column.getToggleSortingHandler(),
                      style: { cursor: 'pointer' }
                    }}>
                      {flexRender(header.column.columnDef.header, header.getContext())}
                      <ColumnSortIndicator column={header.column} />
                    </div>
                  )}
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody>
          {loading ? (
            <LoadingRow
              colSpan={columns.length}
              loading={loading}
              pageIndex={controlledPageIndex}
              pageSize={pageSize}
              recordCount={recordCount}
            />
          ) : (
            table.getRowModel().rows.map(row => (
              <tr key={row.id}>
                {row.getVisibleCells().map(cell => (
                  <td key={cell.id}>
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </td>
                ))}
              </tr>
            ))
          )}
        </tbody>
      </table>
      <Pagination
        pageCount={controlledPageCount}
        pageIndex={controlledPageIndex}
        canPreviousPage={controlledPageIndex > 0}
        canNextPage={controlledPageIndex < controlledPageCount - 1}
        gotoPage={(pageIndex) => fetchData({ pageIndex, pageSize })}
        previousPage={() => fetchData({ pageIndex: controlledPageIndex - 1, pageSize })}
        nextPage={() => fetchData({ pageIndex: controlledPageIndex + 1, pageSize })}
      />
    </div>
  );
};

export default GenericTable;
