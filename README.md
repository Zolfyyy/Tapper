# Tapper

[![Platform](https://img.shields.io/badge/platform-macOS-blue?style=flat-square)]()
[![.NET](https://img.shields.io/badge/.NET-9-512BD4?style=flat-square&logo=.net)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE)

Tapper is a lightweight, macOS utility designed to programmatically control slide presentations from various presentation software including PowerPoint, Keynote, and Canva. It provides a command-line interface to navigate through slides efficiently, making it perfect for integration with automation tools and custom keyboard shortcuts.

## Requirements

- .NET 9.0 or later

## Installation

Go to the [releases](https://github.com/KitRifty/Tapper/releases) page and download the appropriate version for your platform.

## Commands

| Command | Description                                                   | Usage                                     |
| ------- | ------------------------------------------------------------- | ----------------------------------------- |
| `query` | Outputs the matching PID using the given target and operation | `tapper query -t <target> -o <operation>` |
| `next`  | Advance to the next slide                                     | `tapper next -t <target> -o <operation>`  |
| `prev`  | Go back to the previous slide                                 | `tapper prev -t <target> -o <operation>`  |

## Example Usage

This section demontrates using Tapper with popular software.

### Karabiner-Elements

You can combine Tapper with [Karabiner-Elements](https://karabiner-elements.pqrs.org/) to remap keyboard inputs to a specific program action. You can even filter the modification to apply only to specific devices, effectively redirecting the device's inputs to affect the target program only. Here's a sample complex modification that maps Page Up/Down keys to control your PowerPoint presentation:

```json
{
  "description": "Tapper Presentation Controls",
  "manipulators": [
    {
      "from": {
        "key_code": "page_down"
      },
      "to": [
        {
          "shell_command": "zsh --login -c \"tapper next -t 'Microsoft PowerPoint'\""
        }
      ],
      "type": "basic",
      "conditions": [
        {
          "type": "device_if",
          "identifiers": [
            {
              "vendor_id": 1133,
              "product_id": 50488,
              "description": "Presentation Clicker"
            }
          ]
        }
      ]
    },
    {
      "from": {
        "key_code": "page_up"
      },
      "to": [
        {
          "shell_command": "zsh --login -c \"tapper prev -t 'Microsoft PowerPoint'\""
        }
      ],
      "type": "basic",
      "conditions": [
        {
          "type": "device_if",
          "identifiers": [
            {
              "vendor_id": 1133,
              "product_id": 50488,
              "description": "Presentation Clicker"
            }
          ]
        }
      ]
    }
  ]
}
```

> [!NOTE]
> This configuration assumes Tapper is installed within a `$PATH` directory, hence why `zsh --login -c` is also used. You can replace `tapper` with the actual path to the executable if desired.

To use this configuration:

1. Open Karabiner-Elements
2. Navigate to Complex Modifications
3. Add the above rule using the import feature

Now you can use Page Up/Down keys to control your presentations using Tapper.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
