#[macro_use]
extern crate lazy_static;

mod effects;
mod screens;
mod shared;

use app::database::Database;
use common::serializables::GameState;
use storage;

use screens::main_menu;
use shared::action::CliAction;
use shared::context;
use shared::display;
use shared::orchestrator;

fn main() {
    // TODO: Init the app loading the database from the server and not from the mocks.
    let database_or_error = Database::init_with(
        r#"
            {
                "compatibleWith": "0.1.0",
                "countries": [
                    {
                        "id": "CZK",
                        "name": "Czech Republic",
                        "population": 10690000,
                        "cities": [
                            {
                                "id": "PRG",
                                "name": "Prague",
                                "population": 1309000,
                                "places": [
                                    {
                                        "id": "test_1",
                                        "name": "Test Place",
                                        "rooms": [
                                            {
                                                "id": "test_1",
                                                "description": "A dark room just to test"
                                            }
                                        ]
                                    },
                                    {
                                        "id": "test_2",
                                        "name": "Test Place 2",
                                        "rooms": [
                                            {
                                                "id": "test_1",
                                                "description": "A dark room just to test, but again"
                                            }
                                        ]
                                    },
                                    {
                                        "id": "test_3",
                                        "name": "Test Place 3",
                                        "rooms": [
                                            {
                                                "id": "test_1",
                                                "description": "A dark room just to test, but again"
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ],
                "genres": [
                    {
                        "name": "Blackgaze",
                        "compatibleWith": []
                    }
                ],
                "instruments": [
                    {
                        "name": "Guitar",
                        "allowsAnotherInstrument": true
                    }
                ]
            }
        "#
        .to_string(),
    );

    let game_state_option = storage::retrieve_game_state().ok();

    match game_state_option {
        Some(game_state) => load_and_start(database_or_error.unwrap(), game_state),
        None => start_from_scratch(database_or_error.unwrap()),
    }
}

fn load_and_start(database: Database, game_state: GameState) {
    let context = context::Context {
        database: database,
        game_state: game_state.clone(),
    };

    match context.validate_integrity() {
        Err(error) => {
            display::show_error(&error);
            return;
        }
        _ => start_with(context, Some(game_state)),
    }
}

fn start_from_scratch(database: Database) {
    let context = context::Context {
        database: database,
        game_state: GameState::default(),
    };

    start_with(context, None)
}

fn start_with(context: context::Context, game_state_option: Option<GameState>) {
    context::set_global_context(context);

    let main_menu_screen = main_menu::create_main_screen(game_state_option);
    display::clear();
    orchestrator::start_with(CliAction::Screen(main_menu_screen));
}
