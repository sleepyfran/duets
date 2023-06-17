[<RequireQualifiedAccess>]
module Duets.Cli.Text.Concert

open Duets.Agents
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Concerts.Live

let adjustDrumsMessage =
    Styles.success
        "You tinker with the drum position yet again. Maybe now it'll be just right..."

let spaceStartConcert = "Start concert"

let faceBandMessage =
    Styles.success
        "You're now looking towards your band... Do you look cooler now?"

let faceCrowdMessage =
    Styles.success
        "You're now looking towards the crowd, everything's normal now"

let failedCharacterPassedOut =
    Styles.error
        "Your concert failed because your character passed out in the middle of it. Maybe keep an eye on your health next time? At least the surprise of the crowd will hopefully generate some buzz around your band"

let failedBandMissing (band: Band) (place: Place) concert =
    Styles.error
        $"Your band {band.Name} was supposed to have a concert {Date.simple concert.Date} {Generic.dayMomentName concert.DayMoment} at {place.Name} but didn't make it in time. The concert has been cancelled and fame took a little hit because of it"

let noSongsToPlay =
    $"""{Styles.error "You don't have any finished songs to play!"} Why are you even scheduling concerts if you haven't finished any song yet? That's going to be embarrassing to explain to the audience..."""

let selectSongToPlay = "Which song do you want to play?"

let songNameWithPractice (song: Song) =
    $"""{Styles.song song.Name} (Length: {song.Length |> Generic.length |> Styles.time}, Practice level: {Styles.Level.from song.Practice}%%)"""

let alreadyPlayedSongWithPractice (song: Song) =
    Styles.crossed $"""{Styles.song song.Name} (Already played)"""

let energyPrompt = "How much energy do you want to put into this?"

let energyEnergetic = "Energetic"
let energyNormal = "Normal"
let energyLow = "Low"

let actionPrompt date dayMoment attributes points =
    $"""{Generic.infoBar date dayMoment attributes} | {Emoji.concert} {Styles.Level.from points} points
{Styles.action "It's your time to shine!"} What do you want to do?"""

let bassSoloSlappingThatBass = Styles.progress "Slapping that bass..."

let bassSoloMovingFingersQuickly =
    Styles.progress "Moving fingers REALLY quickly..."

let bassSoloGrooving = Styles.progress "Grooving hard..."

let drumSoloDoingDrumstickTricks = Styles.progress "Spinning drumsticks..."

let drumSoloPlayingReallyFast = Styles.progress "Playing really fast..."

let drumSoloPlayingWeirdRhythms = Styles.progress "Playing REALLY fast..."

let drumstickSpinningBadResult points =
    Styles.Level.bad
        $"""That didn't really go so well, your drumsticks are now on the floor and you need some new ones... That's {points} {Generic.simplePluralOf "point" points}"""

let drumstickSpinningGoodResult points =
    Styles.Level.great
        $"""Wooooooah. {points} {Generic.simplePluralOf "point" points}"""

let guitarSoloDoingSomeTapping = Styles.progress "Doing some tapping..."

let guitarSoloPlayingReallyFast = Styles.progress "Playing REALLY fast..."

let guitarSoloPlayingWithTeeth = Styles.progress "Playing with teeth..."

let playSongLimitedEnergyDescription =
    Styles.progress
        "Barely moving and with a dull face you play the song to a confused audience..."

let playSongNormalEnergyDescription =
    Styles.progress
        "With just the right attitude you deliver that song. The audience seems to enjoy themselves, again, with just the right amount of enthusiasm"

let playSongEnergeticEnergyDescription =
    Styles.progress
        "Jumping around and hectically moving on the stage you play the song. The audience catches your enthusiasm and jumps at the rhythm of the music!"

let playSongProgressPlaying (song: Song) = $"Playing {Styles.song song.Name}"

let playSongRepeatedSongReaction (song: Song) =
    Styles.danger
        $"Why... why are you playing {Styles.song song.Name} again? The audience is confused, wondering whether you're forgetting the track-list or if you simply didn't come prepared to the concert"

let playSongRepeatedTipReaction points =
    Styles.information
        $"""That got you {points} {Generic.simplePluralOf "point" points}. Try not to repeat songs in a concert, it's never good"""

let playSongLowPerformanceReaction energy reasons points =
    match energy with
    | Energetic ->
        "At least your energetic performance gave the audience some nice feeling"
    | PerformEnergy.Normal
    | Limited -> "Not like you tried hard anyway"
    |> fun energyText ->
        let reasonsText =
            Generic.listOf reasons (fun reason ->
                match reason with
                | CharacterDrunk -> "you were quite drunk"
                | LowPractice -> "you didn't practice the song enough"
                | LowSkill -> "you didn't practice at home enough"
                | LowQuality -> "the song was not that good")

        Styles.Level.bad
            $"""Unfortunately it seems like {reasonsText}. You got {points} {Generic.simplePluralOf "point" points}. {energyText}"""

let playSongMediumPerformanceReaction reasons points =
    let reasonsText =
        Generic.listOf reasons (fun reason ->
            match reason with
            | CharacterDrunk -> "being drunk"
            | LowPractice -> "not having practiced the song enough"
            | LowSkill -> "not having good skills"
            | LowQuality -> "the song not being so good")

    Styles.Level.normal
        $"""You didn't nail the performance, probably {reasonsText} didn't help. But anyway, you got {points} {Generic.simplePluralOf "point" points}"""

let playSongHighPerformanceReaction energy points =
    match energy with
    | Energetic -> "the audience really enjoyed your energy"
    | PerformEnergy.Normal -> "the audience quite liked your energy"
    | Limited ->
        "well, you were quite boring on stage but at least the music was good"
    |> fun energyText ->
        Styles.Level.great
            $"""That was awesome! Your performance was great and {energyText}. You got {points} {Generic.simplePluralOf "point" points} for that"""

let putMicOnStandMessage =
    Styles.success
        "You left the mic on the stand, now let's try to figure out something to do with your hands..."

let soloResultLowPerformance reasons points =
    let reasonsText =
        Generic.listOf reasons (fun reason ->
            match reason with
            | CharacterDrunk -> "you are drunk"
            | _ -> "your skills are not that great anyway")

    Styles.Level.bad
        $"Well... That was really bad. Maybe try not to bring any extra attention towards you if {reasonsText}. That got you {points} points"

let soloResultAveragePerformance reasons points =
    let reasonsText =
        Generic.listOf reasons (fun reason ->
            match reason with
            | CharacterDrunk -> "not being drunk"
            | _ -> "a little more practice")

    Styles.Level.normal
        $"That was not bad, maybe {reasonsText} wouldn't hurt, but that got you {points} points"

let soloResultGreatPerformance points =
    Styles.Level.great
        $"That was awesome! People absolutely loved you. That got you {points} points"

let greetAudienceGreetedMoreThanOnceTip points =
    Styles.danger
        $"""The audience is confused since you've already greeted them before... How many times does a normal person say hello? Anyway, that's {points} {Generic.simplePluralOf "point" points}"""

let greetAudienceDone points =
    Styles.success
        $"""The audience says hello back! That's {points} {Generic.simplePluralOf "point" points}"""

let getOffStageEncorePossible =
    Styles.Level.great
        "You get off the stage and head into the backstage but the audience still wants more! You can hear the whistling and claps as you enter the backstage, maybe you can try an encore if you're still in the mood"

let getOffStageNoEncorePossible =
    Styles.Level.bad
        "You get off the stage and head into the backstage, the audience starts heading out. The concert is over"

let encoreComingBackToStage =
    Styles.success
        "You come back to the stage and the the audience starts clapping with enthusiasm!"

let finishedPoorly =
    Styles.Level.bad
        "The concert was a disaster. The audience was visibly disappointed and let out a chorus of boos and jeers. Your performance was lacklustre and uninspired, and it was clear that you had not put in enough practice"

let finishedNormally =
    Styles.Level.normal
        "The concert was mediocre. The audience seemed to enjoy it, but there were not many standing ovations. Your performance was decent, but it lacked the energy and passion that would have made it truly memorable. You could tell that the crowd was a bit underwhelmed and that you didn't quite live up to their expectations. Overall, the concert was okay"

let finishedGreat =
    Styles.Level.great
        "The concert was a huge success! The audience was on their feet, cheering, and clapping throughout the performance. Your playing was spot on, and you had great stage presence that kept everyone engaged throughout the show. The crowd sang along to many of your songs and it was clear that you had a real connection with your fans"

let concertSummary concert income =
    let attendance = concert.TicketsSold

    let concertSpaceCut =
        Queries.World.placeInCityById concert.CityId concert.VenueId
        |> Queries.Concerts.concertSpaceTicketPercentage
        |> (*) 100.0
        |> Math.ceilToNearest

    match concert.ParticipationType with
    | Headliner ->
        $"""{attendance} {Generic.pluralOf "person" "people" attendance} came to the concert and you made {Styles.money income} in tickets after removing the {concertSpaceCut}%% cut of the venue"""
    | OpeningAct(headlinerId, _) ->
        let band = Queries.Bands.byId (State.get ()) headlinerId
        $"""{attendance} {Generic.pluralOf "person" "people" attendance} came to {band.Name}'s concert. Your band made {Styles.money income} in your share of the tickets after removing the {concertSpaceCut}%% cut of the venue"""

let makeCrowdSingLowPerformance points =
    Styles.Level.bad
        $"""That was a little awkward, you tried to make them sing but they were just not interested. Well, maybe next time, that got you {points} {Generic.simplePluralOf "point" points}"""

let makeCrowdSingAveragePerformance points =
    Styles.Level.normal
        $"""That was not so bad! Some fans sang along you, although not all the crowd was up for it. That got you {points} {Generic.simplePluralOf "point" points}"""

let makeCrowdSingGreatPerformance points =
    Styles.Level.great
        $"That was amazing! The entire crowd was singing along you. That's {points} points"

let speechProgress = "Blablablaba... Blablaba... Bla..."

let speechGivenLowSkill points =
    Styles.Level.bad
        $"""Well.. Did you ever take an English class or were you just too scared of the audience? That was an embarrassing speech, maybe don't try again. You got {points} {Generic.simplePluralOf "point" points}"""

let speechGivenMediumSkill points =
    Styles.Level.normal
        $"""You would definitely not convince anyone to jump off a bridge with those skills, but the speech didn't go that bad. You got {points} {Generic.simplePluralOf "point" points}"""

let speechGivenHighSkill points =
    Styles.Level.great
        $"""That was amazing! You really have a way with words. That got you {points} {Generic.simplePluralOf "point" points}"""

let takeMicMessage =
    Styles.success "You took the microphone on your hands, free to move now!"

let tuneInstrumentDone points =
    Styles.success
        $"""You tuned the instrument and it now sounds awesome. {points} more {Generic.simplePluralOf "point" points}"""

let tooManySpeeches =
    Styles.danger
        "The audience is already bored of hearing you talk. Just play some songs and stop talking!"

let tooManyDedications =
    Styles.danger
        "The audience cut you in the middle of your dedication. They are already bored of hearing you dedicate songs. Just play and stop talking!"

let tooManyDrumstickSpins =
    Styles.danger
        "Nobody is really paying attention anymore, but feel free to keep doing it"

let tooMuchSingAlong =
    Styles.danger
        "You've tried that too many times, the crowd does not want to sing anymore. Try something else"

let tooManySolos points =
    Styles.danger
        $"The audience started booing as soon as they saw you were getting started for another solo... A bit more variation, please. That costed you {points} points"

let tooMuchTuning =
    Styles.danger "You tuned the instrument again. No points this time"
