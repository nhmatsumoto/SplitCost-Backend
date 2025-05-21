import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { useExpenses, ExpenseDto } from "../../hooks/useExpenses";
import {
  useReactTable,
  getCoreRowModel,
  getSortedRowModel,
  getFilteredRowModel,
  flexRender,
  ColumnDef,
  SortingState,
} from "@tanstack/react-table";

const ExpensesPage = () => {
  const { get } = useExpenses();
  const [expenses, setExpenses] = useState<ExpenseDto[]>([]);
  const [globalFilter, setGlobalFilter] = useState("");
  const [sorting, setSorting] = useState<SortingState>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let ignore = false;
    const fetchExpenses = async () => {
      setLoading(true);
      setError(null);
      try {
        const data = await get();
        if (!ignore) setExpenses(data);
      } catch (err: any) {
        if (!ignore) setError(err?.message || "Erro ao buscar despesas");
      } finally {
        if (!ignore) setLoading(false);
      }
    };
    fetchExpenses();
    return () => { ignore = true; };
    // O hook useExpenses retorna uma função estável, então o array de dependências deve ser vazio
    // eslint-disable-next-line
  }, []); 

  const columns = useMemo<ColumnDef<ExpenseDto>[]>(
    () => [
      {
        accessorKey: "expenseType",
        header: "Tipo",
        cell: info => info.getValue(),
      },
      {
        accessorKey: "amount",
        header: "Valor",
        cell: info => `R$ ${Number(info.getValue()).toFixed(2)}`,
      },
      {
        accessorKey: "date",
        header: "Data",
        cell: info => new Date(info.getValue() as string).toLocaleDateString(),
      },
      {
        accessorKey: "IsSharedAmongMembers",
        header: "Compartilhada?",
        cell: info => (info.getValue() ? "Sim" : "Não"),
      },
    ],
    []
  );

  const table = useReactTable({
    data: expenses,
    columns,
    state: {
      sorting,
      globalFilter,
    },
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    onSortingChange: setSorting,
    onGlobalFilterChange: setGlobalFilter,
    globalFilterFn: (row, columnId, filterValue) => {
      const value = row.getValue(columnId);
      return String(value ?? "")
        .toLowerCase()
        .includes(String(filterValue).toLowerCase());
    },
  });

  return (
    <>
      <Link to="/expense/create">
        <button className="bg-[#00796B] text-white px-4 py-2 rounded-lg mb-4">
          Create Expense
        </button>
      </Link>

      <div className="mb-4">
        <input
          className="border px-3 py-2 rounded w-full max-w-xs"
          placeholder="Pesquisar despesa..."
          value={globalFilter ?? ""}
          onChange={e => setGlobalFilter(e.target.value)}
        />
      </div>

      <div className="mt-4">
        {loading && <div>Carregando despesas...</div>}
        {error && <div className="text-red-500">{error}</div>}
        {!loading && !error && (
          <div className="overflow-x-auto">
            <table className="min-w-full bg-white border border-gray-200">
              <thead>
                {table.getHeaderGroups().map(headerGroup => (
                  <tr key={headerGroup.id}>
                    {headerGroup.headers.map(header => (
                      <th
                        key={header.id}
                        className="py-2 px-4 border-b cursor-pointer select-none"
                        onClick={header.column.getToggleSortingHandler()}
                      >
                        {flexRender(header.column.columnDef.header, header.getContext())}
                        {{
                          asc: " ▲",
                          desc: " ▼",
                        }[header.column.getIsSorted() as string] ?? null}
                      </th>
                    ))}
                  </tr>
                ))}
              </thead>
              <tbody>
                {table.getRowModel().rows.length === 0 ? (
                  <tr>
                    <td colSpan={columns.length} className="text-center py-4">
                      Nenhuma despesa encontrada.
                    </td>
                  </tr>
                ) : (
                  table.getRowModel().rows.map(row => (
                    <tr key={row.id}>
                      {row.getVisibleCells().map(cell => (
                        <td key={cell.id} className="py-2 px-4 border-b">
                          {flexRender(cell.column.columnDef.cell, cell.getContext())}
                        </td>
                      ))}
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </>
  );
};

export default ExpensesPage;