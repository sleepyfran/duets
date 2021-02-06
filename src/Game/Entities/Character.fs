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

let getDefault () = { Name = ""; Age = 0; Gender = Other }
