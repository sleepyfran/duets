module Duets.UI.Text.World.Interactions

open Duets.Entities

let emoji (item: InteractionWithMetadata) : string =
    match item.Interaction with
    | Interaction.Airport a ->
        match a with
        | AirportInteraction.BoardAirplane _ -> "✈️"
        | AirportInteraction.PassSecurity -> "🔒"
        | AirportInteraction.WaitUntilLanding _ -> "⏳"
    | Interaction.Career c ->
        match c with
        | CareerInteraction.Work _ -> "💼"
    | Interaction.Cinema c ->
        match c with
        | CinemaInteraction.BuyTicket _ -> "🎟️"
        | CinemaInteraction.WatchMovie _ -> "🎬"
    | Interaction.Concert c ->
        match c with
        | ConcertInteraction.AdjustDrums _ -> "🥁"
        | ConcertInteraction.BassSolo _ -> "🎸"
        | ConcertInteraction.DedicateSong _ -> "💬"
        | ConcertInteraction.DoEncore _ -> "🎶"
        | ConcertInteraction.DrumSolo _ -> "🥁"
        | ConcertInteraction.FaceBand _ -> "⤴️"
        | ConcertInteraction.FaceCrowd _ -> "⤵️"
        | ConcertInteraction.FinishConcert _ -> "⏹️"
        | ConcertInteraction.GetOffStage _ -> "🔽"
        | ConcertInteraction.GiveSpeech _ -> "🎙️"
        | ConcertInteraction.GreetAudience _ -> "👋🏻"
        | ConcertInteraction.GuitarSolo _ -> "🎸"
        | ConcertInteraction.MakeCrowdSing _ -> "🎤"
        | ConcertInteraction.PerformSoundCheck _ -> "🔊"
        | ConcertInteraction.PlaySong _ -> "🎶"
        | ConcertInteraction.PutMicOnStand _ -> "🎙️"
        | ConcertInteraction.SetupMerchStand _ -> "👕"
        | ConcertInteraction.SpinDrumsticks _ -> "🥁"
        | ConcertInteraction.StartConcert _ -> "🎤"
        | ConcertInteraction.TakeMic _ -> "🎙️"
        | ConcertInteraction.TuneInstrument _ -> "📻"
    | Interaction.FreeRoam f ->
        match f with
        | FreeRoamInteraction.Clock _ -> "🕐"
        | FreeRoamInteraction.Enter _ -> "🚪"
        | FreeRoamInteraction.GoOut _ -> "🚪"
        | FreeRoamInteraction.GoToStreet _ -> "🗺️"
        | FreeRoamInteraction.Inventory _ -> "🎒"
        | FreeRoamInteraction.Look _ -> "👁️"
        | FreeRoamInteraction.Map -> "🗺️"
        | FreeRoamInteraction.Move _ -> "🚶"
        | FreeRoamInteraction.Phone -> "📱"
        | FreeRoamInteraction.Wait -> "⏸️"
    | Interaction.Gym g ->
        match g with
        | GymInteraction.PayEntrance _ -> "🏋️"
    | Interaction.Item i ->
        match i with
        | ItemInteraction.Cook _ -> "🍳"
        | ItemInteraction.Drink -> "🥃"
        | ItemInteraction.Eat -> "🍽️"
        | ItemInteraction.Exercise -> "🏃"
        | ItemInteraction.Open -> "📦"
        | ItemInteraction.Put -> "📥"
        | ItemInteraction.Play -> "▶️"
        | ItemInteraction.Read -> "📖"
        | ItemInteraction.Ride _ -> "🚗"
        | ItemInteraction.Sleep -> "💤"
        | ItemInteraction.Watch -> "📺"
    | Interaction.MerchandiseWorkshop m ->
        match m with
        | MerchandiseWorkshopInteraction.ListOrderedMerchandise _ -> "📋"
        | MerchandiseWorkshopInteraction.OrderMerchandise _ -> "👕"
        | MerchandiseWorkshopInteraction.PickUpMerchandise _ -> "📦"
    | Interaction.MiniGame _ -> "🎮"
    | Interaction.Rehearsal r ->
        match r with
        | RehearsalInteraction.BandInventory _ -> "🎒"
        | RehearsalInteraction.ComposeNewSong -> "🎸"
        | RehearsalInteraction.DiscardSong _ -> "❌"
        | RehearsalInteraction.FinishSong _ -> "✅"
        | RehearsalInteraction.FireMember _ -> "🔥"
        | RehearsalInteraction.HireMember -> "🔎"
        | RehearsalInteraction.ImproveSong _ -> "✳️"
        | RehearsalInteraction.ListMembers _ -> "📑"
        | RehearsalInteraction.ListSongs _ -> "📃"
        | RehearsalInteraction.PracticeSong _ -> "❇️"
        | RehearsalInteraction.SwitchGenre _ -> "🎸"
    | Interaction.Shop s ->
        match s with
        | ShopInteraction.Buy _ -> "🛍️"
        | ShopInteraction.BuyCar _ -> "🚗"
        | ShopInteraction.Order _ -> "💬"
        | ShopInteraction.SeeMenu _ -> "🍽️"
    | Interaction.Social _ -> "💬"
    | Interaction.Studio s ->
        match s with
        | StudioInteraction.AddSongToAlbum _ -> "🎵"
        | StudioInteraction.CreateAlbum _ -> "💿"
        | StudioInteraction.EditAlbumName _ -> "✏️"
        | StudioInteraction.ListUnreleasedAlbums _ -> "📋"
        | StudioInteraction.ReleaseAlbum _ -> "✅"
    | Interaction.Travel t ->
        match t with
        | TravelInteraction.Drive _ -> "🚗"
        | TravelInteraction.LeaveVehicle -> "🚶"
        | TravelInteraction.TravelByMetroTo _ -> "🚇"
        | TravelInteraction.WaitForMetro -> "⏳"
