import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { vetsApi, specializationsApi } from '@/lib/api';
import type { CreateVetRequest, UpdateVetRequest, AssignSpecializationsRequest } from '@/types/models';
import { toast } from 'sonner';

export function useVets(query?: string) {
  return useQuery({
    queryKey: ['vets', query],
    queryFn: () => vetsApi.getAll(query),
  });
}

export function useVet(id: string | undefined) {
  return useQuery({
    queryKey: ['vets', id],
    queryFn: () => vetsApi.getById(id!),
    enabled: !!id,
  });
}

export function useSpecializations() {
  return useQuery({
    queryKey: ['specializations'],
    queryFn: () => specializationsApi.getAll(),
  });
}

export function useCreateVet() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateVetRequest) => vetsApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['vets'] });
      toast.success('Vet created successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to create vet: ${error.message}`);
    },
  });
}

export function useUpdateVet() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateVetRequest }) =>
      vetsApi.update(id, data),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['vets'] });
      queryClient.invalidateQueries({ queryKey: ['vets', variables.id] });
      toast.success('Vet updated successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to update vet: ${error.message}`);
    },
  });
}

export function useDeleteVet() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => vetsApi.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['vets'] });
      toast.success('Vet deleted successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to delete vet: ${error.message}`);
    },
  });
}

export function useAssignSpecializations() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: AssignSpecializationsRequest }) =>
      vetsApi.assignSpecializations(id, data),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['vets'] });
      queryClient.invalidateQueries({ queryKey: ['vets', variables.id] });
    },
    onError: (error: Error) => {
      toast.error(`Failed to assign specializations: ${error.message}`);
    },
  });
}
