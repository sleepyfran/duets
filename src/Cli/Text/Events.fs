[<RequireQualifiedAccess>]
module Cli.Text.Events

let healthDepletedFirst =
    Styles.danger "You start to feel lightheaded and your vision begins to blur"

let healthDepletedSecond =
    Styles.danger
        "The background noise starts to fade into the background and a buzz grows inside your ear..."

let hospitalized =
    Styles.information
        "You wake up in the hospital a week later. Your head hurts a bit, but other than that it seems like you will make it this time"
