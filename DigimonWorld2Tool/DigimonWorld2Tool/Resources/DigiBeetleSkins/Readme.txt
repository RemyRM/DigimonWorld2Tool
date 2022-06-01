To add a new skin add a new folder to the respective body type directory:
Steel: DigimonWorld2Tool\DigimonWorld2Tool\Resources\DigiBeetleSkins\Steel
Titanium: DigimonWorld2Tool\DigimonWorld2Tool\Resources\DigiBeetleSkins\Titanium
Adamant: DigimonWorld2Tool\DigimonWorld2Tool\Resources\DigiBeetleSkins\Adamant
Give this new folder the name of the skin you want to add.

The skins folder that you have just created needs to contains the skin a total of 6 times. This is because for every type of tyre in the game a new texture is used. These files need to be complient to the following naming scheme:
{BodyType}_{SkinName}_{Index}.BIN
Where:
BodyType: equal to the type of body the skin is for, with the legal values being Steel, Titanium or Adamant.
SkinName: The name of the skin you want to add. This should be equal to the folder name
Index: The index of the tyre the skin is for, this is in a range from 0 through 5 inclusive.

To add a preview for the tool to display add a .png following the naming convention:
{BodyType}_{SkinName}_PREVIEW.png

For example if we want to add the skin "Red" for each body type we create a new folder "Red" in all three directories "Steel", "Titanium", "Adamant" and add the following files containing the skin data to each of these folders:

Steel:
Steel_Red_0.BIN
Steel_Red_1.BIN
Steel_Red_2.BIN
Steel_Red_3.BIN
Steel_Red_4.BIN
Steel_Red_5.BIN
Steel_red_PREVIEW.PNG

Titanium:
Titanium_Red_0.BIN
Titanium_Red_1.BIN
Titanium_Red_2.BIN
Titanium_Red_3.BIN
Titanium_Red_4.BIN
Titanium_Red_5.BIN
Titanium_red_PREVIEW.PNG

Adamant:
Adamant_Red_0.BIN
Adamant_Red_1.BIN
Adamant_Red_2.BIN
Adamant_Red_3.BIN
Adamant_Red_4.BIN
Adamant_Red_5.BIN
Adamant_red_PREVIEW.PNG