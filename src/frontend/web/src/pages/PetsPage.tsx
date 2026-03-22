import { useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { Plus, Filter } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { PetsTable } from '@/components/pets/PetsTable';
import { OwnerCombobox } from '@/components/owners/OwnerCombobox';
import { PetFormDrawer } from '@/components/pets/PetFormDrawer';
import { DeletePetDialog } from '@/components/pets/DeletePetDialog';
import { usePets } from '@/hooks/usePets';
import { useOwners } from '@/hooks/useOwners';
import type { Pet } from '@/types/models';

export function PetsPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const [searchQuery, setSearchQuery] = useState('');
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedPet, setSelectedPet] = useState<Pet | null>(null);
  const selectedOwnerId = searchParams.get('ownerId') || '';

  const { data: owners = [] } = useOwners();
  const { data: pets = [], isLoading } = usePets(selectedOwnerId, searchQuery);

  const handleEdit = (pet: Pet) => {
    setSelectedPet(pet);
    setDrawerOpen(true);
  };

  const handleDelete = (pet: Pet) => {
    setSelectedPet(pet);
    setDeleteDialogOpen(true);
  };

  const handleAddNew = () => {
    setSelectedPet(null);
    setDrawerOpen(true);
  };

  const handleDrawerClose = (open: boolean) => {
    setDrawerOpen(open);
    if (!open) {
      setSelectedPet(null);
    }
  };

  const handleOwnerChange = (ownerId: string) => {
    if (ownerId) {
      setSearchParams({ ownerId });
    } else {
      setSearchParams({});
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Pets</h1>
          <p className="text-muted-foreground">
            Manage pets and their information
          </p>
        </div>
        <Button onClick={handleAddNew} disabled={!selectedOwnerId}>
          <Plus className="mr-2 h-4 w-4" />
          Add Pet
        </Button>
      </div>

      <div className="flex items-center gap-2">
        <div className="flex items-center gap-2 flex-1">
          <Filter className="h-4 w-4 text-muted-foreground" />
          <div className="w-[250px]">
            <OwnerCombobox
              owners={owners}
              value={selectedOwnerId}
              onValueChange={handleOwnerChange}
              placeholder="Select an owner"
              includeAllOption={true}
            />
          </div>
        </div>

        {selectedOwnerId && (
          <Input
            placeholder="Search pets..."
            value={searchQuery}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSearchQuery(e.target.value)}
            className="max-w-sm"
          />
        )}
      </div>

      {!selectedOwnerId ? (
        <div className="rounded-md border p-8 text-center text-muted-foreground">
          <p>Please select an owner to view their pets.</p>
        </div>
      ) : (
        <PetsTable
          pets={pets}
          onEdit={handleEdit}
          onDelete={handleDelete}
          isLoading={isLoading}
        />
      )}

      <PetFormDrawer
        open={drawerOpen}
        onOpenChange={handleDrawerClose}
        pet={selectedPet}
        defaultOwnerId={selectedOwnerId}
      />

      <DeletePetDialog
        open={deleteDialogOpen}
        onOpenChange={setDeleteDialogOpen}
        pet={selectedPet}
      />
    </div>
  );
}
