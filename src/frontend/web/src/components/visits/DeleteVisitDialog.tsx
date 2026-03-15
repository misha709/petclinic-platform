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
import type { Visit } from '@/types/models';
import { useDeleteVisit } from '@/hooks/useVisits';

interface DeleteVisitDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  visit: Visit | null;
}

export function DeleteVisitDialog({ open, onOpenChange, visit }: DeleteVisitDialogProps) {
  const deleteVisit = useDeleteVisit();

  const handleDelete = async () => {
    if (!visit) return;
    try {
      await deleteVisit.mutateAsync(visit.id);
      onOpenChange(false);
    } catch {
      // handled in hook
    }
  };

  return (
    <AlertDialog open={open} onOpenChange={onOpenChange}>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Are you sure?</AlertDialogTitle>
          <AlertDialogDescription>
            This will permanently delete the visit scheduled for{' '}
            <strong>
              {visit ? new Date(visit.scheduledAt).toLocaleString() : ''}
            </strong>
            . This action cannot be undone.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel>Keep</AlertDialogCancel>
          <AlertDialogAction
            onClick={handleDelete}
            className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
            disabled={deleteVisit.isPending}
          >
            {deleteVisit.isPending ? 'Deleting…' : 'Delete'}
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}
