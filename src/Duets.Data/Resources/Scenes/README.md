# Scene templates

Each `.scene` file is the drawing of a room. When showing a room, the game
uses the first template it finds of:

1. `places/{city}/{place-name}/{room}.scene`
2. `cities/{city}/{place-type}.{room}.scene`
3. `cities/{city}/{room}.scene`
4. `rooms/{place-type}.{room}.scene`
5. `rooms/{room}.scene`

Names are lowercase and separated by dashes, without accents. For example:
`new-york`, `concert-space`, `rehearsal-room`. Restaurants are just
`restaurant`, no matter the cuisine.

## Format

```
# Comment.

[art]
┌──88──┐
│ p  ii│
└─0000─┘

[legend]
┌─┐│└┘ = $wall
░ = bold indianred
default = grey50

[slots]
8 = exit north else ─
0 = exit street else ─
p = player
i = item sleepable
```

`[art]` is the drawing itself. Stick to characters that take one column,
emojis and CJK characters will move the walls around.

`[legend]` sets the color of each glyph. Apart from the normal Spectre styles
you can use:

- `$wall`: wall color of the city.
- `$accent`: one of the sign colors of the city.
- `$lamp`: on at night, off during the day.
- `$stage`: on during concerts.

`[slots]` replaces a character of the art with something from the game. The
`else` part is what gets drawn when the slot is empty (a space if not set).
The character can't appear anywhere else in the art, not even in text like
signs, or it'll get replaced too.

- `player`: the player.
- `npc`: the people in the room.
- `exit <direction>` / `exit street`: an arrow if there's an exit there.
- `item [category]`: an item of the room. Categories are `cookware`,
  `drinkable`, `edible`, `fitness`, `listenable`, `playable`, `readable`,
  `rideable`, `sleepable`, `storage`, `watchable` and `wearable`. Without one,
  it takes whatever item is left.
- `window`: the sky, which changes with the time and weather.
- `crowd`: the audience during a concert.
- `band`: the rest of the band during a concert.
- `cuisine`: the sign of the restaurant.
- `prop a b | c | d`: a decoration that depends on the quality of the place,
  from the lowest tier to the highest. One of the options of the tier gets
  picked, `_` means nothing.

Consecutive slot characters in a row are a single spot for items, exits,
props and signs, which get drawn in the middle of it. People, windows and the
crowd use each character as a separate position.

The default templates use the numpad for the exits (`7 8 9`, `4 6`, `1 2 3`
and `0` for the street), but any character works.

## Skylines

`skylines/{city}.scene` is drawn in the distance behind the façades of the
streets, and `skylines/{city}.{zone}.scene` replaces it in a specific zone.
They use the same format but only `[art]` and `[legend]` (with `$lamp` for
windows that light up at night). The art is centered and its bottom sits on
the sidewalk, so make it taller than the façades (around 12 rows) or only the
gaps between buildings will show it.
