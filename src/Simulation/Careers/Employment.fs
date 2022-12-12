module Simulation.Careers.Employment

open Entities
open Simulation

/// Gives the character the specified job. If there were any previous jobs, it'll
/// be overriden by this one.
let acceptJob state job =
    let currentCharacter = Queries.Characters.playableCharacter state

    let currentJob = Queries.Career.current state

    [ yield!
          match currentJob with
          | Some currentJob -> [ CareerLeave(currentCharacter.Id, currentJob) ]
          | None -> []

      CareerAccept(currentCharacter.Id, job) ]
