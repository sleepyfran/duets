module Duets.Scenes.BandCreator

open Entities
open Duets.Scenes.Base
open Duets.Text.Constants
open Duets.Types

type BandCreatorScene(navigator: INavigator, character: Character) =
    inherit UiScene()

    let mutable bandName = ""
    let mutable bandGenre: Genre = "Blackgaze"
    let mutable characterRole = Guitar

    override this.OnStart() =
        this.UiRoot.AddText
            (TextConstant BandCreatorTitle)
            TextSize.Title
            centered

        this.UiRoot.AddText
            (TextConstant <| BandCreatorPrompt character.Name)
            TextSize.Body
            centered

        this.UiRoot.AddInput
            (TextConstant BandCreatorBandName)
            (fun text -> bandName <- text)
            centered

        this.UiRoot.AddSelector
            (TextConstant BandCreatorBandGenre)
            (Database.genres ())
            (fun genreId -> bandGenre <- genreId)
            centered

        this.UiRoot.AddSelector
            (TextConstant
             <| BandCreatorCharacterInstrument character.Name)
            (Database.roles ())
            (fun roleId -> characterRole <- Instrument.Type.from roleId)
            centered

        this.UiRoot.AddButton
            (TextConstant BandCreatorCreateLabel)
            (fun () -> navigator.NavigateWithTransition Game)
            centered

        ()
