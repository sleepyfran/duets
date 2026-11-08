module Duets.Cli.Tests.CommandInput

open FsUnit
open NUnit.Framework

open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex

let private command name description =
    { Name = name
      Description = description
      Handler = fun _ -> Scene.World }

let private look = command "look" "Look around"
let private composeSong = command "compose song" "Compose a new song"
let private composeAlbum = command "compose album" "Compose a new album"
let private cheats = command "secretcheats" ""

let private clock = command "clock" "Shows the time"
let private startConversation = command "start conversation" "Talk"

let private commands =
    [ look; composeSong; composeAlbum; clock; startConversation ]

let private analyze = CommandInput.analyze commands [ cheats ]

let private caseName analysis =
    match analysis with
    | CommandInput.Empty -> "Empty"
    | CommandInput.Matched _ -> "Matched"
    | CommandInput.Partial _ -> "Partial"
    | CommandInput.Hidden -> "Hidden"
    | CommandInput.Unknown -> "Unknown"

let private matchedName analysis =
    match analysis with
    | CommandInput.Matched command -> Some command.Name
    | _ -> None

let private partialNames analysis =
    match analysis with
    | CommandInput.Partial candidates -> candidates |> List.map _.Name
    | _ -> []

[<Test>]
let ``analyze returns Empty for blank input`` () =
    analyze "" |> caseName |> should equal "Empty"
    analyze "   " |> caseName |> should equal "Empty"

[<Test>]
let ``analyze matches single and multi-word commands`` () =
    analyze "look" |> matchedName |> should equal (Some "look")

    analyze "compose song"
    |> matchedName
    |> should equal (Some "compose song")

[<Test>]
let ``analyze matches commands followed by arguments`` () =
    analyze "look at bed" |> matchedName |> should equal (Some "look")

[<Test>]
let ``analyze does not match a command that is only a prefix of a word`` () =
    analyze "lookx" |> caseName |> should equal "Unknown"

[<Test>]
let ``analyze returns all candidates for a partial input`` () =
    analyze "comp"
    |> partialNames
    |> should equal [ "compose song"; "compose album" ]

    analyze "compose s" |> partialNames |> should equal [ "compose song" ]

[<Test>]
let ``analyze returns Unknown for input that can't become a command`` () =
    analyze "dance" |> caseName |> should equal "Unknown"

[<Test>]
let ``analyze does not reveal hidden commands`` () =
    analyze "secr" |> caseName |> should equal "Hidden"
    analyze "secretcheats" |> caseName |> should equal "Hidden"

[<Test>]
let ``highlight splits the command name from its arguments`` () =
    let input = "compose song  my song"

    CommandInput.highlight (analyze input) input
    |> List.map fst
    |> should equal [ "compose song"; "  my song" ]

[<Test>]
let ``highlight styles unknown input and leaves partial input plain`` () =
    CommandInput.highlight (analyze "dance") "dance"
    |> List.map (snd >> Option.isSome)
    |> should equal [ true ]

    CommandInput.highlight (analyze "comp") "comp"
    |> List.map (snd >> Option.isSome)
    |> should equal [ false ]

[<Test>]
let ``hint shows the description of the matched command`` () =
    CommandInput.hint (analyze "look at bed")
    |> Option.map Spectre.Console.Markup.Remove
    |> should equal (Some "Look around")

[<Test>]
let ``hint lists the candidates of a partial input`` () =
    let hint =
        CommandInput.hint (analyze "comp")
        |> Option.map Spectre.Console.Markup.Remove

    hint.Value |> should contain "compose song · compose album"

[<Test>]
let ``hint is empty for blank and hidden input`` () =
    CommandInput.hint (analyze "") |> should equal None
    CommandInput.hint (analyze "secr") |> should equal None

[<Test>]
let ``completions complete the first word of matching commands`` () =
    CommandInput.completions commands "" "c"
    |> should equal [ "compose"; "clock" ]

    CommandInput.completions commands "" "lo" |> should equal [ "look" ]

[<Test>]
let ``completions only consider commands that start with the previous words`` () =
    CommandInput.completions commands "start " "c"
    |> should equal [ "conversation" ]

    CommandInput.completions commands "compose " ""
    |> should equal [ "song"; "album" ]

[<Test>]
let ``completions are empty when no command starts with the input`` () =
    CommandInput.completions commands "" "x" |> should be Empty
    CommandInput.completions commands "look " "a" |> should be Empty
