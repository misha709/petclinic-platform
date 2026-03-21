import { z } from 'zod';

export const createVisitSchema = z.object({
  petId: z.guid('Select a pet'),
  vetId: z.guid('Select a vet'),
  scheduledAt: z.string().min(1, 'Schedule date and time is required'),
  durationMinutes: z
    .number()
    .int()
    .min(5, 'Minimum 5 minutes')
    .max(480, 'Maximum 8 hours (480 minutes)'),
  reason: z.string().min(1, 'Reason is required').max(500, 'Reason is too long'),
  notes: z.string().max(1000, 'Notes are too long').optional(),
});

export const updateVisitSchema = createVisitSchema.omit({ petId: true, vetId: true });

export type CreateVisitFormValues = z.infer<typeof createVisitSchema>;
export type UpdateVisitFormValues = z.infer<typeof updateVisitSchema>;
