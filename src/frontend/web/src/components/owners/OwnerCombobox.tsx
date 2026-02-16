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
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import type { Owner } from '@/types';

interface OwnerComboboxProps {
  owners: Owner[];
  value: string;
  onValueChange: (value: string) => void;
  placeholder?: string;
  includeAllOption?: boolean;
  disabled?: boolean;
}

export function OwnerCombobox({
  owners,
  value,
  onValueChange,
  placeholder = 'Select an owner',
  includeAllOption = false,
  disabled = false,
}: OwnerComboboxProps) {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState('');

  const selectedOwner = owners.find((owner) => owner.id === value);

  const filteredOwners = owners.filter((owner) => {
    const searchLower = search.toLowerCase();
    return (
      owner.firstName.toLowerCase().includes(searchLower) ||
      owner.lastName.toLowerCase().includes(searchLower) ||
      owner.telephone.toLowerCase().includes(searchLower)
    );
  });

  const handleSelect = (ownerId: string) => {
    onValueChange(ownerId === value ? '' : ownerId);
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
            ? 'All owners'
            : selectedOwner
            ? `${selectedOwner.firstName} ${selectedOwner.lastName}`
            : placeholder}
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-full p-0" align="start">
        <Command shouldFilter={false}>
          <CommandInput
            placeholder="Search owners..."
            value={search}
            onValueChange={setSearch}
          />
          <CommandList>
            <CommandEmpty>No owner found.</CommandEmpty>
            <CommandGroup>
              {includeAllOption && (
                <CommandItem
                  value="-1"
                  onSelect={() => handleSelect('-1')}
                  className="cursor-pointer"
                >
                  <Check
                    className={cn(
                      'mr-2 h-4 w-4',
                      value === '-1' ? 'opacity-100' : 'opacity-0'
                    )}
                  />
                  All owners
                </CommandItem>
              )}
              {filteredOwners.map((owner) => (
                <CommandItem
                  key={owner.id}
                  value={owner.id}
                  onSelect={() => handleSelect(owner.id)}
                  className="cursor-pointer"
                >
                  <Check
                    className={cn(
                      'mr-2 h-4 w-4',
                      value === owner.id ? 'opacity-100' : 'opacity-0'
                    )}
                  />
                  <div className="flex flex-col">
                    <span>
                      {owner.firstName} {owner.lastName}
                    </span>
                    <span className="text-xs text-muted-foreground">
                      {owner.telephone}
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
