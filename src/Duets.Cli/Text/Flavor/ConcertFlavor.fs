namespace Duets.Cli.Text.Flavor

[<RequireQualifiedAccess>]
module Concert =
    /// Returns what a band member says when calling the player because they
    /// are late to the concert.
    let runningLateCall
        (playerName: string)
        (venueName: string)
        (attendancePercent: int)
        =
        let opening =
            [ $"{playerName}, where the hell are you?!"
              $"{playerName}! Finally! Where are you?"
              "Are you kidding me right now? Where are you?"
              "Please tell me you're around the corner." ]

        let crowd =
            match attendancePercent with
            | pct when pct < 10 ->
                [ "Barely anyone has shown up yet, but that's no excuse to not be here!"
                  "Okay, the place is pretty empty, but the few people here are waiting for us!" ]
            | pct when pct < 30 ->
                [ "There's a small crowd here and they keep looking at the empty stage."
                  "A few people showed up and they're already getting bored." ]
            | pct when pct < 60 ->
                [ "The venue is filling up and everyone is asking when we start."
                  "People keep coming in and we're just standing here without you." ]
            | pct when pct < 85 ->
                [ "The place is packed and people are starting to chant!"
                  "There's a proper crowd out there and they're getting impatient." ]
            | _ ->
                [ "It's absolutely packed and people are getting restless, someone just threw a cup at the stage!"
                  "The whole place is full and they're booing the empty stage!" ]

        let threat =
            [ $"If you don't get to {venueName} right now they're going to cancel the whole show!"
              $"The manager of {venueName} says they'll pull the plug if you're not here soon!"
              $"Get to {venueName} now or there won't be a concert to play!" ]

        Common.paragraph
            [ Common.pickFrom [ opening ]
              Common.pickFrom [ crowd ]
              Common.pickFrom [ threat ] ]
