module Duets.Common.Map

open Aether

/// Prism to a value associated with a key in a map. Similar as Aether's
/// built in but this one always introduces the key in the map regardless
/// whether it already exists or not.
let key_ (k: 'k) : Prism<Map<'k, 'v>, 'v> = Map.tryFind k, Map.add k

/// Prism to a value associated with a key in a map that returns a default value
/// if the key could not be found.
let keyWithDefault_ (k: 'k) (defaultValue: 'v) : Prism<Map<'k, 'v>, 'v> =
    Map.tryFind k >> Option.defaultValue defaultValue >> Some, Map.add k

/// Attempts to find the head of the map.
let tryHead (map: Map<'k, 'v>) =
    map |> Seq.tryHead |> Option.map (_.Value)

/// Returns the head of the map.
let head (map: Map<'k, 'v>) = tryHead map |> Option.get

/// Merges two maps, resolving conflicts by preserving only the value from the
/// left map.
let merge (left: Map<'a, 'b>) (right: Map<'a, 'b>) =
    Map.fold (fun acc key value -> Map.add key value acc) left right
