import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { format } from 'date-fns';
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
} from '@/components/ui/sheet';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import {
  createVisitSchema,
  updateVisitSchema,
  type CreateVisitFormValues,
  type UpdateVisitFormValues,
} from '@/lib/validations/visit';
import type { Visit } from '@/types/models';
import { useCreateVisit, useUpdateVisit } from '@/hooks/useVisits';
import { useVets } from '@/hooks/useVets';
import { useOwners } from '@/hooks/useOwners';
import { usePets } from '@/hooks/usePets';
import { VetCombobox } from './VetCombobox';
import { PetCombobox } from './PetCombobox';
import { OwnerCombobox } from '@/components/owners/OwnerCombobox';

interface VisitFormDrawerProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  visit?: Visit | null;
  defaultPetId?: string;
  defaultVetId?: string;
}

function toLocalDateTimeString(utcString: string): string {
  const date = new Date(utcString);
  return format(date, "yyyy-MM-dd'T'HH:mm");
}

function getDefaultScheduledAt(): string {
  const d = new Date();
  d.setHours(d.getHours() + 1, 0, 0, 0);
  return format(d, "yyyy-MM-dd'T'HH:mm");
}

export function VisitFormDrawer({
  open,
  onOpenChange,
  visit,
  defaultPetId = '',
  defaultVetId = '',
}: VisitFormDrawerProps) {
  const createVisit = useCreateVisit();
  const updateVisit = useUpdateVisit();

  const { data: vets = [] } = useVets();
  const { data: owners = [] } = useOwners();

  const [selectedOwnerId, setSelectedOwnerId] = useState('');
  const { data: pets = [] } = usePets(selectedOwnerId || undefined);

  const isEditing = !!visit;

  const createForm = useForm<CreateVisitFormValues>({
    resolver: zodResolver(createVisitSchema),
    defaultValues: {
      petId: defaultPetId,
      vetId: defaultVetId,
      scheduledAt: getDefaultScheduledAt(),
      durationMinutes: 30,
      reason: '',
      notes: '',
    },
  });

  const updateForm = useForm<UpdateVisitFormValues>({
    resolver: zodResolver(updateVisitSchema),
    defaultValues: {
      scheduledAt: getDefaultScheduledAt(),
      durationMinutes: 30,
      reason: '',
      notes: '',
    },
  });

  useEffect(() => {
    if (!open) return;

    if (visit) {
      updateForm.reset({
        scheduledAt: toLocalDateTimeString(visit.scheduledAt),
        durationMinutes: visit.durationMinutes,
        reason: visit.reason,
        notes: visit.notes ?? '',
      });
    } else {
      setSelectedOwnerId('');
      createForm.reset({
        petId: defaultPetId,
        vetId: defaultVetId,
        scheduledAt: getDefaultScheduledAt(),
        durationMinutes: 30,
        reason: '',
        notes: '',
      });
    }
  }, [visit, open, defaultPetId, defaultVetId, createForm, updateForm]);

  const onSubmitCreate = async (data: CreateVisitFormValues) => {
    try {
      await createVisit.mutateAsync({
        ...data,
        scheduledAt: new Date(data.scheduledAt).toISOString(),
        notes: data.notes || undefined,
      });
      onOpenChange(false);
    } catch {
      // handled in hook
    }
  };

  const onSubmitUpdate = async (data: UpdateVisitFormValues) => {
    if (!visit) return;
    try {
      await updateVisit.mutateAsync({
        id: visit.id,
        data: {
          ...data,
          scheduledAt: new Date(data.scheduledAt).toISOString(),
          notes: data.notes || undefined,
        },
      });
      onOpenChange(false);
    } catch {
      // handled in hook
    }
  };

  const isPending = createVisit.isPending || updateVisit.isPending;

  return (
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent className="sm:max-w-md overflow-y-auto">
        <SheetHeader>
          <SheetTitle>{isEditing ? 'Edit Visit' : 'Schedule Visit'}</SheetTitle>
          <SheetDescription>
            {isEditing ? 'Update visit details' : 'Book a new appointment'}
          </SheetDescription>
        </SheetHeader>

        {isEditing ? (
          <div className="mt-4">
            <div className="mb-4 rounded-md bg-muted/50 p-3 text-sm space-y-1">
              <p className="text-muted-foreground">
                Pet ID: <span className="font-mono text-xs">{visit.petId}</span>
              </p>
              <p className="text-muted-foreground">
                Vet ID: <span className="font-mono text-xs">{visit.vetId}</span>
              </p>
            </div>
            <Form {...updateForm}>
              <form onSubmit={updateForm.handleSubmit(onSubmitUpdate)} className="space-y-4">
                <FormField
                  control={updateForm.control}
                  name="scheduledAt"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Date &amp; Time</FormLabel>
                      <FormControl>
                        <Input type="datetime-local" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={updateForm.control}
                  name="durationMinutes"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Duration (minutes)</FormLabel>
                      <FormControl>
                        <Input type="number" min={5} max={480} step={5} {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={updateForm.control}
                  name="reason"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Reason</FormLabel>
                      <FormControl>
                        <Input placeholder="e.g. Annual checkup" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={updateForm.control}
                  name="notes"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Notes <span className="text-muted-foreground">(optional)</span></FormLabel>
                      <FormControl>
                        <Textarea placeholder="Additional notes…" rows={3} {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <div className="flex gap-2 pt-4">
                  <Button type="button" variant="outline" onClick={() => onOpenChange(false)} className="flex-1">
                    Cancel
                  </Button>
                  <Button type="submit" className="flex-1" disabled={isPending}>
                    {isPending ? 'Saving…' : 'Update'}
                  </Button>
                </div>
              </form>
            </Form>
          </div>
        ) : (
          <Form {...createForm}>
            <form onSubmit={createForm.handleSubmit(onSubmitCreate)} className="space-y-4 mt-4">
              <div className="space-y-2">
                <p className="text-sm font-medium">Owner</p>
                <OwnerCombobox
                  owners={owners}
                  value={selectedOwnerId}
                  onValueChange={(id) => {
                    setSelectedOwnerId(id);
                    createForm.setValue('petId', '');
                  }}
                  placeholder="Select owner to load pets"
                />
              </div>

              <FormField
                control={createForm.control}
                name="petId"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Pet</FormLabel>
                    <FormControl>
                      <PetCombobox
                        pets={pets}
                        value={field.value}
                        onValueChange={field.onChange}
                        placeholder={selectedOwnerId ? 'Select a pet' : 'Select an owner first'}
                        disabled={!selectedOwnerId}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={createForm.control}
                name="vetId"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Vet</FormLabel>
                    <FormControl>
                      <VetCombobox
                        vets={vets}
                        value={field.value}
                        onValueChange={field.onChange}
                        placeholder="Select a vet"
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={createForm.control}
                name="scheduledAt"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Date &amp; Time</FormLabel>
                    <FormControl>
                      <Input type="datetime-local" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={createForm.control}
                name="durationMinutes"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Duration (minutes)</FormLabel>
                    <FormControl>
                      <Input type="number" min={5} max={480} step={5} {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={createForm.control}
                name="reason"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Reason</FormLabel>
                    <FormControl>
                      <Input placeholder="e.g. Annual checkup" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={createForm.control}
                name="notes"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Notes <span className="text-muted-foreground">(optional)</span></FormLabel>
                    <FormControl>
                      <Textarea placeholder="Additional notes…" rows={3} {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <div className="flex gap-2 pt-4">
                <Button type="button" variant="outline" onClick={() => onOpenChange(false)} className="flex-1">
                  Cancel
                </Button>
                <Button type="submit" className="flex-1" disabled={isPending}>
                  {isPending ? 'Scheduling…' : 'Schedule Visit'}
                </Button>
              </div>
            </form>
          </Form>
        )}
      </SheetContent>
    </Sheet>
  );
}
