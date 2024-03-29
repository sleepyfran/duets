[<RequireQualifiedAccess>]
module Duets.Cli.Text.Interaction

let eatResult = Styles.success "You ate something and restored a bit of health"

let exerciseResult = Styles.success "You exercised and restored a bit of health"

let exerciseSteps =
    [ "Sweating..."; "Staring at the mirror..."; "Drinking water..." ]

let sleeping = "Zzz..."

let sleepResult =
    Styles.success "You got a good night sleep and feel much better"

let watchTvResult =
    Styles.success "That mindless channel switching cheered you up a bit"
