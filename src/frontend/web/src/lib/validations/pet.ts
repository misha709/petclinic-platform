import { z } from 'zod';

export const petSchema = z.object({
  name: z.string().min(1, 'Pet name is required').max(50, 'Pet name is too long'),
  petType: z.enum(['Dog', 'Cat', 'Bird', 'Fish', 'Rabbit', 'Hamster', 'GuineaPig', 'Ferret', 'Reptile', 'Other'], {
    message: 'Pet type is required',
  }),
  breed: z.string().min(1, 'Breed is required').max(50, 'Breed is too long'),
  birthDate: z.date().optional().nullable(),
  ownerId: z.uuidv4('Invalid owner ID'),
});

export type PetFormValues = z.infer<typeof petSchema>;
