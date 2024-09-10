# Twin Finder
![license](https://img.shields.io/github/license/TrueBubo/TwinFinder?style=flat)
![GitHub Release](https://img.shields.io/github/v/release/TrueBubo/TwinFinder)

Twin Finder is a command line tool used for finding the most similar files

## Development documentation
You can find the documentation [here](https://truebubo.github.io/TwinFinder/) 

## Installation
### AppImage
- Go to releases and download the latest version

### Manual
1. Clone the repository 
```bash
git clone https://github.com/TrueBubo/TwinFinder
cd TwinFinder/TwinFinder
```
2. Build the project
```bash
dotnet restore
dotnet build 
```

## Running the program
### AppImage
- Make the AppImage executable
- Run it

### Manual
```bash
dotnet run
```

## Configuration
You can find the configuration files in

---
### Unix-Based 
`$HOME/.config/TwinFinder`
### Windows
`C:\Users\<Username>\AppData\Roaming`

---

To edit options you can edit the `config.toml` file

If you wish you can edit files with synonyms in `SynonymsFiles`

## Options
```
-h          To display the help menu
-m          What technique will be used to determine similar files 
--normalize Removes accents and diacritics
-n          Pairs to find
-s          Number of synonyms to consider
--lang      Language the texts are in
--absolute  Prints text locations using absolute paths instead of relative
-o          Where should the outputs of the program be sent
```

Everything after these options will be considered as a location of files to be searched.

## How to use
### Example calls
`TwinFinder essays/*`  
`TwinFinder -n 10 papers/* materials/*`  
`TwinFinder -o output.txt theses/*`

