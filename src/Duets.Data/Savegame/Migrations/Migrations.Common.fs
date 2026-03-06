module Data.Savegame.Migrations.Common

open FSharp.Data

type JsonRecord = (string * JsonValue) array

/// Sets a field in the given JsonValue to the provided value if the JSON is
/// an object, otherwise returns the value as-is.
let replaceField fieldName updatedValue value =
    match value with
    | JsonValue.Record value ->
        value
        |> Array.map (fun (key, value) ->
            if key = fieldName then
                (key, updatedValue)
            else
                (key, value))
        |> JsonValue.Record
    | _ -> value

/// Deletes a field with the given name in the given JsonVAlue if the JSON is
/// an object, otherwise returns the value as-is.
let deleteField fieldName value =
    match value with
    | JsonValue.Record value ->
        value
        |> Array.filter (fun (key, _) -> key <> fieldName)
        |> JsonValue.Record
    | _ -> value

/// Adds a field with the given name and value to the given JSON value if it is
/// an object, otherwise returns the JSON as-is.
let addField fieldName fieldValue (values: JsonValue) =
    match values with
    | JsonValue.Record(props) ->
        JsonValue.Record(props |> Array.append [| fieldName, fieldValue |])
    | _ -> values

/// Sets the version field in the given JsonValue to the provided version if the JSON
/// is an object, otherwise returns the value as-is.
let setVersion version =
    replaceField "Version" (JsonValue.Number version)
