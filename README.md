# DigimonWorld2Visualizer

Work in progress visualizer for the map data of the game "Digimon World 2".

The current visualizer only renders each possible map layout per domain in the console as coloured text or whitespace, with no object data yet being shown besides special type floor tiles.
Next on the roadmap is adding the following object data:
- Warps
- Chests
- Traps
- Digimon

When the proof of concept is operational in the console I will move on to porting it to WinForms to make it easier to add a GUI for better usability, as well as adding a real renderer for the map layouts to increase performance.

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