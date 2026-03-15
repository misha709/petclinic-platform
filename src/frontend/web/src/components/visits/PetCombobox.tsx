import { useState } from 'react';
import { Check, ChevronsUpDown } from 'lucide-react';
import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from '@/components/ui/command';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import type { Pet } from '@/types/models';

interface PetComboboxProps {
  pets: Pet[];
  value: string;
  onValueChange: (value: string) => void;
  placeholder?: string;
  disabled?: boolean;
}

export function PetCombobox({
  pets,
  value,
  onValueChange,
  placeholder = 'Select a pet',
  disabled = false,
}: PetComboboxProps) {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState('');

  const selectedPet = pets.find((p) => p.id === value);

  const filtered = pets.filter((p) => {
    const term = search.toLowerCase();
    return p.name.toLowerCase().includes(term) || p.breed.toLowerCase().includes(term);
  });

  const handleSelect = (petId: string) => {
    onValueChange(petId === value ? '' : petId);
    setOpen(false);
  };

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className="w-full justify-between"
          disabled={disabled}
        >
          {selectedPet ? `${selectedPet.name} (${selectedPet.petType})` : placeholder}
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-full p-0" align="start">
        <Command shouldFilter={false}>
          <CommandInput
            placeholder="Search pets..."
            value={search}
            onValueChange={setSearch}
          />
          <CommandList>
            <CommandEmpty>No pet found.</CommandEmpty>
            <CommandGroup>
              {filtered.map((pet) => (
                <CommandItem
                  key={pet.id}
                  value={pet.id}
                  onSelect={() => handleSelect(pet.id)}
                  className="cursor-pointer"
                >
                  <Check
                    className={cn('mr-2 h-4 w-4', value === pet.id ? 'opacity-100' : 'opacity-0')}
                  />
                  <div className="flex flex-col">
                    <span>{pet.name}</span>
                    <span className="text-xs text-muted-foreground">
                      {pet.petType} · {pet.breed}
                    </span>
                  </div>
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}
