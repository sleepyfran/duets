module Duets.Simulation.Tests.Events.Moodlets.NotInspired

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Common
open Duets.Entities
open Duets.Simulation

let private song =
    Song.from
        "Test"
        (Time.Length.parse "3:54" |> Result.unwrap)
        VocalStyle.Instrumental

let private finishedSong = Finished(song, 100<quality>)

let private songFinishedEffect =
    SongFinished(dummyBand, finishedSong, dummyToday)

[<Test>]
let ``tick of song finished should not apply any extra effects if there were no other composed songs``
    ()
    =
    Simulation.tickOne dummyState songFinishedEffect
    |> fst
    |> should haveLength 1 (* This includes the effect we ticked. *)

[<Test>]
let ``tick of song finished should not apply any extra effects if the previous song was composed more than a week ago``
    ()
    =
    [ 8; 10; 20; 30 ]
    |> List.iter (fun daysAgo ->
        let finishDate = Calendar.Ops.addDays (daysAgo * 1<days>) dummyToday

        let stateWithSong =
            dummyState
            |> State.Songs.addFinished dummyBand dummyFinishedSong finishDate
            |> State.Songs.addFinished
                dummyBand
                finishedSong
                dummyToday (* Events run after the state has been updated. *)

        Simulation.tickOne stateWithSong songFinishedEffect
        |> fst
        |> should haveLength 1 (* This includes the effect we ticked. *) )

[<Test>]
let ``tick of song finished should apply NotInspired moodlet if the previous song was composed less than a week ago``
    ()
    =
    [ 1..7 ]
    |> List.iter (fun daysAgo ->
        let finishDate = Calendar.Ops.addDays (daysAgo * 1<days>) dummyToday

        let stateWithSong =
            dummyState
            |> State.Songs.addFinished dummyBand dummyFinishedSong finishDate
            |> State.Songs.addFinished
                dummyBand
                finishedSong
                dummyToday (* Events run after the state has been updated. *)

        let moodletEffect =
            Simulation.tickOne stateWithSong songFinishedEffect
            |> fst
            |> List.item 1 (* Position 0 is the effect we've ticked. *)

        match moodletEffect with
        | CharacterMoodletsChanged(_, Diff(prevMoodlet, currMoodlet)) ->
            prevMoodlet |> should haveCount 0
            currMoodlet |> should haveCount 1

            let moodlet = currMoodlet |> Set.toList |> List.head

            moodlet.MoodletType
            |> should be (ofCase <@ MoodletType.NotInspired @>)

            moodlet.StartedOn |> should equal dummyToday
        | _ -> failwith "Unexpected effect")

[<Test>]
let ``tick of AdvanceTime should not remove any moodlets if none have expired``
    ()
    =
    let yesterday = dummyToday |> Calendar.Ops.addDays -1<days>
    let lastWeek = dummyToday |> Calendar.Ops.addDays -7<days>

    let moodlets =
        [ Moodlet.create
              MoodletType.NotInspired
              dummyToday
              MoodletExpirationTime.Never
          Moodlet.create
              MoodletType.NotInspired
              yesterday
              (MoodletExpirationTime.AfterDays 3<days>)
          Moodlet.create
              MoodletType.NotInspired
              lastWeek
              (MoodletExpirationTime.AfterDays 30<days>)
          Moodlet.create
              MoodletType.NotInspired
              yesterday
              (MoodletExpirationTime.AfterDayMoments 20<dayMoments>) ]
        |> Set.ofList

    let stateWithMoodlets =
        dummyState |> State.Characters.setMoodlets dummyCharacter.Id moodlets

    let moodletUpdateEffect =
        Simulation.tickOne stateWithMoodlets (TimeAdvanced dummyToday)
        |> fst
        |> List.filter (function
            | CharacterMoodletsChanged _ -> true
            | _ -> false)
        |> List.head

    match moodletUpdateEffect with
    | CharacterMoodletsChanged(_, Diff(prevMoodlet, currMoodlet)) ->
        (prevMoodlet, currMoodlet) ||> Set.difference |> should haveCount 0
    | _ -> failwith "Unexpected effect"

[<Test>]
let ``tick of AdvanceTime should remove any moodlets that have expired`` () =
    let yesterday = dummyToday |> Calendar.Ops.addDays -1<days>
    let lastWeek = dummyToday |> Calendar.Ops.addDays -7<days>

    let nonExpiringMoodlet1 =
        Moodlet.create
            MoodletType.NotInspired
            dummyToday
            MoodletExpirationTime.Never

    let nonExpiringMoodlet2 =
        Moodlet.create
            MoodletType.NotInspired
            yesterday
            (MoodletExpirationTime.AfterDays 3<days>)

    let moodlets =
        [ nonExpiringMoodlet1
          nonExpiringMoodlet2
          Moodlet.create
              MoodletType.NotInspired
              lastWeek
              (MoodletExpirationTime.AfterDays 5<days>)
          Moodlet.create
              MoodletType.NotInspired
              lastWeek
              (MoodletExpirationTime.AfterDayMoments 10<dayMoments>) ]
        |> Set.ofList

    let stateWithMoodlets =
        dummyState |> State.Characters.setMoodlets dummyCharacter.Id moodlets

    let moodletUpdateEffect =
        Simulation.tickOne stateWithMoodlets (TimeAdvanced dummyToday)
        |> fst
        |> List.filter (function
            | CharacterMoodletsChanged _ -> true
            | _ -> false)
        |> List.head

    match moodletUpdateEffect with
    | CharacterMoodletsChanged(_, Diff(prevMoodlet, currMoodlet)) ->
        (prevMoodlet, currMoodlet) ||> Set.difference |> should haveCount 2

        let listedMoodlets = currMoodlet |> Set.toList

        listedMoodlets |> List.head |> should equal nonExpiringMoodlet2
        listedMoodlets |> List.item 1 |> should equal nonExpiringMoodlet1
    | _ -> failwith "Unexpected effect"
