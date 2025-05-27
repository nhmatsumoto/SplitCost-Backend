import React, { useState } from "react";

type User = {
  id: number;
  name: string;
};

const mockUsers: User[] = [
  { id: 1, name: "Alice" },
  { id: 2, name: "Bob" },
  { id: 3, name: "Charlie" },
  { id: 4, name: "Diana" },
];

const UserSelector = () => {
  const [showDropdown, setShowDropdown] = useState(false);
  const [selectedUsers, setSelectedUsers] = useState<User[]>([]);

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setShowDropdown(e.target.checked);
    if (!e.target.checked) {
      setSelectedUsers([]);
    }
  };

  const handleSelectChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedId = Number(e.target.value);
    const user = mockUsers.find((u) => u.id === selectedId);

    if (user && !selectedUsers.some((u) => u.id === user.id)) {
      setSelectedUsers((prev) => [...prev, user]);
    }
  };

  const handleRemoveUser = (id: number) => {
    setSelectedUsers((prev) => prev.filter((user) => user.id !== id));
  };

  return (
    <div className="p-4 border rounded max-w-md space-y-4">
      <label>
        <input type="checkbox" onChange={handleCheckboxChange} />
        <span className="ml-2">Selecionar usuários</span>
      </label>

      {showDropdown && (
        <div>
          <label className="block mb-2">Escolha um usuário:</label>
          <select onChange={handleSelectChange} defaultValue="">
            <option value="" disabled>
              -- Selecione --
            </option>
            {mockUsers.map((user) => (
              <option key={user.id} value={user.id}>
                {user.name}
              </option>
            ))}
          </select>
        </div>
      )}

      {selectedUsers.length > 0 && (
        <div>
          <h4 className="font-bold">Usuários selecionados:</h4>
          <ul className="list-disc list-inside">
            {selectedUsers.map((user) => (
              <li key={user.id}>
                {user.name}{" "}
                <button
                  className="text-red-500 ml-2"
                  onClick={() => handleRemoveUser(user.id)}
                >
                  Remover
                </button>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default UserSelector;
