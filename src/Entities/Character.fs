module Entities.Character

type Gender =
  | Male
  | Female
  | Other

/// Defines a character, be it the one that the player is controlling or any
/// other NPC of the world.
type Character =
  { Name: string
    Age: int
    Gender: Gender }

/// Base character that has no real properties. Only to be used while
/// populating a character during a transformation.
let empty =
  { Name = ""
    Age = 0
    Gender = Gender.Other }
