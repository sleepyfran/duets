module Duets.Data.Movies

open Duets.Common
open Duets.Entities

/// Retrieves all available movies from the data file.
let all: Movie list = ResourceLoader.load Files.DataKey.Movies
