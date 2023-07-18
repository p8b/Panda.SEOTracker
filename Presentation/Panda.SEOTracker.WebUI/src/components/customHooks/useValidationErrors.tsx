import { useState } from 'react';
import { ValidationError } from '../appHttpClient';

export function useValidationErrors(): {
    errors: ValidationError[],
    /**
     * Checks the value is a valid ValidationError type before setting it.
     * @param value
     * @returns
     */
    set: (value: any) => void
} {
    const [errors, setLocalValidationErrors] = useState<ValidationError[]>([]);

    function set(value: any) {
        if (Array.isArray(value)) {
            var filterdArray = value.filter(x => x instanceof ValidationError)
            setLocalValidationErrors(filterdArray as ValidationError[]);
        }
    }
    return { errors, set };
}