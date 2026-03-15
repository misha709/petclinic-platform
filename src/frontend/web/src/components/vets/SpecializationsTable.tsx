import type { Specialization } from '@/types/models';
import { Button } from '@/components/ui/button';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Pencil, Trash2 } from 'lucide-react';
import { formatDate } from '@/lib/utils';

interface SpecializationsTableProps {
  specializations: Specialization[];
  onEdit: (specialization: Specialization) => void;
  onDelete: (specialization: Specialization) => void;
  isLoading?: boolean;
}

export function SpecializationsTable({
  specializations,
  onEdit,
  onDelete,
  isLoading,
}: SpecializationsTableProps) {
  if (isLoading) {
    return (
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Name</TableHead>
              <TableHead>Created</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {[...Array(3)].map((_, i) => (
              <TableRow key={i}>
                <TableCell><div className="h-4 bg-muted animate-pulse rounded" /></TableCell>
                <TableCell><div className="h-4 bg-muted animate-pulse rounded" /></TableCell>
                <TableCell><div className="h-4 bg-muted animate-pulse rounded" /></TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    );
  }

  if (specializations.length === 0) {
    return (
      <div className="rounded-md border p-8 text-center text-muted-foreground">
        <p>No specializations found. Add one to get started.</p>
      </div>
    );
  }

  return (
    <div className="rounded-md border">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Name</TableHead>
            <TableHead>Created</TableHead>
            <TableHead className="text-right">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {specializations.map((spec) => (
            <TableRow key={spec.id}>
              <TableCell className="font-medium">{spec.name}</TableCell>
              <TableCell className="text-muted-foreground">{formatDate(spec.createdAt)}</TableCell>
              <TableCell className="text-right">
                <div className="flex justify-end gap-2">
                  <Button
                    variant="ghost"
                    size="icon"
                    onClick={() => onEdit(spec)}
                    title="Edit"
                  >
                    <Pencil className="h-4 w-4" />
                  </Button>
                  <Button
                    variant="ghost"
                    size="icon"
                    onClick={() => onDelete(spec)}
                    title="Delete"
                  >
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
