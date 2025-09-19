# CraftingAdditions

A Vintage Story mod that improves the anvil crafting experience by expanding the recipe selector interface.

## What it does

This mod uses a Harmony monkey patch to expand the anvil recipe selector GUI from 7 columns to 14 columns, making it much easier to browse and select recipes when working at the anvil.

## Features

- **Expanded Interface**: Doubles the recipe grid from 7 to 14 columns
- **Better Visibility**: See more recipes at once without scrolling
- **Non-intrusive**: Simple patch that doesn't affect game balance
- **Lightweight**: Minimal performance impact

## Installation

1. Download the latest release from the [Releases](../../releases) page
2. Place the `.zip` file in your Vintage Story `Mods` folder
3. Launch the game - the mod will be automatically loaded

## Compatibility

- **Vintage Story Version**: 1.19.x (adjust based on your testing)
- **Dependencies**: None
- **Client/Server**: Client-side only

## Technical Details

This mod demonstrates a clean implementation of Harmony transpiler patching:

- Uses `HarmonyTranspiler` to modify IL code at runtime
- Targets the `GuiDialogBlockEntityRecipeSelector.SetupDialog` method
- Changes the column count constant from 7 to 14
- Follows Vintage Story modding best practices

## Building from Source

1. Clone this repository
2. Ensure you have the Vintage Story modding environment set up
3. Build using your preferred method (Visual Studio, dotnet CLI, etc.)

## Contributing

Feel free to submit issues or pull requests if you have suggestions for improvements!

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Author

**Coelor** - *Initial work*