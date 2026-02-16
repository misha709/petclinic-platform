import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  CommandDialog,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from '@/components/ui/command';
import { useGlobalSearch } from '@/hooks/useGlobalSearch';
import { User, PawPrint } from 'lucide-react';

interface GlobalSearchProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function GlobalSearch({ open, onOpenChange }: GlobalSearchProps) {
  const [query, setQuery] = useState('');
  const { results } = useGlobalSearch(query);
  const navigate = useNavigate();

  useEffect(() => {
    const down = (e: KeyboardEvent) => {
      if (e.key === 'k' && (e.metaKey || e.ctrlKey)) {
        e.preventDefault();
        onOpenChange(!open);
      }
    };

    document.addEventListener('keydown', down);
    return () => document.removeEventListener('keydown', down);
  }, [open, onOpenChange]);

  const handleSelect = (result: typeof results[0]) => {
    if (result.type === 'owner') {
      navigate(`/owners?id=${result.data.id}`);
    } else if (result.type === 'pet') {
      navigate(`/pets?id=${result.data.id}`);
    }
    onOpenChange(false);
    setQuery('');
  };

  const owners = results.filter((r) => r.type === 'owner');
  const pets = results.filter((r) => r.type === 'pet');

  return (
    <CommandDialog open={open} onOpenChange={onOpenChange}>
      <CommandInput
        placeholder="Search owners and pets..."
        value={query}
        onValueChange={setQuery}
      />
      <CommandList>
        <CommandEmpty>No results found.</CommandEmpty>
        
        {owners.length > 0 && (
          <CommandGroup heading="Owners">
            {owners.map((result) => {
              const owner = result.data;
              return (
                <CommandItem
                  key={owner.id}
                  onSelect={() => handleSelect(result)}
                  className="cursor-pointer"
                >
                  <User className="mr-2 h-4 w-4" />
                  <div className="flex flex-col">
                    <span>{owner.firstName} {owner.lastName}</span>
                    <span className="text-xs text-muted-foreground">
                      {owner.city} - {owner.telephone}
                    </span>
                  </div>
                </CommandItem>
              );
            })}
          </CommandGroup>
        )}

        {pets.length > 0 && (
          <CommandGroup heading="Pets">
            {pets.map((result) => {
              const pet = result.data;
              return (
                <CommandItem
                  key={pet.id}
                  onSelect={() => handleSelect(result)}
                  className="cursor-pointer"
                >
                  <PawPrint className="mr-2 h-4 w-4" />
                  <div className="flex flex-col">
                    <span>{pet.name}</span>
                    <span className="text-xs text-muted-foreground">
                      {pet.petType} - {pet.breed}
                    </span>
                  </div>
                </CommandItem>
              );
            })}
          </CommandGroup>
        )}
      </CommandList>
    </CommandDialog>
  );
}
