module Entities.Band

type BandValidationError =
  | NameTooShort
  | NameTooLong
  | NoMembersGiven

/// Creates a band given its name, genre and members, if possible.
let from name genre members =
  if String.length name < 1 then
    Error NameTooShort
  else if String.length name > 100 then
    Error NameTooLong
  else if List.length members < 1 then
    Error NoMembersGiven
  else
    Ok
      { Id = BandId <| Identity.create ()
        Name = name
        Genre = genre
        Members = members
        PastMembers = [] }

/// Returns default values for a band to serve as a placeholder to build a band
/// upon. Generates a valid ID.
let empty =
  { Id = BandId <| Identity.create ()
    Name = ""
    Genre = ""
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

module Repertoire =
  /// Creates an empty band repertoire.
  let empty =
    { Unfinished = Map.empty
      Finished = Map.empty }
