/// Defines the information about the game like the version and other info that we might need to
/// show during execution.
pub struct GameInfo {
    pub name: String,
    pub author: String,
    pub version: String,
}

/// Returns the current information through Cargo's environment variables.
pub fn get_game_info() -> GameInfo {
    GameInfo {
        name: "duets".into(),
        author: env!("CARGO_PKG_AUTHORS").into(),
        version: env!("CARGO_PKG_VERSION").into(),
    }
}
