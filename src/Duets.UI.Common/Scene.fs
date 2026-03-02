namespace Duets.UI.Common

open Duets.Entities

/// A renderer abstracts over how content is displayed and how user input is
/// collected. CLI and GUI provide separate implementations.
type IRenderer =
    // ── Display ──────────────────────────────────────────────────────────────
    abstract Show: ShowContent -> Async<unit>

    // ── Generic asks (return type is the generic parameter) ──────────────────
    abstract Ask<'T> : AskContent<'T> -> Async<'T>

    abstract AskMultiple<'T> :
        values: 'T list * display: ('T -> string) -> Async<'T list>

    // ── Typed asks (return type is fixed) ────────────────────────────────────
    abstract Confirm: question: string -> Async<bool>
    abstract Continue: unit -> Async<unit>
    abstract AskCity: prompt: string -> Async<City option>
    abstract AskDate: title: string * initial: Date -> Async<Date option>
    abstract AskCommand: title: string * commands: obj list -> Async<obj>

/// A scene is a function from a renderer to an async result.
/// Callers never see the renderer — they only use the smart constructors below.
type Scene<'T> = IRenderer -> Async<'T>

type SceneBuilder() =
    member _.Return(x) : Scene<'T> = fun _ -> async.Return x
    member _.ReturnFrom(s: Scene<'T>) : Scene<'T> = s
    member _.Zero() : Scene<unit> = fun _ -> async.Return()
    member _.Delay(f: unit -> Scene<'T>) : Scene<'T> = fun r -> f () r

    member _.Combine(a: Scene<unit>, b: Scene<'T>) : Scene<'T> =
        fun r -> async.Bind(a r, fun () -> b r)

    member _.Bind(run: Scene<'a>, f: 'a -> Scene<'b>) : Scene<'b> =
        fun r -> async.Bind(run r, fun x -> f x r)

    member _.For(xs: 'a seq, f: 'a -> Scene<unit>) : Scene<unit> =
        fun r ->
            async {
                for x in xs do
                    do! f x r
            }

    member _.While(guard, body: Scene<unit>) : Scene<unit> =
        fun r ->
            async {
                while guard () do
                    do! body r
            }

    member _.TryWith(run: Scene<'T>, h: exn -> Scene<'T>) : Scene<'T> =
        fun r ->
            async {
                try
                    return! run r
                with ex ->
                    return! h ex r
            }

    member _.TryFinally(run: Scene<'T>, comp: unit -> unit) : Scene<'T> =
        fun r ->
            async {
                try
                    return! run r
                finally
                    comp ()
            }

[<AutoOpen>]
module Scene =

    let scene = SceneBuilder()

    // ── Show helpers ──────────────────────────────────────────────────────────

    let show content : Scene<unit> = fun r -> r.Show content
    let showText msg : Scene<unit> = show (ShowContent.Text msg)
    let showFiglet text : Scene<unit> = show (ShowContent.Figlet text)
    let showGameInfo ver : Scene<unit> = show (ShowContent.GameInfo ver)
    let showSep label : Scene<unit> = show (ShowContent.Separator label)
    let lineBreak: Scene<unit> = show ShowContent.LineBreak
    let wait ms : Scene<unit> = show (ShowContent.Wait ms)
    let stream seq : Scene<unit> = show (ShowContent.LLMStream seq)

    let showProgress steps dur : Scene<unit> =
        show (ShowContent.Progress(steps, dur))

    let showNotification t m : Scene<unit> =
        show (ShowContent.Notification(t, m))

    let showTip t m : Scene<unit> = show (ShowContent.Tip(t, m))
    let showTips tips : Scene<unit> = show (ShowContent.Tips tips)
    let showBarChart items : Scene<unit> = show (ShowContent.BarChart items)

    // ── Ask helpers ───────────────────────────────────────────────────────────

    let askChoice vs disp : Scene<'T> =
        fun r -> r.Ask(AskContent.Choice(vs, disp))

    let askSearch vs disp : Scene<'T> =
        fun r -> r.Ask(AskContent.SearchChoice(vs, disp))

    let askText ph : Scene<string> =
        fun r -> r.Ask(AskContent.TextBox(ph, Some))

    let askParsed ph p : Scene<'T> =
        fun r -> r.Ask(AskContent.TextBox(ph, p))

    let askMany vs disp : Scene<'T list> = fun r -> r.AskMultiple(vs, disp)
    let askConfirm q : Scene<bool> = fun r -> r.Confirm q
    let askContinue: Scene<unit> = fun r -> r.Continue()
    let askCity prompt : Scene<City option> = fun r -> r.AskCity prompt
    let askDate t init : Scene<Date option> = fun r -> r.AskDate(t, init)
