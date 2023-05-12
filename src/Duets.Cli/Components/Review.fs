[<AutoOpen>]
module Duets.Cli.Components.Review

open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Spectre.Console

let private reviewerName reviewerId =
    match reviewerId with
    | Metacritic -> "Metacritic"
    | Pitchfork -> "Pitchfork"
    | RateYourMusic -> "Rate Your Music"
    | SputnikMusic -> "Sputnik Music"

let private scoreDescription score =
    match score with
    | s when s < 20 ->
        [ "Truly abysmal"
          "A complete failure"
          "A disaster"
          "This album is like a failed science experiment, where the only conclusion is that music can indeed make your ears bleed"
          "I recommend it if you're looking for a unique way to torture your enemies"
          "Listening to this album was like trying to untangle earbuds after they've been in your pocket for a week"
          "I would rather listen to a chorus of barking dogs on helium"
          "I would rather listen to nails on a chalkboard"
          $"""I would rather listen to a ten hour loop of {Styles.highlight "Baby Shark"}"""
          $"""I would rather listen to {Styles.highlight "Nickelback"} on repeat for a week"""
          "Worst album of the year" ]
    | s when s < 50 ->
        [ "Nice effort, but not quite there yet"
          "It's like musical oatmeal—filling, but lacking any real flavor"
          "Listening to this album is like riding a stationary bike in a gym while staring at a motivational poster"
          "This album is like that okay-ish friend who always brings store-bought cookies to a potluck"
          "This album is a mixed bag"
          "Not bad, not good either"
          "A bit of a letdown" ]
    | s when s < 80 ->
        [ "Not great, not terrible"
          "A solid album"
          "Fine."
          "Balanced, as all things should be"
          "The album we deserve, but not the one we need right now" ]
    | _ ->
        [ "Absolutely amazing"
          "A gorgeous album"
          "If you don't like this album, you're wrong"
          "A masterpiece"
          "This album is so good, it should come with a warning label: Highly addictive"
          "If Picasso was a musician, he would have made this album"
          "If Shakespeare was a musician, he would have made this album" ]
    |> List.sample

let showReview (review: Review) =
    let panelBody =
        $"""
    "{scoreDescription review.Score |> Styles.faded}"
    Score: {Styles.Level.from review.Score}
    """

    Panel(
        Markup(panelBody),
        Header = PanelHeader(reviewerName review.Reviewer |> Styles.header),
        Border = BoxBorder.Rounded,
        Expand = false
    )
    |> AnsiConsole.Write

let showReviews album =
    AnsiConsole.AlternateScreen(fun () ->
        showSeparator None

        album.Reviews
        |> List.iter (fun review ->
            showReview review
            wait 1500<millisecond>)

        showSeparator None
        showContinuationPrompt ())
