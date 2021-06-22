module Entities.Band

type BandValidationError =
    | NameTooShort
    | NameTooLong
    | NoMembersGiven

/// Creates a band given its name, genre and members, if possible.
let from name genre members today =
    if String.length name < 1 then
        Error NameTooShort
    else if String.length name > 100 then
        Error NameTooLong
    else if List.length members < 1 then
        Error NoMembersGiven
    else
        Ok
            { Id = BandId <| Identity.create ()
              StartDate = today
              Name = name
              Genre = genre
              Fame = 1
              Members = members
              PastMembers = [] }

/// Returns default values for a band to serve as a placeholder to build a band
/// upon. Generates a valid ID.
let empty =
    { Id = BandId <| Identity.create ()
      StartDate = Calendar.gameBeginning
      Name = ""
      Genre = ""
      Fame = 1
      Members = []
      PastMembers = [] }

module Member =
    /// Creates a member from a character and a role from today onwards.
    let from character role today =
        { Character = character
          Role = role
          Since = today }

    /// Creates a current member of the band given a member available for hiring.
    let fromMemberForHire (memberForHire: MemberForHire) =
        from memberForHire.Character memberForHire.Role

module MemberForHire =
    /// Creates a member for hire given a character, its role and its skills.
    let from character role skills =
        { Character = character
          Role = role
          Skills = skills }

module PastMember =
    /// Creates a past member given a current member with today as its fired date.
    let fromMember (currentMember: CurrentMember) today =
        { Character = currentMember.Character
          Role = currentMember.Role
          Period = (currentMember.Since, today) }

module Repertoire =
    /// Creates a completely empty repertoire.
    let empty =
        { UnfinishedSongs = Map.empty
          FinishedSongs = Map.empty
          UnreleasedAlbums = Map.empty
          ReleasedAlbums = Map.empty }

    /// Creates an empty band repertoire for a given band.
    let emptyFor bandId =
        { UnfinishedSongs = [ (bandId, Map.empty) ] |> Map.ofSeq
          FinishedSongs = [ (bandId, Map.empty) ] |> Map.ofSeq
          UnreleasedAlbums = [ (bandId, Map.empty) ] |> Map.ofSeq
          ReleasedAlbums = [ bandId, Map.empty ] |> Map.ofSeq }
