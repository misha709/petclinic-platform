import { useEffect, useState, type ChangeEvent } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
} from '@/components/ui/sheet';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Calendar } from '@/components/ui/calendar';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { petSchema, type PetFormValues } from '@/lib/validations/pet';
import type { Pet } from '@/types/models';
import { useCreatePet, useUpdatePet } from '@/hooks/usePets';
import { useOwners } from '@/hooks/useOwners';
import { PetTypeSelect } from './PetTypeSelect';
import { OwnerCombobox } from '@/components/owners/OwnerCombobox';
import { CalendarIcon } from 'lucide-react';
import { format, parse, isValid } from 'date-fns';

interface BirthDateFieldProps {
  value: Date | null;
  onChange: (date: Date | null) => void;
}

function BirthDateField({ value, onChange }: BirthDateFieldProps) {
  const [open, setOpen] = useState(false);
  const [prevValue, setPrevValue] = useState(value);
  const [inputValue, setInputValue] = useState(() =>
    value ? format(value, 'MM/dd/yyyy') : ''
  );

  if (prevValue !== value) {
    setPrevValue(value);
    setInputValue(value ? format(value, 'MM/dd/yyyy') : '');
  }

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    let raw = e.target.value.replace(/\D/g, '');
    if (raw.length >= 2) raw = raw.slice(0, 2) + '/' + raw.slice(2);
    if (raw.length >= 5) raw = raw.slice(0, 5) + '/' + raw.slice(5, 9);
    setInputValue(raw);
    if (raw.length === 10) {
      const parsed = parse(raw, 'MM/dd/yyyy', new Date());
      if (isValid(parsed) && parsed <= new Date() && parsed >= new Date('1900-01-01')) {
        onChange(parsed);
      }
    } else if (raw.length === 0) {
      onChange(null);
    }
  };

  const handleCalendarSelect = (date: Date | undefined) => {
    onChange(date ?? null);
    setInputValue(date ? format(date, 'MM/dd/yyyy') : '');
    setOpen(false);
  };

  return (
    <FormItem className="flex flex-col">
      <FormLabel>Birth Date</FormLabel>
      <Popover open={open} onOpenChange={setOpen}>
        <div className="relative">
          <FormControl>
            <Input
              placeholder="MM/DD/YYYY"
              value={inputValue}
              onChange={handleInputChange}
              maxLength={10}
            />
          </FormControl>
          <PopoverTrigger asChild>
            <Button
              type="button"
              variant="ghost"
              size="icon"
              className="absolute right-0 top-0 h-full px-3"
            >
              <CalendarIcon className="h-4 w-4 opacity-50" />
            </Button>
          </PopoverTrigger>
        </div>
        <PopoverContent className="w-auto p-0" align="start">
          <Calendar
            mode="single"
            captionLayout="dropdown"
            selected={value || undefined}
            onSelect={handleCalendarSelect}
            disabled={(date: Date) =>
              date > new Date() || date < new Date('1900-01-01')
            }
            startMonth={new Date()}
            autoFocus
          />
        </PopoverContent>
      </Popover>
      <FormMessage />
    </FormItem>
  );
}

interface PetFormDrawerProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  pet?: Pet | null;
  defaultOwnerId?: string;
}

export function PetFormDrawer({ open, onOpenChange, pet, defaultOwnerId }: PetFormDrawerProps) {
  const createPet = useCreatePet();
  const updatePet = useUpdatePet();
  const { data: owners = [] } = useOwners();

  const form = useForm<PetFormValues>({
    resolver: zodResolver(petSchema),
    defaultValues: {
      name: '',
      petType: 'Dog',
      breed: '',
      birthDate: null,
      ownerId: defaultOwnerId || '',
    },
  });

  useEffect(() => {
    if (pet) {
      form.reset({
        name: pet.name,
        petType: pet.petType,
        breed: pet.breed,
        birthDate: pet.birthDate ? new Date(pet.birthDate) : null,
        ownerId: pet.ownerId,
      });
    } else {
      form.reset({
        name: '',
        petType: 'Dog',
        breed: '',
        birthDate: null,
        ownerId: defaultOwnerId || '',
      });
    }
  }, [pet, defaultOwnerId, form]);

  const onSubmit = async (data: PetFormValues) => {
    try {
      const submitData = {
        ...data,
        birthDate: data.birthDate?.toISOString(),
      };

      if (pet) {
        await updatePet.mutateAsync({ id: pet.id, data: submitData });
      } else {
        await createPet.mutateAsync(submitData);
      }
      onOpenChange(false);
      form.reset();
    } catch {
      // Error handling is done in the mutation hooks
    }
  };

  return (
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent className="sm:max-w-md overflow-y-auto">
        <SheetHeader>
          <SheetTitle>{pet ? 'Edit Pet' : 'Add New Pet'}</SheetTitle>
          <SheetDescription>
            {pet ? 'Update pet information' : 'Create a new pet record'}
          </SheetDescription>
        </SheetHeader>

        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 mt-4">
            <FormField
              control={form.control}
              name="name"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Pet Name</FormLabel>
                  <FormControl>
                    <Input placeholder="Fluffy" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="petType"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Pet Type</FormLabel>
                  <FormControl>
                    <PetTypeSelect
                      value={field.value}
                      onValueChange={field.onChange}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="breed"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Breed</FormLabel>
                  <FormControl>
                    <Input placeholder="Golden Retriever" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="birthDate"
              render={({ field }) => (
                <BirthDateField value={field.value || null} onChange={field.onChange} />
              )}
            />

            <FormField
              control={form.control}
              name="ownerId"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Owner</FormLabel>
                  <FormControl>
                    <OwnerCombobox
                      owners={owners}
                      value={field.value}
                      onValueChange={field.onChange}
                      placeholder="Select an owner"
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <div className="flex gap-2 pt-4">
              <Button
                type="button"
                variant="outline"
                onClick={() => onOpenChange(false)}
                className="flex-1"
              >
                Cancel
              </Button>
              <Button
                type="submit"
                className="flex-1"
                disabled={createPet.isPending || updatePet.isPending}
              >
                {createPet.isPending || updatePet.isPending
                  ? 'Saving...'
                  : pet
                  ? 'Update'
                  : 'Create'}
              </Button>
            </div>
          </form>
        </Form>
      </SheetContent>
    </Sheet>
  );
}
