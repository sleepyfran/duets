use super::Database;

#[derive(Debug, Clone)]
pub enum DatabaseLoaderError {
    NoConnection,
    DatabaseCorrupted,
}

/// Attempts to load the database from the server.
pub fn retrieve_from_server() -> Result<Database, DatabaseLoaderError> {
    // TODO: Implement.
    Err(DatabaseLoaderError::NoConnection)
}
