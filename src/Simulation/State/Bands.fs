module Simulation.State.Bands

open Aether
open Aether.Operators
open Entities

let addMember (band: Band) (currentMember: CurrentMember) =
    let membersLens = Lenses.FromState.Bands.members_ band.Id

    let addMember = List.append [ currentMember ]

    Optic.map membersLens addMember

let addPastMember (band: Band) (pastMember: PastMember) =
    let pastMembersLens = Lenses.FromState.Bands.pastMembers_ band.Id

    let addPastMember = List.append [ pastMember ]

    Optic.map pastMembersLens addPastMember

let removeMember (band: Band) (currentMember: CurrentMember) =
    let membersLens = Lenses.FromState.Bands.members_ band.Id

    let removeMember =
        List.filter (fun (m: CurrentMember) ->
            m.CharacterId <> currentMember.CharacterId)

    Optic.map membersLens removeMember

let changeFans (band: Band) (fans: int) =
    let fansLens =
        Lenses.FromState.Bands.band_ band.Id
        >?> Lenses.Band.fans_

    Optic.set fansLens fans
