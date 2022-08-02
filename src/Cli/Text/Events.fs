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

let feelingTipsy =
    Styles.information
        "You feel a bit tipsy, your eyes start to lower a bit and you seem to have a fixed smile on your face"

let feelingDrunk =
    Styles.information
        "You feel a bit drunk, doing stuff seems a bit more difficult than before"

let feelingReallyDrunk =
    Styles.danger
        "You feel really drunk. Your eyes are blurry and your legs don't seem to be able to follow the same pattern. A part of your body asks you to stop, but the other one wants a bit more fun..."

let soberingTipsy =
    Styles.information "You're feeling much better now, just slightly tipsy"

let soberingDrunk =
    Styles.information
        "You're starting to get sober, still feeling really drunk, but better than before"
