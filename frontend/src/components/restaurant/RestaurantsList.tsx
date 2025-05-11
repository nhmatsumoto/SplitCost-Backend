import { useEffect, useMemo, useState } from 'react';
import { useKeycloak } from '@react-keycloak/web';
import { createApiClient } from '../../api/client';
import { useNavigate } from 'react-router-dom';
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
import { RestaurantDto } from '../../hooks/useRestaurants';
import Loader from '../ui/Loader';

export const RestaurantsList = () => {
  const { keycloak } = useKeycloak();
  const navigate = useNavigate();

  const api = useMemo(() => createApiClient(keycloak), [keycloak]);

  const [restaurants, setRestaurants] = useState<RestaurantDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [globalFilter, setGlobalFilter] = useState('');

  useEffect(() => {
    const fetchRestaurants = async () => {
      try {
        const response = await api.get<RestaurantDto[]>('/restaurants');
        setRestaurants(response.data);
      } catch (err) {
        console.error('Erro ao buscar restaurantes:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchRestaurants();
  }, [api]);

  const columns: ColumnDef<RestaurantDto>[] = useMemo(
    () => [
      {
        accessorKey: 'name',
        header: 'Nome',
        cell: (info) => info.getValue(),
      },
      {
        accessorKey: 'totalDishesSold',
        header: 'Pratos Vendidos',
        cell: (info) => Number(info.getValue()).toLocaleString(),
      },
      {
        id: 'actions',
        header: () => <div className="text-center">Ações</div>,
        cell: ({ row }) => (
          <div className="text-center">
            <button
              onClick={() => navigate(`/restaurantes/${row.original.restaurantId}`)}
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

  const table = useReactTable<RestaurantDto>({
    data: restaurants,
    columns,
    state: { globalFilter },
    onGlobalFilterChange: setGlobalFilter,
    getCoreRowModel: getCoreRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    getSortedRowModel: getSortedRowModel(),
  });

  if (loading) return <Loader />;

  return (
    <div className="space-y-6">
      {/* Filtro */}
      <div className="flex justify-between items-center">
        <input
          type="text"
          placeholder="Buscar restaurante..."
          value={globalFilter}
          onChange={(e) => setGlobalFilter(e.target.value)}
          className="w-full max-w-md px-4 py-2 border border-[#E0E0E0] rounded-lg shadow-sm focus:ring-2 focus:ring-[#00796B] focus:outline-none text-sm text-[#2E2E2E]"
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
                    className="px-6 py-3 text-left font-semibold cursor-pointer select-none"
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
      <div className="flex items-center justify-between gap-2 text-sm">
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