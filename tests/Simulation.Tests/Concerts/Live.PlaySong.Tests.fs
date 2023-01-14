module Simulation.Tests.Concerts.Live.PlaySong

open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Entities
open Simulation
open Simulation.Concerts.Live

[<Test>]
let ``playSong with energetic energy gives up to 25 points when song is long``
    ()
    =
    Generators.Song.finishedGenerator
        { Generators.Song.defaultOptions with
            LengthRange = 10<minute>, 20<minute> }
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        let response = playSong dummyState dummyOngoingConcert song Energetic

        response
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (inRange 0<quality> 25<quality>)

        response
        |> pointsFromResponse
        |> should be (inRange 0<quality> 25<quality>))

[<Test>]
let ``playSong with energetic energy gives up to 15 points`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        let response = playSong dummyState dummyOngoingConcert song Energetic

        response
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (inRange 0<quality> 15<quality>)

        response
        |> pointsFromResponse
        |> should be (inRange 0<quality> 15<quality>))

[<Test>]
let ``playSong with normal energy gives up to 15 points when song is long`` () =
    Generators.Song.finishedGenerator
        { Generators.Song.defaultOptions with
            LengthRange = 10<minute>, 20<minute> }
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        let response =
            playSong dummyState dummyOngoingConcert song PerformEnergy.Normal

        response
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (inRange 0<quality> 15<quality>)

        response
        |> pointsFromResponse
        |> should be (inRange 0<quality> 15<quality>))

[<Test>]
let ``playSong with normal energy gives up to 8 points`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        let response =
            playSong dummyState dummyOngoingConcert song PerformEnergy.Normal

        response
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (inRange 0<quality> 8<quality>)

        response
        |> pointsFromResponse
        |> should be (inRange 0<quality> 8<quality>))

[<Test>]
let ``playSong with limited energy gives up to 8 points when song is long`` () =
    Generators.Song.finishedGenerator
        { Generators.Song.defaultOptions with
            LengthRange = 10<minute>, 20<minute> }
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        let response = playSong dummyState dummyOngoingConcert song Limited

        response
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (inRange 0<quality> 8<quality>)

        response
        |> pointsFromResponse
        |> should be (inRange 0<quality> 8<quality>))

[<Test>]
let ``playSong with limited energy gives up to 2 points`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        let response = playSong dummyState dummyOngoingConcert song Limited

        response
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (inRange 0<quality> 2<quality>)

        response
        |> pointsFromResponse
        |> should be (inRange 0<quality> 2<quality>))

[<Test>]
let ``playSong decreases points by 50 if song has been played already`` () =
    let finishedSong =
        Generators.Song.finishedGenerator Generators.Song.defaultOptions
        |> Gen.sample 0 1
        |> List.head

    let FinishedSong song, _ = finishedSong

    let ongoingConcert =
        { dummyOngoingConcert with
            Events = [ PlaySong(song, Energetic) ]
            Points = 50<quality> }

    playSong dummyState ongoingConcert finishedSong Energetic
    |> ongoingConcertFromResponse
    |> Optic.get Lenses.Concerts.Ongoing.points_
    |> should equal 0

[<Test>]
let ``playSong returns low performance result if practice and quality ares below 25``
    ()
    =
    Generators.Song.finishedGenerator
        { Generators.Song.defaultOptions with
            PracticeMin = 0
            PracticeMax = 24
            QualityMin = 0
            QualityMax = 24 }
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        let response = playSong dummyState dummyOngoingConcert song Energetic

        response
        |> resultFromResponse
        |> should be (ofCase <@ LowPerformance @>))

[<Test>]
let ``playSong returns average performance result if practice and quality ares between 25 50``
    ()
    =
    Generators.Song.finishedGenerator
        { Generators.Song.defaultOptions with
            PracticeMin = 25
            PracticeMax = 49
            QualityMin = 25
            QualityMax = 49 }
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        let response = playSong dummyState dummyOngoingConcert song Energetic

        response
        |> resultFromResponse
        |> should be (ofCase <@ AveragePerformance @>))

[<Test>]
let ``playSong returns good performance result if practice and quality are below 75``
    ()
    =
    Generators.Song.finishedGenerator
        { Generators.Song.defaultOptions with
            PracticeMin = 50
            PracticeMax = 74
            QualityMin = 50
            QualityMax = 74 }
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        let response = playSong dummyState dummyOngoingConcert song Energetic

        response
        |> resultFromResponse
        |> should be (ofCase <@ GoodPerformance @>))

[<Test>]
let ``playSong returns great performance result if practice and quality are above 75``
    ()
    =
    Generators.Song.finishedGenerator
        { Generators.Song.defaultOptions with
            PracticeMin = 76
            PracticeMax = 100 }
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        let response = playSong dummyState dummyOngoingConcert song Energetic

        response
        |> resultFromResponse
        |> should be (ofCase <@ GreatPerformance @>))

[<Test>]
let ``playSong lowers result depending on character's drunkenness`` () =
    [ (20, <@ GreatPerformance @>)
      (35, <@ GoodPerformance [ CharacterDrunk ] @>)
      (55, <@ AveragePerformance [ CharacterDrunk ] @>)
      (80, <@ LowPerformance [ CharacterDrunk ] @>) ]
    |> List.iter (fun (drunkenness, expectedResult) ->
        Generators.Song.finishedGenerator
            { Generators.Song.defaultOptions with
                PracticeMin = 100
                PracticeMax = 100 }
        |> Gen.sample 0 1
        |> List.iter (fun song ->
            let drunkState =
                Character.Attribute.add
                    dummyCharacter
                    CharacterAttribute.Drunkenness
                    drunkenness
                |> State.Root.applyEffect dummyState

            let response =
                playSong drunkState dummyOngoingConcert song Energetic

            response |> resultFromResponse |> should be (ofCase expectedResult)))

[<Test>]
let ``playSong does not decrease points below 0`` () =
    let finishedSong =
        Generators.Song.finishedGenerator Generators.Song.defaultOptions
        |> Gen.sample 0 1
        |> List.head

    let ongoingConcert =
        { dummyOngoingConcert with
            Events = [ PlaySong(Song.fromFinished finishedSong, Energetic) ]
            Points = 20<quality> }

    playSong dummyState ongoingConcert finishedSong Energetic
    |> ongoingConcertFromResponse
    |> Optic.get Lenses.Concerts.Ongoing.points_
    |> should equal 0

[<Test>]
let ``playSong should add points to the previous count to ongoing concert`` () =
    let ongoingConcert = { dummyOngoingConcert with Points = 50<quality> }

    Generators.Song.finishedGenerator
        { Generators.Song.defaultOptions with PracticeMin = 1 }
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        playSong dummyState ongoingConcert song Energetic
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (greaterThan 50<quality>))

[<Test>]
let ``playSong does not increase above 100`` () =
    let ongoingConcert = { dummyOngoingConcert with Points = 98<quality> }

    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        playSong dummyState ongoingConcert song Energetic
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (lessThanOrEqualTo 100<quality>))

[<Test>]
let ``playSong should add event when the song hasn't been played before`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1
    |> List.head
    |> fun song ->
        playSong dummyState dummyOngoingConcert song Energetic
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.events_
        |> should contain (PlaySong(Song.fromFinished song, Energetic))

[<Test>]
let ``playSong should add event when the song was played before`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1
    |> List.head
    |> fun finishedSong ->
        let song = Song.fromFinished finishedSong

        let ongoingConcert =
            { dummyOngoingConcert with
                Events = [ PlaySong(song, Energetic) ]
                Points = 40<quality> }

        playSong dummyState ongoingConcert finishedSong Energetic
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.events_
        |> List.filter (fun event -> event = (PlaySong(song, Energetic)))
        |> should haveLength 2

[<Test>]
let ``playSong should decrease health by 2 points and energy by 5 when performing energetically``
    ()
    =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1
    |> List.head
    |> fun finishedSong ->
        let effects =
            playSong dummyState dummyOngoingConcert finishedSong Energetic
            |> effectsFromResponse

        effects
        |> List.item 0
        |> fun effect ->
            match effect with
            | CharacterAttributeChanged (_, attr, amount) ->
                attr |> should equal CharacterAttribute.Health
                amount |> should equal (Diff(100, 98))
            | _ -> failwith "Effect was not of correct type"

        effects
        |> List.item 1
        |> fun effect ->
            match effect with
            | CharacterAttributeChanged (_, attr, amount) ->
                attr |> should equal CharacterAttribute.Energy
                amount |> should equal (Diff(100, 95))
            | _ -> failwith "Effect was not of correct type"

[<Test>]
let ``playSong should energy by 3 points when performing normally`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1
    |> List.head
    |> fun finishedSong ->
        let effects =
            playSong
                dummyState
                dummyOngoingConcert
                finishedSong
                PerformEnergy.Normal
            |> effectsFromResponse

        effects
        |> List.item 0
        |> fun effect ->
            match effect with
            | CharacterAttributeChanged (_, attr, amount) ->
                attr |> should equal CharacterAttribute.Energy
                amount |> should equal (Diff(100, 97))
            | _ -> failwith "Effect was not of correct type"

[<Test>]
let ``playSong should energy by 1 point when performing in limited`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1
    |> List.head
    |> fun finishedSong ->
        let effects =
            playSong dummyState dummyOngoingConcert finishedSong Limited
            |> effectsFromResponse

        effects
        |> List.item 0
        |> fun effect ->
            match effect with
            | CharacterAttributeChanged (_, attr, amount) ->
                attr |> should equal CharacterAttribute.Energy
                amount |> should equal (Diff(100, 99))
            | _ -> failwith "Effect was not of correct type"
