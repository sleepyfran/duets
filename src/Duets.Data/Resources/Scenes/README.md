# Scene templates

Each `.scene` file is the drawing of a room. When showing a room, the game
uses the first template it finds of:

1. `places/{city}/{place-name}/{room}.scene`
2. `rooms/{place-type}.{room}.{tag}.scene`, for each tag of the place
3. `cities/{city}/{place-type}.{room}.scene`
4. `cities/{city}/{room}.scene`
5. `rooms/{place-type}.{room}.{tier}.scene`, only for concert spaces
6. `rooms/{place-type}.{room}.scene`
7. `rooms/{room}.scene`

Names are lowercase and separated by dashes, without accents. For example:
`new-york`, `concert-space`, `rehearsal-room`. Restaurants are just
`restaurant`, no matter the cuisine.

Tags are added to places in `Duets.Data/World/Cities` with `World.Place.tag`
(eg: `pub`, `jazz-club`, `seated`). When a place has more than one tag, the
first one that has a template wins. The tier depends on the capacity of the
concert space: `club` (up to 500), `theatre` (up to 3000), `arena` (up to
25000) and `stadium` (anything bigger).

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
- `crowd [glyphs]`: the audience during a concert. The glyphs, separated by
  spaces, replace the default people (eg: `crowd ♥ ✦` for lightsticks). The
  crowd reacts to the concert: a bored crowd uses its own glyphs and part of
  it moves to the `npc` slots (so put some by the bar), a hyped one raises its
  arms and an ecstatic one gets lighters out.
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

## Drawing rooms

Rooms are seen from the side, like the streets: a cut through the building
with the ceiling on top, the back wall in the middle and the floor at the
bottom. Check `rooms/bar.scene` for an example.

- Keep rooms 56 columns wide and around 10 rows tall (12 at most, big stages
  can go up to 14).
- The first row is the ceiling (`▛▀▜`), the last one the floor (`▙▄▟`) and
  the sides are the walls (`▌ ▐`), all with `$wall`.
- Things hang from the ceiling (lamps, lights, speakers), windows, shelves,
  posters and doors go on the back wall and furniture stands on the floor.
- People (`player`, `npc`, `band`, `crowd`) stand on the row right above the
  floor, between the furniture. Tall furniture like counters or beds takes
  the two rows above the floor.
- Exits:
    - `north`, `north-west` and `north-east` are doors in the back wall
      (`┌───┐` with the slot inside, `else ▪` so a closed door has a window).
    - `west` and `east` are gaps in the side walls on the people row
      (`else ▌` and `else ▐`).
    - `south`, `south-west`, `south-east` and `street` are towards the viewer,
      in the floor (`else ▄`).

## Skylines

`skylines/{city}.scene` is drawn in the distance behind the façades of the
streets, and `skylines/{city}.{zone}.scene` replaces it in a specific zone.
Zones with an airport and no skyline of their own use `skylines/airport.scene`.
They use the same format but only `[art]` and `[legend]` (with `$lamp` for
windows that light up at night). The art is centered and its bottom sits on
the sidewalk, so make it taller than the façades (around 12 rows) or only the
gaps between buildings will show it.

## Landmarks

`landmarks/{tag}.scene` changes the scene of the streets tagged with `tag`
(see `World.Street.tag`). Tags are applied in order. Every section is optional, but a
landmark has to change something:

```
# Comment.

[art]
 ▄█▄
▐█·█▌

[legend]
★ = gold1
· = $lamp
default = grey30

[options]
layer = back
align = center
elevation = 0
traffic = no

[sidewalk]
──★──

[road]
═══
≈~≈~

[props]
♣ ┬
│ ║

[captions]
A sentence about the landmark.
```

- `[art]`: with `layer = back` (the default) it goes over the skyline and
  behind the façades, so make it taller than them. With `layer = front` it
  stands on the sidewalk, in front of everything but the people, and hides
  what's behind it.
- `align`: `center` (the default), or `left` and `right` to stand next to the
  first or last façade. Side art always stays behind the façades, so keep it
  around 10 columns wide or it gets hidden on busy streets.
- `elevation`: rows between the sidewalk and the bottom of the art, for things
  like bridges over the street.
- `[legend]`: colors the glyphs of every section. Without one, the sidewalk
  and the road use the city color.
- `[sidewalk]`: a line repeated along the sidewalk.
- `[road]`: lines repeated along the road, one row each. There's no traffic
  unless `traffic = yes`.
- `[props]`: each column is a prop between the façades, replacing the city
  ones.
- `[captions]`: sentences that replace the city ones in the caption.

Trailing spaces are dropped, so don't end a pattern with one.
