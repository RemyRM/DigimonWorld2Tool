# DigimonWorld2Visualizer

Work in progress visualizer for the map data of the game "Digimon World 2".

The current visualizer only renders each possible map layout per domain in the console as coloured text or whitespace showing the following object data:
- Warp    Position+Type
- Chest   Position
- Trap    Position+Type
- Digimon Position

The following items are next on the to-do list:
- Switching to Winforms 
- Adding GUI Layout for selecting maps
- Pixel/Vector based renderer for map layouts

# Roadmap

Functionality I may or may not add at some point in the future, in semi order of relevance (subject to change and my motivation):
- Graphical interface
- Map layout editor
- Map layout randomizer
- Map object editor
- Map object randomizer
- A*/Dijkstra algorithm to find best routes
- 3D model extractor/viewer
- 3D model injector (add your own digimon!)
- Sprite/texture extractor/viewer
- Sprite/texture editor/injector

# Build steps

- Create a directory called "Maps" in your `AppDomain.CurrentDomain.BaseDirectory` directory (e.g. DigimonWorld2MapVisualizer\DigimonWorld2MapVisualizer\bin\Debug\netcoreapp3.1\Maps`)
- Add the binary files for the maps you want to be able to view to the \Maps\ directory (e.g. "DUNG4000.BIN")
- Launch the application from your IDE or build the application
- Upon launch you will be asked for the 4 digit identifier of the domain you want to load (e.g. "4000")

# Resources

- [Format example (Pastebin)](https://pastebin.com/pJSjQrna)
- [Digimon World 2 Modding Info (Google spreadsheet)](https://docs.google.com/spreadsheets/d/1UiDU4MsSfxO1vhpK6err1KsLRZM53JUOuYqYhfEFp8o/edit#gid=305512343)

# Acknowledgements

These people have helped immensely in helping to figure out the data format, providing additional insights, and sharing a lot of information:
- [Rhymu8354](https://github.com/rhymu8354/)
- nDoorn 
- Luminaires

# Feedback
Found a bug, got a suggestion, or any other kind of feedback? Please create an issue or hit me up on discord @RemyRm#8070