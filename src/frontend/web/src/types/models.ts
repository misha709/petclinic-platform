export interface Owner {
  id: string;
  firstName: string;
  lastName: string;
  address: string;
  city: string;
  telephone: string;
  email: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateOwnerRequest {
  firstName: string;
  lastName: string;
  address: string;
  city: string;
  telephone: string;
  email: string;
}

export interface UpdateOwnerRequest {
  firstName: string;
  lastName: string;
  address: string;
  city: string;
  telephone: string;
  email: string;
}

export interface Pet {
  id: string;
  name: string;
  petType: PetType;
  breed: string;
  birthDate?: string;
  ownerId: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreatePetRequest {
  name: string;
  petType: string;
  breed: string;
  birthDate?: string;
  ownerId: string;
}

export interface UpdatePetRequest {
  name: string;
  petType: string;
  breed: string;
  birthDate?: string;
  ownerId: string;
}

export type PetType = 
  | 'Dog'
  | 'Cat'
  | 'Bird'
  | 'Fish'
  | 'Rabbit'
  | 'Hamster'
  | 'GuineaPig'
  | 'Ferret'
  | 'Reptile'
  | 'Other';

export const PET_TYPES: PetType[] = [
  'Dog',
  'Cat',
  'Bird',
  'Fish',
  'Rabbit',
  'Hamster',
  'GuineaPig',
  'Ferret',
  'Reptile',
  'Other',
];

export interface Specialization {
  id: number;
  name: string;
  createdAt: string;
  updatedAt?: string;
}

export interface Vet {
  id: string;
  firstName: string;
  lastName: string;
  specializations: Specialization[];
  createdAt: string;
  updatedAt?: string;
}

export interface CreateVetRequest {
  firstName: string;
  lastName: string;
}

export interface UpdateVetRequest {
  firstName: string;
  lastName: string;
}

export interface AssignSpecializationsRequest {
  specializationIds: number[];
}

export interface CreateSpecializationRequest {
  name: string;
}

export interface UpdateSpecializationRequest {
  name: string;
}

export type VisitStatus = 'Scheduled' | 'Completed' | 'Cancelled';

export const VISIT_STATUSES: VisitStatus[] = ['Scheduled', 'Completed', 'Cancelled'];

export interface Visit {
  id: string;
  petId: string;
  vetId: string;
  scheduledAt: string;
  durationMinutes: number;
  reason: string;
  notes?: string;
  status: VisitStatus;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateVisitRequest {
  petId: string;
  vetId: string;
  scheduledAt: string;
  durationMinutes: number;
  reason: string;
  notes?: string;
}

export interface UpdateVisitRequest {
  scheduledAt: string;
  durationMinutes: number;
  reason: string;
  notes?: string;
}

export interface CancelVisitRequest {
  notes?: string;
}
