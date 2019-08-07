/**
 * This file defines custom hooks to make the injection of commands and queries easier.
 */
import { useContext } from 'react'
import { InjectionsContext } from '@ui/contexts/injections.context'

export const useQueries = () => useContext(InjectionsContext).queries
export const useCommands = () => useContext(InjectionsContext).commands
