namespace Duets.Cli.Text.Prompts

[<RequireQualifiedAccess>]
module Common =
    /// Creates a prompt that improves the response quality of the language model.
    /// Currently tuned for Gemma 3, which requires explicit turn markers to get
    /// anything useful out of it.
    let internal createPrompt prompt =
        $"""
    <start_of_turn>user
    {prompt}
    <end_of_turn>
    <start_of_turn>model
    """
