import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import type { Specialization } from '@/types/models';
import { useDeleteSpecialization } from '@/hooks/useVets';

interface DeleteSpecializationDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  specialization: Specialization | null;
}

export function DeleteSpecializationDialog({
  open,
  onOpenChange,
  specialization,
}: DeleteSpecializationDialogProps) {
  const deleteSpecialization = useDeleteSpecialization();

  const handleDelete = async () => {
    if (!specialization) return;

    try {
      await deleteSpecialization.mutateAsync(specialization.id);
      onOpenChange(false);
    } catch {
      // Error handling is done in the mutation hook
    }
  };

  return (
    <AlertDialog open={open} onOpenChange={onOpenChange}>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Are you sure?</AlertDialogTitle>
          <AlertDialogDescription>
            This will permanently delete the specialization{' '}
            <strong>{specialization?.name}</strong>. Any vets assigned to this
            specialization will be unaffected but it will no longer be available
            for assignment. This action cannot be undone.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel>Cancel</AlertDialogCancel>
          <AlertDialogAction
            onClick={handleDelete}
            className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
            disabled={deleteSpecialization.isPending}
          >
            {deleteSpecialization.isPending ? 'Deleting...' : 'Delete'}
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}
