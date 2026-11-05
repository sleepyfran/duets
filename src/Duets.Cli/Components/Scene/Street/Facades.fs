[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Street.Facades

open Duets.Cli.Text
open Duets.Entities

module Styles = SceneStyles.Facade

/// Style of the box drawing characters used for the frame of the building.
type Frame =
    | Single
    | Double
    | Heavy
    | Rounded

/// Defines how the façade of a place type looks like. All façades have the
/// same width and are drawn bottom-up: storefront, awning, sign, floors, frame
/// top and an optional roof decoration.
type Facade =
    {
        Frame: Frame
        /// Lines drawn on top of the building, from top to bottom. They're
        /// centered over the façade.
        Roof: string list
        Floors: int
        Awning: char option
        /// Storefront pattern, where W is a shop window, D a door, S stairs
        /// and G graffiti. Any other character is left empty.
        Storefront: string
    }

/// Total width of a façade, including its frame.
let width = 18

/// Width of the inside of a façade, without its frame.
let innerWidth = width - 2

/// Returns the characters used for the top-left corner, top edge, top-right
/// corner and sides of the given frame.
let frameGlyphs frame =
    match frame with
    | Single -> Styles.singleFrame
    | Double -> Styles.doubleFrame
    | Heavy -> Styles.heavyFrame
    | Rounded -> Styles.roundedFrame

/// Returns the façade of the given place.
let of' (place: Place) =
    match place.PlaceType with
    | Airport ->
        { Frame = Heavy
          Roof = Styles.controlTowerRoof
          Floors = 1
          Awning = None
          Storefront = " WWWW DDDD WWWW" }
    | Bar ->
        { Frame = Single
          Roof = []
          Floors = 1
          Awning = Some Styles.flatAwning
          Storefront = " WWW   DD   WWW" }
    | Bookstore ->
        { Frame = Single
          Roof = Styles.pitchedRoof
          Floors = 1
          Awning = Some Styles.thinAwning
          Storefront = " WWWWW DD WWWWW" }
    | CarDealer _ ->
        { Frame = Rounded
          Roof = []
          Floors = 0
          Awning = None
          Storefront = " WWWWWWWWWW DD" }
    | Cafe ->
        { Frame = Rounded
          Roof = Styles.pitchedRoof
          Floors = 1
          Awning = Some Styles.stripedAwning
          Storefront = " WWWW  DD  WWWW" }
    | Casino ->
        { Frame = Double
          Roof = Styles.marqueeRoof
          Floors = 2
          Awning = Some Styles.flatAwning
          Storefront = " WWWW DDDD WWWW" }
    | Cinema ->
        { Frame = Double
          Roof = Styles.marqueeRoof
          Floors = 1
          Awning = Some Styles.thickAwning
          Storefront = " WWW  DDDD  WWW" }
    | ConcertSpace concertSpace ->
        { Frame = Double
          Roof = Styles.marqueeRoof
          Floors = 1 + min 2 (concertSpace.Capacity / 1500)
          Awning = Some Styles.thickAwning
          Storefront = " WWW  DDDD  WWW" }
    | Gym ->
        { Frame = Heavy
          Roof = []
          Floors = 1
          Awning = None
          Storefront = " WWWWW DD WWWWW" }
    | Home ->
        { Frame = Single
          Roof = Styles.chimneyRoof
          Floors = 2
          Awning = None
          Storefront = "  W    DD    W" }
    | Hotel _ ->
        { Frame = Single
          Roof = []
          Floors = 3
          Awning = Some Styles.flatAwning
          Storefront = " WWW  DDDD  WWW" }
    | Hospital ->
        { Frame = Heavy
          Roof = Styles.crossRoof
          Floors = 2
          Awning = None
          Storefront = " WWWW DDDD WWWW" }
    | MerchandiseWorkshop ->
        { Frame = Single
          Roof = Styles.sawtoothRoof
          Floors = 0
          Awning = None
          Storefront = " WWW  DDDDDD WWW" }
    | MetroStation ->
        { Frame = Rounded
          Roof = []
          Floors = 0
          Awning = None
          Storefront = "    SSDDDDSS" }
    | RadioStudio _ ->
        { Frame = Single
          Roof = Styles.antennaRoof
          Floors = 1
          Awning = None
          Storefront = " WWWW  DD  WWWW" }
    | RehearsalSpace _ ->
        { Frame = Heavy
          Roof = []
          Floors = 0
          Awning = None
          Storefront = " WW   DD   GGGG" }
    | Restaurant ->
        { Frame = Single
          Roof = []
          Floors = 1
          Awning = Some Styles.flatAwning
          Storefront = " WWWW  DD  WWWW" }
    | Studio _ ->
        { Frame = Single
          Roof = []
          Floors = 1
          Awning = None
          Storefront = " WW    DD    WW" }
    | Street ->
        { Frame = Single
          Roof = []
          Floors = 1
          Awning = None
          Storefront = "" }
