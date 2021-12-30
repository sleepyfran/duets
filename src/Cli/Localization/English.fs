module Cli.Localization.English

open Common
open Cli.View
open Cli.View.Text
open Entities
open System

let verbConjugationByGender =
    dict [ (Have,
            dict [ (Male, "Has")
                   (Female, "Has")
                   (Other, "Have") ]) ]

/// Transforms TextConstants into strings.
let rec toString text =
    match text with
    | Text constant -> fromConstant constant
    | Literal string -> string

/// Returns the formatted instrument name given its type.
and instrumentName instrumentType =
    match instrumentType with
    | Bass -> "Bass"
    | Drums -> "Drums"
    | Guitar -> "Guitar"
    | Vocals -> "Microphone"

/// Returns the formatted skill name given its ID.
and skillName id =
    match id with
    | Composition -> "Composition"
    | Genre genre -> $"{genre} (Genre)"
    | SkillId.Instrument instrument ->
        $"{instrumentName instrument} (Instrument)"
    | MusicProduction -> "Music production"

/// Returns the command associated with the given direction.
and directionCommand direction =
    match direction with
    | North -> "n"
    | NorthEast -> "ne"
    | East -> "e"
    | SouthEast -> "se"
    | South -> "s"
    | SouthWest -> "sw"
    | West -> "w"
    | NorthWest -> "nw"

/// Returns the name of the given direction.
and directionName direction =
    match direction with
    | North -> "north"
    | NorthEast -> "north-east"
    | East -> "east"
    | SouthEast -> "south-east"
    | South -> "south"
    | SouthWest -> "south-west"
    | West -> "west"
    | NorthWest -> "north-west"

/// Returns the correct pronoun for the given gender (he, she, they).
and subjectPronounForGender gender =
    match gender with
    | Male -> "He"
    | Female -> "She"
    | Other -> "They"

/// Returns the correct object pronoun for the given gender (him, her, them).
and objectPronounForGender gender =
    match gender with
    | Male -> "Him"
    | Female -> "Her"
    | Other -> "Them"

/// Returns the correct possessive for the given gender (his, her, its).
and possessiveAdjectiveForGender gender =
    match gender with
    | Male -> "His"
    | Female -> "Her"
    | Other -> "Its"

/// Returns the correct conjugation for the given verb that matches with the
/// specified gender.
and verbConjugationForGender verb gender =
    verbConjugationByGender.[verb].[gender]

/// Returns the correct name of the given account holder.
and accountHolderName holder =
    match holder with
    | Character _ -> "your character"
    | Band _ -> "your band"

/// Returns the name of the given moment of the day.
and dayMomentName dayMoment =
    match dayMoment with
    | Dawn -> "🌞 Dawn"
    | Morning -> "🌞 Morning"
    | Midday -> "🌞 Midday"
    | Sunset -> "🌝 Sunset"
    | Dusk -> "🌝 Dusk"
    | Night -> "🌝 Night"
    | Midnight -> "🌝 Midnight"

/// Returns the formatted name for an album type.
and albumType t =
    match t with
    | Single -> "Single"
    | EP -> "EP"
    | LP -> "LP"

/// Returns the name of the given object type.
and objectName object =
    match object with
    | ObjectType.Instrument instrument -> instrumentName instrument

/// Returns a formatted list as empty if it contains nothing, "a" if it contains
/// only one element, "a and b" with two elements and "a, b and c" for all other
/// lengths.
and listOf (stuff: 'a list) toStr =
    match stuff with
    | [] -> ""
    | [ head ] -> toStr head
    | [ head; tail ] -> $"%s{toStr head} and %s{toStr tail}"
    | head :: tail -> $"%s{toStr head}, %s{listOf tail toStr}"

/// Formats a number with the thousands specifier.
and formatNumber (amount: 'a) = String.Format("{0:#,0}", amount)

and bankText key =
    match key with
    | BankTitle -> "Bank"
    | BankWelcome (characterBalance, bandBalance) ->
        $"""{TextStyles.highlight "You"} currently have {TextStyles.money characterBalance}. {TextStyles.highlight "Your band"} has {TextStyles.money bandBalance}"""
    | BankPrompt -> "What do you want to do?"
    | BankTransferToBand -> "Transfer money to band"
    | BankTransferFromBand -> "Transfer money from band"
    | BankTransferAmount holder ->
        match holder with
        | Character _ -> "How much do you want to transfer to your band?"
        | Band _ -> "How much do you want to transfer from your band?"
    | BankTransferSuccess (holder, transaction) ->
        match transaction with
        | Incoming (amount, _) ->
            $"Transferred {TextStyles.money amount} to {accountHolderName holder}'s account"
        | Outgoing (amount, _) ->
            $"Transferred {TextStyles.money amount} from {accountHolderName holder}'s account"
    | BankTransferNotEnoughFunds ->
        TextStyles.error "Not enough funds in the sender account"

and commandText key =
    match key with
    | CommandCommonPrompt ->
        TextStyles.prompt "What do you want to do? Type 'help' if you're lost"
    | CommandHelpDescription ->
        "Here are all the commands you can execute right now"
    | CommandHelpEntry (entryName, entryDescription) ->
        $"{TextStyles.action entryName} — {toString entryDescription}"
    | CommandDirectionDescription direction ->
        $"Follows the {directionName direction} direction"
    | CommandLookDescription -> "Shows all the objects you have around you"
    | CommandLookEntranceDescription entrances ->
        $"""You can enter {listOf
                               entrances
                               (fun (direction, name) ->
                                   $"{TextStyles.place (toString name)} through the ({TextStyles.information (directionCommand direction)})")}"""
    | CommandLookNoObjectsAround -> "There are no objects around you"
    | CommandLookVisibleObjectsPrefix -> "You can see:"
    | CommandLookObjectEntry (objectType, commandNames) ->
        $"- {objectName objectType |> TextStyles.object}, you can interact with it by calling {listOf commandNames id |> TextStyles.action}"
    | CommandExitDescription -> "Exits the game saving the progress"
    | CommandMapDescription ->
        "Shows the map of the game where you can quickly travel to other places"
    | CommandPhoneDescription ->
        "Opens your phone where you can check statistics and manage your bank"
    | CommandTalkDescription ->
        $"""Allows you to talk with a character in the world. Use as {TextStyles.information "talk to {name}"}. You can reference characters by their full name or just their first name"""
    | CommandTalkInvalidInput ->
        TextStyles.error
            $"""I didn't quite catch that. Make sure you are referencing characters by their first or full name with {TextStyles.information "talk to {name}"}"""
    | CommandTalkNpcNotFound name ->
        TextStyles.error $"There are no characters named {name} around"
    | CommandTalkNothing -> "Nothing"

and commonText key =
    match key with
    | GameName -> "Duets"
    | CommonYouAreIn place -> $"You're currently in {place}"
    | CommonChoiceSelection selection ->
        $"""{TextStyles.prompt "You selected"} {TextStyles.object selection}"""
    | CommonMultiChoiceMoreChoices ->
        TextStyles.faded "(Move up and down to reveal more choices)"
    | CommonMultiChoiceInstructions ->
        $"""Press {TextStyles.information "space"} to select a choice and {TextStyles.information "enter"} to finish the selection"""
    | CommonNoUnfinishedSongs ->
        TextStyles.error "You don't have any songs, create one first"
    | CommonSkills -> "Skills"
    | CommonBack -> TextStyles.faded "Go back"
    | CommonCancel -> TextStyles.faded "Cancel"
    | CommonBackToMainMenu -> TextStyles.faded "Back to main menu"
    | CommonBackToMap -> TextStyles.faded "Back to map"
    | CommonBackToPhone -> TextStyles.faded "Back to phone"
    | CommonBackToWorld -> TextStyles.faded "Back to world"
    | CommonSkillImproved (characterName,
                           characterGender,
                           skill,
                           previousLevel,
                           currentLevel) ->
        TextStyles.success
            $"""{characterName} improved {(possessiveAdjectiveForGender characterGender)
                                          |> String.lowercase} {skillName skill.Id |> String.lowercase} skill from {previousLevel} to {currentLevel}"""
    | CommonInvalidLength ->
        TextStyles.error
            $"""Couldn't recognize that length. Try the format {TextStyles.information "mm:ss"} as in 6:55 (6 minutes, 55 seconds)"""
    | CommonInvalidCommand ->
        TextStyles.error
            $"""That command was not valid. Maybe try again or enter {TextStyles.information "help"} if you're lost"""

and creatorText key =
    match key with
    | CharacterCreatorInitialPrompt ->
        $"""Creating a new game, what's the {TextStyles.highlight "name"} of your character?"""
    | CharacterCreatorGenderPrompt ->
        $"""What's their {TextStyles.highlight "gender"}?"""
    | CharacterCreatorGenderMale -> "Male"
    | CharacterCreatorGenderFemale -> "Female"
    | CharacterCreatorGenderOther -> "Other"
    | CharacterCreatorAgePrompt ->
        $"""How {TextStyles.highlight "old"} are they? (Minimum 18)"""
    | BandCreatorInitialPrompt ->
        $"""Let's create your first band. What's the {TextStyles.highlight "band's name"}"""
    | BandCreatorGenrePrompt ->
        $"""What {TextStyles.highlight "genre"} are they going to be playing? You can always change this later"""
    | BandCreatorInstrumentPrompt ->
        $"""And lastly, what will you be {TextStyles.highlight "playing"}?"""
    | BandCreatorConfirmationPrompt (characterName,
                                     bandName,
                                     bandGenre,
                                     instrument) ->
        $"""You'll be playing as {TextStyles.highlight characterName} in the band {TextStyles.highlight bandName} playing {TextStyles.highlight bandGenre} as a {TextStyles.highlight instrument}"""
    | CreatorErrorCharacterNameTooShort ->
        TextStyles.error "Your character's name is too short"
    | CreatorErrorCharacterNameTooLong ->
        TextStyles.error "Your character's name is too long"
    | CreatorErrorCharacterAgeTooYoung ->
        TextStyles.error "Your character is too young"
    | CreatorErrorCharacterAgeTooOld ->
        TextStyles.error "Your character is too old"
    | CreatorErrorBandNameTooShort ->
        TextStyles.error "Your band's name is too short"
    | CreatorErrorBandNameTooLong ->
        TextStyles.error "Your band's name is too long"

and mainMenuText key =
    match key with
    | MainMenuIncompatibleSavegame ->
        TextStyles.error
            "Your savegame is incompatible or malformed and was ignored"
    | MainMenuPrompt -> "Select an option to begin"
    | MainMenuNewGame -> "New game"
    | MainMenuLoadGame -> "Load game"
    | MainMenuExit -> TextStyles.faded "Exit"
    | MainMenuSavegameNotAvailable ->
        TextStyles.error "No savegame available. Create a new game"

and phoneText key =
    match key with
    | PhoneTitle -> "Phone"
    | PhoneOptionBank -> "Bank App"
    | PhoneOptionStatistics -> "Statistics App"
    | PhonePrompt -> $"""{TextStyles.prompt "DuetsPhone v1.0"}"""

and rehearsalText key =
    match key with
    | RehearsalSpaceLobbyName -> "Lobby"
    | RehearsalSpaceBarName -> "Bar"
    | RehearsalSpaceRehearsalRoomName -> "Rehearsal rooms"
    | RehearsalSpaceLobbyDescription ->
        $"""You are in the {TextStyles.place "lobby"} of the rehearsal room. Not much to do here, just enter already!"""
    | RehearsalSpaceBarDescription ->
        $"""The {TextStyles.place "rehearsal room's bar"} smells really weird. There's three people sitting and drinking beer."""
    | RehearsalSpaceRehearsalRoomDescription ->
        $"""You are in the {TextStyles.place "rehearsal room"} inside an old and quite smelly building. You can feel the smoke in the air and hear {TextStyles.band "AC/DC"} being played in the room nearby."""
    | RehearsalRoomManageDescription ->
        "Opens the band management menu which allows you to hire new members or fire current ones"
    | RehearsalRoomStatistics -> "Statistics"
    | RehearsalRoomInstrumentPlayDescription ->
        "Starts the rehearsal, which allows the band to compose new songs, finish previously started songs and practice finished songs."
    | ComposePrompt -> "What do you want to compose?"
    | ComposeSong -> "Compose new song"
    | ComposeSongTitlePrompt ->
        $"""Creating a new song, how are you going to {TextStyles.highlight "name"} it?"""
    | ComposeSongLengthPrompt ->
        $"""How {TextStyles.highlight "long"} is it going to be? (format {TextStyles.information "minutes:seconds"})"""
    | ComposeSongGenrePrompt ->
        $"""What {TextStyles.highlight "genre"} will the song have? Keep in mind that selecting a different genre that the main one of your band might reduce quality"""
    | ComposeSongVocalStylePrompt ->
        $"""What {TextStyles.highlight "vocal style"} should it have?"""
    | ComposeSongConfirmation title ->
        TextStyles.success
            $"""Your band has started working on the song "{title}". You can finish or improve it through the compose section in the rehearsal room"""
    | ComposeSongErrorNameTooShort ->
        TextStyles.error "The name of the song is too short"
    | ComposeSongErrorNameTooLong ->
        TextStyles.error "The name of the song is too long"
    | ComposeSongErrorLengthTooShort ->
        TextStyles.error "Songs can't be less than 20 seconds long"
    | ComposeSongErrorLengthTooLong ->
        TextStyles.error "Songs can't be more than 30 minutes long"
    | ComposeSongProgressBrainstorming -> TextStyles.progress "Brainstorming..."
    | ComposeSongProgressConfiguringReverb ->
        TextStyles.progress "Configuring reverb..."
    | ComposeSongProgressTryingChords ->
        TextStyles.progress "Trying out some chords..."
    | ImproveSong -> "Improve an unfinished song"
    | ImproveSongSelection -> "Which song do you want to improve?"
    | ImproveSongCanBeFurtherImproved (previousQuality, currentQuality) ->
        TextStyles.success
            $"You've improved the song. It improved from {previousQuality} to {currentQuality}"
    | ImproveSongReachedMaxQuality ->
        TextStyles.success
            "Your band has decided that the song does not need any further improvements. You can add it to the band's repertoire from the 'Finish an unfinished song' option"
    | ImproveSongProgressAddingSomeMelodies ->
        TextStyles.progress "Adding some melodies..."
    | ImproveSongProgressPlayingFoosball ->
        TextStyles.progress "Playing foosball..."
    | ImproveSongProgressModifyingChordsFromAnotherSong ->
        TextStyles.progress
            $"""{TextStyles.crossed "Copying"} Modifying chords from another song"""
    | FinishSong -> "Finish an unfinished song"
    | FinishSongSelection ->
        $"""Which song do you want to finish? {TextStyles.danger "You won't be able to improve the song after this"}"""
    | FinishSongFinished (name, quality) ->
        TextStyles.success
            $"""Your band finished the song "{name}". The result quality is {quality}"""
    | DiscardSong -> "Discard an unfinished song"
    | DiscardSongSelection ->
        TextStyles.danger "Which song do you want to discard?"
    | DiscardSongDiscarded name ->
        TextStyles.error $"Your band decided to stop working on {name}"
    | PracticeSong -> "Practice a finished song"
    | HireMemberRolePrompt -> "What role are you looking to hire?"
    | HireMemberSkillSummary (name, gender) ->
        $"{TextStyles.highlight name} is interested in joining your band. {subjectPronounForGender gender} {verbConjugationForGender Have gender
                                                                                                            |> String.lowercase} the following skills:"
    | HireMemberSkillLine (id, level) ->
        $"{TextStyles.highlight (skillName id)} - {TextStyles.level level}"
    | HireMemberConfirmation gender ->
        $"Do you want to hire {objectPronounForGender gender}?"
    | HireMemberHired -> TextStyles.success "You just hired a new member!"
    | HireMemberContinueConfirmation ->
        "Do you want to continue looking for members?"
    | FireMemberListItem (name, role) -> $"{name} ({instrumentName role})"
    | FireMemberNoMembersToFire ->
        TextStyles.error
            "You are the only member of the band, you can't fire yourself!"
    | FireMemberPrompt -> $"""Who do you want to {TextStyles.danger "fire"}?"""
    | FireMemberConfirmation name ->
        TextStyles.danger $"Are you sure you want to fire {name}?"
    | FireMemberConfirmed name -> TextStyles.danger $"You fired {name}"
    | MemberListCurrentTitle -> TextStyles.title "Current members"
    | MemberListCurrentMember (name, role, since) ->
        $"{name} ({instrumentName role}) since {since.Year}"
    | MemberListPastTitle -> TextStyles.title "Past members"
    | MemberListPastMember (name, role, since, until) ->
        $"{name} ({instrumentName role}) from {since.Year} until {until.Year}"
    | ManagementTitle -> "Management"
    | ManagementPrompt -> "What do you want to to?"
    | ManagementHireMember -> "Hire a new member"
    | ManagementFireMember -> "Fire a member"
    | ManagementMemberList -> "List members"

and statisticsText key =
    match key with
    | StatisticsTitle -> "Statistics"
    | StatisticsSectionPrompt ->
        $"""{TextStyles.prompt "What data do you want to visualize?"}"""
    | StatisticsSectionBand -> "Band's statistics"
    | StatisticsSectionAlbums -> "Albums' statistics"
    | StatisticsBandName name -> TextStyles.title name
    | StatisticsBandStartDate date ->
        $"Playing since {TextStyles.highlight date.Year}"
    | StatisticsBandFame fame -> $"Fame: {TextStyles.level fame}"
    | StatisticsAlbumNoEntries -> "No albums released yet"
    | StatisticsAlbumName (name, albumT) ->
        TextStyles.information $"{name} ({TextStyles.faded (albumType albumT)})"
    | StatisticsAlbumReleaseDate date ->
        TextStyles.highlight "Released on "
        + TextStyles.highlight $"{date.Day}/{date.Month}/{date.Year}"
    | StatisticsAlbumStreams streams ->
        $"Streams so far: {TextStyles.highlight (formatNumber streams)}"
    | StatisticsAlbumRevenue revenue ->
        $"Generated revenue: {TextStyles.money revenue}"

and studioText key =
    match key with
    | StudioCommonAlbumReleased name ->
        TextStyles.success $"Your band just released {name}!"
    | StudioCommonPromptReleaseAlbum name ->
        $"""Do you want to release {TextStyles.highlight name}?"""
    | StudioMasteringRoomName -> "Mastering room"
    | StudioMasteringRoomDescription studio ->
        $"""You are in the mastering room, where the producer, {TextStyles.person studio.Producer.Name} sits in front of a computer and a bunch of knobs."""
    | StudioRecordingRoomName -> "Recording room"
    | StudioRecordingRoomDescription ->
        "A recording room with all the instruments you can imagine, although for now the only one that matters is the one that you can play."
    | StudioTalkIntroduction (studioName, fee) ->
        $"""Welcome to {TextStyles.place studioName}! Are you ready to record some stuff? All I ask is for {TextStyles.money fee} per song to record and master it. What do you say?"""
    | StudioTalkCreateRecord -> "Let's record a new album!"
    | StudioTalkContinueRecord ->
        "Let me continue with an album I previously recorded"
    | StudioCreateNoSongs ->
        TextStyles.error
            "You don't have any finished song to record. Create some songs first and finish them in the rehearsal room"
    | StudioCreateRecordName ->
        $"""What's going to be the {TextStyles.highlight "name"} of the record?"""
    | StudioCreateTrackListPrompt ->
        $"""Select what {TextStyles.highlight "songs"} will be on the {TextStyles.highlight "track-list"}. You can select multiple. The order in which you select them will be the order in which they'll appear in the album"""
    | StudioConfirmRecordingPrompt (name, albumRecordType) ->
        $"""Are you sure you want to record {TextStyles.album name}? Given its track-list it will be released as a {TextStyles.information (albumType albumRecordType)}"""
    | StudioCreateErrorNameTooShort ->
        TextStyles.error "The name of the album is too short"
    | StudioCreateErrorNameTooLong ->
        TextStyles.error "The name of the album is too long"
    | StudioCreateErrorNotEnoughMoney studioBill ->
        TextStyles.error
            $"""Your band doesn't have enough money to pay the studio fee. The studio is asking for {TextStyles.money studioBill}, but you don't have enough money on the band's account. Check the Bank app on your phone to transfer money to your band's account"""
    | StudioCreateAlbumRecorded albumName ->
        TextStyles.success $"Your band just finished recording {albumName}!"
    | StudioCreateProgressEatingSnacks ->
        TextStyles.progress "Eating some snacks"
    | StudioCreateProgressRecordingWeirdSounds ->
        TextStyles.progress "Recording weird sounds"
    | StudioCreateProgressMovingKnobs ->
        TextStyles.progress "Moving knobs up and down"
    | StudioContinueRecordPrompt ->
        "Which record do you want to continue working on?"
    | StudioContinueRecordActionPrompt ->
        "What do you want to do with this album?"
    | StudioContinueRecordActionPromptEditName -> "Edit name"
    | StudioContinueRecordActionPromptRelease -> "Release"
    | StudioContinueRecordAlbumRenamed albumName ->
        TextStyles.success $"""The album was renamed to "{albumName}"""

and worldText key =
    match key with
    | WorldTitle -> "World"
    | StreetBoringDescription name ->
        $"{TextStyles.place name} is just a boring old street with not much to do."

and fromConstant textNamespace =
    match textNamespace with
    | BankText key -> bankText key
    | CommandText key -> commandText key
    | CommonText key -> commonText key
    | CreatorText key -> creatorText key
    | MainMenuText key -> mainMenuText key
    | PhoneText key -> phoneText key
    | RehearsalSpaceText key -> rehearsalText key
    | StatisticsText key -> statisticsText key
    | StudioText key -> studioText key
    | WorldText key -> worldText key
