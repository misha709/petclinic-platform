import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Plus, Search } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { OwnersTable } from '@/components/owners/OwnersTable';
import { OwnerFormDrawer } from '@/components/owners/OwnerFormDrawer';
import { DeleteOwnerDialog } from '@/components/owners/DeleteOwnerDialog';
import { useOwners } from '@/hooks/useOwners';
import type { Owner } from '@/types/models';

export function OwnersPage() {
  const [searchQuery, setSearchQuery] = useState('');
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedOwner, setSelectedOwner] = useState<Owner | null>(null);
  const navigate = useNavigate();

  const { data: owners = [], isLoading } = useOwners(searchQuery);

  const handleEdit = (owner: Owner) => {
    setSelectedOwner(owner);
    setDrawerOpen(true);
  };

  const handleDelete = (owner: Owner) => {
    setSelectedOwner(owner);
    setDeleteDialogOpen(true);
  };

  const handleViewPets = (owner: Owner) => {
    navigate(`/pets?ownerId=${owner.id}`);
  };

  const handleAddNew = () => {
    setSelectedOwner(null);
    setDrawerOpen(true);
  };

  const handleDrawerClose = (open: boolean) => {
    setDrawerOpen(open);
    if (!open) {
      setSelectedOwner(null);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Owners</h1>
          <p className="text-muted-foreground">
            Manage pet owners and their information
          </p>
        </div>
        <Button onClick={handleAddNew}>
          <Plus className="mr-2 h-4 w-4" />
          Add Owner
        </Button>
      </div>

      <div className="flex items-center gap-2">
        <div className="relative flex-1 max-w-sm">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input
            placeholder="Search owners..."
            value={searchQuery}
                onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSearchQuery(e.target.value)}
            className="pl-9"
          />
        </div>
      </div>

      <OwnersTable
        owners={owners}
        onEdit={handleEdit}
        onDelete={handleDelete}
        onViewPets={handleViewPets}
        isLoading={isLoading}
      />

      <OwnerFormDrawer
        open={drawerOpen}
        onOpenChange={handleDrawerClose}
        owner={selectedOwner}
      />

      <DeleteOwnerDialog
        open={deleteDialogOpen}
        onOpenChange={setDeleteDialogOpen}
        owner={selectedOwner}
      />
    </div>
  );
}
