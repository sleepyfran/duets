module Duets.Scenes.MainMenu

open System
open Nez

open Nez.UI

type MainMenuScene() =
    inherit Scene()

    [<DefaultValue>]
    val mutable Canvas: UICanvas

    [<DefaultValue>]
    val mutable UiRoot: Table

    override this.Initialize() =
        base.Initialize()

        this.Canvas <- this.CreateEntity("ui").AddComponent<UICanvas>()
        this.SetupView()

    member this.SetupView() =
        let skin = Skin.CreateDefaultSkin()

        this.UiRoot <- this.Canvas.Stage.AddElement(Table())

        this
            .UiRoot
            .Defaults()
            .SetPadTop(10.0f)
            .SetMinWidth(170.0f)
            .SetMinHeight(30.0f)
        |> ignore

        this.UiRoot.SetFillParent(true).Center() |> ignore

        this
            .UiRoot
            .Add(Label("Duets").SetFontScale(10.0f))
            .Center()
        |> ignore

        this.UiRoot.Row() |> ignore

        this
            .UiRoot
            .Add(TextButton("Create new game", skin))
            .GetElement<TextButton>()
            .add_OnClicked (fun _ -> printf "Huh")

        this.UiRoot.Row() |> ignore

        this
            .UiRoot
            .Add(TextButton("Exit", skin))
            .GetElement<TextButton>()
            .add_OnClicked (fun _ -> Environment.Exit 0)

        this.UiRoot.Row() |> ignore

        ()
