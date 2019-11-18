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

/**
 * Creates an item in a given array if it doesn't exists or updates it if it exists.
 * @param input Input to check.
 * @param item Item to add or modify.
 * @param compareProperty Property used to compare two items.
 */
export const createOrUpdate = <T, K extends keyof T>(input: ReadonlyArray<T>, item: T, compareProperty: K) => {
    const clonedInput = [...input]
    const index = clonedInput.findIndex(it => it[compareProperty] === item[compareProperty])
    if (index < 0) {
        return [...clonedInput, item]
    } else {
        clonedInput[index] = item
        return clonedInput
    }
}

type Grouped<T, K extends keyof T> = [T[K], T[]][]

/**
 * Groups an array of a certain type by a given key of that type.
 * @param input Input to group.
 * @param key Key to use in the grouping.
 */
export const groupBy = <T, K extends keyof T>(input: T[] | ReadonlyArray<T>, key: K): Grouped<T, K> =>
    input.reduce((result, item) => {
        const itemKey = item[key]
        const existingValue = result.find(it => it[0] === itemKey)
        if (!existingValue) {
            result.push([itemKey, [item]])
        } else {
            existingValue[1].push(item)
        }

        return result
    }, [] as Grouped<T, K>)
