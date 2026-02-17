import type { Owner, CreateOwnerRequest, UpdateOwnerRequest, Pet, CreatePetRequest, UpdatePetRequest } from '@/types/models';

const API_BASE = 'http://localhost:8080/api';

async function fetchJson<T>(url: string, options?: RequestInit): Promise<T> {
  const response = await fetch(url, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...options?.headers,
    },
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || `Request failed with status ${response.status}`);
  }

  return response.json();
}

// Owner API
export const ownersApi = {
  getAll: async (searchQuery?: string): Promise<Owner[]> => {
    const url = searchQuery
      ? `${API_BASE}/owners-service/owners?query=${encodeURIComponent(searchQuery)}`
      : `${API_BASE}/owners-service/owners`;
    return fetchJson<Owner[]>(url);
  },

  getById: async (id: string): Promise<Owner> => {
    return fetchJson<Owner>(`${API_BASE}/owners-service/owners/${id}`);
  },

  create: async (data: CreateOwnerRequest): Promise<Owner> => {
    return fetchJson<Owner>(`${API_BASE}/owners-service/owners`, {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },

  update: async (id: string, data: UpdateOwnerRequest): Promise<Owner> => {
    return fetchJson<Owner>(`${API_BASE}/owners-service/owners/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  },

  delete: async (id: string): Promise<void> => {
    await fetch(`${API_BASE}/owners-service/owners/${id}`, {
      method: 'DELETE',
    });
  },
};

// Pet API
export const petsApi = {
  getAll: async (ownerId?: string, query?: string): Promise<Pet[]> => {
    const url = new URL(`${API_BASE}/pets-service/pets`);
    if (ownerId && ownerId !== '-1') url.searchParams.set('ownerId', ownerId); // TODO review better way to handle this && ownerId !== '-1'
    if (query) url.searchParams.set('query', query);

    return fetchJson<Pet[]>(url.toString());
  },

  getById: async (id: string): Promise<Pet> => {
    return fetchJson<Pet>(`${API_BASE}/pets-service/pets/${id}`);
  },

  create: async (data: CreatePetRequest): Promise<Pet> => {
    return fetchJson<Pet>(`${API_BASE}/pets-service/pets`, {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },

  update: async (id: string, data: UpdatePetRequest): Promise<Pet> => {
    return fetchJson<Pet>(`${API_BASE}/pets-service/pets/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  },

  delete: async (id: string): Promise<void> => {
    await fetch(`${API_BASE}/pets-service/pets/${id}`, {
      method: 'DELETE',
    });
  },
};
