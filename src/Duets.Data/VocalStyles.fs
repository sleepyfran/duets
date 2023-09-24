module Duets.Data.VocalStyles

open Duets.Common
open Duets.Entities

let all = Union.allCasesOf<VocalStyle> ()

let allNames = all |> List.map (fun vs -> vs, vs.ToString())
