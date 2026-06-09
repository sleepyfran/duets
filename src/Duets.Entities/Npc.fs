module Duets.Entities.Npc

/// Creates a present NPC from a given character and goal.
let fromCharacterWithGoal goal character = { Npc = character; Goal = goal }
