namespace State

module Songs =
  open Aether
  open Entities

  let private applyToUnfinished map bandId op =
    let unfinishedSongLens =
      Lenses.FromState.Songs.unfinishedByBand_ bandId

    map (Optic.map unfinishedSongLens op)

  let private applyToFinished map bandId op =
    let finishedSongLens =
      Lenses.FromState.Songs.finishedByBand_ bandId

    map (Optic.map finishedSongLens op)

  let addUnfinished map (band: Band) unfinishedSong =
    let song = Song.fromUnfinished unfinishedSong

    let addUnfinishedSong = Map.add song.Id unfinishedSong
    applyToUnfinished map band.Id addUnfinishedSong

  let addFinished map (band: Band) finishedSong =
    let song = Song.fromFinished finishedSong

    let addFinishedSong = Map.add song.Id finishedSong

    applyToFinished map band.Id addFinishedSong

  let removeUnfinished map (band: Band) songId =
    let removeUnfinishedSong = Map.remove songId

    applyToUnfinished map band.Id removeUnfinishedSong

  let removeFinishedSong map (band: Band) songId =
    let removeFinishedSong = Map.remove songId

    applyToFinished map band.Id removeFinishedSong
