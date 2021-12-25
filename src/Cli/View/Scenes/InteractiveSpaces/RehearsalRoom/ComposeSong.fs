module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.ComposeSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Songs.Composition.ComposeSong

let rec composeSongSubScene () =
    seq {
        yield
            Prompt
                { Title = TextConstant ComposeSongTitlePrompt
                  Content = TextPrompt(lengthPrompt) }
    }

and lengthPrompt name =
    seq {
        yield
            Prompt
                { Title = TextConstant ComposeSongLengthPrompt
                  Content = LengthPrompt(genrePrompt name) }
    }

and genrePrompt name length =
    seq {
        yield
            Prompt
                { Title = TextConstant ComposeSongGenrePrompt
                  Content =
                      ChoicePrompt
                      <| MandatoryChoiceHandler
                          { Choices = genreOptions
                            Handler = vocalStylePrompt name length } }
    }

and vocalStylePrompt name length selectedGenre =
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
                            Handler = handleSong name length selectedGenre.Id } }
    }

and handleSong name length genre selectedVocalStyle =
    let vocalStyle =
        Song.VocalStyle.from selectedVocalStyle.Id

    seq {
        match Song.from name length vocalStyle genre with
        | Ok song -> yield! composeWithProgressbar song
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

and composeWithProgressbar song =
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

        yield Scene Scene.World
    }

and handleError message =
    seq {
        yield Message <| message
        yield! composeSongSubScene ()
    }
