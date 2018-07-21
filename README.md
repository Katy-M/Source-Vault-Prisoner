# Source-Vault-Prisoner
## Katy Mollenkopf
The source for my top-down tile based game, Vault Prisoner (excludes content folder).
## Key Classes and Brief Descriptions
### Game1.cs
The entry point for the application. Loads all content and graphics from the content folder (not included in this repo) and contains the update and main drawing loop. Creates the grids and keeps track of the different states of the game.
### Situation.cs
Abstract class that is the base for all the situations in-game (monster attack, trap, mishap, weapon pickup, health pickup,and armor pickup)
### Menu.cs
Holds data pertaining to a menu in the game that is read in from an external text file included in the project (text files not included in this repo)
### Gui.cs
Sets up the menus in the game
### Tile.cs
Represents and holds data for a tile on the grid that the player can navigate
### Grid.cs
Sets up a grid of tiles in the game

## Credits
Designed and created by Katy Mollenkopf

Textures on graphics obtained and modifed using images from https://www.textures.com/

### Fonts 
* “Edo” under Vic Fieger License
* “Exo” under SIL Open Font License

(Textures and font files not included in this repo)
