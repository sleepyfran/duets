module Data.Savegame.Migrations.AddSocialFields

open Data.Savegame.Migrations.Common
open Duets.Data.Savegame.Types
open FSharp.Data

/// Migration that adds all the new social fields.
let migrate (root: JsonValue) =
    let data = root.TryGetProperty("Data")

    match data with
    | Some data ->
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

            let dataField = data |> replaceField "Characters" updatedCharacters

            Ok(root |> replaceField "Data" dataField |> setVersion 2m)
        | _ ->
            Error(InvalidStructure "Characters should be present in the data")
    | _ -> Error(InvalidStructure "Data field should be on the root")
