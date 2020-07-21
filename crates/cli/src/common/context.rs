use app::database::Database;

/// Context passed to every action performed in the CLI. Contains any stateful values that might
/// be necessary during execution.
pub struct Context {
    pub database: Database,
}
