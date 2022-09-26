namespace UI

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open Avalonia.FuncUI.Hosts
open Avalonia.Themes.Fluent
open UI.Scenes

type MainWindow() as this =
    inherit HostWindow()

    do
        base.Title <- "UI"
        base.MinWidth <- 1280.0
        base.MinHeight <- 720.0
        this.Content <- SceneRoot.view

        this.Background <- Theme.Brush.bg
        this.Padding <- Thickness(0, 10)
        this.ExtendClientAreaToDecorationsHint <- true

#if DEBUG
        this.AttachDevTools()
#endif

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add(
            FluentTheme(baseUri = null, Mode = FluentThemeMode.Dark)
        )

        this.Styles.Load "avares://UI/Styles.xaml"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main (args: string []) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
