import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { visitsApi } from '@/lib/api';
import type { CreateVisitRequest, UpdateVisitRequest, CancelVisitRequest } from '@/types/models';
import { toast } from 'sonner';

interface VisitFilters {
  petId?: string;
  vetId?: string;
  date?: string;
  status?: string;
}

export function useVisits(filters?: VisitFilters) {
  return useQuery({
    queryKey: ['visits', filters],
    queryFn: () => visitsApi.getAll(filters),
  });
}

export function useVisit(id: string | undefined) {
  return useQuery({
    queryKey: ['visits', id],
    queryFn: () => visitsApi.getById(id!),
    enabled: !!id,
  });
}

export function useCreateVisit() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateVisitRequest) => visitsApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['visits'] });
      toast.success('Visit scheduled successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to schedule visit: ${error.message}`);
    },
  });
}

export function useUpdateVisit() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateVisitRequest }) =>
      visitsApi.update(id, data),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['visits'] });
      queryClient.invalidateQueries({ queryKey: ['visits', variables.id] });
      toast.success('Visit updated successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to update visit: ${error.message}`);
    },
  });
}

export function useCancelVisit() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: CancelVisitRequest }) =>
      visitsApi.cancel(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['visits'] });
      toast.success('Visit cancelled');
    },
    onError: (error: Error) => {
      toast.error(`Failed to cancel visit: ${error.message}`);
    },
  });
}

export function useCompleteVisit() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => visitsApi.complete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['visits'] });
      toast.success('Visit marked as completed');
    },
    onError: (error: Error) => {
      toast.error(`Failed to complete visit: ${error.message}`);
    },
  });
}

export function useDeleteVisit() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => visitsApi.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['visits'] });
      toast.success('Visit deleted successfully');
    },
    onError: (error: Error) => {
      toast.error(`Failed to delete visit: ${error.message}`);
    },
  });
}
