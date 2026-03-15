import { useState } from 'react';
import {
  AlertDialog,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';
import type { Visit } from '@/types/models';
import { useCancelVisit } from '@/hooks/useVisits';

interface CancelVisitDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  visit: Visit | null;
}

export function CancelVisitDialog({ open, onOpenChange, visit }: CancelVisitDialogProps) {
  const cancelVisit = useCancelVisit();
  const [notes, setNotes] = useState('');

  const handleCancel = async () => {
    if (!visit) return;
    try {
      await cancelVisit.mutateAsync({ id: visit.id, data: { notes: notes || undefined } });
      setNotes('');
      onOpenChange(false);
    } catch {
      // handled in hook
    }
  };

  return (
    <AlertDialog open={open} onOpenChange={(o) => { if (!o) setNotes(''); onOpenChange(o); }}>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Cancel this visit?</AlertDialogTitle>
          <AlertDialogDescription>
            This will cancel the visit scheduled for{' '}
            <strong>
              {visit ? new Date(visit.scheduledAt).toLocaleString() : ''}
            </strong>
            . This action cannot be undone.
          </AlertDialogDescription>
        </AlertDialogHeader>

        <div className="py-2">
          <Textarea
            placeholder="Reason for cancellation (optional)"
            value={notes}
            onChange={(e) => setNotes(e.target.value)}
            rows={3}
          />
        </div>

        <AlertDialogFooter>
          <AlertDialogCancel>Keep visit</AlertDialogCancel>
          <Button
            variant="destructive"
            onClick={handleCancel}
            disabled={cancelVisit.isPending}
          >
            {cancelVisit.isPending ? 'Cancelling…' : 'Cancel visit'}
          </Button>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}
