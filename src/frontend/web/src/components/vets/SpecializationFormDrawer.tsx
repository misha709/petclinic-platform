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
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { specializationSchema, type SpecializationFormValues } from '@/lib/validations/specialization';
import type { Specialization } from '@/types/models';
import { useCreateSpecialization, useUpdateSpecialization } from '@/hooks/useVets';

interface SpecializationFormDrawerProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  specialization?: Specialization | null;
}

export function SpecializationFormDrawer({
  open,
  onOpenChange,
  specialization,
}: SpecializationFormDrawerProps) {
  const createSpecialization = useCreateSpecialization();
  const updateSpecialization = useUpdateSpecialization();

  const form = useForm<SpecializationFormValues>({
    resolver: zodResolver(specializationSchema),
    defaultValues: { name: '' },
  });

  useEffect(() => {
    form.reset({ name: specialization?.name ?? '' });
  }, [specialization, form]);

  const onSubmit = async (data: SpecializationFormValues) => {
    try {
      if (specialization) {
        await updateSpecialization.mutateAsync({ id: specialization.id, data });
      } else {
        await createSpecialization.mutateAsync(data);
      }
      onOpenChange(false);
      form.reset();
    } catch {
      // Error handling is done in the mutation hooks
    }
  };

  const isPending = createSpecialization.isPending || updateSpecialization.isPending;

  return (
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent className="sm:max-w-md">
        <SheetHeader>
          <SheetTitle>
            {specialization ? 'Edit Specialization' : 'Add Specialization'}
          </SheetTitle>
          <SheetDescription>
            {specialization
              ? 'Update the specialization name'
              : 'Create a new specialization that can be assigned to vets'}
          </SheetDescription>
        </SheetHeader>

        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 mt-4">
            <FormField
              control={form.control}
              name="name"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Name</FormLabel>
                  <FormControl>
                    <Input placeholder="e.g. Cardiology" {...field} />
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
              <Button type="submit" className="flex-1" disabled={isPending}>
                {isPending ? 'Saving...' : specialization ? 'Update' : 'Create'}
              </Button>
            </div>
          </form>
        </Form>
      </SheetContent>
    </Sheet>
  );
}
