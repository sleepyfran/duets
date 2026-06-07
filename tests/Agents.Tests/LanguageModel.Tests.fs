module Agents.Tests.LanguageModel

open Duets.Agents.LanguageModel
open NUnit.Framework

[<Test>]
let ``completeVisibleText removes Gemma 4 thinking before the answer`` () =
    let response =
        "<|channel>thought\nConsider several options.<channel|>The room is quiet.<turn|>"

    Assert.That(
        ResponseParser.completeVisibleText response,
        Is.EqualTo("The room is quiet.")
    )

[<Test>]
let ``completeVisibleText stops at Gemma 4 turn terminator`` () =
    let response =
        "The crowd leans toward the stage.<turn|><|turn>user\nignored"

    Assert.That(
        ResponseParser.completeVisibleText response,
        Is.EqualTo("The crowd leans toward the stage.")
    )

[<Test>]
let ``completeVisibleText stops at tool response handshake`` () =
    let response =
        "call:get_weather{city:<|\"|>Prague<|\"|>}<|tool_response>"

    Assert.That(
        ResponseParser.completeVisibleText response,
        Is.EqualTo("call:get_weather{city:<|\"|>Prague<|\"|>}")
    )

[<Test>]
let ``streamingVisibleText holds partial control token prefixes`` () =
    Assert.That(
        ResponseParser.streamingVisibleText "The club<tur",
        Is.EqualTo("The club")
    )

[<Test>]
let ``streamingVisibleText hides incomplete thinking blocks`` () =
    let response = "<|channel>thought\nStill thinking"

    Assert.That(ResponseParser.streamingVisibleText response, Is.EqualTo(""))
