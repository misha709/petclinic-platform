import { useState } from 'react';
import { Plus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { SpecializationsTable } from '@/components/vets/SpecializationsTable';
import { SpecializationFormDrawer } from '@/components/vets/SpecializationFormDrawer';
import { DeleteSpecializationDialog } from '@/components/vets/DeleteSpecializationDialog';
import { useSpecializations } from '@/hooks/useVets';
import type { Specialization } from '@/types/models';

export function SpecializationsPage() {
  const [searchQuery, setSearchQuery] = useState('');
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedSpecialization, setSelectedSpecialization] = useState<Specialization | null>(null);

  const { data: specializations = [], isLoading } = useSpecializations();

  const filtered = searchQuery
    ? specializations.filter((s) =>
        s.name.toLowerCase().includes(searchQuery.toLowerCase())
      )
    : specializations;

  const handleEdit = (specialization: Specialization) => {
    setSelectedSpecialization(specialization);
    setDrawerOpen(true);
  };

  const handleDelete = (specialization: Specialization) => {
    setSelectedSpecialization(specialization);
    setDeleteDialogOpen(true);
  };

  const handleAddNew = () => {
    setSelectedSpecialization(null);
    setDrawerOpen(true);
  };

  const handleDrawerClose = (open: boolean) => {
    setDrawerOpen(open);
    if (!open) {
      setSelectedSpecialization(null);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Specializations</h1>
          <p className="text-muted-foreground">
            Manage vet specializations available for assignment
          </p>
        </div>
        <Button onClick={handleAddNew}>
          <Plus className="mr-2 h-4 w-4" />
          Add Specialization
        </Button>
      </div>

      <Input
        placeholder="Search specializations..."
        value={searchQuery}
        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSearchQuery(e.target.value)}
        className="max-w-sm"
      />

      <SpecializationsTable
        specializations={filtered}
        onEdit={handleEdit}
        onDelete={handleDelete}
        isLoading={isLoading}
      />

      <SpecializationFormDrawer
        open={drawerOpen}
        onOpenChange={handleDrawerClose}
        specialization={selectedSpecialization}
      />

      <DeleteSpecializationDialog
        open={deleteDialogOpen}
        onOpenChange={setDeleteDialogOpen}
        specialization={selectedSpecialization}
      />
    </div>
  );
}
