module Ui.Screens.Creator

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Elmish
open Ui.Types

type Msg = unit

let init () : Msg * Cmd<_> =
    (), Cmd.none

let update msg state : PreGameState * Cmd<_> =
    state, Cmd.none
    
let view (state: PreGameState) dispatch =
    TextBlock.create [
        TextBlock.text "Creator"
    ]