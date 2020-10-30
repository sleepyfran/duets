mod effects;
mod instruments;
mod interface;
mod requirement;

use pipe_trait::*;
use std::rc::Rc;

pub use interface::*;
pub use requirement::*;

use common::entities::{Object, ObjectType};
use simulation::commands::action_registry::ActionRegistryCommands;
use simulation::queries::action_registry::ActionRegistryQueries;

use crate::context::Context;

/// Returns all the available interactions for the given object.
pub fn get_for(object: &Object) -> Vec<Rc<dyn Interaction>> {
    match &object.r#type {
        ObjectType::Instrument(instrument) => instruments::get_interactions(instrument),
        _ => vec![],
    }
}

/// Checks for all the requirements of the given interaction and, if all passed, returns the
/// sequence needed to execute the interaction.
pub fn sequence(interaction: &dyn Interaction, context: &Context) -> InteractSequence {
    check_requirements(context, interaction.requirements())?;
    interaction.sequence(context)
}

/// Applies all the effects given in the interaction to the context.
pub fn result(interaction: &dyn Interaction, input: SequenceInput) -> (String, Context) {
    let can_interact = check_interacted_within_limit(interaction, &input);

    register_action(interaction, &input)
        .pipe(|input| process_always_applied_effects(interaction, &input))
        .pipe(|input| {
            if can_interact {
                process_applied_after_interaction_effects(interaction, &input)
            } else {
                input
            }
        })
        .pipe(|input| {
            let messages = interaction.messages(&input.context);
            if can_interact {
                (messages.0, input.context)
            } else {
                (messages.1, input.context)
            }
        })
}

fn check_interacted_within_limit(interaction: &dyn Interaction, input: &SequenceInput) -> bool {
    let tracking_available = interaction.track_action();
    if tracking_available {
        let interaction_limit = interaction.limit_daily_interactions();
        match interaction_limit {
            InteractionTimes::Unlimited => true,
            InteractionTimes::Once => check_interaction_performed_at_most(1, interaction, input),
            InteractionTimes::Multiple(times) => {
                check_interaction_performed_at_most(times, interaction, input)
            }
        }
    } else {
        true
    }
}

fn check_interaction_performed_at_most(
    times: u8,
    interaction: &dyn Interaction,
    input: &SequenceInput,
) -> bool {
    let today_actions = input
        .context
        .game_state
        .action_registry
        .get_from_date(&interaction.id(), input.context.game_state.calendar.date);

    today_actions.len() <= times.into()
}

fn register_action(interaction: &dyn Interaction, input: &SequenceInput) -> SequenceInput {
    let tracking_available = interaction.track_action();
    if tracking_available {
        input
            .clone()
            .with_context(&input.context.clone().modify_game_state(|game_state| {
                game_state.modify_action_registry(|action_registry| {
                    action_registry.register(&interaction.id(), &input.context.game_state.calendar)
                })
            }))
    } else {
        input.clone()
    }
}

fn process_always_applied_effects(
    interaction: &dyn Interaction,
    input: &SequenceInput,
) -> SequenceInput {
    process_effects(interaction.effects(&input.context).always_applied, input)
}

fn process_applied_after_interaction_effects(
    interaction: &dyn Interaction,
    input: &SequenceInput,
) -> SequenceInput {
    process_effects(
        interaction
            .effects(&input.context)
            .applied_after_interaction,
        input,
    )
}

fn process_effects(effects: Vec<InteractionEffect>, input: &SequenceInput) -> SequenceInput {
    effects.into_iter().fold(input.clone(), |input, effect| {
        process_effect(effect, &input)
    })
}

fn process_effect(effect: InteractionEffect, input: &SequenceInput) -> SequenceInput {
    let updated_context = match effect {
        InteractionEffect::Time(consumption) => effects::time::apply(consumption, &input.context),
        InteractionEffect::Health(effect) => effects::health::apply(effect, &input.context),
        InteractionEffect::Energy(effect) => effects::energy::apply(effect, &input.context),
        InteractionEffect::Skill(skill, effect) => {
            effects::skills::apply(skill, effect, &input.context)
        }
        InteractionEffect::Song(effect) => effects::song::apply(effect, input),
    };

    input.clone().with_context(&updated_context)
}
