import { PET_TYPES, type PetType } from '@/types/models';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Dog, Cat, Bird, Fish, Rabbit, Rat, Bug } from 'lucide-react';

interface PetTypeSelectProps {
  value: string;
  onValueChange: (value: string) => void;
  disabled?: boolean;
}

export function PetTypeSelect({ value, onValueChange, disabled }: PetTypeSelectProps) {
  const getIcon = (type: PetType) => {
    const className = "h-4 w-4 mr-2";
    switch (type) {
      case 'Dog':
        return <Dog className={className} />;
      case 'Cat':
        return <Cat className={className} />;
      case 'Bird':
        return <Bird className={className} />;
      case 'Fish':
        return <Fish className={className} />;
      case 'Rabbit':
        return <Rabbit className={className} />;
      case 'Hamster':
      case 'GuineaPig':
        return <Rat className={className} />;
      case 'Ferret':
      case 'Reptile':
      case 'Other':
        return <Bug className={className} />;
      default:
        return null;
    }
  };

  return (
    <Select value={value} onValueChange={onValueChange} disabled={disabled}>
      <SelectTrigger>
        <SelectValue placeholder="Select pet type" />
      </SelectTrigger>
      <SelectContent>
        {PET_TYPES.map((type) => (
          <SelectItem key={type} value={type}>
            <div className="flex items-center">
              {getIcon(type)}
              <span>{type}</span>
            </div>
          </SelectItem>
        ))}
      </SelectContent>
    </Select>
  );
}
