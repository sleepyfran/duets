namespace State

module Bands =
  open Aether
  open Entities

  let addMember map (band: Band) (currentMember: CurrentMember) =
    let membersLens = Lenses.FromState.Bands.members_ band.Id
    let addMember = List.append [ currentMember ]

    map (Optic.map membersLens addMember)

  let addPastMember map (band: Band) (pastMember: PastMember) =
    let pastMembersLens =
      Lenses.FromState.Bands.pastMembers_ band.Id

    let addPastMember = List.append [ pastMember ]

    map (Optic.map pastMembersLens addPastMember)

  let removeMember map (band: Band) (currentMember: CurrentMember) =
    let membersLens = Lenses.FromState.Bands.members_ band.Id

    let removeMember =
      List.filter
        (fun (m: CurrentMember) -> m.Character.Id <> currentMember.Character.Id)

    map (Optic.map membersLens removeMember)
