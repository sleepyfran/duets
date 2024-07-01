[<RequireQualifiedAccess>]
module Duets.Cli.Text.Studio

let commonAlbumReleased name =
    Styles.success $"Your band just released {name}!"

let commonPromptReleaseAlbum name albumType =
    $"""Are you sure you want to release {Styles.highlight name}? Given its track-list it will be released as a {Generic.albumType albumType |> Styles.information}"""

let createNoSongs =
    Styles.error
        "You don't have any finished song to record. Create some songs first and finish them in the rehearsal room"

let createRecordName =
    $"""What's going to be the {Styles.highlight "name"} of the record?"""

let createTrackListPrompt =
    $"""Select which {Styles.highlight "song"} will be on the {Styles.highlight "track-list"} first"""

let producerPrompt =
    $"""Who will be in charge of {Styles.highlight "producing, mixing and mastering"} the record?"""

let producerPlayableCharacterSelection skillLevel =
    $"""You (Music production skill: {Styles.Level.from skillLevel}, {Styles.success "free"})"""

let producerStudioProducerSelection pricePerSong studioQuality =
    $"""Studio's producer (Skill level: {Styles.Level.from studioQuality}, {Styles.money pricePerSong} extra per song)"""

let confirmRecordingPrompt name =
    $"""Are you sure you want to record {Styles.song name}? It will be the first song in the album, and it can't be changed"""

let createErrorNameTooShort = Styles.error "The name of the album is too short"

let createErrorNameTooLong = Styles.error "The name of the album is too long"

let createErrorNotEnoughMoney studioBill =
    Styles.error
        $"""Your band doesn't have enough money to pay the studio fee. The studio is asking for {Styles.money studioBill}, but you don't have enough money on the band's account. Check the Bank app on your phone to transfer money to your band's account"""

let createAlbumRecorded albumName =
    Styles.success
        $"Your band started recording {albumName}! You can continue it or release it from the studio"

let createProgressEatingSnacks = Styles.progress "Eating some snacks"

let createProgressRecordingWeirdSounds =
    Styles.progress "Recording weird sounds"

let createProgressMovingKnobs = Styles.progress "Moving knobs up and down"

let continueRecordPrompt = "Which record do you want to continue working on?"

let continueRecordActionPrompt = "What do you want to do with this album?"

let continueRecordActionPromptEditName = "Edit name"

let continueRecordActionPromptRelease = "Release"
