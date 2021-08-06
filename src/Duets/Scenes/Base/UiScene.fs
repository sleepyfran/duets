[<AutoOpen>]
module Duets.Scenes.Base.UiScene

open Nez

/// A base scene to be used when the scene contains any UI. Setups a canvas and
/// a root to add UI elements to.
[<AbstractClass>]
type UiScene() =
    inherit Scene()

    abstract member SetupView : unit -> unit

    [<DefaultValue>]
    val mutable Canvas: UICanvas

    [<DefaultValue>]
    val mutable UiRoot: Table

    override this.Initialize() =
        base.Initialize()
        this.Canvas <- this.CreateEntity("ui").AddComponent<UICanvas>()
        this.UiRoot <- this.Canvas.Stage.AddElement(Table())

        this
            .UiRoot
            .SetFillParent(true)
            .Center()
            .Defaults()
            .SetPadTop(10.0f)
            .SetMinWidth(170.0f)
            .SetMinHeight(30.0f)
        |> ignore

        this.SetupView()
