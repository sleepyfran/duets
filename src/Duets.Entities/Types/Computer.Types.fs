namespace Duets.Entities

[<AutoOpen>]
module ComputerTypes =
    /// Represents the kind of applications that can be used in the computer.
    type App = | WordProcessor

    /// Represents the different states that the computer can be in.
    type ComputerState =
        | Booting
        | AppSwitcher
        | AppRunning of App

    /// Types related to the word processor application.
    module WordProcessor =
        /// Represents a book that the character is currently working on.
        type BookProject =
            { Title: string
              Genre: BookGenre
              Progress: int<percent> }

        /// Represents a collection of books that the character is currently
        /// working on.
        type WordProcessorStorage = { BookProjects: BookProject list }

    /// Represents what's stored in a computer.
    type ComputerStorage =
        { WordProcessor: WordProcessor.WordProcessorStorage }

    /// Resembles a computer that the character can use.
    type Computer =
        {
            /// Represents the kind of performance that can be extracted from the
            /// computer. The performance depends on the computer's hardware
            /// and depletes over time.
            Performance: decimal<percent>

            /// Represents the current state of the computer.
            ComputerState: ComputerState

            /// Represents the storage available in the computer.
            Storage: ComputerStorage
        }
