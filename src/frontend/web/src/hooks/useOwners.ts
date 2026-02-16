import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { ownersApi } from '@/lib/api';
import type { CreateOwnerRequest, UpdateOwnerRequest } from '@/types/models';
import { toast } from 'sonner';

export function useOwners(searchQuery?: string) {
  return useQuery({
    queryKey: ['owners', searchQuery],
    queryFn: () => ownersApi.getAll(searchQuery),
  });
}

export function useOwner(id: string | undefined) {
  return useQuery({
    queryKey: ['owners', id],
    queryFn: () => ownersApi.getById(id!),
    enabled: !!id,
  });
}

export function useCreateOwner() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateOwnerRequest) => ownersApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['owners'] });
      toast.success('Owner created successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to create owner: ${error.message}`);
    },
  });
}

export function useUpdateOwner() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateOwnerRequest }) =>
      ownersApi.update(id, data),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['owners'] });
      queryClient.invalidateQueries({ queryKey: ['owners', variables.id] });
      toast.success('Owner updated successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to update owner: ${error.message}`);
    },
  });
}

export function useDeleteOwner() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => ownersApi.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['owners'] });
      toast.success('Owner deleted successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to delete owner: ${error.message}`);
    },
  });
}
