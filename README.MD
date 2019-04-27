# Gal.Io

The backbone of the League of Legends Custom Game Tracker. This project allows manual adding of players to anonymized custom games data, so that trends can be tracked.

## Getting Started

clone this repo.

### Prerequisites

this project runs on .NET Core 2.2 - ensure that the SDK is installed before continuing.

You *can* run this solution without the front-end - but it's highly recommended that you use it.


### Installing

Ensure that you have all the packages needed by doing a dotnet restore

```
dotnet restore
```
From here, it would be best to make an initial database via EntityFramework.

```
dotnet ef migrations add initialCommit
dotnet ef database update
```

You can then run the code by doing dotnet run

```
dotnet run
```


## Built With

* [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/) - Development platform of choice
* [AutoMapper](https://automapper.org/) - Handling some database Copies
* [Newtonsoft](https://www.newtonsoft.com/json) - JSON Framework for .NET
* [Riot Games API](https://developer.riotgames.com/) - Used to gather League of Legends data


## Authors

* **Kyle Humphrey** - *Initial work* - [Kuenaimaku](https://github.com/Kuenaimaku)

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details