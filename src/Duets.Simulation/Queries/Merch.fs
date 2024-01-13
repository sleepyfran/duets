namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities
open Duets.Simulation

module Merch =
    /// Retrieves the price of a merch item for the given band.
    let itemPrice bandId itemProperty =
        let lens =
            Lenses.State.merchPrices_ >-> Map.keyWithDefault_ bandId Map.empty
            >?> Map.key_ itemProperty

        Optic.get lens

    /// Returns the cost of producing the given item.
    let itemProductionCost itemProperty =
        match itemProperty with
        | Listenable(CD, _) -> Config.MusicSimulation.Merch.cdPrice
        | Listenable(Vinyl, _) -> Config.MusicSimulation.Merch.vinylPrice
        | Wearable(Hoodie) -> Config.MusicSimulation.Merch.hoodiePrice
        | Wearable(TShirt) -> Config.MusicSimulation.Merch.tShirtPrice
        | Wearable(ToteBag) -> Config.MusicSimulation.Merch.toteBagPrice
        | _ -> 0m<dd>

    /// Returns the recommended price of a merch item so that it will sell for
    /// the given band.
    let recommendedItemPrice state bandId itemProperty =
        let bandFame = Bands.estimatedFameLevel state bandId

        let bandFameModifier =
            match bandFame with
            | fame when fame < 25 -> 1.5m
            | fame when fame < 50 -> 2m
            | fame when fame < 75 -> 3m
            | _ -> 4m

        itemProductionCost itemProperty * bandFameModifier
