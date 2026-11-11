module Data.Savegame.Migrations.GenericSoloEvents

open Data.Savegame.Migrations.Common
open Duets.Data.Savegame.Types
open FSharp.Data

let private soloInstrument case =
    match case with
    | "GuitarSolo" -> Some "Guitar"
    | "BassSolo" -> Some "Bass"
    | "DrumSolo" -> Some "Drums"
    | _ -> None

let private migrateEvent (event: JsonValue) =
    match event.TryGetProperty("Case") with
    | Some(JsonValue.String case) ->
        match soloInstrument case with
        | Some instrument ->
            JsonValue.Record
                [| ("Case", JsonValue.String "Solo")
                   ("Fields",
                    JsonValue.Array
                        [| JsonValue.Record
                               [| ("Case", JsonValue.String instrument) |] |]) |]
        | None -> event
    | _ -> event

let private singleFieldOf case (value: JsonValue) =
    match value.TryGetProperty("Case"), value.TryGetProperty("Fields") with
    | Some(JsonValue.String valueCase), Some(JsonValue.Array [| field |]) when
        valueCase = case
        ->
        Some field
    | _ -> None

let private withSingleField field value =
    value |> replaceField "Fields" (JsonValue.Array [| field |])

(* Events are only stored while a concert is ongoing, inside the situation. *)
let private migrateSituation situation =
    singleFieldOf "Concert" situation
    |> Option.bind (fun concertSituation ->
        singleFieldOf "InConcert" concertSituation
        |> Option.bind (fun ongoingConcert ->
            match ongoingConcert.TryGetProperty("Events") with
            | Some(JsonValue.Array events) ->
                let updatedOngoingConcert =
                    ongoingConcert
                    |> replaceField
                        "Events"
                        (events |> Array.map migrateEvent |> JsonValue.Array)

                concertSituation
                |> withSingleField updatedOngoingConcert
                |> fun updatedConcertSituation ->
                    situation |> withSingleField updatedConcertSituation
                |> Some
            | _ -> None))
    |> Option.defaultValue situation

/// Migration that turns the guitar, bass and drum solo events of an ongoing
/// concert into solo events with their instrument.
let migrate (root: JsonValue) =
    match root.TryGetProperty("Data") with
    | Some data ->
        let dataField =
            match data.TryGetProperty("Situation") with
            | Some situation ->
                data |> replaceField "Situation" (migrateSituation situation)
            | None -> data

        Ok(root |> replaceField "Data" dataField |> setVersion 3m)
    | _ -> Error(InvalidStructure "Data field should be on the root")
