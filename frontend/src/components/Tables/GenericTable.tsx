/* eslint react/jsx-key: "off" */
// Turn off the jsx-key warning; it goes off because the spread operators used
// by react-table are not obviously supplying the key (but it actually does get
// supplied when rendering in browser).

import React, { type JSX, useEffect, useState } from 'react';
import {
  useReactTable, getCoreRowModel, getSortedRowModel,
  getPaginationRowModel, ColumnDef, flexRender, SortingState
} from '@tanstack/react-table';
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

  const [sorting, setSorting] = useState<SortingState>([]);

  useEffect(() => {
    fetchData({ pageIndex: controlledPageIndex, pageSize, sortBy: sorting });
  }, [controlledPageIndex, pageSize, sorting, fetchData]);

  const table = useReactTable({
    data,
    columns,
    pageCount: controlledPageCount,
    state: {
      pagination: {
        pageIndex: controlledPageIndex,
        pageSize
      },
      sorting
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
    onSortingChange: setSorting
  });

  return (
    <div>
      <Pagination
        pageCount={controlledPageCount}
        pageIndex={controlledPageIndex}
        canPreviousPage={controlledPageIndex > 0}
        canNextPage={controlledPageIndex < controlledPageCount - 1}
        gotoPage={(pageIndex) => fetchData({ pageIndex, pageSize })}
        previousPage={() => fetchData({ pageIndex: controlledPageIndex - 1, pageSize })}
        nextPage={() => fetchData({ pageIndex: controlledPageIndex + 1, pageSize })}
      />
      <table className="table table-sm table-striped">
        <thead>
          {table.getHeaderGroups().map((headerGroup) => {
            return (
              <tr key={headerGroup.id}>
                {headerGroup.headers.map((header) => {
                  return (
                    <th key={header.id} colSpan={header.colSpan}>
                      {header.isPlaceholder ? null : (
                        <span {...{
                          onClick: header.column.getToggleSortingHandler(),
                          title: 'Toggle SortBy',
                          style: { cursor: 'pointer' }
                        }}>
                          {flexRender(header.column.columnDef.header, header.getContext())}
                          <ColumnSortIndicator column={header.column} />
                        </span>
                      )}
                    </th>
                  )
                })}
              </tr>
            )
          })}
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
            table.getRowModel().rows.map((row) => {
              return (
                <tr key={row.id}>
                  {row.getVisibleCells().map((cell) => {
                    return (
                      <td key={cell.id}>
                        {flexRender(cell.column.columnDef.cell, cell.getContext())}
                      </td>
                    );
                  })}
                </tr>
              )
            })
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
