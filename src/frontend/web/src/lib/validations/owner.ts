import { z } from 'zod';

export const ownerSchema = z.object({
  firstName: z.string().min(1, 'First name is required').max(50, 'First name is too long'),
  lastName: z.string().min(1, 'Last name is required').max(50, 'Last name is too long'),
  address: z.string().min(1, 'Address is required').max(100, 'Address is too long'),
  city: z.string().min(1, 'City is required').max(50, 'City is too long'),
  telephone: z.string().min(10, 'Phone number must be at least 10 digits').max(20, 'Phone number is too long'),
});

export type OwnerFormValues = z.infer<typeof ownerSchema>;
