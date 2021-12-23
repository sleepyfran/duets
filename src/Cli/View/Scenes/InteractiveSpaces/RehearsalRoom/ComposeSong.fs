module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.ComposeSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Songs.Composition.ComposeSong

let rec composeSongScene space rooms =
    seq {
        yield
            Prompt
                { Title = TextConstant ComposeSongTitlePrompt
                  Content = TextPrompt(lengthPrompt space rooms) }
    }

and lengthPrompt space rooms name =
    seq {
        yield
            Prompt
                { Title = TextConstant ComposeSongLengthPrompt
                  Content = LengthPrompt(genrePrompt space rooms name) }
    }

and genrePrompt space rooms name length =
    seq {
        yield
            Prompt
                { Title = TextConstant ComposeSongGenrePrompt
                  Content =
                      ChoicePrompt
                      <| MandatoryChoiceHandler
                          { Choices = genreOptions
                            Handler = vocalStylePrompt space rooms name length } }
    }

and vocalStylePrompt space rooms name length selectedGenre =
    let vocalStyleOptions =
        Database.vocalStyleNames ()
        |> List.map
            (fun vocalStyle ->
                { Id = vocalStyle.ToString()
                  Text = Literal(vocalStyle.ToString()) })

    seq {
        yield
            Prompt
                { Title = TextConstant ComposeSongVocalStylePrompt
                  Content =
                      ChoicePrompt
                      <| MandatoryChoiceHandler
                          { Choices = vocalStyleOptions
                            Handler =
                                handleSong
                                    space
                                    rooms
                                    name
                                    length
                                    selectedGenre.Id } }
    }

and handleSong space rooms name length genre selectedVocalStyle =
    let vocalStyle =
        Song.VocalStyle.from selectedVocalStyle.Id

    seq {
        match Song.from name length vocalStyle genre with
        | Ok song -> yield! composeWithProgressbar space rooms song
        | Error Song.NameTooShort ->
            yield!
                handleError space rooms
                <| TextConstant ComposeSongErrorNameTooShort
        | Error Song.NameTooLong ->
            yield!
                handleError space rooms
                <| TextConstant ComposeSongErrorNameTooLong
        | Error Song.LengthTooShort ->
            yield!
                handleError space rooms
                <| TextConstant ComposeSongErrorLengthTooShort
        | Error Song.LengthTooLong ->
            yield!
                handleError space rooms
                <| TextConstant ComposeSongErrorLengthTooLong
    }

and composeWithProgressbar space rooms song =
    let state = State.Root.get ()

    seq {

        yield
            ProgressBar
                { StepNames =
                      [ TextConstant ComposeSongProgressBrainstorming
                        TextConstant ComposeSongProgressConfiguringReverb
                        TextConstant ComposeSongProgressTryingChords ]
                  StepDuration = 2<second>
                  Async = true }

        yield
            Message
            <| TextConstant(ComposeSongConfirmation song.Name)

        yield Effect <| composeSong state song

        yield Scene(Scene.RehearsalRoom(space, rooms))
    }

and handleError space rooms message =
    seq {
        yield Message <| message
        yield SubScene(SubScene.RehearsalRoomComposeSong(space, rooms))
    }
