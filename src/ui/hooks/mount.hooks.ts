import { useEffect, EffectCallback } from 'react'

/**
 * Defines a useEffect that only executes on mount. I chose to use this instead of manually using useEffect since the linter
 * will always fail if we don't specify the real dependencies and this case we don't need to do it.
 * @param effect Effect to apply.
 */
export const useMountEffect = (effect: EffectCallback) => useEffect(effect, [])
