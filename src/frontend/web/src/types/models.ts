export interface Owner {
  id: string;
  firstName: string;
  lastName: string;
  address: string;
  city: string;
  telephone: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateOwnerRequest {
  firstName: string;
  lastName: string;
  address: string;
  city: string;
  telephone: string;
}

export interface UpdateOwnerRequest {
  firstName: string;
  lastName: string;
  address: string;
  city: string;
  telephone: string;
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
