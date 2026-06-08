module Data.Savegame.Migrations.Common

open FSharp.Data

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

/// Maps a specific field off a record if it is an object, otherwise returns the
/// JSON as-is.
let mapField fieldName mapper (values: JsonValue) =
    match values with
    | JsonValue.Record(props) ->
        JsonValue.Record(
            props
            |> Array.map (fun (fName, fValue) ->
                if fName = fieldName then
                    (fName, mapper fValue)
                else
                    (fName, fValue))
        )
    | _ -> values

/// Adds a field with the given name and value to the given JSON value if it is
/// an object and the field does not already exist in the object, otherwise
/// returns the JSON as-is.
let rec addFieldIfNonExistent fieldName fieldValue (values: JsonValue) =
    match values with
    | JsonValue.Record(props) ->
        let prop = props |> Array.tryFind (fun (pName, _) -> pName = fieldName)

        match prop with
        | Some _ -> values
        | None -> addField fieldName fieldValue values
    | _ -> values

/// Maps the values of a JSON array that represents a tuple of two elements.
let mapTuple2 mapper (tuple: JsonValue) =
    match tuple with
    | JsonValue.Array([| fst; snd |]) ->
        let fst, snd = mapper (fst, snd)
        [| fst; snd |] |> JsonValue.Array
    | _ -> tuple

/// Maps the values of a JSON value if it is an array, otherwise returns the JSON
/// as-is.
let mapArray mapper (arr: JsonValue) =
    match arr with
    | JsonValue.Array(arr) -> JsonValue.Array(arr |> Array.map mapper)
    | _ -> arr

/// Sets the version field in the given JsonValue to the provided version if the JSON
/// is an object, otherwise returns the value as-is.
let setVersion version =
    replaceField "Version" (JsonValue.Number version)
