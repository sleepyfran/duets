module Data.Savegame.Migrations.AddSocialFields

open Data.Savegame.Migrations.Common
open Duets.Data.Savegame.Types
open FSharp.Data

let private migrateCharacters (data: JsonValue) =
    let oldCharacters = data.TryGetProperty("Characters")

    match oldCharacters with
    | Some(JsonValue.Array characters) ->
        let updatedCharacters =
            characters
            |> Array.map (
                mapTuple2 (fun (characterId, character) ->
                    (characterId,
                     addFieldIfNonExistent
                         "Traits"
                         (JsonValue.Array(Array.empty))
                         character))
            )
            |> JsonValue.Array


        data |> replaceField "Characters" updatedCharacters |> Ok
    | _ -> Error(InvalidStructure "Characters should be present in the data")

let private migrateRelationships (data: JsonValue) =
    let oldRelationships = data.TryGetProperty("Relationships")

    match oldRelationships with
    | Some(relationships) ->
        let mapper =
            mapArray (
                mapTuple2 (fun (characterId, relationship) ->
                    (characterId,
                     addFieldIfNonExistent
                         "DiscoveredTraits"
                         (JsonValue.Array(Array.empty))
                         relationship))
            )

        let updatedRelationships =
            relationships |> mapField "ByCharacterId" mapper

        data |> replaceField "Relationships" updatedRelationships |> Ok
    | _ -> Error(InvalidStructure "Relationships should be present in the data")

/// Migration that adds all the new social fields.
let migrate (root: JsonValue) =
    let data = root.TryGetProperty("Data")

    match data with
    | Some data ->
        data
        |> migrateCharacters
        |> Result.bind migrateRelationships
        |> Result.map (fun data ->
            root |> replaceField "Data" data |> setVersion 2m)
    | _ -> Error(InvalidStructure "Data field should be on the root")
