module Entities.Character

type Sex =
  | Male
  | Female
  | Other

/// Defines a character, be it the one that the player is controlling or any
/// other NPC of the world.
type Character =
  { Name: string
    Birthday: Calendar.Date
    Sex: Sex }

let getDefault () =
  { Name = ""
    Birthday = Calendar.fromDayMonth 1 1
    Sex = Other }
