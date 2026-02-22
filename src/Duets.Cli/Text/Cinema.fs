[<RequireQualifiedAccess>]
module Duets.Cli.Text.Cinema

let watchMovieMessage =
    Styles.progress "You settle into your seat as the lights dim and the movie begins..."

let ticketPurchased (movieTitle: string) =
    Styles.success $"You got a ticket for {movieTitle}! Head to the screening room to watch it."

let movieFinished (movieTitle: string) (quality: int) =
    let feeling =
        match quality with
        | q when q <= 3 -> "It was pretty bad, but at least you got out of the house."
        | q when q <= 6 -> "It was decent enough."
        | q when q <= 8 -> "You really enjoyed it!"
        | _ -> "That was an amazing film!"

    Styles.success $"{movieTitle} is over. {feeling}"
