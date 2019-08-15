import { tryCatch } from 'fp-ts/lib/Either'

export const tryParseJson = (json: string) => {
    return tryCatch(() => JSON.parse(json), error => new Error(String(error)))
}
