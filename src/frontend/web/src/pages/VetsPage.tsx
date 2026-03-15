import { useState } from 'react';
import { Plus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { VetsTable } from '@/components/vets/VetsTable';
import { VetFormDrawer } from '@/components/vets/VetFormDrawer';
import { DeleteVetDialog } from '@/components/vets/DeleteVetDialog';
import { useVets } from '@/hooks/useVets';
import type { Vet } from '@/types/models';

export function VetsPage() {
  const [searchQuery, setSearchQuery] = useState('');
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedVet, setSelectedVet] = useState<Vet | null>(null);

  const { data: vets = [], isLoading } = useVets(searchQuery || undefined);

  const handleEdit = (vet: Vet) => {
    setSelectedVet(vet);
    setDrawerOpen(true);
  };

  const handleDelete = (vet: Vet) => {
    setSelectedVet(vet);
    setDeleteDialogOpen(true);
  };

  const handleAddNew = () => {
    setSelectedVet(null);
    setDrawerOpen(true);
  };

  const handleDrawerClose = (open: boolean) => {
    setDrawerOpen(open);
    if (!open) {
      setSelectedVet(null);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Vets</h1>
          <p className="text-muted-foreground">Manage veterinarians and their specializations</p>
        </div>
        <Button onClick={handleAddNew}>
          <Plus className="mr-2 h-4 w-4" />
          Add Vet
        </Button>
      </div>

      <Input
        placeholder="Search vets..."
        value={searchQuery}
        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSearchQuery(e.target.value)}
        className="max-w-sm"
      />

      <VetsTable
        vets={vets}
        onEdit={handleEdit}
        onDelete={handleDelete}
        isLoading={isLoading}
      />

      <VetFormDrawer
        open={drawerOpen}
        onOpenChange={handleDrawerClose}
        vet={selectedVet}
      />

      <DeleteVetDialog
        open={deleteDialogOpen}
        onOpenChange={setDeleteDialogOpen}
        vet={selectedVet}
      />
    </div>
  );
}
