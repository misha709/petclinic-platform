import { createBrowserRouter, Navigate } from 'react-router-dom';
import { MainLayout } from '@/components/layout/MainLayout';
import { LoginPage } from '@/pages/LoginPage';
import { OwnersPage } from '@/pages/OwnersPage';
import { PetsPage } from '@/pages/PetsPage';
import { VetsPage } from '@/pages/VetsPage';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <MainLayout />,
    children: [
      {
        index: true,
        element: <Navigate to="/owners" replace />,
      },
      {
        path: 'owners',
        element: <OwnersPage />,
      },
      {
        path: 'pets',
        element: <PetsPage />,
      },
      {
        path: 'vets',
        element: <VetsPage />,
      },
    ],
  },
  {
    path: '/login',
    element: <LoginPage />,
  },
]);
