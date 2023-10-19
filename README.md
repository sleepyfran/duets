# Duets 🎸

Duets is a music/life simulation game focused on allowing the player to be the leader of their own band, composing songs and making gigs to become a star.

<img src="https://github.com/sleepyfran/duets/assets/6024783/d734ae25-aad4-4059-b1f9-66d4614f0777" height="230px" width="500px" />
<img src="https://github.com/sleepyfran/duets/assets/6024783/04696499-af6c-4f3c-9248-42ee4df04b21" height="230px" width="500px" />
<img src="https://github.com/sleepyfran/duets/assets/6024783/0ff57806-bc50-4e90-9362-3a129ee7e8f7" height="230px" width="500px" />
<img src="https://github.com/sleepyfran/duets/assets/6024783/f048df05-1c1e-4f32-af4a-174b17f92a40" height="230px" width="500px" />

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

> [!WARNING]
> The game is nowhere near done or bug-free. I'm constantly doing changes to the savegame format and thus constantly breaking
> savegames. In the future when the game will be more complete I'll introduce some mechanism to automatically migrate savegames
> but for the time being you either need to tinker with the savegame file to adapt it when new versions come up or be okay
> with losing your savegames.

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
