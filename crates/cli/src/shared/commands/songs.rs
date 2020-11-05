use std::collections::HashSet;
use std::sync::Arc;

use common::entities::Song;

use super::Command;
use crate::shared::action::CliAction;
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

[edit] - Shows a list of songs to edit the name of
[remove] - Shows a list of songs to remove
        "#
        .into(),
        execute: Arc::new(move |_args, global_context| {
            let unfinished_songs = &global_context.game_state.character.songs_in_progress;
            show_songs(unfinished_songs);

            CliAction::Continue
        }),
    }
}

fn show_songs(unfinished_songs: &HashSet<Song>) {
    let max_column_separator = unfinished_songs
        .iter()
        .map(|song| song.name.len())
        .max()
        .unwrap_or(0)
        + 4;
    for (index, song) in unfinished_songs.iter().enumerate() {
        show_song(index, song, max_column_separator)
    }
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

fn get_quality_emoji_for(song: &Song) -> String {
    if song.quality > 70 {
        emoji::for_good_quality().into()
    } else if song.quality > 40 {
        emoji::for_medium_quality().into()
    } else {
        emoji::for_bad_quality().into()
    }
}
