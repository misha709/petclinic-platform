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
import type { Vet } from '@/types/models';
import { useDeleteVet } from '@/hooks/useVets';

interface DeleteVetDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  vet: Vet | null;
}

export function DeleteVetDialog({ open, onOpenChange, vet }: DeleteVetDialogProps) {
  const deleteVet = useDeleteVet();

  const handleDelete = async () => {
    if (!vet) return;

    try {
      await deleteVet.mutateAsync(vet.id);
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
            This will permanently delete{' '}
            <strong>
              {vet?.firstName} {vet?.lastName}
            </strong>
            . This action cannot be undone.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel>Cancel</AlertDialogCancel>
          <AlertDialogAction
            onClick={handleDelete}
            className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
            disabled={deleteVet.isPending}
          >
            {deleteVet.isPending ? 'Deleting...' : 'Delete'}
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}
