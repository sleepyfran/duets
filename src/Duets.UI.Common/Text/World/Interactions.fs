module Duets.UI.Common.Text.World.Interactions

open Duets.Entities

let label (item: InteractionWithMetadata) : string =
    match item.Interaction with
    | Interaction.Airport a ->
        match a with
        | AirportInteraction.BoardAirplane _ -> "Board plane"
        | AirportInteraction.PassSecurity -> "Pass security"
        | AirportInteraction.WaitUntilLanding _ -> "Wait for landing"
    | Interaction.Career c ->
        match c with
        | CareerInteraction.Work _ -> "Work"
    | Interaction.Cinema c ->
        match c with
        | CinemaInteraction.BuyTicket _ -> "Buy ticket"
        | CinemaInteraction.WatchMovie _ -> "Watch movie"
    | Interaction.Concert c ->
        match c with
        | ConcertInteraction.AdjustDrums _ -> "Adjust drums"
        | ConcertInteraction.BassSolo _ -> "Bass solo"
        | ConcertInteraction.DedicateSong _ -> "Dedicate a song"
        | ConcertInteraction.DoEncore _ -> "Perform encore"
        | ConcertInteraction.DrumSolo _ -> "Drum solo"
        | ConcertInteraction.FaceBand _ -> "Face band"
        | ConcertInteraction.FaceCrowd _ -> "Face crowd"
        | ConcertInteraction.FinishConcert _ -> "End concert"
        | ConcertInteraction.GetOffStage _ -> "Get off stage"
        | ConcertInteraction.GiveSpeech _ -> "Give speech"
        | ConcertInteraction.GreetAudience _ -> "Greet audience"
        | ConcertInteraction.GuitarSolo _ -> "Guitar solo"
        | ConcertInteraction.MakeCrowdSing _ -> "Make crowd sing"
        | ConcertInteraction.PerformSoundCheck _ -> "Sound check"
        | ConcertInteraction.PlaySong _ -> "Play song"
        | ConcertInteraction.PutMicOnStand _ -> "Put mic on stand"
        | ConcertInteraction.SetupMerchStand _ -> "Setup merch stand"
        | ConcertInteraction.SpinDrumsticks _ -> "Spin drumsticks"
        | ConcertInteraction.StartConcert _ -> "Start concert"
        | ConcertInteraction.TakeMic _ -> "Take mic"
        | ConcertInteraction.TuneInstrument _ -> "Tune instrument"
    | Interaction.FreeRoam f ->
        match f with
        | FreeRoamInteraction.Clock _ -> "Clock"
        | FreeRoamInteraction.Enter _ -> "Enter"
        | FreeRoamInteraction.GoOut _ -> "Go out"
        | FreeRoamInteraction.GoToStreet _ -> "Go to street"
        | FreeRoamInteraction.Inventory _ -> "Inventory"
        | FreeRoamInteraction.Look _ -> "Look"
        | FreeRoamInteraction.Map -> "Map"
        | FreeRoamInteraction.Move _ -> "Move"
        | FreeRoamInteraction.Phone -> "Phone"
        | FreeRoamInteraction.Wait -> "Wait"
    | Interaction.Gym g ->
        match g with
        | GymInteraction.PayEntrance _ -> "Pay entrance"
    | Interaction.Item i ->
        match i with
        | ItemInteraction.Cook _ -> "Cook"
        | ItemInteraction.Drink -> "Drink"
        | ItemInteraction.Eat -> "Eat"
        | ItemInteraction.Exercise -> "Exercise"
        | ItemInteraction.Open -> "Open"
        | ItemInteraction.Put -> "Put away"
        | ItemInteraction.Play -> "Play"
        | ItemInteraction.Read -> "Read"
        | ItemInteraction.Ride _ -> "Ride"
        | ItemInteraction.Sleep -> "Sleep"
        | ItemInteraction.Watch -> "Watch"
    | Interaction.MerchandiseWorkshop m ->
        match m with
        | MerchandiseWorkshopInteraction.ListOrderedMerchandise _ -> "List orders"
        | MerchandiseWorkshopInteraction.OrderMerchandise _ -> "Order merchandise"
        | MerchandiseWorkshopInteraction.PickUpMerchandise _ -> "Pick up merchandise"
    | Interaction.MiniGame _ -> "Play mini game"
    | Interaction.Rehearsal r ->
        match r with
        | RehearsalInteraction.BandInventory _ -> "Band inventory"
        | RehearsalInteraction.ComposeNewSong -> "Compose new song"
        | RehearsalInteraction.DiscardSong _ -> "Discard song"
        | RehearsalInteraction.FinishSong _ -> "Finish song"
        | RehearsalInteraction.FireMember _ -> "Fire member"
        | RehearsalInteraction.HireMember -> "Hire member"
        | RehearsalInteraction.ImproveSong _ -> "Improve song"
        | RehearsalInteraction.ListMembers _ -> "List members"
        | RehearsalInteraction.ListSongs _ -> "List songs"
        | RehearsalInteraction.PracticeSong _ -> "Practice song"
        | RehearsalInteraction.SwitchGenre _ -> "Switch genre"
    | Interaction.Shop s ->
        match s with
        | ShopInteraction.Buy _ -> "Buy"
        | ShopInteraction.BuyCar _ -> "Buy car"
        | ShopInteraction.Order _ -> "Order"
        | ShopInteraction.SeeMenu _ -> "See menu"
    | Interaction.Social _ -> "Talk"
    | Interaction.Studio s ->
        match s with
        | StudioInteraction.AddSongToAlbum _ -> "Add song to album"
        | StudioInteraction.CreateAlbum _ -> "Create album"
        | StudioInteraction.EditAlbumName _ -> "Edit album name"
        | StudioInteraction.ListUnreleasedAlbums _ -> "List albums"
        | StudioInteraction.ReleaseAlbum _ -> "Release album"
    | Interaction.Travel t ->
        match t with
        | TravelInteraction.Drive _ -> "Drive"
        | TravelInteraction.LeaveVehicle -> "Leave vehicle"
        | TravelInteraction.TravelByMetroTo _ -> "Travel by metro"
        | TravelInteraction.WaitForMetro -> "Wait for metro"
