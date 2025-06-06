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

let readProgress =
    Styles.success "You open the book and start reading a few pages..."

let videoGameResults =
    [ "You got lost in the world of The Elder Scrolls, battled dragons, and saved kingdoms"
      "You told yourself 'just one more turn'... and then played Civilization for hours"
      "You strategized your next move in StarCraft, and the Zerg rush was indeed imminent!"
      "You explored the vast open world of The Witcher"
      "You tried to beat your high score in Tetris, but the blocks fell faster and faster!"
      "You built a masterpiece in Minecraft, one block at a time"
      "You got a Victory Royale in Fortnite!"
      "You delved deep into a Persona 5 playthrough, stole hearts, and changed minds"
      "You raced through the streets of Los Santos in Grand Theft Auto V"
      "You solved some intricate puzzles in Portal 2, thinking with portals!" ]
