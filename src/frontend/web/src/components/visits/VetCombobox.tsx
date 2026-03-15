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
import type { Vet } from '@/types/models';

interface VetComboboxProps {
  vets: Vet[];
  value: string;
  onValueChange: (value: string) => void;
  placeholder?: string;
  includeAllOption?: boolean;
  disabled?: boolean;
}

export function VetCombobox({
  vets,
  value,
  onValueChange,
  placeholder = 'Select a vet',
  includeAllOption = false,
  disabled = false,
}: VetComboboxProps) {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState('');

  const selectedVet = vets.find((v) => v.id === value);

  const filtered = vets.filter((v) => {
    const term = search.toLowerCase();
    return (
      v.firstName.toLowerCase().includes(term) ||
      v.lastName.toLowerCase().includes(term)
    );
  });

  const handleSelect = (vetId: string) => {
    onValueChange(vetId === value ? '' : vetId);
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
          {value === '-1' && includeAllOption
            ? 'All vets'
            : selectedVet
            ? `${selectedVet.firstName} ${selectedVet.lastName}`
            : placeholder}
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-full p-0" align="start">
        <Command shouldFilter={false}>
          <CommandInput
            placeholder="Search vets..."
            value={search}
            onValueChange={setSearch}
          />
          <CommandList>
            <CommandEmpty>No vet found.</CommandEmpty>
            <CommandGroup>
              {includeAllOption && (
                <CommandItem
                  value="-1"
                  onSelect={() => handleSelect('-1')}
                  className="cursor-pointer"
                >
                  <Check
                    className={cn('mr-2 h-4 w-4', value === '-1' ? 'opacity-100' : 'opacity-0')}
                  />
                  All vets
                </CommandItem>
              )}
              {filtered.map((vet) => (
                <CommandItem
                  key={vet.id}
                  value={vet.id}
                  onSelect={() => handleSelect(vet.id)}
                  className="cursor-pointer"
                >
                  <Check
                    className={cn('mr-2 h-4 w-4', value === vet.id ? 'opacity-100' : 'opacity-0')}
                  />
                  <div className="flex flex-col">
                    <span>
                      {vet.firstName} {vet.lastName}
                    </span>
                    {vet.specializations.length > 0 && (
                      <span className="text-xs text-muted-foreground">
                        {vet.specializations.map((s) => s.name).join(', ')}
                      </span>
                    )}
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
