module Duets.Cli.Text.World.RadioStudio

open Duets.Cli.Text
open Duets.Entities

let private lobbyDescription
    (placeName: string)
    (musicGenre: Genre)
    (quality: Quality)
    =
    let genreSound =
        match musicGenre.ToLower() with
        | "pop" -> "catchy pop tunes"
        | "rock" -> "a driving rock beat"
        | "jazz" -> "smooth jazz melodies"
        | _ -> $"some interesting {musicGenre.ToLower()} music"

    let couchDesc, deskDesc, postersDesc, airDesc =
        if quality <= 80<quality> then
            "a well-worn vinyl-covered couch with a few suspicious springs threatening to escape",
            "a reception desk that looks like it's been through several decades of rock 'n' roll (and hasn't been cleaned since)",
            "A few faded posters of smiling, heavily hair-sprayed musicians cling precariously to the walls",
            "The air smells strongly of stale coffee, old cigarettes, and faded ambition."
        elif quality <= 98<quality> then
            "A vinyl-covered couch sits against one wall",
            "opposite a reception desk that looks like it hasn't seen active duty since the invention of the CD",
            "A few framed posters of smiling, heavily hair-sprayed musicians adorn the walls",
            "The air smells faintly of old coffee and ambition."
        else
            "a surprisingly plush and modern-looking couch",
            "opposite a sleek, minimalist reception desk that occasionally flickers with a subtle LED glow",
            "Several tastefully framed, high-quality prints of iconic musicians adorn the walls",
            "The air is crisp and clean, with just a hint of expensive air freshener and the quiet hum of success."

    $"You are in a lobby of {Styles.place placeName}. Faint strains of {genreSound} drift from a hidden speaker. {couchDesc}, {deskDesc}. {postersDesc} – some of them even seem to be {musicGenre.ToLower()} artists. {airDesc} Close to you, a heavy door is marked with 'ON AIR'"

let private recordingRoomDescription
    (placeName: string)
    (musicGenre: Genre)
    (quality: Quality)
    =
    let genreFlavor =
        match musicGenre.ToLower() with
        | "pop" ->
            "The mixing board looks ready to lay down the next chart-topper."
        | "rock" ->
            "You can almost feel the lingering energy of a powerful guitar solo."
        | "jazz" ->
            "An upright bass stand sits quietly in the corner, a silent testament to soulful sessions."
        | _ ->
            $"Instruments and equipment suitable for recording {musicGenre.ToLower()} are neatly arranged."

    let equipmentDesc, micsDesc, soundproofingDesc, windowDesc =
        if quality <= 80<quality> then
            "aging, somewhat temperamental audio equipment. Some of it looks like it belongs in a museum",
            "Dusty microphones on wobbly articulated arms loom over a central table",
            "Patches of discolored soundproofing panels cover most surfaces, though a few are peeling at the edges",
            "A grimy window looks into what you assume is a control booth, currently dark and possibly haunted"
        elif quality <= 98<quality> then
            "intimidatingly complex audio equipment",
            "Microphones on articulated arms loom over a central table",
            "Soundproofing panels cover every surface, giving the room a strangely muffled quiet",
            "A large window looks into what you assume is a control booth, currently dark"
        else
            "state-of-the-art, gleaming audio equipment. It all looks incredibly expensive",
            "Pristine, high-end microphones on smooth, silent articulated arms are positioned perfectly over a polished central table",
            "Thick, professionally installed soundproofing panels create an almost unnervingly silent environment",
            "A crystal-clear, multi-paned window offers a perfect view into a brightly lit and bustling control booth"

    $"This is the heart of {Styles.place placeName}, a room crammed with {equipmentDesc}. {micsDesc}. {soundproofingDesc}. {genreFlavor} {windowDesc}. The faint hum of electronics is the only sound."

let description (place: Place) (roomType: RoomType) =
    match place.PlaceType with
    | PlaceType.RadioStudio radioDetails ->
        let genre = radioDetails.MusicGenre
        let quality = place.Quality

        match roomType with
        | RoomType.Lobby -> lobbyDescription place.Name genre quality
        | RoomType.RecordingRoom ->
            recordingRoomDescription place.Name genre quality
        | _ -> "You are in a nondescript room in the radio station."
    | _ -> "You are in an unidentifiable part of what might be a radio station."
