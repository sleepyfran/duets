module Serializer

open System.Text.Json
open System.Text.Json.Serialization

let private jsonOptions =
  let options = JsonSerializerOptions()
  options.Converters.Add(JsonFSharpConverter())
  options

/// Deserializes a string into whichever type is passed.
let deserialize (str: string) =
  JsonSerializer.Deserialize<'a>(str, jsonOptions)

/// Serializes the input into a string.
let serialize input =
  JsonSerializer.Serialize(input, jsonOptions)
