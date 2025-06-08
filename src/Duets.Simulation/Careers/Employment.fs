module rec Duets.Simulation.Careers.Employment

open Duets.Entities
open Duets.Simulation

/// Gives the character the specified job. If there were any previous jobs, it'll
/// be overriden by this one.
let acceptJob state job =
    let currentCharacter = Queries.Characters.playableCharacter state
    let currentJob = Queries.Career.current state

    [ yield!
          match currentJob with
          | Some currentJob ->
              [ CareerLeave(currentCharacter.Id, currentJob)
                yield! dropRequiredItems currentJob ]
          | None -> []

      yield! addRequiredItemsToInventory job
      CareerAccept(currentCharacter.Id, job) ]

let private addRequiredItemsToInventory job =
    gatherJobRequiredItems job |> List.map ItemAddedToCharacterInventory

let private dropRequiredItems job =
    gatherJobRequiredItems job |> List.map ItemRemovedFromCharacterInventory

let private gatherJobRequiredItems job =
    let room = job.Location |||> Queries.World.roomById

    room.RequiredItemsForEntrance
    |> Option.map _.Items
    |> Option.defaultValue []
