[<RequireQualifiedAccess>]
module Cli.Text.Rehearsal

open Common

let manageDescription =
    "Opens the band management menu which allows you to hire new members or fire current ones"

let composePrompt = "What do you want to compose?"

let composeSong = "Compose new song"

let composeSongTitlePrompt =
    $"""Creating a new song, how are you going to {Styles.highlight "name"} it?"""

let composeSongLengthPrompt =
    $"""How {Styles.highlight "long"} is it going to be? (format {Styles.information "minutes:seconds"})"""

let composeSongGenrePrompt =
    $"""What {Styles.highlight "genre"} will the song have? Keep in mind that selecting a different genre that the main one of your band might reduce quality"""

let composeSongVocalStylePrompt =
    $"""What {Styles.highlight "vocal style"} should it have?"""

let composeSongConfirmation title =
    Styles.success
        $"""Your band has started working on the song "{title}". You can finish or improve it through the compose section in the rehearsal room"""

let composeSongErrorNameTooShort =
    Styles.error "The name of the song is too short"

let composeSongErrorNameTooLong =
    Styles.error "The name of the song is too long"

let composeSongErrorLengthTooShort =
    Styles.error "Songs can't be less than 20 seconds long"

let composeSongErrorLengthTooLong =
    Styles.error "Songs can't be more than 30 minutes long"

let composeSongProgressBrainstorming = Styles.progress "Brainstorming..."

let composeSongProgressConfiguringReverb =
    Styles.progress "Configuring reverb..."

let composeSongProgressTryingChords =
    Styles.progress "Trying out some chords..."

let improveSong = "Improve an unfinished song"

let improveSongSelection = "Which song do you want to improve?"

let improveSongCanBeFurtherImproved (previousQuality, currentQuality) =
    Styles.success
        $"You've improved the song. It improved from {previousQuality} to {currentQuality}"

let improveSongReachedMaxQuality =
    Styles.success
        $"""Your band has decided that the song does not need any further improvements. You can add it to the band's repertoire by using {Styles.information "finish song"}"""

let improveSongProgressAddingSomeMelodies =
    Styles.progress "Adding some melodies..."

let improveSongProgressPlayingFoosball = Styles.progress "Playing foosball..."

let improveSongProgressModifyingChordsFromAnotherSong =
    Styles.progress
        $"""{Styles.crossed "Copying"} Modifying chords from another song"""

let finishSong = "Finish an unfinished song"

let finishSongSelection =
    $"""Which song do you want to finish? {Styles.danger "You won't be able to improve the song after this"}"""

let finishSongFinished (name, quality) =
    Styles.success
        $"""Your band finished the song "{name}". The result quality is {quality}"""

let discardSong = "Discard an unfinished song"

let discardSongSelection = Styles.danger "Which song do you want to discard?"

let discardSongDiscarded name =
    Styles.error $"Your band decided to stop working on {name}"

let practiceSong = "Practice a finished song"

let practiceSongSelection = "Which song do you want to practice?"

let practiceSongItemDescription name practiceLevel =
    $"""{Styles.song name} (Practice level: {Styles.Level.from practiceLevel}%%)"""

let practiceSongImproved name practiceLevel =
    $"Your band improved its practice of {name} to {Styles.Level.from practiceLevel}%%"

let practiceSongAlreadyImprovedToMax name =
    $"Your band already knows {Styles.song name} perfectly"

let practiceSongProgressLosingTime = Styles.progress "Losing time..."

let practiceSongProgressTryingSoloOnceMore =
    Styles.progress "Trying that solo once more..."

let practiceSongProgressGivingUp = Styles.progress "Giving up..."

let hireMemberRolePrompt = "What role are you looking to hire?"

let hireMemberCharacterDescription name gender =
    $"{Styles.highlight name} is interested in joining your band. {Generic.subjectPronounForGender gender} {Generic.verbConjugationForGender Generic.Have gender
                                                                                                            |> String.lowercase} the following skills:"

let hireMemberConfirmation gender =
    $"Do you want to hire {Generic.objectPronounForGender gender}?"

let hireMemberHired = Styles.success "You just hired a new member!"

let hireMemberContinueConfirmation =
    "Do you want to continue looking for members?"

let fireMemberListItem (name, role) =
    $"{name} ({Generic.instrumentName role})"

let fireMemberNoMembersToFire =
    Styles.error "You are the only member of the band, you can't fire yourself!"

let fireMemberPrompt = $"""Who do you want to {Styles.danger "fire"}?"""

let fireMemberConfirmation name =
    Styles.danger $"Are you sure you want to fire {name}?"

let fireMemberConfirmed name = Styles.danger $"You fired {name}"

let memberListCurrentTitle = Styles.title "Current members"

let memberListNameHeader = Styles.header "Name"

let memberListRoleHeader = Styles.header "Role"

let memberListSinceHeader = Styles.header "Since"

let memberListUntilHeader = Styles.header "Until"

let memberListPastTitle = Styles.title "Past members"

let memberListName = Styles.person

let memberListRole = Generic.roleName

let memberListSince = Date.simple

let memberListUntil = Date.simple

let managementTitle = "Management"

let managementPrompt = "What do you want to to?"

let managementHireMember = "Hire a new member"

let managementFireMember = "Fire a member"
let managementMemberList = "List members"
