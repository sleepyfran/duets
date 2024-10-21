module Duets.Simulation.State.Bands

open Aether
open Aether.Operators
open Duets.Entities

let private createBandLens (band: Band) state =
    let characterBandsLens =
        Lenses.State.bands_ >-> Lenses.Bands.characterBand_ band.Id

    let simulatedBandsLens =
        Lenses.State.bands_ >-> Lenses.Bands.simulatedBand_ band.Id

    Optic.get characterBandsLens state
    |> Option.map (fun _ -> characterBandsLens)
    |> Option.orElseWith (fun () ->
        Optic.get simulatedBandsLens state
        |> Option.map (fun _ -> simulatedBandsLens))
    |> Option.defaultValue
        characterBandsLens (* Default to character, worst case scenario the Optic will do nothing since it's a Prism. *)

let addCharacterBand (band: Band) =
    let lens = Lenses.State.bands_ >-> Lenses.Bands.characterBands_

    let addBand = Map.add band.Id band

    Optic.map lens addBand

let addSimulated (band: Band) =
    let lens = Lenses.State.bands_ >-> Lenses.Bands.simulatedBands_

    let addBand = Map.add band.Id band

    Optic.map lens addBand

let addMember (band: Band) (currentMember: CurrentMember) state =
    let membersLens = createBandLens band state >?> Lenses.Band.members_

    let addMember = List.append [ currentMember ]

    Optic.map membersLens addMember state

let addPastMember (band: Band) (pastMember: PastMember) state =
    let pastMembersLens = createBandLens band state >?> Lenses.Band.pastMembers_

    let addPastMember = List.append [ pastMember ]

    Optic.map pastMembersLens addPastMember state

let removeMember (band: Band) (currentMember: CurrentMember) state =
    let membersLens = createBandLens band state >?> Lenses.Band.members_

    let removeMember =
        List.filter (fun (m: CurrentMember) ->
            m.CharacterId <> currentMember.CharacterId)

    Optic.map membersLens removeMember state

let changeFans (band: Band) fans state =
    let fansLens = createBandLens band state >?> Lenses.Band.fans_

    Optic.set fansLens fans state

let changeGenre (band: Band) (genre: Genre) state =
    let genreLens = createBandLens band state >?> Lenses.Band.genre_

    Optic.set genreLens genre state
