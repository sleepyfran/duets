module Duets.Data.Genres

open Duets.Entities

let private allRoles = { Core = Roles.all; Common = [] }

let private instrumentsByGenre: (Genre * GenreInstruments) list =
    [ "Ambient", { Core = [ Guitar ]; Common = [ Bass; Vocals ] }
      "Black Metal", allRoles
      "Blackgaze", allRoles
      "Electronic", { Core = [ Drums; Bass; Vocals ]; Common = [ Guitar ] }
      "Folk", { Core = [ Guitar; Vocals ]; Common = [ Bass; Drums ] }
      "Hip-Hop", { Core = [ Vocals ]; Common = [ Bass; Drums ] }
      "Jazz", { Core = [ Guitar; Bass; Drums ]; Common = [ Vocals ] }
      "Pop", allRoles
      "Rock", allRoles
      "Shoegaze", allRoles ]

/// Contains all available genres in the game.
let all: Genre list = instrumentsByGenre |> List.map fst

/// Returns the instruments that bands of the given genre usually have.
let instrumentsFor genre =
    instrumentsByGenre
    |> List.tryFind (fst >> (=) genre)
    |> Option.map snd
    |> Option.defaultValue allRoles
