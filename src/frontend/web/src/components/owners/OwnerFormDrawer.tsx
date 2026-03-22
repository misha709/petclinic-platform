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
import { ownerSchema, type OwnerFormValues } from '@/lib/validations/owner';
import type { Owner } from '@/types/models';
import { useCreateOwner, useUpdateOwner } from '@/hooks/useOwners';

interface OwnerFormDrawerProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  owner?: Owner | null;
}

export function OwnerFormDrawer({ open, onOpenChange, owner }: OwnerFormDrawerProps) {
  const createOwner = useCreateOwner();
  const updateOwner = useUpdateOwner();

  const form = useForm<OwnerFormValues>({
    resolver: zodResolver(ownerSchema),
    defaultValues: {
      firstName: '',
      lastName: '',
      address: '',
      city: '',
      telephone: '',
      email: '',
    },
  });

  useEffect(() => {
    if (owner) {
      form.reset({
        firstName: owner.firstName,
        lastName: owner.lastName,
        address: owner.address,
        city: owner.city,
        telephone: owner.telephone,
        email: owner.email ?? '',
      });
    } else {
      form.reset({
        firstName: '',
        lastName: '',
        address: '',
        city: '',
        telephone: '',
        email: '',
      });
    }
  }, [owner, form]);

  const onSubmit = async (data: OwnerFormValues) => {
    try {
      if (owner) {
        await updateOwner.mutateAsync({ id: owner.id, data });
      } else {
        await createOwner.mutateAsync(data);
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
          <SheetTitle>{owner ? 'Edit Owner' : 'Add New Owner'}</SheetTitle>
          <SheetDescription>
            {owner ? 'Update owner information' : 'Create a new owner record'}
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
                    <Input placeholder="John" {...field} />
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
                    <Input placeholder="Doe" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="address"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Address</FormLabel>
                  <FormControl>
                    <Input placeholder="123 Main St" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="city"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>City</FormLabel>
                  <FormControl>
                    <Input placeholder="New York" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="telephone"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Telephone</FormLabel>
                  <FormControl>
                    <Input placeholder="1234567890" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Email</FormLabel>
                  <FormControl>
                    <Input type="email" placeholder="john.doe@example.com" {...field} />
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
                disabled={createOwner.isPending || updateOwner.isPending}
              >
                {createOwner.isPending || updateOwner.isPending
                  ? 'Saving...'
                  : owner
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
