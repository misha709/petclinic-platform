import { useEffect, useState } from 'react';
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
import { cn } from '@/lib/utils';
import { format, parse, isValid } from 'date-fns';

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
    } catch (error) {
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
              render={({ field }) => {
                const [open, setOpen] = useState(false);
                const [inputValue, setInputValue] = useState(() => 
                  field.value ? format(field.value, 'MM/dd/yyyy') : ''
                );

                // Update input value when field value changes (e.g., when editing existing pet)
                useEffect(() => {
                  if (field.value) {
                    setInputValue(format(field.value, 'MM/dd/yyyy'));
                  } else {
                    setInputValue('');
                  }
                }, [field.value]);

                const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
                  let value = e.target.value.replace(/\D/g, '');
                  
                  // Apply mask MM/DD/YYYY
                  if (value.length >= 2) {
                    value = value.slice(0, 2) + '/' + value.slice(2);
                  }
                  if (value.length >= 5) {
                    value = value.slice(0, 5) + '/' + value.slice(5, 9);
                  }
                  
                  setInputValue(value);

                  // Try to parse complete date
                  if (value.length === 10) {
                    const parsed = parse(value, 'MM/dd/yyyy', new Date());
                    if (isValid(parsed)) {
                      const today = new Date();
                      const minDate = new Date('1900-01-01');
                      if (parsed <= today && parsed >= minDate) {
                        field.onChange(parsed);
                      }
                    }
                  } else if (value.length === 0) {
                    field.onChange(null);
                  }
                };

                const handleCalendarSelect = (date: Date | undefined) => {
                  field.onChange(date);
                  if (date) {
                    setInputValue(format(date, 'MM/dd/yyyy'));
                  } else {
                    setInputValue('');
                  }
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
                          selected={field.value || undefined}
                          onSelect={handleCalendarSelect}
                          disabled={(date: Date) =>
                            date > new Date() || date < new Date('1900-01-01')
                          }
                          fromYear={1900}
                          toYear={new Date().getFullYear()}
                          initialFocus
                        />
                      </PopoverContent>
                    </Popover>
                    <FormMessage />
                  </FormItem>
                );
              }}
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
