mod common;
mod effects;
mod screens;

use app::database::Database;

use common::action::CliAction;
use common::context::Context;
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
                ]
            }
        "#
        .to_string(),
    );

    let context = Context {
        database: database_or_error.unwrap(),
    };

    let main_menu_screen = main_menu::create_main_screen();
    orchestrator::start_with(CliAction::Screen(main_menu_screen), context);
}
