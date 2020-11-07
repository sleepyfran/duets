use common::entities::Song;
use game::context::Context;
use game::music::songs;

fn create_context_with_song(song: &Song) -> Context {
    Context::default().modify_game_state(|state| {
        state.modify_character(|character| character.add_or_modify_song_in_progress(song.clone()))
    })
}

#[test]
fn discard_should_remove_given_song() {
    let song_to_remove = Song::default();
    let context = create_context_with_song(&song_to_remove);

    let updated_context = songs::discard(song_to_remove, &context);

    assert_eq!(
        0,
        updated_context.game_state.character.songs_in_progress.len()
    )
}

#[test]
fn edit_should_change_song_name_to_given_one() {
    let song_to_edit = Song::default();
    let context = create_context_with_song(&song_to_edit);

    let updated_context = songs::edit(song_to_edit.clone(), "Life Is A Test".into(), &context);

    assert_eq!(
        "Life Is A Test".to_string(),
        updated_context
            .game_state
            .character
            .songs_in_progress
            .get(&song_to_edit.id)
            .unwrap()
            .name
    )
}
