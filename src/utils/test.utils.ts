/**
 * Creates a partial object of a type with only the specified property.
 * @param property Only property to add to the object.
 * @param value Value to set the property to.
 */
export const createPartialOf = <T>(property: any, value: any) => (({ [property]: value } as unknown) as T)
