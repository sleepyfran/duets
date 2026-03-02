namespace Duets.UI

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open Avalonia.FuncUI.Hosts
open Avalonia.Themes.Fluent
open Duets.UI.Scenes

type MainWindow() as this =
    inherit HostWindow()

    do
        base.Title <- "Duets"
        base.MinWidth <- 1280.0
        base.MinHeight <- 720.0
        this.Content <- SceneRoot.view

        this.Background <- Theme.Brush.bg
        this.Padding <- Thickness(0, 10)
        this.ExtendClientAreaToDecorationsHint <- true

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add(FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark
        this.Styles.Load "avares://Duets.UI/Styles.xaml"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            desktopLifetime.MainWindow <- mainWindow
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main (args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
