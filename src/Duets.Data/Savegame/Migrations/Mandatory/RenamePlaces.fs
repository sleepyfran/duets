module Data.Savegame.Migrations.Mandatory.RenamePlaces

open Duets.Entities
open FSharp.Data

(*
Places get their ID from their name, so renaming or removing a place leaves
old savegames pointing to a place that no longer exists. Removed places map
to the place that replaced them.
*)
let private renamedPlaces =
    [ "Microsoft Theater", "Peacock Theater"
      "Palacio de Deportes de la Comunidad", "WiZink Center"
      "The Ocean Room", "McCabe's Guitar Shop"
      "The Central Club", "Harvelle's"
      "The Pacific Hideaway", "Water Grill Santa Monica"
      "The Coastal Spa", "Hotel Casa del Mar"
      "The Ocean House", "Fairmont Miramar Hotel & Bungalows"
      "Ocean View Books", "Angel City Books & Records"
      "The Beach Bean", "Urth Caffé"
      "Vintage Guitar Shop", "Truetone Music"
      "Santa Monica Urgent Care", "UCLA Santa Monica Medical Center"
      "Local Iron Gym", "Equinox Santa Monica" ]
    |> List.map (fun (oldName, newName) ->
        Identity.Reproducible.create oldName,
        Identity.Reproducible.create newName)
    |> Map.ofList

let private renameId id =
    renamedPlaces |> Map.tryFind id |> Option.defaultValue id

let rec private renameIn (value: JsonValue) =
    match value with
    | JsonValue.String id -> JsonValue.String(renameId id)
    | JsonValue.Array items -> items |> Array.map renameIn |> JsonValue.Array
    | JsonValue.Record fields ->
        fields
        |> Array.map (fun (key, field) -> renameId key, renameIn field)
        |> JsonValue.Record
    | _ -> value

/// Migration that replaces the IDs of renamed or removed places with the IDs
/// of the places that replaced them.
let migrate (root: JsonValue) = Ok(renameIn root)
