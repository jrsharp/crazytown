# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Crazytown is a custom lock screen for OpenFirmware Macintosh computers, written in Forth. It provides a graphical boot screen with password prompt and keyboard input functionality.

## Development Notes

### Language and Environment
- **Language**: Forth
- **Target Platform**: OpenFirmware Macintosh computers
- **Purpose**: Custom lock screen with graphical boot interface

### Current Status
The project appears to be in early development stage with only documentation present. No Forth source files have been created yet.

### Forth Development Context
When implementing Forth code for this project:
- OpenFirmware provides a Forth environment on PowerPC Macs
- Code will need to interface with OpenFirmware's graphics and keyboard primitives
- Consider memory constraints typical of firmware environments

### Future Implementation Considerations
- Forth files typically use extensions: `.fs`, `.fth`, `.forth`, `.4th`, or `.f`
- OpenFirmware Forth includes specific words for graphics and input handling
- Password security in a firmware environment requires careful implementation