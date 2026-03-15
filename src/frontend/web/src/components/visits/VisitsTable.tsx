import type { Visit, VisitStatus } from '@/types/models';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Pencil, Trash2, CheckCircle, XCircle } from 'lucide-react';
import { formatDate } from '@/lib/utils';
import { usePet } from '@/hooks/usePets';
import { useVet } from '@/hooks/useVets';

function PetCell({ petId }: { petId: string }) {
  const { data: pet, isLoading } = usePet(petId);
  if (isLoading) return <div className="h-4 w-24 bg-muted animate-pulse rounded" />;
  if (!pet) return <span className="text-muted-foreground text-xs">{petId.slice(0, 8)}…</span>;
  return <span>{pet.name} <span className="text-muted-foreground text-xs">({pet.petType})</span></span>;
}

function VetCell({ vetId }: { vetId: string }) {
  const { data: vet, isLoading } = useVet(vetId);
  if (isLoading) return <div className="h-4 w-24 bg-muted animate-pulse rounded" />;
  if (!vet) return <span className="text-muted-foreground text-xs">{vetId.slice(0, 8)}…</span>;
  return <span>Dr. {vet.firstName} {vet.lastName}</span>;
}

function StatusBadge({ status }: { status: VisitStatus }) {
  if (status === 'Scheduled') {
    return <Badge className="bg-blue-100 text-blue-800 border-blue-200 hover:bg-blue-100">Scheduled</Badge>;
  }
  if (status === 'Completed') {
    return <Badge className="bg-green-100 text-green-800 border-green-200 hover:bg-green-100">Completed</Badge>;
  }
  return <Badge variant="destructive">Cancelled</Badge>;
}

interface VisitsTableProps {
  visits: Visit[];
  onEdit: (visit: Visit) => void;
  onCancel: (visit: Visit) => void;
  onComplete: (visit: Visit) => void;
  onDelete: (visit: Visit) => void;
  isLoading?: boolean;
}

export function VisitsTable({ visits, onEdit, onCancel, onComplete, onDelete, isLoading }: VisitsTableProps) {
  if (isLoading) {
    return (
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Scheduled</TableHead>
              <TableHead>Pet</TableHead>
              <TableHead>Vet</TableHead>
              <TableHead>Reason</TableHead>
              <TableHead>Status</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {[...Array(3)].map((_, i) => (
              <TableRow key={i}>
                {[...Array(6)].map((__, j) => (
                  <TableCell key={j}><div className="h-4 bg-muted animate-pulse rounded" /></TableCell>
                ))}
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    );
  }

  if (visits.length === 0) {
    return (
      <div className="rounded-md border p-8 text-center text-muted-foreground">
        <p>No visits found. Schedule a visit to get started.</p>
      </div>
    );
  }

  return (
    <div className="rounded-md border">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Scheduled</TableHead>
            <TableHead>Duration</TableHead>
            <TableHead>Pet</TableHead>
            <TableHead>Vet</TableHead>
            <TableHead>Reason</TableHead>
            <TableHead>Status</TableHead>
            <TableHead className="text-right">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {visits.map((visit) => (
            <TableRow key={visit.id}>
              <TableCell className="whitespace-nowrap">
                {formatDate(visit.scheduledAt)}
                <div className="text-xs text-muted-foreground">
                  {new Date(visit.scheduledAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                </div>
              </TableCell>
              <TableCell className="text-muted-foreground text-sm">{visit.durationMinutes} min</TableCell>
              <TableCell><PetCell petId={visit.petId} /></TableCell>
              <TableCell><VetCell vetId={visit.vetId} /></TableCell>
              <TableCell className="max-w-[160px] truncate" title={visit.reason}>
                {visit.reason}
              </TableCell>
              <TableCell><StatusBadge status={visit.status} /></TableCell>
              <TableCell className="text-right">
                <div className="flex justify-end gap-1">
                  {visit.status === 'Scheduled' && (
                    <>
                      <Button variant="ghost" size="icon" onClick={() => onEdit(visit)} title="Edit">
                        <Pencil className="h-4 w-4" />
                      </Button>
                      <Button variant="ghost" size="icon" onClick={() => onComplete(visit)} title="Mark complete">
                        <CheckCircle className="h-4 w-4 text-green-600" />
                      </Button>
                      <Button variant="ghost" size="icon" onClick={() => onCancel(visit)} title="Cancel visit">
                        <XCircle className="h-4 w-4 text-amber-600" />
                      </Button>
                    </>
                  )}
                  <Button variant="ghost" size="icon" onClick={() => onDelete(visit)} title="Delete">
                    <Trash2 className="h-4 w-4 text-destructive" />
                  </Button>
                </div>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}
