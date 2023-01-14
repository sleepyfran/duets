module Test.Common.Generators.Song

open FsCheck

open FSharp.Data.UnitSystems.SI.UnitNames
open Entities

type SongGenOptions =
    { PracticeMin: int
      PracticeMax: int
      QualityMin: int
      QualityMax: int
      LengthRange: int<minute> * int<minute> }

let defaultOptions =
    { PracticeMin = 0
      PracticeMax = 100
      QualityMin = 100
      QualityMax = 100
      LengthRange = 3<minute>, 5<minute> }

let generator opts =
    gen {
        let! initialSong = Arb.generate<Song>

        let minMinutes, maxMinutes = opts.LengthRange

        let! lengthMinute =
            Gen.choose (
                minMinutes / 1<minute>,
                (* Reduce one minute since seconds can go up to 59. *)
                (maxMinutes - 1<minute>) / 1<minute>
            )
            |> Gen.map ((*) 1<minute>)

        let! lengthSeconds = Gen.choose (0, 59) |> Gen.map ((*) 1<second>)

        let length =
            { Minutes = lengthMinute
              Seconds = lengthSeconds }

        let! practice =
            Gen.choose (opts.PracticeMin, opts.PracticeMax)
            |> Gen.map ((*) 1<practice>)

        return
            { initialSong with
                Practice = practice
                Length = length }
    }

let finishedGenerator opts =
    gen {
        let! song = generator opts

        let! quality =
            Gen.choose (opts.QualityMin, opts.QualityMax)
            |> Gen.map ((*) 1<quality>)

        return FinishedSongWithQuality(FinishedSong song, quality)
    }
