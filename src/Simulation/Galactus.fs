/// This name was deliberately chosen because I have no idea how to properly
/// name this module, so what's better than referencing this piece of art:
/// https://www.youtube.com/watch?v=y8OnoxKotPQ
module Simulation.Galactus

open Entities
open Simulation.Skills.ImproveSkills

/// Takes an effect and runs all the correspondent simulation functions
/// gathering their effects as well and adding them to a final list with all the
/// effects that were created. Useful for situations in which an effect should
/// trigger other effects such as starting a new song or improving an existing
/// one, which should trigger an improvement in the band's skills.
let runOne state effect =
    match effect with
    | SongStarted (band, _) ->
        improveBandSkillsAfterComposing state band
        |> List.append [ effect ]
    | SongImproved (band, _) ->
        improveBandSkillsAfterComposing state band
        |> List.append [ effect ]
    | _ -> [ effect ]

/// Takes multiple effects and runs them through `runOne` combining all
/// the resulting effects into a list.
let runMultiple state effects =
    effects |> List.map (runOne state) |> List.concat
