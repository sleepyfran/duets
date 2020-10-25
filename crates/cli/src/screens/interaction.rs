use game::world::interactions::{InteractInput, InteractItem, Interaction};
use game::world::interactions;

use crate::context::{Context, ScreenContext};
use crate::shared::screen::Screen;
use crate::shared::action::CliAction;
use crate::shared::display;
use crate::effects;

pub type InteractionContext = ScreenContext<InteractInput>;

pub fn create_interaction_screen(
    interaction: &dyn Interaction,
    sequence: InteractItem,
    context: Context,
) -> Screen {
    Screen {
        name: "Interaction".into(),
        action: action_chain_from_sequence(interaction, sequence, &InteractionContext {
            global_context: context.clone(),
            state: InteractInput {
                input: vec![],
                context
            },
            next_action: None,
        })
    }
}

fn action_chain_from_sequence(
    interaction: &dyn Interaction,
    sequence: InteractItem,
    context: &InteractionContext,
) -> CliAction {
    match sequence {
        InteractItem::Chain(first, second) => {
            CliAction::Chain(
                Box::new(action_chain_from_sequence(interaction, *first, context)),
                Box::new(action_chain_from_sequence(interaction, *second, context))
            )
        },
        InteractItem::Options(_options) => {
            CliAction::Continue
        },
        InteractItem::End => {
            let result = interactions::result(interaction, &context.global_context);
            display::show_info(&result.0);
            effects::set_state(result.1.game_state)
        },
    }
}