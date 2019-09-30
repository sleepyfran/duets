/**
 * Creates a method that acts similar to C#'s nameof operator but allowing only keys belonging to the type T.
 */
export const createNameOfType = <T>() => (name: keyof T) => name
