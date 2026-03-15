import { z } from 'zod';

export const specializationSchema = z.object({
  name: z.string().min(1, 'Name is required').max(100, 'Name is too long'),
});

export type SpecializationFormValues = z.infer<typeof specializationSchema>;
