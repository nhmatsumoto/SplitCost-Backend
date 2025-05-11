import {
  useReactTable,
  getCoreRowModel,
  getFilteredRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  flexRender,
  ColumnDef,
} from '@tanstack/react-table';
import { Eye } from 'lucide-react';
import { useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { RecipeDto, RecipeIngredientDto } from '../../hooks/useRecipes';

interface RecipeListProps {
  recipes: RecipeDto[];
}

export const RecipeList = ({ recipes }: RecipeListProps) => {
  const navigate = useNavigate();
  const [globalFilter, setGlobalFilter] = useState('');

  const columns: ColumnDef<RecipeDto>[] = useMemo(
    () => [
      {
        accessorKey: 'name',
        header: 'Nome',
        cell: (info) => info.getValue(),
      },
      {
        accessorKey: 'profitMargin',
        header: 'Margem de Lucro',
        cell: (info) => `${(Number(info.getValue()) * 100).toFixed(0)}%`,
      },
      {
        accessorKey: 'ingredients',
        header: 'Nº de Ingredientes',
        cell: (info) => (info.getValue() as RecipeIngredientDto[]).length,
      },
      {
        id: 'actions',
        header: () => <div className="text-center">Ações</div>,
        cell: ({ row }) => (
          <div className="text-center">
            <button
              onClick={() => navigate(`/receitas/${row.original.recipeId}`)}
              className="text-[#00796B] hover:text-[#005B4D] transition-colors duration-200"
              title="Ver detalhes"
            >
              <Eye size={18} />
            </button>
          </div>
        ),
      },
    ],
    [navigate]
  );

  const table = useReactTable<RecipeDto>({
    data: recipes,
    columns,
    state: { globalFilter },
    onGlobalFilterChange: setGlobalFilter,
    getCoreRowModel: getCoreRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    getSortedRowModel: getSortedRowModel(),
  });

  return (
    <div className="space-y-6">
      {/* Campo de busca */}
      <div className="flex justify-between items-center">
        <input
          type="text"
          placeholder="Buscar receita..."
          value={globalFilter}
          onChange={(e) => setGlobalFilter(e.target.value)}
          className="w-full max-w-md px-4 py-2 border border-[#E0E0E0] rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-[#00796B] text-sm text-[#2E2E2E]"
        />
      </div>

      {/* Tabela */}
      <div className="overflow-x-auto rounded-lg shadow-sm border border-[#E0E0E0]">
        <table className="min-w-full bg-white text-sm">
          <thead className="bg-[#D8F3DC] text-[#00796B]">
            {table.getHeaderGroups().map((headerGroup) => (
              <tr key={headerGroup.id}>
                {headerGroup.headers.map((header) => (
                  <th
                    key={header.id}
                    onClick={header.column.getToggleSortingHandler()}
                    className="px-6 py-3 font-semibold cursor-pointer select-none text-left"
                  >
                    {flexRender(header.column.columnDef.header, header.getContext())}
                    {header.column.getIsSorted() === 'asc'
                      ? ' 🔼'
                      : header.column.getIsSorted() === 'desc'
                      ? ' 🔽'
                      : ''}
                  </th>
                ))}
              </tr>
            ))}
          </thead>
          <tbody>
            {table.getRowModel().rows.map((row) => (
              <tr
                key={row.id}
                className="border-t border-[#F0F0F0] hover:bg-[#FAFAFA] transition-colors duration-200"
              >
                {row.getVisibleCells().map((cell) => (
                  <td key={cell.id} className="px-6 py-4 text-[#2E2E2E]">
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Paginação */}
      <div className="flex items-center justify-between text-sm">
        <button
          onClick={() => table.previousPage()}
          disabled={!table.getCanPreviousPage()}
          className="px-4 py-2 rounded bg-[#E0E0E0] text-[#2E2E2E] disabled:opacity-50"
        >
          Anterior
        </button>
        <span className="text-[#9EA7AD]">
          Página {table.getState().pagination.pageIndex + 1} de {table.getPageCount()}
        </span>
        <button
          onClick={() => table.nextPage()}
          disabled={!table.getCanNextPage()}
          className="px-4 py-2 rounded bg-[#E0E0E0] text-[#2E2E2E] disabled:opacity-50"
        >
          Próxima
        </button>
      </div>
    </div>
  );
};