module Data.ResourceLoader

open Common

/// Loads the content of a given file key.
let internal load key =
    Files.dataFile key
    |> Files.readAll
    |> Option.defaultValue ""
    |> Serializer.deserialize
