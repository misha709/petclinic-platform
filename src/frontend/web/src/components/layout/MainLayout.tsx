import { useState } from 'react';
import { Outlet } from 'react-router-dom';
import { TopNav } from './TopNav';
import { GlobalSearch } from '@/components/search/GlobalSearch';

export function MainLayout() {
  const [searchOpen, setSearchOpen] = useState(false);

  return (
    <div className="min-h-screen bg-background">
      <TopNav onSearchClick={() => setSearchOpen(true)} />
      <main className="container mx-auto px-4 py-8">
        <Outlet />
      </main>
      <GlobalSearch open={searchOpen} onOpenChange={setSearchOpen} />
    </div>
  );
}
