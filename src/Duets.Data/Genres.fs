module Duets.Data.Genres

open Duets.Entities

let private rockLineup = [ Guitar; Bass; Drums; Vocals ]

let private instrumentsByGenre: (Genre * GenreInstruments) list =
    [ "Ambient",
      { Core = [ Synthesizer; Guitar ]
        Common = [ Keyboard; Cello; Violin; Bass; Vocals ] }
      "Black Metal",
      { Core = rockLineup
        Common = [ Synthesizer; Keyboard ] }
      "Blackgaze", { Core = rockLineup; Common = [ Synthesizer ] }
      "Bluegrass",
      { Core = [ Banjo; Guitar; Violin; UprightBass; Vocals ]
        Common = [ Harmonica ] }
      "Blues",
      { Core = rockLineup
        Common = [ Harmonica; Keyboard; Saxophone; Trumpet ] }
      "Country",
      { Core = rockLineup
        Common = [ Banjo; Violin; Harmonica; Keyboard ] }
      "Death Metal", { Core = rockLineup; Common = [] }
      "Electronic",
      { Core = [ Synthesizer; Turntables ]
        Common = [ Drums; Vocals; Keyboard; Bass; HandPercussion ] }
      "Folk",
      { Core = [ Guitar; Vocals ]
        Common = [ Bass; Violin; Banjo; Harmonica; Cello; HandPercussion; Drums ] }
      "Funk",
      { Core = rockLineup @ [ Keyboard ]
        Common = [ Saxophone; Trumpet; Trombone; HandPercussion; Synthesizer ] }
      "Grunge", { Core = rockLineup; Common = [] }
      "Hip-Hop",
      { Core = [ Vocals; Turntables ]
        Common = [ Drums; Bass; Keyboard; Synthesizer ] }
      "House",
      { Core = [ Synthesizer; Turntables ]
        Common = [ Vocals; Keyboard; HandPercussion ] }
      "Indie",
      { Core = rockLineup
        Common = [ Keyboard; Synthesizer; Violin ] }
      "J-Pop",
      { Core = [ Vocals; Synthesizer; Guitar; Bass; Drums ]
        Common = [ Keyboard; Violin ] }
      "Jazz",
      { Core = [ Drums; UprightBass; Keyboard; Saxophone ]
        Common = [ Trumpet; Trombone; Guitar; Vocals; Bass ] }
      "Pop",
      { Core = rockLineup
        Common = [ Keyboard; Synthesizer ] }
      "Post-Punk",
      { Core = rockLineup
        Common = [ Synthesizer; Keyboard ] }
      "Post-Rock",
      { Core = [ Guitar; Bass; Drums ]
        Common = [ Keyboard; Synthesizer; Violin; Cello; Trumpet ] }
      "Punk", { Core = rockLineup; Common = [] }
      "Reggae",
      { Core = rockLineup @ [ Keyboard ]
        Common = [ HandPercussion; Saxophone; Trumpet; Trombone ] }
      "Rock",
      { Core = rockLineup
        Common = [ Keyboard; Harmonica ] }
      "Salsa",
      { Core = [ HandPercussion; Keyboard; Bass; Vocals; Trumpet ]
        Common = [ Trombone; Saxophone; Drums; Guitar ] }
      "Shoegaze",
      { Core = rockLineup
        Common = [ Synthesizer; Keyboard ] }
      "Ska",
      { Core = rockLineup @ [ Trumpet; Trombone ]
        Common = [ Saxophone; Keyboard ] }
      "Soul",
      { Core = [ Vocals; Keyboard; Bass; Drums ]
        Common = [ Guitar; Saxophone; Trumpet; Trombone ] }
      "Synthpop",
      { Core = [ Synthesizer; Vocals; Drums ]
        Common = [ Keyboard; Bass; Guitar ] }
      "Techno",
      { Core = [ Synthesizer; Turntables ]
        Common = [ HandPercussion ] }
      "Thrash Metal", { Core = rockLineup; Common = [] }
      "Trip-Hop",
      { Core = [ Turntables; Vocals; Bass; Synthesizer ]
        Common = [ Drums; Keyboard; Guitar; Violin; Cello ] } ]

/// Contains all available genres in the game.
let all: Genre list = instrumentsByGenre |> List.map fst

/// Returns the instruments that bands of the given genre usually have.
let instrumentsFor genre =
    instrumentsByGenre
    |> List.tryFind (fst >> (=) genre)
    |> Option.map snd
    |> Option.defaultValue { Core = rockLineup; Common = [] }
