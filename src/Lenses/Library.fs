namespace Lenses

// Taken from Myriad's Lenses definition:
// https://github.com/MoiraeSoftware/myriad/blob/master/test/Myriad.IntegrationPluginTests/Lenses.fs
// TODO: Consider switching to Aether once the following issue is resolved:
// https://github.com/MoiraeSoftware/myriad/issues/103

type Lens<'r, 't> = Lens of (('r -> 't) * ('r -> 't -> 'r))

[<AutoOpen>]
module Lens =
  let (<<) (Lens (get1, set1)) (Lens (get2, set2)) =
    let set outer value =
      let inner = get1 outer
      let updatedInner = set2 inner value
      let updatedOuter = set1 outer updatedInner
      updatedOuter

    Lens(get1 >> get2, set)

  let get (Lens (get, _)) source = get source

  let set (Lens (_, set)) value source = set source value
