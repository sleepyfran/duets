module Data.VocalStyles

open Entities

let all = [ Instrumental; Normal; Growl; Screamo ]

let allNames =
    all |> List.map (fun vs -> vs, vs.ToString())
