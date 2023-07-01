module Duets.Cli.Text.World.RehearsalSpace

open Duets.Entities

let rec description (place: Place) (roomType: RoomType) =
    match roomType with
    | RoomType.Bar _ ->
        "The bar is managed by the same person that takes care of the rooms. It's a small place, but it has a decent selection of drinks and snacks"
    | RoomType.Lobby ->
        "The entrance of the building is decorated with posters of bands that have rehearsed here before, there's a board with the schedule of the rooms and some hand-written ads of bands looking for members"
    | RoomType.RehearsalRoom -> rehearsalRoomDescription place
    | _ -> failwith "Room type not supported in rehearsal space"

and private rehearsalRoomDescription (place: Place) =
    match place.Quality with
    | q when q < 30<quality> ->
        "The air is thick with a musty smell, as if years of neglect have settled into every corner. The once pristine wooden floors are littered with broken props, tangled wires, and discarded scripts. The walls, once adorned with posters of past productions, are now peeling and faded, revealing patches of mold and dampness. Dim flickering lights barely illuminate the room, casting eerie shadows that dance along the cracked mirrors lining one wall. The sound system crackles with static, emitting a garbled mix of forgotten melodies and disjointed dialogues."
    | q when q < 60<quality> ->
        "The space is well-organized and neatly arranged, with a hint of creativity in its design. The wooden floors gleam under the soft glow of overhead lights, inviting you to explore the area. The walls are adorned with motivational quotes and posters from successful productions, inspiring a sense of creativity and ambition. A row of neatly arranged costumes hangs on a rack, each one carefully labeled and ready for use. The sound system hums softly, providing a clear and crisp audio experience."
    | _ ->
        "The room is bathed in warm, natural light streaming through floor-to-ceiling windows, casting a vibrant glow on the polished hardwood floors. The walls are adorned with beautifully framed artwork and photographs, showcasing the rich history of past performances and inspiring a sense of wonder. State-of-the-art soundproofing ensures an immersive experience, with crystal-clear acoustics that reverberate throughout the room. Rows of plush seating are arranged in a semicircle, providing a comfortable and intimate space for observers. The room is equipped with cutting-edge technology, including a sophisticated lighting system that can transform the ambiance with a mere touch."
