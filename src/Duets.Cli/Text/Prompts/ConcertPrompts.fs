namespace Duets.Cli.Text.Prompts

open Duets.Entities

[<RequireQualifiedAccess>]
module Concert =
    /// Creates a prompt for an angry/anxious phone call from a band member who
    /// is worried because the player is late to the concert.
    let createRunningLateCallPrompt
        (callerName: string)
        (playerName: string)
        (venueName: string)
        (cityName: string)
        (attendancePercent: int)
        =
        let crowdDescription =
            match attendancePercent with
            | pct when pct < 10 -> "Barely anyone has shown up yet"
            | pct when pct < 30 -> "Only a small crowd is here so far"
            | pct when pct < 60 -> "The venue is filling up"
            | pct when pct < 85 -> "The place is pretty packed"
            | _ -> "The venue is absolutely packed and people are getting restless"

        Common.createPrompt
            $"""
You are roleplaying as {callerName}, a band member in a music group.
You are calling {playerName}, the band leader, because the concert at {venueName}
in {cityName} it's supposed to start right now and they are nowhere to be seen,
while the rest of the band is already there. {crowdDescription}.

Rules:
- You are frustrated and anxious, not just politely worried.
- Keep it short, one or two sentences of dialogue only.
- Mention the venue and the risk of the concert being cancelled if they don't show up.
- Reference the crowd situation naturally — don't just repeat it verbatim.
- Speak directly and urgently, in first person.
- Do not format the text in any way, no headers or extra line breaks.
- Only output the spoken dialogue, nothing else.
"""
