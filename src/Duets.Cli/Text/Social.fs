module Duets.Cli.Text.Social

open Duets.Entities

let actionPrompt date dayMoment attributes npc =
    $"""{Generic.infoBar date dayMoment attributes}
{Emoji.socializing} Talking with {npc.Name |> Styles.person}. What do you want to do?"""
    |> Styles.prompt
