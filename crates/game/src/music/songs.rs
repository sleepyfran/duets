use lenses::lens::Getter;
use lenses::lens::Lens;

use common::entities::Song;

use crate::context::Context;

/// Discards the given song from the character's unfinished songs.
pub fn discard(song: Song, context: &Context) -> Context {
    let context_lens = gen_lens!(clone Context, game_state.character.songs_in_progress);
    let mut songs_in_progress = context_lens.view(&context);
    songs_in_progress.remove(&song);
    context_lens.set(&context, &songs_in_progress)
}

/// Edits the name of a song to the given one.
pub fn edit(song: Song, name: String, context: &Context) -> Context {
    let context_lens = gen_lens!(clone Context, game_state.character.songs_in_progress);
    let mut songs_in_progress = context_lens.view(&context);

    let song_lens = gen_lens!(clone Song, name);
    let song = song_lens.set(&song, &name);
    songs_in_progress.replace(song);

    context_lens.set(&context, &songs_in_progress)
}
