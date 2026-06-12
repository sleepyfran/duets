module Duets.Common.Serializer

open Nerdbank.MessagePack
open PolyType
open PolyType.ReflectionProvider
open System
open System.Text.Json
open System.Text.Json.Serialization

let private jsonOptions =
    let options = JsonSerializerOptions()
    options.Converters.Add(JsonFSharpConverter())
    options

let private messagePackSerializer = MessagePackSerializer()

/// Deserializes a string into whichever type is passed.
let deserialize (str: string) =
    JsonSerializer.Deserialize<'a>(str, jsonOptions)

/// Serializes the input into a string.
let serialize input =
    JsonSerializer.Serialize(input, jsonOptions)

/// Deserializes MessagePack bytes into whichever type is passed.
let deserializeBinary<'a> (bytes: byte array) : 'a =
    let bytes = ReadOnlyMemory<byte>(bytes)

    messagePackSerializer.Deserialize(
        bytes,
        ReflectionTypeShapeProvider.Default.GetTypeShapeOrThrow<'a>()
    )

/// Attempts to deserialize MessagePack bytes into whichever type is passed.
let tryDeserializeBinary<'a> (bytes: byte array) : 'a option =
    try
        deserializeBinary<'a> bytes |> Some
    with _ ->
        None

/// Serializes the input into MessagePack bytes.
let serializeBinary<'a> (input: 'a) =
    let mutable input = input

    messagePackSerializer.Serialize(
        &input,
        ReflectionTypeShapeProvider.Default.GetTypeShapeOrThrow<'a>()
    )
