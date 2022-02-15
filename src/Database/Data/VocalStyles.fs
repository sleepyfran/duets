module Data.VocalStyles

open Entities

let get = [ Instrumental; Normal; Growl; Screamo ]

let getWithNames =
    get |> List.map (fun vs -> vs, vs.ToString())
