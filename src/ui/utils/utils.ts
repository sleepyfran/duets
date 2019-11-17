/**
 * Checks that the given date is not an invalid date.
 * @param date Date to check.
 */
export const validDate = (date: Date) => isFinite(Number(date))

/**
 * Attempts to parse the given input and checks if it's valid. Returns the parsed date or the default one if it's
 * invalid.
 * @param input Input to parse.
 */
export const parseDateOrDefault = (input: string) => {
    const parsedDate = new Date(input)
    return validDate(parsedDate) ? parsedDate : new Date()
}

/**
 * Extracts the keys of a given object in a type-safe way.
 * @param input Input to extract the keys from.
 */
export const keysOf = <T, K extends keyof T>(input: T) => Object.keys(input) as K[]
