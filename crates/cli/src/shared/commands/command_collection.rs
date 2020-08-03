use super::Command;

/// Represents a collection of command with some utility methods to easily managed it.
#[derive(Clone, Default)]
pub struct CommandCollection {
    commands: Vec<Command>,
}

impl CommandCollection {
    /// Adds a new element to the collection.
    pub fn add(&mut self, command: Command) -> &mut CommandCollection {
        self.commands.push(command);
        self
    }

    /// Attempts to find a command with the given name.
    pub fn find_by_name(&self, name: &String) -> Option<Command> {
        self.commands
            .iter()
            .filter(|command| matches_command(name, command))
            .nth(0)
            .cloned()
    }

    /// Adds a given list of commands into the current set.
    pub fn chain(&mut self, other_collection: Vec<Command>) -> &mut CommandCollection {
        self.commands = self
            .commands
            .clone()
            .into_iter()
            .chain(other_collection)
            .collect();
        self
    }

    /// Transforms the internal structure into a vector.
    pub fn to_vec(&self) -> Vec<Command> {
        self.commands.clone()
    }
}

fn matches_command(input: &str, command: &Command) -> bool {
    command.name == input || command.matching_names.contains(&input.to_string())
}
