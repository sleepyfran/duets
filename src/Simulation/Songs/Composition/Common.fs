module Simulation.Songs.Composition.Common

open Simulation.Skills.Queries
open Simulation.Songs.Lenses
open Simulation.Songs.Queries
open Entities
open Entities.Skill
open Lenses
open Storage.State

/// Computes the score associated with each member of the band for the song.
let qualityForMember genre (currentMember: CurrentMember) =
  let genreSkill = create <| Genre(genre)

  let influencingSkills =
    [ Composition
      (Instrument
       <| Instrument.createInstrument currentMember.Role)
      genreSkill.Id ]

  influencingSkills
  |> List.map (characterSkillWithLevel currentMember.Character.Id)
  |> List.map snd
  |> List.sum
  |> fun total -> total / influencingSkills.Length

/// Computes the maximum score that the band can achieve for a song in a moment
/// in time given each member's skills in composition, the current genre and
/// the member's instrument skill.
let qualityForBand band =
  band.Members
  |> List.map (qualityForMember band.Genre)
  |> List.sum
  |> fun score ->
       match score with
       | score when score < 5 -> 5<quality>
       | _ -> score * 1<quality>

/// Calculates at what rate the score of a song should increase based on its
/// maximum quality.
let calculateQualityIncreaseOf (maximum: MaxQuality) =
  maximum
  |> float
  |> fun max -> max * 0.20
  |> ceil
  |> int
  |> fun increase -> increase * 1<quality>

/// Saves the updated songs of the given band into the specified lens.
let saveSongsToState (band: Band) lenses fullList updatedSongs =
  updatedSongs
  |> fun updatedSongs -> Map.add band.Id updatedSongs fullList
  |> fun updatedSongs -> updatedSongs |> Lens.set lenses |> modifyState

/// Adds or modifies a given unfinished song into the given band's repertoire.
let addUnfinishedSong (band: Band) unfinishedSong =
  let (UnfinishedSong (song), _, _) = unfinishedSong

  unfinishedSongsByBand band.Id
  |> Map.add song.Id unfinishedSong
  |> saveSongsToState band unfinishedSongsLenses (unfinishedSongs ())

/// Removes an unfinished song and returns it back.
let removeUnfinishedSong (band: Band) unfinishedSong =
  let (UnfinishedSong (song), _, _) = unfinishedSong

  unfinishedSongsByBand band.Id
  |> Map.remove song.Id
  |> saveSongsToState band unfinishedSongsLenses (unfinishedSongs ())
  |> fun () -> unfinishedSong

/// Adds or modifies a given unfinished song in the band's finished repertoire.
let addFinishedSong (band: Band) unfinishedSong =
  let (UnfinishedSong (song), _, quality) = unfinishedSong

  finishedSongsByBand band.Id
  |> Map.add song.Id (FinishedSong song, quality)
  |> saveSongsToState band finishedSongsLenses (finishedSongs ())
