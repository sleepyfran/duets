module UI.Theme

open Avalonia.Media
open Avalonia.Media.Immutable

module Color =
    let bg = 0x0ff414141u
    let containerBg = 0x0ff212121u
    let destructive = 0x0ffB2474Du

module Brush =
    let bg =
        ImmutableSolidColorBrush(Color.FromUInt32(Color.bg))

    let containerBg =
        ImmutableSolidColorBrush(Color.FromUInt32(Color.containerBg))

    let destructive =
        ImmutableSolidColorBrush(Color.FromUInt32(Color.destructive))
