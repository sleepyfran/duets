module rec Duets.UI.Text.World.Interactions

open Duets.Entities

let get _state (interactionWithState: InteractionWithMetadata) =
    match interactionWithState.Interaction with
    | Interaction.Concert concertInteraction ->
        match concertInteraction with
        | ConcertInteraction.AdjustDrums _ -> "🥁 Adjust drums"
        | ConcertInteraction.BassSolo _ -> "🎸 Bass solo"
        | ConcertInteraction.DedicateSong _ -> "💬 Dedicate a song"
        | ConcertInteraction.DoEncore _ -> "🎶 Perform encore"
        | ConcertInteraction.DrumSolo _ -> "🥁 Drum solo"
        | ConcertInteraction.FinishConcert _ -> "⏹️ End concert"
        | ConcertInteraction.GetOffStage _ -> "🔽 Get off stage"
        | ConcertInteraction.GiveSpeech _ -> "🎙️ Give speech"
        | ConcertInteraction.GreetAudience _ -> "👋🏻 Greet audience"
        | ConcertInteraction.GuitarSolo _ -> "🎸 Guitar solo"
        | ConcertInteraction.FaceBand _ -> "⤴️ Face band"
        | ConcertInteraction.FaceCrowd _ -> "⤵️ Face crowd"
        | ConcertInteraction.MakeCrowdSing _ -> "🎤 Make crowd sing"
        | ConcertInteraction.PlaySong _ -> "🎶 Play song"
        | ConcertInteraction.PutMicOnStand _ -> "🎙️ Put mic on stand"
        | ConcertInteraction.SpinDrumsticks _ -> "🥁 Spin drumsticks"
        | ConcertInteraction.TakeMic _ -> "🎙️ Take mic"
        | ConcertInteraction.TuneInstrument _ -> "📻 Tune instrument"
        | ConcertInteraction.PerformSoundCheck _ -> "🔊 Sound check"
        | ConcertInteraction.SetupMerchStand _ -> "👕 Setup merch stand"
        | ConcertInteraction.StartConcert _ -> "🎤 Start concert"
    | Interaction.Item itemInteraction ->
        match itemInteraction with
        | ItemInteraction.Drink -> "🥃 Drink"
        | ItemInteraction.Eat -> "🍽️ Eat"
        | ItemInteraction.Sleep -> "💤 Sleep"
        | ItemInteraction.Play -> "▶️ Play"
        | ItemInteraction.Watch -> "📺 Watch"
        | ItemInteraction.Cook _ -> "🍳 Cook"
        | ItemInteraction.Exercise -> "🏃 Exercise"
        | ItemInteraction.Open -> "📦 Open"
        | ItemInteraction.Put -> "📥 Put away"
        | ItemInteraction.Read -> "📖 Read"
        | ItemInteraction.Ride _ -> "🚗 Ride"
    | Interaction.FreeRoam freeRoamInteraction ->
        match freeRoamInteraction with
        | FreeRoamInteraction.Wait -> "⏸️ Wait"
        | _ -> "Not supported"
    | Interaction.Rehearsal rehearsalInteraction ->
        match rehearsalInteraction with
        | RehearsalInteraction.ComposeNewSong -> "🎸 Compose new song"
        | RehearsalInteraction.DiscardSong _ -> "❌ Discard song"
        | RehearsalInteraction.FinishSong _ -> "✅ Finish song"
        | RehearsalInteraction.FireMember _ -> "🔥 Fire member"
        | RehearsalInteraction.HireMember -> "🔎 Hire member"
        | RehearsalInteraction.ImproveSong _ -> "✳️ Improve song"
        | RehearsalInteraction.ListMembers _ -> "📑 List members"
        | RehearsalInteraction.ListSongs _ -> "📃 List songs"
        | RehearsalInteraction.PracticeSong _ -> "❇️ Practice song"
        | RehearsalInteraction.BandInventory _ -> "🎒 Band inventory"
        | RehearsalInteraction.SwitchGenre _ -> "🎸 Switch genre"
    | Interaction.Shop shopInteraction ->
        match shopInteraction with
        | ShopInteraction.Order _ -> "💬 Order"
        | ShopInteraction.SeeMenu _ -> "🍽️ See menu"
        | ShopInteraction.Buy _ -> "🛍️ Buy"
        | ShopInteraction.BuyCar _ -> "🚗 Buy car"
    | Interaction.Studio studioInteraction ->
        match studioInteraction with
        | StudioInteraction.CreateAlbum _ -> "💿 Create album"
        | StudioInteraction.EditAlbumName _ -> "✏️ Edit album name"
        | StudioInteraction.ReleaseAlbum _ -> "✅ Release album"
        | StudioInteraction.AddSongToAlbum _ -> "🎵 Add song to album"
        | StudioInteraction.ListUnreleasedAlbums _ -> "📋 List albums"
    | Interaction.Airport airportInteraction ->
        match airportInteraction with
        | AirportInteraction.BoardAirplane _ -> "✈️ Board plane"
        | AirportInteraction.PassSecurity -> "🔒 Pass security"
        | AirportInteraction.WaitUntilLanding _ -> "⏳ Wait for landing"
    | Interaction.Career careerInteraction ->
        match careerInteraction with
        | CareerInteraction.Work _ -> "💼 Work"
    | Interaction.Cinema cinemaInteraction ->
        match cinemaInteraction with
        | CinemaInteraction.WatchMovie _ -> "🎬 Watch movie"
        | CinemaInteraction.BuyTicket _ -> "🎟️ Buy ticket"
    | Interaction.Gym gymInteraction ->
        match gymInteraction with
        | GymInteraction.PayEntrance _ -> "🏋️ Pay entrance"
    | Interaction.MiniGame _ -> "🎮 Play mini game"
    | Interaction.MerchandiseWorkshop _ -> "👕 Merchandise"
    | Interaction.Social _ -> "💬 Talk"
    | Interaction.Travel _ -> "🚗 Drive"
