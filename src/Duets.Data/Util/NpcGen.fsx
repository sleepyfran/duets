(*
This is a very dirty, very quick script that generates NPCs of a given gender based on a list of names.
It attempts to generate a JSON array that contains the NPCs in the specified format, but I'm too lazy
to handle the commas properly so it outputs one extra comma that has to be removed in the last array
element.
*)

// Add the list of names as a string, divided by a new-line.
let input =
    """
First Name
Second Name
"""

// Same type as entity: Male, Female or Other.
let gender = "Other"

input.Split("\n")
|> Array.map (_.Trim())
|> Array.fold
    (fun output name ->
        if System.String.IsNullOrEmpty name then
            output
        else
            output
            + $"""
    [
        "{name}",
        {{
            "Case": "{gender}"
        }}
    ],""")
    "[\n"
|> fun output -> output + "\n]"
|> System.Console.WriteLine
