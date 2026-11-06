module Duets.Cli.Tests.SceneGrid

open FsUnit
open NUnit.Framework

open Duets.Cli.Components.Scene

[<Test>]
let ``stamp treats spaces as transparent`` () =
    let grid = SceneGrid.create 5 1
    SceneGrid.stamp grid 0 0 "xxxxx" None
    SceneGrid.stamp grid 0 0 "a b" None

    SceneGrid.toMarkup grid |> should equal [ "axbxx" ]

[<Test>]
let ``stampOpaque overwrites with spaces`` () =
    let grid = SceneGrid.create 5 1
    SceneGrid.stamp grid 0 0 "xxxxx" None
    SceneGrid.stampOpaque grid 0 0 "a b" None

    SceneGrid.toMarkup grid |> should equal [ "a bxx" ]

[<Test>]
let ``set ignores positions outside of the grid`` () =
    let grid = SceneGrid.create 2 2
    SceneGrid.set grid -1 0 'x' None
    SceneGrid.set grid 0 5 'x' None

    SceneGrid.toMarkup grid |> should equal [ ""; "" ]

[<Test>]
let ``toMarkup groups cells with the same style and escapes brackets`` () =
    let grid = SceneGrid.create 4 1
    SceneGrid.stamp grid 0 0 "[]" (Some "red")
    SceneGrid.stamp grid 2 0 "ab" (Some "blue")

    SceneGrid.toMarkup grid |> should equal [ "[red][[]][/][blue]ab[/]" ]

[<Test>]
let ``toMarkup trims trailing spaces`` () =
    let grid = SceneGrid.create 6 1
    SceneGrid.stamp grid 0 0 "ab" None

    SceneGrid.toMarkup grid |> should equal [ "ab" ]
