mod effects;
mod instruments;
mod interact;
mod requirement;

use pipe_trait::*;
use std::rc::Rc;

pub use interact::*;
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
pub fn result(interaction: &dyn Interaction, input: InteractInput) -> (String, Context) {
    let can_interact = check_interacted_within_limit(interaction, &input.context);

    register_action(interaction, &input.context)
        .pipe(|ctx| process_always_applied_effects(interaction, &ctx))
        .pipe(|ctx| {
            if can_interact {
                process_applied_after_interaction_effects(interaction, &ctx)
            } else {
                ctx
            }
        })
        .pipe(|ctx| {
            let messages = interaction.messages(&input.context);
            if can_interact {
                (messages.0, ctx)
            } else {
                (messages.1, ctx)
            }
        })
}

fn check_interacted_within_limit(interaction: &dyn Interaction, context: &Context) -> bool {
    let tracking_available = interaction.track_action();
    if tracking_available {
        let interaction_limit = interaction.limit_daily_interactions();
        match interaction_limit {
            InteractionTimes::Unlimited => true,
            InteractionTimes::Once => check_interaction_performed_at_most(1, interaction, context),
            InteractionTimes::Multiple(times) => {
                check_interaction_performed_at_most(times, interaction, context)
            }
        }
    } else {
        true
    }
}

fn check_interaction_performed_at_most(
    times: u8,
    interaction: &dyn Interaction,
    context: &Context,
) -> bool {
    let today_actions = context
        .game_state
        .action_registry
        .get_from_date(&interaction.id(), context.game_state.calendar.date);

    if today_actions.len() <= times.into() {
        true
    } else {
        false
    }
}

fn register_action(interaction: &dyn Interaction, context: &Context) -> Context {
    let tracking_available = interaction.track_action();
    if tracking_available {
        context.clone().modify_game_state(|game_state| {
            game_state.modify_action_registry(|action_registry| {
                action_registry.register(&interaction.id(), &context.game_state.calendar)
            })
        })
    } else {
        context.clone()
    }
}

fn process_always_applied_effects(interaction: &dyn Interaction, context: &Context) -> Context {
    process_effects(interaction.effects(context).always_applied, context)
}

fn process_applied_after_interaction_effects(
    interaction: &dyn Interaction,
    context: &Context,
) -> Context {
    process_effects(
        interaction.effects(context).applied_after_interaction,
        context,
    )
}

fn process_effects(effects: Vec<InteractionEffect>, context: &Context) -> Context {
    effects.into_iter().fold(context.clone(), |ctx, effect| {
        process_effect(effect, context)
    })
}

fn process_effect(effect: InteractionEffect, context: &Context) -> Context {
    match effect {
        InteractionEffect::Time(consumption) => effects::time::apply(consumption, context),
        InteractionEffect::Health(effect) => effects::health::apply(effect, context),
        InteractionEffect::Energy(effect) => effects::energy::apply(effect, context),
        InteractionEffect::Skill(skill, effect) => effects::skills::apply(skill, effect, context),
        InteractionEffect::Song(effect) => context.clone(),
    }
}
