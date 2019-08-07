import React from 'react'
import { GameInfo } from '@ui/types/game-info'

/**
 * Context in which we'll have access to the information about the game.
 */
export const GameInfoContext = React.createContext<GameInfo>({} as GameInfo)
