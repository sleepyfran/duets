module Duets.Cli.Text.Social

open Duets.Entities

let actionPrompt date dayMoment attributes npc relationshipLevel =
    $"""{Generic.infoBar date dayMoment attributes}
{Emoji.socializing} Talking with {npc.Name |> Styles.person} | {Emoji.relationshipLevel} {relationshipLevel}
What do you want to do?"""
    |> Styles.prompt

let relationshipType =
    function
    | Friend -> "Friend"
    | Bandmate -> "Bandmate"
