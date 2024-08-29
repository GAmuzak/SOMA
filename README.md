# SOMA: Scriptable Object Mass Adjuster for Unity

A Unity Editor tool that allows for mass editing of Scriptable Objects based on type and a Regex-based search pattern. This tool provides an Editor Window to select a defined scriptable object type, and view the associated layout. Edits can be made on this layout. Based on the regex pattern, all matching files will have the changes applied to it. Untouched fields will be left as is.

### ⚠ WARNING ⚠
This is currently in alpha, and needs a lot of testing. Please do not use on assets critical to your workflow.

## Features

- **Fetch All Scriptable Object Types**: Automatically list all Scriptable Object types available in the Unity project.
- **Search for Scriptable Objects by Name**: Filter through the available Scriptable Object types.
- **Regex-Based File Matching**: Define a regex pattern to search for specific asset files of the selected Scriptable Object type.
- **Mass Edit Scriptable Objects**: Edit properties in an easy-to-use Editor Window and apply changes to all matching files.

## Installation

This package can be added to your project via [Git UPM](https://docs.unity3d.com/2022.3/Documentation/Manual/upm-git.html).
To add this package, copy:

```shell
https://github.com/GAmuzak/SOMA.git
```

and add it as a git package in the Unity Package Manager

![Add_Package_Via_Git](https://github.com/GAmuzak/SOMA/blob/main/Documentation~/add_package_from_git.png)

## Requirements

- Unity 2021.1 or later *(Not yet validated!)*

## Usage

- **Open SOMA Window**: In the top menu bar, select Tools → (SOMA) Scriptable Object Mass Adjuster
- **Search for your target Scriptable Object**: If `Find My SOs only` is enabled, it will only show scriptable objects defined within the default user-defined assembly. Use the search bar to filter through scriptable object types in the dropdown.
- **Pick your target Scriptable Object**: Use the dropdown field to view all defined scriptable objects, and pick your target type.
- **Enter the regex logic**: The changes defined will apply to all files matching the regular expression defined here

## Limitations

- Only displays `ints`, `floats`, `strings` and `bools` for now
- `ints`, `floats` and `bools` are parsed from strings, and validated in console

## TO-DO

- Add more complex type and custom class support
- Keep track of last selected type when toggling between `Find My SOs only`

## Contributing

Please see the [CONTRIBUTING.md](CONTRIBUTING.md) file for details on contributing to this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
