#[macro_use]
extern crate lazy_static;

mod common;
mod effects;
mod screens;

use app::database::Database;
use app::serializables::GameState;

use common::action::CliAction;
use common::context;
use common::display;
use common::orchestrator;
use screens::main_menu;

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

    // Set the initial context. TODO: Load real values.
    let context = context::Context {
        database: database_or_error.unwrap(),
        game_state: GameState::default(),
    };

    context::set_global_context(context);

    let main_menu_screen = main_menu::create_main_screen();
    display::clear();
    orchestrator::start_with(CliAction::Screen(main_menu_screen));
}
