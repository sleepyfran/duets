module Entities.Band

type BandNameValidationError =
    | NameTooShort
    | NameTooLong

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

/// Creates a band given its name, genre and initial member.
let from name genre initialMember today =
    { Id = BandId <| Identity.create ()
      StartDate = today
      Name = name
      Genre = genre
      Fame = 1
      Members = [ initialMember ]
      PastMembers = [] }

/// Validates whether the name of the band is valid or not.
let validateName name =
    if String.length name < 1 then
        Error NameTooShort
    else if String.length name > 100 then
        Error NameTooLong
    else
        Ok name

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

module SongRepertoire =
    /// Creates a completely empty song repertoire.
    let empty =
        { UnfinishedSongs = Map.empty
          FinishedSongs = Map.empty }

    /// Creates an empty band song repertoire for a given band.
    let emptyFor bandId =
        { UnfinishedSongs = [ (bandId, Map.empty) ] |> Map.ofSeq
          FinishedSongs = [ bandId, Map.empty ] |> Map.ofSeq }

module AlbumRepertoire =
    /// Creates a completely empty album repertoire.
    let empty =
        { UnreleasedAlbums = Map.empty
          ReleasedAlbums = Map.empty }

    /// Creates an empty band album repertoire for a given band.
    let emptyFor bandId =
        { UnreleasedAlbums = [ (bandId, Map.empty) ] |> Map.ofSeq
          ReleasedAlbums = [ bandId, Map.empty ] |> Map.ofSeq }
