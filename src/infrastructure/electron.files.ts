import { tryCatch } from 'fp-ts/lib/TaskEither'
import { promises as fs } from 'fs'
import { remote } from 'electron'

/**
 * Transforms a call to fs.readFile into a TaskEither.
 * @param path Path of the file to read.
 */
export const readFile = (path: string) => tryCatch(() => fs.readFile(path, 'utf8'), error => new Error(String(error)))

/**
 * Transforms a call to fs.writeFile into a TaskEither.
 * @param path Path of the file to read.
 * @param content Content to write to the file.
 */
export const writeFile = (path: string, content: string) =>
    tryCatch(() => fs.writeFile(path, content, 'utf-8'), error => new Error(String(error)))

/**
 * Returns the userData folder path.
 */
export const duetsDataPath = () => remote.app.getPath('userData')

/**
 * Returns the cached database file path.
 */
export const duetsCachedDatabasePath = () => `${duetsDataPath()}/duets.db`
