module Data.Savegame.Migrations.MultiInstrumentMembers

open Data.Savegame.Migrations.Common
open Duets.Data.Savegame.Types
open FSharp.Data

let private wrapRole (bandMember: JsonValue) =
    match bandMember.TryGetProperty("Role") with
    | Some role ->
        bandMember
        |> deleteField "Role"
        |> addField "Roles" (JsonValue.Array [| role |])
    | None -> bandMember

let private migrateMembers fieldName (band: JsonValue) =
    match band.TryGetProperty(fieldName) with
    | Some(JsonValue.Array members) ->
        band
        |> replaceField fieldName (members |> Array.map wrapRole |> JsonValue.Array)
    | _ -> band

let private migrateBand band =
    band |> migrateMembers "Members" |> migrateMembers "PastMembers"

(* Band maps are serialized as an array of [id, band] pairs. *)
let private migrateBandMap bandMap =
    match bandMap with
    | JsonValue.Array entries ->
        entries
        |> Array.map (fun entry ->
            match entry with
            | JsonValue.Array [| id; band |] ->
                JsonValue.Array [| id; migrateBand band |]
            | _ -> entry)
        |> JsonValue.Array
    | _ -> bandMap

let private migrateBandMapField fieldName (bands: JsonValue) =
    match bands.TryGetProperty(fieldName) with
    | Some bandMap -> bands |> replaceField fieldName (migrateBandMap bandMap)
    | None -> bands

/// Migration that turns the single role of every band member into a list of
/// roles.
let migrate (root: JsonValue) =
    let data = root.TryGetProperty("Data")

    match data with
    | Some data ->
        match data.TryGetProperty("Bands") with
        | Some bands ->
            let bandsField =
                bands
                |> migrateBandMapField "Character"
                |> migrateBandMapField "Simulated"

            let dataField = data |> replaceField "Bands" bandsField

            Ok(root |> replaceField "Data" dataField |> setVersion 2m)
        | None -> Error(InvalidStructure "Bands should be present in the data")
    | _ -> Error(InvalidStructure "Data field should be on the root")
