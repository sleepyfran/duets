module Cli.View.Text

open System
open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities

let verbConjugationByGender =
    dict [ (Have,
            dict [ (Male, "Has")
                   (Female, "Has")
                   (Other, "Have") ]) ]

/// Transforms TextConstants into strings.
let rec toString text =
    match text with
    | TextConstant constant -> fromConstant constant
    | Literal string -> string

/// Returns the formatted instrument name given its type.
and instrumentName instrumentType =
    match instrumentType with
    | Bass -> "Bass"
    | Drums -> "Drums"
    | Guitar -> "Guitar"
    | Vocals -> "Vocals"

/// Returns the formatted skill name given its ID.
and skillName id =
    match id with
    | Composition -> "Composition"
    | Genre genre -> $"{genre} (Genre)"
    | Instrument instrument -> $"{instrumentName instrument} (Instrument)"
    | MusicProduction -> "Music production"

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

and fromConstant constant =
    match constant with
    | GameName -> "Duets"
    | CommonYouAreIn place -> String.Format("You're currently in {0}", place)
    | CommonMultiChoiceMoreChoices ->
        "[grey](Move up and down to reveal more choices)[/]"
    | CommonMultiChoiceInstructions ->
        "Press [bold blue]space[/] to select a choice and [bold blue]enter[/] to finish the selection"
    | CommonNoUnfinishedSongs ->
        "[red]You don't have any songs, create one first[/]"
    | CommonSkills -> "Skills"
    | CommonBack -> "[bold]Go back[/]"
    | CommonCancel -> "[bold]Cancel[/]"
    | CommonBackToMainMenu -> "[bold]Back to main menu[/]"
    | CommonBackToMap -> "[bold]Back to map[/]"
    | CommonPressKeyToContinue -> "Press [bold blue]any[/] key to continue"
    | CommonSkillImproved (characterName,
                           characterGender,
                           skill,
                           previousLevel,
                           currentLevel) ->
        String.Format(
            "[green]{0} improved {1} {2} skill from {3}% to {4}%[/]",
            characterName,
            (possessiveAdjectiveForGender characterGender)
                .ToLower(),
            (skillName skill.Id).ToLower(),
            previousLevel,
            currentLevel
        )
    | CommonStatusBar (date, dayMoment, characterBalance, bandBalance) ->
        sprintf
            "[bold]%i/%i/%i [blue]%s[/] | Character's balance: [green]%i$d[/] | Band's balance: [green]%i$d[/][/]"
            date.Day
            date.Month
            date.Year
            (dayMomentName dayMoment)
            characterBalance
            bandBalance
    | MainMenuIncompatibleSavegame ->
        "[bold red]Your savegame is incompatible or malformed and was ignored[/]"
    | MainMenuPrompt -> "Select an option to begin"
    | MainMenuNewGame -> "New game"
    | MainMenuLoadGame -> "Load game"
    | MainMenuExit -> "[bold]Exit[/]"
    | MainMenuSavegameNotAvailable ->
        "[red]No savegame available. Create a new game[/]"
    | CharacterCreatorInitialPrompt ->
        "Creating a new game, what's the [bold blue]name[/] of your character?"
    | CharacterCreatorGenderPrompt -> "What's their [bold blue]gender[/]?"
    | CharacterCreatorGenderMale -> "Male"
    | CharacterCreatorGenderFemale -> "Female"
    | CharacterCreatorGenderOther -> "Other"
    | CharacterCreatorAgePrompt ->
        "How [bold blue]old[/] are they? (Minimum 18)"
    | BandCreatorInitialPrompt ->
        "Let's create your first band. What's the [bold blue]band's name[/]?"
    | BandCreatorGenrePrompt ->
        "What [bold blue]genre[/] are they going to be playing? You can always change this later"
    | BandCreatorInstrumentPrompt ->
        "And lastly, what will you be [bold blue]playing[/]?"
    | BandCreatorConfirmationPrompt (characterName,
                                     bandName,
                                     bandGenre,
                                     instrument) ->
        String.Format(
            "You'll be playing as [bold blue]{0}[/] in the band [bold blue]{1}[/] playing [bold blue]{2}[/] as a [bold blue]{3}[/]",
            characterName,
            bandName,
            bandGenre,
            instrument
        )
    | CreatorErrorCharacterNameTooShort ->
        "[red]Your character's name is too short[/]"
    | CreatorErrorCharacterNameTooLong ->
        "[red]Your character's name is too long[/]"
    | CreatorErrorCharacterAgeTooYoung -> "[red]Your character is too young[/]"
    | CreatorErrorCharacterAgeTooOld -> "[red]Your character is too old[/]"
    | CreatorErrorBandNameTooShort -> "[red]Your band's name is too short[/]"
    | CreatorErrorBandNameTooLong -> "[red]Your band's name is too long[/]"
    | MapTitle -> "Map"
    | MapPrompt -> "Where are you heading?"
    | MapOptionRehearsalRoom -> "Band's rehearsal room"
    | MapOptionBank -> "Bank"
    | MapOptionStudios -> "Studio"
    | RehearsalRoomTitle -> "Rehearsal Room"
    | RehearsalRoomCompose -> "Compose"
    | RehearsalRoomManage -> "Manage band"
    | RehearsalRoomPrompt -> "What do you want to do today?"
    | ComposePrompt -> "What do you want to compose?"
    | ComposeSong -> "Compose new song"
    | ComposeSongTitlePrompt ->
        "Creating a new song, how are you going to [bold blue]name[/] it?"
    | ComposeSongLengthPrompt ->
        "How [bold blue]long[/] is it going to be? (in seconds)"
    | ComposeSongGenrePrompt ->
        "What [bold blue]genre[/] will the song have? [bold]Keep in mind that selecting a different genre that the main one of your band might reduce quality[/]"
    | ComposeSongVocalStylePrompt ->
        "What [bold blue]vocal style[/] should it have?"
    | ComposeSongConfirmation title ->
        String.Format(
            "[green]Your band has started working on the song \"{0}\"[/]. [blue]You can finish or improve it through the compose section in the rehearsal room[/]",
            title
        )
    | ComposeSongErrorNameTooShort ->
        "[red]The name of the song is too short[/]"
    | ComposeSongErrorNameTooLong -> "[red]The name of the song is too long[/]"
    | ComposeSongErrorLengthTooShort ->
        "[red]Songs can't be less than 20 seconds long[/]"
    | ComposeSongErrorLengthTooLong ->
        "[red]Songs can't be more than 30 minutes long[/]"
    | ComposeSongProgressBrainstorming -> "[deepskyblue3]Brainstorming...[/]"
    | ComposeSongProgressConfiguringReverb ->
        "[deepskyblue3_1]Configuring reverb...[/]"
    | ComposeSongProgressTryingChords ->
        "[dodgerblue1]Trying out some chords...[/]"
    | ImproveSong -> "Improve an unfinished song"
    | ImproveSongSelection -> "Which song do you want to improve?"
    | ImproveSongCanBeFurtherImproved (previousQuality, currentQuality) ->
        String.Format(
            "[green]You've improved the song. It improved from {0}% to {1}%[/]",
            previousQuality,
            currentQuality
        )
    | ImproveSongReachedMaxQuality ->
        "[springgreen4]Your band has decided that the song does not need any further improvements[/]. [blue]You can add it to the band's repertoire from the 'Finish an unfinished song' option[/]"
    | ImproveSongProgressAddingSomeMelodies ->
        "[springgreen3_1]Adding some melodies...[/]"
    | ImproveSongProgressPlayingFoosball ->
        "[springgreen2_1]Playing foosball...[/]"
    | ImproveSongProgressModifyingChordsFromAnotherSong ->
        "[springgreen1][strikethrough]Copying[/] Modifying chords from another song[/]"
    | FinishSong -> "Finish an unfinished song"
    | FinishSongSelection ->
        "Which song do you want to finish? [red]You won't be able to improve the song after this[/]"
    | FinishSongFinished (name, quality) ->
        String.Format(
            "[green]Your band finished the song \"{0}\". The result quality is {1}%[/]",
            name,
            quality
        )
    | DiscardSong -> "Discard an unfinished song"
    | DiscardSongSelection -> "[red]Which song do you want to discard?[/]"
    | DiscardSongDiscarded name ->
        String.Format("[red]Your band decided to stop working on {0}[/]", name)
    | PracticeSong -> "Practice a finished song"
    | ManagementTitle -> "Management"
    | ManagementPrompt -> "What do you want to to?"
    | ManagementHireMember -> "Hire a new member"
    | ManagementFireMember -> "Fire a member"
    | ManagementMemberList -> "List members"
    | HireMemberRolePrompt -> "What role are you looking to hire?"
    | HireMemberSkillSummary (name, gender) ->
        String.Format(
            "[bold blue]{0}[/] is interested in joining your band. {1} {2} the following skills:",
            name,
            subjectPronounForGender gender,
            (verbConjugationForGender Have gender).ToLower()
        )
    | HireMemberSkillLine (id, level) ->
        String.Format(
            "[bold]{0}[/] - [{1}]{2}[/]",
            skillName id,
            colorForLevel level,
            level
        )
    | HireMemberConfirmation gender ->
        String.Format("Do you want to hire {0}?", objectPronounForGender gender)
    | HireMemberHired -> "[bold green]You just hired a new member![/]"
    | HireMemberContinueConfirmation ->
        "Do you want to continue looking for members?"
    | FireMemberListItem (name, role) ->
        String.Format("{0} ({1})", name, instrumentName role)
    | FireMemberNoMembersToFire ->
        "[red]You are the only member of the band, you can't fire yourself![/]"
    | FireMemberPrompt -> "Who do you want to [bold red]fire[/]?"
    | FireMemberConfirmation name ->
        String.Format("Are you sure you want to [bold red]fire[/] {0}?", name)
    | FireMemberConfirmed name ->
        String.Format("You [bold red]fired[/] {0}", name)
    | MemberListCurrentTitle -> "[bold underline]Current members[/]"
    | MemberListCurrentMember (name, role, since) ->
        String.Format(
            "{0} ({1}) since {2}",
            name,
            instrumentName role,
            since.Year
        )
    | MemberListPastTitle -> "[bold underline]Past members[/]"
    | MemberListPastMember (name, role, since, until) ->
        String.Format(
            "{0} ({1}) from {2} until {3}",
            name,
            instrumentName role,
            since.Year,
            until.Year
        )
    | BankTitle -> "Bank"
    | BankWelcome (characterBalance, bandBalance) ->
        String.Format(
            "[bold blue]You[/] currently have [green]{0}d$[/]. [bold blue]Your band[/] has: [green]{1}d$[/]",
            characterBalance,
            bandBalance
        )
    | BankPrompt -> "What do you want to do?"
    | BankTransferToBand -> "Transfer money to band"
    | BankTransferFromBand -> "Transfer money from band"
    | BankTransferAmount holder ->
        match holder with
        | Character _ -> "How much do you want to transfer to your band?"
        | Band _ -> "How much do you want to transfer from your band?"
    | BankTransferSuccess (holder, transaction) ->
        match transaction with
        | Incoming amount ->
            sprintf
                "[bold green]Transferred %id$ to %s's account[/]"
                amount
                (accountHolderName holder)
        | Outgoing amount ->
            sprintf
                "[bold red]Transferred %id$ from %s's account[/]"
                amount
                (accountHolderName holder)
    | BankTransferNotEnoughFunds ->
        "[bold red]Not enough funds in the sender account[/]"
    | StudioCommonAlbumReleased name ->
        sprintf "[bold green]Your band just released %s![/]" name
    | StudioCommonPromptReleaseAlbum name ->
        sprintf "Do you want to release [blue bold]%s[/]?" name
    | StudioTitle -> "Studio"
    | StudioWelcomePrice (name, price) ->
        sprintf
            "Welcome to [bold blue]%s[/]. The recording session costs [bold red]%id$[/] per [bold]song[/]"
            name
            price
    | StudioPrompt -> "What do you want to do?"
    | StudioStartRecord -> "Start a new record"
    | StudioContinueRecord -> "Continue a record"
    | StudioDiscardRecord -> "Discard a record"
    | StudioCreateNoSongs ->
        "[bold red]You don't have any finished song to record. Create some songs first and finish them in the rehearsal room[/]"
    | StudioCreateRecordName ->
        "What's going to be the [bold blue]name[/] of the record?"
    | StudioCreateTrackListPrompt ->
        "Select what [bold blue]songs[/] will be on the [bold blue]track list[/]. You can select multiple. The order in which you select them will be the order in which they'll appear in the album"
    | StudioCreateErrorNameTooShort ->
        "[bold red]The name of the album is too short[/]"
    | StudioCreateErrorNameTooLong ->
        "[bold red]The name of the album is too long[/]"
    | StudioCreateErrorNotEnoughMoney (bandBalance, studioBill) ->
        sprintf
            "[bold red]Your band doesn't have enough money to pay the studio fee. The studio requires %id$, but your band only has %id$[/]"
            studioBill
            bandBalance
    | StudioCreateAlbumRecorded albumName ->
        sprintf "[bold green]Your band just finished recording %s![/]" albumName
    | StudioCreateProgressEatingSnacks -> "[deepskyblue3]Eating some snacks[/]"
    | StudioCreateProgressRecordingWeirdSounds ->
        "[deepskyblue3_1]Recording weird sounds[/]"
    | StudioCreateProgressMovingKnobs ->
        "[dodgerblue1]Moving knobs up and down[/]"
    | StudioContinueRecordPrompt ->
        "Which record do you want to continue working on?"
    | StudioContinueRecordActionPrompt ->
        "What do you want to do with this album?"
    | StudioContinueRecordActionPromptEditName -> "Edit name"
    | StudioContinueRecordActionPromptRelease -> "Release"
    | StudioContinueRecordAlbumRenamed albumName ->
        sprintf "[bold green]The album was renamed to \"%s\"[/]" albumName
