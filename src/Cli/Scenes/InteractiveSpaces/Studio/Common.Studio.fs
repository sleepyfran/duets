module Cli.Scenes.Studio.Common

open Cli.Components
open Cli.Text
open Entities

let showAlbumNameError name error =
    match error with
    | Album.NameTooShort -> StudioText StudioCreateErrorNameTooShort
    | Album.NameTooLong -> StudioText StudioCreateErrorNameTooLong
    |> I18n.translate
    |> showMessage
