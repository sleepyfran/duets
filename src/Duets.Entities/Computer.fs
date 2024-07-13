module Duets.Entities.Computer

/// Default state of a computer's storage.
let defaultStorage = { WordProcessor = { BookProjects = [] } }

/// Creates a computer with the default state and storage and the given performance.
let forPerformance perf =
    { Performance = perf
      ComputerState = Booting
      Storage = defaultStorage }
