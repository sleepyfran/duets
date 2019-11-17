/**
 * This file defines custom hooks to make the injection of core actions easier.
 */
import { useContext } from 'react'
import { CommandsContext } from '@ui/contexts/injections.context'

export const useCommands = () => useContext(CommandsContext)
