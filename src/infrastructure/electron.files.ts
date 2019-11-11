import { promises as fs } from 'fs'
import { remote } from 'electron'

/**
 * Wraps a fs.readFile call.
 * @param path Path of the file to read.
 */
export const readFile = (path: string) => fs.readFile(path, 'utf8')

/**
 * Wraps a fs.writeFile call.
 * @param path Path of the file to read.
 * @param content Content to write to the file.
 */
export const writeFile = (path: string, content: string) => fs.writeFile(path, content, 'utf-8')

/**
 * Returns the userData folder path.
 */
export const duetsDataPath = () => remote.app.getPath('userData')

/**
 * Returns the cached database file path.
 */
export const duetsCachedDatabasePath = () => `${duetsDataPath()}/duets.db`
