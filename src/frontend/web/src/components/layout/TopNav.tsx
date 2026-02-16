import { Link, useLocation } from 'react-router-dom';
import { Search, PawPrint } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { cn } from '@/lib/utils';

interface TopNavProps {
  onSearchClick: () => void;
}

export function TopNav({ onSearchClick }: TopNavProps) {
  const location = useLocation();

  const navItems = [
    { path: '/owners', label: 'Owners' },
    { path: '/pets', label: 'Pets' },
    { path: '/login', label: 'Login' },
  ];

  return (
    <nav className="border-b bg-background">
      <div className="container mx-auto px-4">
        <div className="flex h-16 items-center justify-between">
          <div className="flex items-center gap-8">
            <Link to="/" className="flex items-center gap-2 font-bold text-xl">
              <PawPrint className="h-6 w-6" />
              <span>Pet Clinic</span>
            </Link>
            
            <div className="flex items-center gap-1">
              {navItems.map((item) => (
                <Link
                  key={item.path}
                  to={item.path}
                  className={cn(
                    'px-4 py-2 rounded-md text-sm font-medium transition-colors',
                    location.pathname === item.path
                      ? 'bg-primary text-primary-foreground'
                      : 'text-muted-foreground hover:bg-accent hover:text-accent-foreground'
                  )}
                >
                  {item.label}
                </Link>
              ))}
            </div>
          </div>

          <Button
            variant="outline"
            size="sm"
            onClick={onSearchClick}
            className="gap-2"
          >
            <Search className="h-4 w-4" />
            <span className="hidden sm:inline">Search</span>
            <kbd className="pointer-events-none hidden h-5 select-none items-center gap-1 rounded border bg-muted px-1.5 font-mono text-[10px] font-medium opacity-100 sm:flex">
              <span className="text-xs">⌘</span>K
            </kbd>
          </Button>
        </div>
      </div>
    </nav>
  );
}
