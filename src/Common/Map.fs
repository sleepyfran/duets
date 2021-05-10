module Common.Map

open Aether

/// Prism to a value associated with a key in a map. Similar as Aether's
/// built in but this one always introduces the key in the map regardless
/// whether it already exists or not.
let key_ (k: 'k) : Prism<Map<'k, 'v>, 'v> = Map.tryFind k, Map.add k

/// Returns the head of the map.
let head (map: Map<'k, 'v>) = map |> Seq.head |> fun kvp -> kvp.Value
