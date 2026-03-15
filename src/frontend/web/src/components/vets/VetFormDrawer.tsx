import { useEffect } from 'react';
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
import { Badge } from '@/components/ui/badge';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { vetSchema, type VetFormValues } from '@/lib/validations/vet';
import type { Vet } from '@/types/models';
import { useCreateVet, useUpdateVet, useAssignSpecializations, useSpecializations } from '@/hooks/useVets';
import { Check } from 'lucide-react';
import { cn } from '@/lib/utils';

interface VetFormDrawerProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  vet?: Vet | null;
}

export function VetFormDrawer({ open, onOpenChange, vet }: VetFormDrawerProps) {
  const createVet = useCreateVet();
  const updateVet = useUpdateVet();
  const assignSpecializations = useAssignSpecializations();
  const { data: allSpecializations = [] } = useSpecializations();

  const form = useForm<VetFormValues>({
    resolver: zodResolver(vetSchema),
    defaultValues: {
      firstName: '',
      lastName: '',
      specializationIds: [],
    },
  });

  useEffect(() => {
    if (vet) {
      form.reset({
        firstName: vet.firstName,
        lastName: vet.lastName,
        specializationIds: vet.specializations.map((s) => s.id),
      });
    } else {
      form.reset({
        firstName: '',
        lastName: '',
        specializationIds: [],
      });
    }
  }, [vet, form]);

  const onSubmit = async (data: VetFormValues) => {
    try {
      const { specializationIds, ...nameData } = data;

      if (vet) {
        await updateVet.mutateAsync({ id: vet.id, data: nameData });
        await assignSpecializations.mutateAsync({
          id: vet.id,
          data: { specializationIds },
        });
      } else {
        const created = await createVet.mutateAsync(nameData);
        if (specializationIds.length > 0) {
          await assignSpecializations.mutateAsync({
            id: created.id,
            data: { specializationIds },
          });
        }
      }

      onOpenChange(false);
      form.reset();
    } catch {
      // Error handling is done in the mutation hooks
    }
  };

  const toggleSpecialization = (id: number, current: number[]) => {
    return current.includes(id) ? current.filter((s) => s !== id) : [...current, id];
  };

  const isPending = createVet.isPending || updateVet.isPending || assignSpecializations.isPending;

  return (
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent className="sm:max-w-md overflow-y-auto">
        <SheetHeader>
          <SheetTitle>{vet ? 'Edit Vet' : 'Add New Vet'}</SheetTitle>
          <SheetDescription>
            {vet ? 'Update vet information' : 'Create a new vet record'}
          </SheetDescription>
        </SheetHeader>

        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 mt-4">
            <FormField
              control={form.control}
              name="firstName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>First Name</FormLabel>
                  <FormControl>
                    <Input placeholder="Jane" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="lastName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Last Name</FormLabel>
                  <FormControl>
                    <Input placeholder="Smith" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="specializationIds"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Specializations</FormLabel>
                  <FormControl>
                    <div className="flex flex-wrap gap-2 rounded-md border p-3 min-h-[48px]">
                      {allSpecializations.length === 0 ? (
                        <span className="text-sm text-muted-foreground">No specializations available</span>
                      ) : (
                        allSpecializations.map((spec) => {
                          const selected = field.value.includes(spec.id);
                          return (
                            <button
                              key={spec.id}
                              type="button"
                              onClick={() =>
                                field.onChange(toggleSpecialization(spec.id, field.value))
                              }
                              className={cn(
                                'inline-flex items-center gap-1 rounded-full border px-3 py-1 text-sm font-medium transition-colors cursor-pointer',
                                selected
                                  ? 'bg-primary text-primary-foreground border-primary'
                                  : 'bg-background text-muted-foreground hover:bg-accent hover:text-accent-foreground'
                              )}
                            >
                              {selected && <Check className="h-3 w-3" />}
                              {spec.name}
                            </button>
                          );
                        })
                      )}
                    </div>
                  </FormControl>
                  <FormMessage />
                  {field.value.length > 0 && (
                    <div className="flex flex-wrap gap-1 mt-1">
                      {field.value.map((id) => {
                        const spec = allSpecializations.find((s) => s.id === id);
                        return spec ? (
                          <Badge key={id} variant="secondary" className="text-xs">
                            {spec.name}
                          </Badge>
                        ) : null;
                      })}
                    </div>
                  )}
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
              <Button type="submit" className="flex-1" disabled={isPending}>
                {isPending ? 'Saving...' : vet ? 'Update' : 'Create'}
              </Button>
            </div>
          </form>
        </Form>
      </SheetContent>
    </Sheet>
  );
}
