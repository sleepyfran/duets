use std::collections::HashSet;
use std::sync::Arc;

use common::entities::Song;
use common::extensions::option::OptionCloneExtensions;
use game::music::songs;

use super::Command;
use crate::effects;
use crate::shared::action::{Choice, CliAction, Prompt, PromptText};
use crate::shared::display;
use crate::shared::emoji;

/// Allows the user to retrieve, edit and remove unfinished songs.
pub fn create_songs_command() -> Command {
    Command {
        name: String::from("songs"),
        matching_names: vec![],
        explanation: String::from("Allows to show, edit and remove unfinished songs"),
        help: r#"
songs
----
When called with no paramaters will display all unfinished songs. Can also be called with:

[edit] - Shows the list of all unfinished songs and allows to choose one to edit
[discard] - Shows the list of all unfinished songs and allows to choose one to remove
        "#
        .into(),
        execute: Arc::new(move |args, global_context| {
            let unfinished_songs = global_context
                .game_state
                .character
                .songs_in_progress
                .clone();

            let command = args.first();
            match command {
                Some(command) => match command.as_str() {
                    "edit" => edit_song(unfinished_songs),
                    "discard" => discard_song(unfinished_songs),
                    _ => show_songs(unfinished_songs),
                },
                _ => show_songs(unfinished_songs),
            }
        }),
    }
}

fn show_songs(unfinished_songs: HashSet<Song>) -> CliAction {
    let max_column_separator = unfinished_songs
        .iter()
        .map(|song| song.name.len())
        .max()
        .unwrap_or(0)
        + 4;

    display::show_text_with_new_line("These are the songs you have unfinished:");
    display::show_line_break();
    for (index, song) in unfinished_songs.iter().enumerate() {
        show_song(index, song, max_column_separator)
    }
    display::show_line_break();
    display::show_text_with_new_line("You can finish any of them at any time by recording them");

    CliAction::Continue
}

fn show_song(index: usize, song: &Song, name_padding: usize) {
    let needed_padding = name_padding - song.name.len();
    let padding: String = vec![0; needed_padding].into_iter().map(|_| " ").collect();

    display::show_text(&format!(
        "{}. {}{}| {} Quality: {}",
        index + 1,
        song.name,
        padding,
        get_quality_emoji_for(song),
        song.quality
    ));

    display::show_line_break();
}

fn edit_song(unfinished_songs: HashSet<Song>) -> CliAction {
    CliAction::Prompt(Prompt::ChoiceInput {
        text: PromptText::WithEmoji("Which song do you want to edit?".into()),
        choices: unfinished_songs
            .iter()
            .enumerate()
            .map(|(index, song)| Choice {
                id: index,
                text: format!("{} (Quality: {})", song.name, song.quality),
            })
            .collect(),
        on_action: Box::new(move |choice, _| {
            let selected_song = unfinished_songs.iter().nth(choice.id).unwrap_cloned();

            CliAction::Prompt(Prompt::TextInput {
                text: PromptText::WithEmoji("What's the new name of the song?".into()),
                on_action: Box::new(move |name, global_context| {
                    let updated_state =
                        songs::edit(selected_song, name.clone(), global_context).game_state;
                    display::show_line_break();
                    display::show_info(&format!("Song name changed to {}", name));
                    effects::set_state(updated_state)
                }),
            })
        }),
    })
}

fn discard_song(unfinished_songs: HashSet<Song>) -> CliAction {
    CliAction::Prompt(Prompt::ChoiceInput {
        text: PromptText::WithEmoji("Which song do you want to discard?".into()),
        choices: unfinished_songs
            .iter()
            .enumerate()
            .map(|(index, song)| Choice {
                id: index,
                text: format!("{} (Quality: {})", song.name, song.quality),
            })
            .collect(),
        on_action: Box::new(move |choice, global_context| {
            let selected_song = unfinished_songs.iter().nth(choice.id).unwrap_cloned();
            let updated_state = songs::discard(selected_song.clone(), global_context).game_state;
            display::show_line_break();
            display::show_info(&format!("Song {} removed", selected_song.name));
            effects::set_state(updated_state)
        }),
    })
}

fn get_quality_emoji_for(song: &Song) -> String {
    if song.quality > 70 {
        emoji::for_good_quality().into()
    } else if song.quality > 40 {
        emoji::for_medium_quality().into()
    } else {
        emoji::for_bad_quality().into()
    }
}
