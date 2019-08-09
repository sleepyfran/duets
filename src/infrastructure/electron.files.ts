import { tryCatch } from 'fp-ts/lib/TaskEither'
import { promises as fs } from 'fs'

/**
 * Transforma a call to fs.readFile into a TaskEither.
 * @param path Path of the file to read.
 */
export const readFile = (path: string) => tryCatch(() => fs.readFile(path, 'utf8'), error => new Error(String(error)))
