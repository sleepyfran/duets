module UI.Text.World.Interactions

open Entities

let get interactionWithState =
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
    | Interaction.Item itemInteraction ->
        match itemInteraction with
        | ItemInteraction.Consumable ConsumableItemInteraction.Drink ->
            "🥃 Drink"
        | ItemInteraction.Consumable ConsumableItemInteraction.Eat -> "🍽️ Eat"
        | ItemInteraction.Interactive InteractiveItemInteraction.Sleep ->
            "💤 Sleep"
        | ItemInteraction.Interactive InteractiveItemInteraction.Play ->
            "▶️ Play"
        | ItemInteraction.Interactive InteractiveItemInteraction.Watch ->
            "📺 Watch"
    | Interaction.FreeRoam freeRoamInteraction ->
        match freeRoamInteraction with
        | FreeRoamInteraction.GoOut _ -> "🔙 Go out"
        | FreeRoamInteraction.Look _ -> "👀 Look around"
        | FreeRoamInteraction.Move (direction, _) ->
            $"🚶Move to {Directions.directionName direction}"
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
        | RehearsalInteraction.PracticeSong _ -> "❇️ Practice song"
    | Interaction.Bar shopInteraction ->
        match shopInteraction with
        | BarInteraction.Order _ -> "💬 Order"
        | BarInteraction.SeeMenu _ -> "🍽️ See menu"
    | Interaction.Studio studioInteraction ->
        match studioInteraction with
        | StudioInteraction.CreateAlbum _ -> "💿 Create album"
        | StudioInteraction.EditAlbumName _ -> "✏️ Edit album name"
        | StudioInteraction.ReleaseAlbum _ -> "✅ Release album"
