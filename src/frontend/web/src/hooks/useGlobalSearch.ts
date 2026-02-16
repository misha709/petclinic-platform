import { useState, useEffect } from 'react';
import { useQuery } from '@tanstack/react-query';
import { ownersApi } from '@/lib/api';
import type { Owner, Pet } from '@/types/models';

export type SearchResult = 
  | { type: 'owner'; data: Owner }
  | { type: 'pet'; data: Pet };

export function useGlobalSearch(query: string) {
  const [debouncedQuery, setDebouncedQuery] = useState(query);

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedQuery(query);
    }, 300);

    return () => clearTimeout(timer);
  }, [query]);

  const { data: owners = [] } = useQuery({
    queryKey: ['owners', debouncedQuery],
    queryFn: () => ownersApi.getAll(debouncedQuery),
    enabled: debouncedQuery.length > 0,
  });

  // For pets, we'll need to search across all pets
  // Since the API only supports filtering by ownerId, we'll implement client-side filtering
  const results: SearchResult[] = [
    ...owners.map((owner) => ({ type: 'owner' as const, data: owner })),
  ];

  return { results, isLoading: false };
}
