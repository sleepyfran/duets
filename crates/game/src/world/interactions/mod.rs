mod effects;
mod instruments;
mod interface;
mod outcomes;
mod requirement;

use pipe_trait::*;
use std::rc::Rc;

pub use interface::*;
pub use outcomes::*;
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
pub fn result(interaction: &dyn Interaction, input: SequenceInput) -> SequenceOutput {
    let output = SequenceOutput {
        values: input.values,
        context: input.context,
        outcomes: vec![],
    };
    let can_interact = check_interacted_within_limit(interaction, &output);

    register_action(interaction, &output)
        .pipe(|_| process_always_applied_effects(interaction, &output))
        .pipe(|output| {
            if can_interact {
                process_applied_after_interaction_effects(interaction, &output)
            } else {
                output
            }
        })
}

fn check_interacted_within_limit(interaction: &dyn Interaction, output: &SequenceOutput) -> bool {
    let tracking_available = interaction.track_action();
    if tracking_available {
        let interaction_limit = interaction.limit_daily_interactions();
        match interaction_limit {
            InteractionTimes::Unlimited => true,
            InteractionTimes::Once => check_interaction_performed_at_most(1, interaction, output),
            InteractionTimes::Multiple(times) => {
                check_interaction_performed_at_most(times, interaction, output)
            }
        }
    } else {
        true
    }
}

fn check_interaction_performed_at_most(
    times: u8,
    interaction: &dyn Interaction,
    output: &SequenceOutput,
) -> bool {
    let today_actions = output
        .context
        .game_state
        .action_registry
        .get_from_date(&interaction.id(), output.context.game_state.calendar.date);

    today_actions.len() <= times.into()
}

fn register_action(interaction: &dyn Interaction, output: &SequenceOutput) -> SequenceOutput {
    let tracking_available = interaction.track_action();
    if tracking_available {
        output.clone().modify_context(|context| {
            context.modify_game_state(|game_state| {
                game_state.modify_action_registry(|action_registry| {
                    action_registry.register(&interaction.id(), &output.context.game_state.calendar)
                })
            })
        })
    } else {
        output.clone()
    }
}

fn process_always_applied_effects(
    interaction: &dyn Interaction,
    output: &SequenceOutput,
) -> SequenceOutput {
    process_effects(interaction.effects(&output.context).always_applied, output)
}

fn process_applied_after_interaction_effects(
    interaction: &dyn Interaction,
    output: &SequenceOutput,
) -> SequenceOutput {
    process_effects(
        interaction
            .effects(&output.context)
            .applied_after_interaction,
        output,
    )
}

fn process_effects(effects: Vec<InteractionEffect>, output: &SequenceOutput) -> SequenceOutput {
    effects.into_iter().fold(output.clone(), |output, effect| {
        process_effect(effect, &output)
    })
}

fn process_effect(effect: InteractionEffect, output: &SequenceOutput) -> SequenceOutput {
    match effect {
        InteractionEffect::Time(consumption) => effects::time::apply(consumption, &output),
        InteractionEffect::Health(effect) => effects::health::apply(effect, &output),
        InteractionEffect::Energy(effect) => effects::energy::apply(effect, &output),
        InteractionEffect::Skill(skill, effect) => effects::skills::apply(skill, effect, &output),
        InteractionEffect::Song(effect) => effects::song::apply(effect, &output),
    }
}
