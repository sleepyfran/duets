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
                        "name": "Czech Republic",
                        "population": 10690000,
                        "cities": [
                            {
                                "name": "Prague",
                                "countryName": "Czech Republic",
                                "population": 1309000
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

    let context = context::Context {
        database: database_or_error.unwrap(),
        game_state: game_state_option.clone().unwrap_or_default(),
    };

    context::set_global_context(context);

    let main_menu_screen = main_menu::create_main_screen(game_state_option);
    display::clear();
    orchestrator::start_with(CliAction::Screen(main_menu_screen));
}
