use std::rc::Rc;

use game::world::interactions;
use game::world::interactions::*;

use crate::context::{Context, ScreenContext};
use crate::effects;
use crate::shared::action::{Choice, CliAction, ConfirmationChoice, Prompt, PromptText};
use crate::shared::display;
use crate::shared::screen::Screen;

pub type InteractionContext = ScreenContext<SequenceInput>;

pub fn create_interaction_screen(
    interaction: Rc<dyn Interaction>,
    sequence: InteractItem,
    context: Context,
) -> Screen {
    Screen {
        name: "Interaction".into(),
        action: action_from_sequence(
            interaction,
            sequence,
            InteractionContext {
                global_context: context.clone(),
                state: SequenceInput {
                    values: vec![],
                    context,
                },
                next_action: None,
            },
        ),
    }
}

fn action_from_sequence(
    interaction: Rc<dyn Interaction>,
    sequence: InteractItem,
    context: InteractionContext,
) -> CliAction {
    match sequence {
        InteractItem::Chain(first, second) => action_from_sequence(
            Rc::clone(&interaction),
            *first,
            context.with_next_action(Some(Box::new(|context| {
                action_from_sequence(interaction, *second, context)
            }))),
        ),
        InteractItem::TextInput(text) => CliAction::Prompt(Prompt::TextInput {
            text: PromptText::WithEmoji(text),
            on_action: Box::new(|input, global_context| {
                continue_with_action(context, global_context, InputType::Text(input))
            }),
        }),
        InteractItem::Confirmation { question, branches } => {
            CliAction::Prompt(Prompt::ConfirmationInput {
                text: PromptText::WithEmoji(question),
                on_action: Box::new(|choice, global_context| {
                    let (input_type, branch_sequence) = match choice {
                        ConfirmationChoice::Yes => (InputType::Confirmation(true), branches.0),
                        ConfirmationChoice::No => (InputType::Confirmation(false), branches.1),
                    };

                    action_from_sequence(
                        interaction,
                        *branch_sequence,
                        context
                            .with_context(global_context)
                            .modify_state(|state| state.add_input(input_type)),
                    )
                }),
            })
        }
        InteractItem::Options { question, options } => CliAction::Prompt(Prompt::ChoiceInput {
            text: PromptText::WithEmoji(question),
            choices: options
                .iter()
                .enumerate()
                .map(|(index, option)| Choice {
                    id: index,
                    text: option.text.to_string(),
                })
                .collect(),
            on_action: Box::new(move |chosen_choice, global_context| {
                let chosen_option = &options[chosen_choice.id];

                continue_with_action(
                    context,
                    global_context,
                    InputType::Option(InteractOption {
                        id: chosen_option.id.to_string(),
                        text: chosen_option.text.to_string(),
                    }),
                )
            }),
        }),
        InteractItem::End => {
            let result = interactions::result(&*interaction, context.state);
            display::show_info(&result.0);
            effects::set_state(result.1.game_state)
        }
    }
}

fn continue_with_action(
    context: InteractionContext,
    updated_global_context: &Context,
    input: InputType,
) -> CliAction {
    match context.next_action {
        Some(action) => action(InteractionContext {
            global_context: updated_global_context.clone(),
            state: context.state.add_input(input),
            next_action: None,
        }),
        None => CliAction::Continue,
    }
}
