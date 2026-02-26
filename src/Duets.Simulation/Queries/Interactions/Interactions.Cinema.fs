namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Data.Items
open Duets.Entities
open Duets.Simulation

module Cinema =
    /// Gather all available interactions inside a cinema.
    let internal interactions state cityId currentPlace roomType =
        let currentDate = Queries.Calendar.today state
        let city = Queries.World.cityById cityId

        match roomType with
        | RoomType.Lobby ->
            let shopInteractions =
                [ ShopInteraction.Order Cinema.all |> Interaction.Shop
                  ShopInteraction.SeeMenu Cinema.all |> Interaction.Shop ]

            match Queries.Cinema.currentMovie currentDate with
            | Some movie ->
                let price = Queries.Cinema.ticketPrice city
                Interaction.Cinema(CinemaInteraction.BuyTicket(movie, price)) :: shopInteractions
            | None -> shopInteractions
        | RoomType.ScreeningRoom ->
            match Queries.Cinema.currentMovie currentDate with
            | Some movie ->
                [ Interaction.Cinema(CinemaInteraction.WatchMovie movie) ]
            | None -> []
        | _ -> []
