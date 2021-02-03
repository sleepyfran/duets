module Entities.Genre

/// Defines a musical genre. This basic type is just an alias for the name of
/// the genre, there's more specific types depending on the type of information
/// that we want to query.
type Genre = string

/// Defines the relation between a genre and its popularity in a moment
/// in time.
type Popularity = Genre * byte

/// Defines the percentage compatibility of two genres between 0 and 100.
type Compatibility = Genre * Genre * byte
