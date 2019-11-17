import Validation, { Result } from 'validum'

export type ValidateStartDate = (startDate: Date, propertyName: string) => Result<Date>

/**
 * Creates a validator that checks that the start date is valid.
 */
export default (): ValidateStartDate => (startDate, propertyName) =>
    Validation.of(startDate)
        .notUndefined()
        .withPropertyName(propertyName)
        .result()
