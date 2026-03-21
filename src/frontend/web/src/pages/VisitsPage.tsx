import { useState } from 'react';
import { Plus, X } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { VisitsTable } from '@/components/visits/VisitsTable';
import { VisitFormDrawer } from '@/components/visits/VisitFormDrawer';
import { CancelVisitDialog } from '@/components/visits/CancelVisitDialog';
import { DeleteVisitDialog } from '@/components/visits/DeleteVisitDialog';
import { VetCombobox } from '@/components/visits/VetCombobox';
import { useVisits, useCompleteVisit } from '@/hooks/useVisits';
import { useVets } from '@/hooks/useVets';
import type { Visit } from '@/types/models';

const STATUS_OPTIONS: Array<{ value: string; label: string }> = [
  { value: 'all', label: 'All statuses' },
  { value: 'Scheduled', label: 'Scheduled' },
  { value: 'Completed', label: 'Completed' },
  { value: 'Cancelled', label: 'Cancelled' },
];

export function VisitsPage() {
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [cancelDialogOpen, setCancelDialogOpen] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedVisit, setSelectedVisit] = useState<Visit | null>(null);

  const [filterVetId, setFilterVetId] = useState('');
  const [filterDate, setFilterDate] = useState('');
  const [filterStatus, setFilterStatus] = useState<string>('all');

  const { data: vets = [] } = useVets();
  const completeVisit = useCompleteVisit();

  const filters = {
    vetId: filterVetId || undefined,
    date: filterDate || undefined,
    status: filterStatus !== 'all' ? filterStatus : undefined,
  };

  const { data: visits = [], isLoading } = useVisits(filters);

  const hasFilters = !!filterVetId || !!filterDate || filterStatus !== 'all';

  const clearFilters = () => {
    setFilterVetId('');
    setFilterDate('');
    setFilterStatus('all');
  };

  const handleEdit = (visit: Visit) => {
    setSelectedVisit(visit);
    setDrawerOpen(true);
  };

  const handleCancel = (visit: Visit) => {
    setSelectedVisit(visit);
    setCancelDialogOpen(true);
  };

  const handleComplete = async (visit: Visit) => {
    await completeVisit.mutateAsync(visit.id);
  };

  const handleDelete = (visit: Visit) => {
    setSelectedVisit(visit);
    setDeleteDialogOpen(true);
  };

  const handleAddNew = () => {
    setSelectedVisit(null);
    setDrawerOpen(true);
  };

  const handleDrawerClose = (open: boolean) => {
    setDrawerOpen(open);
    if (!open) setSelectedVisit(null);
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Visits</h1>
          <p className="text-muted-foreground">Schedule and manage clinic appointments</p>
        </div>
        <Button onClick={handleAddNew}>
          <Plus className="mr-2 h-4 w-4" />
          Schedule Visit
        </Button>
      </div>

      <div className="flex flex-wrap items-center gap-2">
        <div className="w-[220px]">
          <VetCombobox
            vets={vets}
            value={filterVetId}
            onValueChange={setFilterVetId}
            placeholder="Filter by vet"
            includeAllOption={false}
          />
        </div>

        <Input
          type="date"
          value={filterDate}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) => setFilterDate(e.target.value)}
          className="w-[160px]"
        />

        <Select value={filterStatus} onValueChange={setFilterStatus}>
          <SelectTrigger className="w-[160px]">
            <SelectValue />
          </SelectTrigger>
          <SelectContent>
            {STATUS_OPTIONS.map((opt) => (
              <SelectItem key={opt.value} value={opt.value}>
                {opt.label}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>

        {hasFilters && (
          <Button variant="ghost" size="sm" onClick={clearFilters} className="gap-1 text-muted-foreground">
            <X className="h-3 w-3" />
            Clear filters
          </Button>
        )}
      </div>

      <VisitsTable
        visits={visits}
        onEdit={handleEdit}
        onCancel={handleCancel}
        onComplete={handleComplete}
        onDelete={handleDelete}
        isLoading={isLoading}
      />

      <VisitFormDrawer
        open={drawerOpen}
        onOpenChange={handleDrawerClose}
        visit={selectedVisit}
      />

      <CancelVisitDialog
        open={cancelDialogOpen}
        onOpenChange={setCancelDialogOpen}
        visit={selectedVisit}
      />

      <DeleteVisitDialog
        open={deleteDialogOpen}
        onOpenChange={setDeleteDialogOpen}
        visit={selectedVisit}
      />
    </div>
  );
}
