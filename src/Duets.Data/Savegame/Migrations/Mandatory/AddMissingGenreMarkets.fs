module Data.Savegame.Migrations.Mandatory.AddMissingGenreMarkets

open Data.Savegame.Migrations.Common
open Duets.Common
open Duets.Data
open Duets.Entities
open FSharp.Data

/// Migration that adds a market for every genre in the game that the savegame
/// doesn't have a market for yet.
let migrate (root: JsonValue) =
    let genreMarketsJson =
        root.TryGetProperty("Data")
        |> Option.bind (fun data -> data.TryGetProperty("GenreMarkets"))

    match genreMarketsJson with
    | Some genreMarketsJson ->
        let genreMarkets: GenreMarketByGenre =
            genreMarketsJson.ToString() |> Serializer.deserialize

        let updatedGenreMarkets =
            Genre.Market.addMissing Random.floatBetween Genres.all genreMarkets

        if updatedGenreMarkets = genreMarkets then
            Ok root
        else
            let dataField =
                root.GetProperty("Data")
                |> replaceField
                    "GenreMarkets"
                    (updatedGenreMarkets |> Serializer.serialize |> JsonValue.Parse)

            Ok(root |> replaceField "Data" dataField)
    (* Nothing to fill in, the savegame will fail to parse later if it's needed. *)
    | None -> Ok root
