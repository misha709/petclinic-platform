import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { petsApi } from '@/lib/api';
import type { CreatePetRequest, UpdatePetRequest } from '@/types/models';
import { toast } from 'sonner';

export function usePets(ownerId?: string, query?: string) {
  return useQuery({
    queryKey: ['pets', ownerId, query],
    queryFn: () => petsApi.getAll(ownerId, query)
  });
}

export function usePet(id: string | undefined) {
  return useQuery({
    queryKey: ['pets', id],
    queryFn: () => petsApi.getById(id!),
    enabled: !!id,
  });
}

export function useCreatePet() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreatePetRequest) => petsApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['pets'] });
      toast.success('Pet created successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to create pet: ${error.message}`);
    },
  });
}

export function useUpdatePet() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdatePetRequest }) =>
      petsApi.update(id, data),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['pets'] });
      queryClient.invalidateQueries({ queryKey: ['pets', variables.id] });
      toast.success('Pet updated successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to update pet: ${error.message}`);
    },
  });
}

export function useDeletePet() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => petsApi.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['pets'] });
      toast.success('Pet deleted successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to delete pet: ${error.message}`);
    },
  });
}
