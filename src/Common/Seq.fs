module Common.Seq

/// Applies a side-effect to each element of a sequence similar to how Seq.iter
/// works but returning the element that is being processed afterwards.
let tap fn =
    Seq.map (fun item ->
        fn item
        item)
