namespace Duets.UI.Common

open Duets.Entities
open FSharp.Control

/// Content that can be displayed without requiring user input.
[<RequireQualifiedAccess>]
type ShowContent =
    | Text of string
    | Figlet of string
    | GameInfo of version: string
    | Separator of label: string option
    | LineBreak
    | Wait of milliseconds: int
    | LLMStream of AsyncSeq<string>
    | Progress of steps: string list * duration: int
    | Notification of title: string * message: string
    | Tip of title: string * message: string
    | Tips of string list
    | BarChart of items: (int * string) list
    | Calendar of year: int<years> * season: Season * events: Date list
    | ConcertDetails of Concert
    | AlbumReviews of Album
    | SocialPost of account: SocialNetworkAccount * post: SocialNetworkPost
    | Path of string
    | Clear

/// Content that asks the user for input, returning a value of type 'T.
/// Only contains cases whose return type is genuinely generic — fixed-return
/// types (bool, City option, etc.) live as dedicated methods on IRenderer.
[<RequireQualifiedAccess>]
type AskContent<'T> =
    | Choice of values: 'T list * display: ('T -> string)
    | SearchChoice of values: 'T list * display: ('T -> string)
    | TextBox of placeholder: string * parse: (string -> 'T option)
