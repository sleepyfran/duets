module Cli.View.Scenes.Interactive.RehearsalRoom.ComposeSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Songs.Composition.ComposeSong

let rec composeSongScene state =
    seq {
        yield
            Prompt
                { Title = TextConstant ComposeSongTitlePrompt
                  Content = TextPrompt(lengthPrompt state) }
    }

and lengthPrompt state name =
    seq {
        yield
            Prompt
                { Title = TextConstant ComposeSongLengthPrompt
                  Content = LengthPrompt(genrePrompt state name) }
    }

and genrePrompt state name length =
    seq {
        yield
            Prompt
                { Title = TextConstant ComposeSongGenrePrompt
                  Content =
                      ChoicePrompt
                      <| MandatoryChoiceHandler
                          { Choices = genreOptions
                            Handler = vocalStylePrompt state name length } }
    }

and vocalStylePrompt state name length selectedGenre =
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
                                handleSong state name length selectedGenre.Id } }
    }

and handleSong state name length genre selectedVocalStyle =
    let vocalStyle =
        Song.VocalStyle.from selectedVocalStyle.Id

    seq {
        match Song.from name length vocalStyle genre with
        | Ok song -> yield! composeWithProgressbar state song
        | Error Song.NameTooShort ->
            yield!
                handleError
                <| TextConstant ComposeSongErrorNameTooShort
        | Error Song.NameTooLong ->
            yield!
                handleError
                <| TextConstant ComposeSongErrorNameTooLong
        | Error Song.LengthTooShort ->
            yield!
                handleError
                <| TextConstant ComposeSongErrorLengthTooShort
        | Error Song.LengthTooLong ->
            yield!
                handleError
                <| TextConstant ComposeSongErrorLengthTooLong
    }

and composeWithProgressbar state song =
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

        yield SceneAfterKey RehearsalRoom
    }

and handleError message =
    seq {
        yield Message <| message
        yield SubScene SubScene.RehearsalRoomComposeSong
    }
