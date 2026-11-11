[<AutoOpen>]
module Duets.Cli.Components.InstrumentPrompt

open Duets
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

let private instrumentGroups genre (available: InstrumentType list) =
    let instruments = Data.Genres.instrumentsFor genre
    let isAvailable instrument = List.contains instrument available

    let core = instruments.Core |> List.filter isAvailable
    let common = instruments.Common |> List.filter isAvailable

    let unusual =
        available
        |> List.filter (fun instrument ->
            not (List.contains instrument core || List.contains instrument common))

    [ Generic.instrumentGroupTypical, core
      Generic.instrumentGroupAlsoFits, common ],
    unusual

/// Shows a prompt to select one of the available instruments, grouped by how
/// usual they are in the given genre.
let showInstrumentPrompt title genre available =
    let groups, unusual = instrumentGroups genre available

    showGroupedChoicePrompt
        title
        Generic.instrumentGroupUnusual
        Generic.roleName
        groups
        unusual

/// Shows a prompt to optionally select one of the available instruments,
/// grouped by how usual they are in the given genre.
let showOptionalInstrumentPrompt title backText genre available =
    let groups, unusual = instrumentGroups genre available

    showGroupedOptionalChoicePrompt
        title
        backText
        Generic.instrumentGroupUnusual
        Generic.roleName
        groups
        unusual

/// Shows a prompt to optionally select an instrument to hire for the given
/// band, listing the typical ones that nobody in the band plays yet first.
let showHireInstrumentPrompt title backText (band: Band) =
    let groups, unusual = instrumentGroups band.Genre Data.Roles.all

    let missing =
        (Data.Genres.instrumentsFor band.Genre).Core
        |> List.filter (Queries.Bands.hasAnyMemberWithRole band >> not)

    let groupsWithMissing =
        (Generic.instrumentGroupMissing, missing)
        :: (groups
            |> List.map (fun (name, items) -> name, List.except missing items))

    showGroupedOptionalChoicePrompt
        title
        backText
        Generic.instrumentGroupUnusual
        Generic.roleName
        groupsWithMissing
        unusual
