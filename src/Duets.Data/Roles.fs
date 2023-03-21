module Duets.Data.Roles

open Duets.Entities

/// List of all available roles in the game.
let all = [ Guitar; Drums; Bass; Vocals ]

/// Returns the usual roles that are used for a specific genre.
let forGenre genre =
    match genre with
    | "Ambient" -> [ Guitar ]
    | "Electronic" -> [ Drums; Bass; Vocals ]
    | "Folk" -> [ Guitar; Vocals ]
    | "Hip-Hop" -> [ Vocals ]
    | "Black Metal"
    | "Blackgaze"
    | "Jazz"
    | "Pop"
    | "Rock"
    | "Shoegaze"
    | _ -> all
