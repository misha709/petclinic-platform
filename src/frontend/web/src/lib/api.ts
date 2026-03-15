import type { Owner, CreateOwnerRequest, UpdateOwnerRequest, Pet, CreatePetRequest, UpdatePetRequest, Vet, CreateVetRequest, UpdateVetRequest, AssignSpecializationsRequest, Specialization, CreateSpecializationRequest, UpdateSpecializationRequest, Visit, CreateVisitRequest, UpdateVisitRequest, CancelVisitRequest } from '@/types/models';

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

// Vets API
export const vetsApi = {
  getAll: async (searchQuery?: string): Promise<Vet[]> => {
    const url = searchQuery
      ? `${API_BASE}/vets-service/vets?query=${encodeURIComponent(searchQuery)}`
      : `${API_BASE}/vets-service/vets`;
    return fetchJson<Vet[]>(url);
  },

  getById: async (id: string): Promise<Vet> => {
    return fetchJson<Vet>(`${API_BASE}/vets-service/vets/${id}`);
  },

  create: async (data: CreateVetRequest): Promise<Vet> => {
    return fetchJson<Vet>(`${API_BASE}/vets-service/vets`, {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },

  update: async (id: string, data: UpdateVetRequest): Promise<Vet> => {
    return fetchJson<Vet>(`${API_BASE}/vets-service/vets/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  },

  delete: async (id: string): Promise<void> => {
    await fetch(`${API_BASE}/vets-service/vets/${id}`, {
      method: 'DELETE',
    });
  },

  assignSpecializations: async (id: string, data: AssignSpecializationsRequest): Promise<Vet> => {
    return fetchJson<Vet>(`${API_BASE}/vets-service/vets/${id}/specializations`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  },
};

// Specializations API
export const specializationsApi = {
  getAll: async (searchQuery?: string): Promise<Specialization[]> => {
    const url = searchQuery
      ? `${API_BASE}/vets-service/specializations?query=${encodeURIComponent(searchQuery)}`
      : `${API_BASE}/vets-service/specializations`;
    return fetchJson<Specialization[]>(url);
  },

  getById: async (id: number): Promise<Specialization> => {
    return fetchJson<Specialization>(`${API_BASE}/vets-service/specializations/${id}`);
  },

  create: async (data: CreateSpecializationRequest): Promise<Specialization> => {
    return fetchJson<Specialization>(`${API_BASE}/vets-service/specializations`, {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },

  update: async (id: number, data: UpdateSpecializationRequest): Promise<Specialization> => {
    return fetchJson<Specialization>(`${API_BASE}/vets-service/specializations/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  },

  delete: async (id: number): Promise<void> => {
    await fetch(`${API_BASE}/vets-service/specializations/${id}`, {
      method: 'DELETE',
    });
  },
};

// Visits API
export const visitsApi = {
  getAll: async (params?: { petId?: string; vetId?: string; date?: string; status?: string }): Promise<Visit[]> => {
    const url = new URL(`${API_BASE}/visits-service/visits`);
    if (params?.petId) url.searchParams.set('petId', params.petId);
    if (params?.vetId) url.searchParams.set('vetId', params.vetId);
    if (params?.date) url.searchParams.set('date', params.date);
    if (params?.status) url.searchParams.set('status', params.status);
    return fetchJson<Visit[]>(url.toString());
  },

  getById: async (id: string): Promise<Visit> => {
    return fetchJson<Visit>(`${API_BASE}/visits-service/visits/${id}`);
  },

  create: async (data: CreateVisitRequest): Promise<Visit> => {
    return fetchJson<Visit>(`${API_BASE}/visits-service/visits`, {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },

  update: async (id: string, data: UpdateVisitRequest): Promise<Visit> => {
    return fetchJson<Visit>(`${API_BASE}/visits-service/visits/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  },

  cancel: async (id: string, data: CancelVisitRequest): Promise<Visit> => {
    return fetchJson<Visit>(`${API_BASE}/visits-service/visits/${id}/cancel`, {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },

  complete: async (id: string): Promise<Visit> => {
    return fetchJson<Visit>(`${API_BASE}/visits-service/visits/${id}/complete`, {
      method: 'POST',
    });
  },

  delete: async (id: string): Promise<void> => {
    await fetch(`${API_BASE}/visits-service/visits/${id}`, {
      method: 'DELETE',
    });
  },
};
