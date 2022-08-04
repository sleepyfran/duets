# Duets 🎸

Duets will be a music simulation focused on allowing the player to be the leader of their own band, composing songs and
making gigs to become a star.

# 🛠 Run it locally

Duets is built with F# as an interactive CLI game. Start by cloning the repository and entering in it:

```bash
git clone https://github.com/sleepyfran/duets.git
cd duets
```

Once you have it, you can either chose to run it with a local installation of the .NET SDK or, if you prefer, through
Docker to avoid installing the SDK on your computer and instead running the game inside a container.

## Running with local .NET

If you you want to install .NET, follow the instructions [here](https://dotnet.microsoft.com/download) to get the latest
version of .NET. Once you have it, you can simply run:

```bash
dotnet build
dotnet run --project src/Cli/Cli.fsproj
```

## Running with Docker

There's an included Dockerfile in the repo that allows you to run the game inside a Docker container if you prefer to do
so, simply run it with:

```bash
docker build -t duets .
docker run -it duets
```

### ⚠️ Important
> The game is nowhere near done or bug-free. I'm constantly doing changes to the savegame format and thus constantly breaking
savegames. So if you want to try out the game please do so and make sure you open issues with any bug you find, but keep in
mind that your savegames might not make it that long 😀

# 🧪 Testing

You can run the tests with:

```bash
dotnet test
```

# 😄 Contributions

Right now the game is in concept phase and it'll probably change quickly until it has certain features so there's no
public roadmap. The best way of contributing right now is running the game and reporting bugs, if you want to contribute
to the code check the project a little bit later, I'll update this section as soon as it's possible to have external
contributions!
