use app::operations::start::SavegameState;

use super::GameScreen;
use crate::effects;
use crate::shared::action::{Choice, CliAction, ConfirmationChoice, Prompt};
use crate::shared::context::Context;
use crate::shared::display;
use crate::shared::screen::Screen;

pub fn create_main_screen(savegame: SavegameState) -> Screen {
    Screen {
        name: String::from("Main Menu"),
        action: Prompt::ChoiceInput {
            text: r#"
.:::::                        .::         
.::   .::                     .::         
.::    .::.::  .::   .::    .:.: .: .:::: 
.::    .::.::  .:: .:   .::   .::  .::    
.::    .::.::  .::.::::: .::  .::    .::: 
.::   .:: .::  .::.:          .::      .::
.:::::      .::.::  .::::      .:: .:: .::

Welcome to Duets! Select an option to begin:
            "#
            .into(),
            choices: vec![
                Choice {
                    id: 0,
                    text: String::from("Start new game"),
                },
                Choice {
                    id: 1,
                    text: String::from("Load game"),
                },
                Choice {
                    id: 2,
                    text: String::from("Exit"),
                },
            ],
            on_action: Box::new(|choice, global_context| match choice.id {
                0 => new_game_selected(savegame, global_context),
                1 => load_game_selected(savegame, global_context),
                2 => CliAction::SideEffect(Box::new(effects::exit)),
                _ => CliAction::NoOp,
            }),
        },
    }
}

fn new_game_selected(savegame: SavegameState, global_context: &Context) -> CliAction {
    match &savegame {
        SavegameState::Ok(_) => {
            display::show_line_break();
            display::show_warning(&String::from(
                "You already have a savegame, continuing with this will override it",
            ));
            CliAction::Prompt(Prompt::ConfirmationInput {
                text: String::from("Are you sure you want to create a new game?"),
                on_action: Box::new(|choice, global_context| match choice {
                    ConfirmationChoice::Yes => {
                        CliAction::Screen(GameScreen::NewGame)
                    }
                    ConfirmationChoice::No => CliAction::Screen(GameScreen::MainMenu(savegame)),
                }),
            })
        }
        SavegameState::None(_) => {
            CliAction::Screen(GameScreen::NewGame)
        }
        _ => unreachable!(),
    }
}

fn load_game_selected(savegame: SavegameState, global_context: &Context) -> CliAction {
    match &savegame {
        SavegameState::Ok(_) => CliAction::Screen(GameScreen::Home),
        SavegameState::None(_) => {
            display::show_line_break();
            display::show_error(&String::from(
                "You don't have any savegame currently. Create a new one",
            ));
            display::show_line_break();
            CliAction::Screen(GameScreen::MainMenu(savegame))
        }
        _ => unreachable!(),
    }
}
