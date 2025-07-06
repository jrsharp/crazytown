# OpenFirmware Documentation References

## Apple Technical Notes (Official Documentation)

### TN1061: Fundamentals of Open Firmware, Part I - The User Interface
**URL**: https://developer.apple.com/library/archive/technotes/tn/tn1061.html
- Describes the Open Firmware User Interface and Forth language
- Covers two-machine mode setup
- Basics of Forth programming
- Introduction to device tree
- Debugging techniques

### TN1062: Fundamentals of Open Firmware, Part II - The Device Tree  
**URL**: https://developer.apple.com/library/archive/technotes/tn/tn1062.html
- Detailed device tree structure
- Display device methods including:
  - `fill-rectangle`
  - `draw-rectangle`
  - `read-rectangle`
  - `color@`, `color!`
  - `get-colors`, `set-colors`
- Power Macintosh 9500 device tree reference

### TN2001: Running Files from a Hard Drive in Open Firmware
**URL**: https://developer.apple.com/library/archive/technotes/tn/tn2001.html
- Loading and running Forth code from storage
- File operations:
  - `load hd:\filename` - Load file to memory
  - `boot hd:\filename` - Execute Forth file
  - `dir hd:\` - List directory contents
- Single-machine development workflow

## Open Firmware Working Group Resources

### IEEE 1275-1994 Standard
- Official Open Firmware specification
- Withdrawn by IEEE in 2005 but still the reference standard

### 8 Bit Graphics Extension
**Document**: "8 Bit Graphics Extension, Draft Version 1.2, Oct. 24, 1995"
**URL**: https://www.devicetree.org/open-firmware/practice/
- Graphics methods for display drivers
- Includes `draw-rectangle` and related methods
- Available in PostScript and PDF formats

## Key OpenFirmware Words for PowerBook G4

### Device Access
```forth
" screen" open-dev to myscreen
" method-name" myscreen $call-method
myscreen close-dev
```

### Confirmed Display Methods
- `fill-rectangle` ( color x y w h -- )
- `frame-buffer-adr` ( -- addr )
- `erase-screen` ( -- )

### Standard Forth Words
All standard Forth words are available including:
- Stack manipulation: `dup`, `drop`, `swap`, `over`, `rot`
- Memory: `@`, `!`, `c@`, `c!`, `move`, `fill`
- Control: `if`, `then`, `else`, `begin`, `until`, `do`, `loop`
- I/O: `key`, `key?`, `emit`, `type`, `cr`

### Memory and Files
- `alloc-mem` ( size -- addr )
- `here` ( -- addr )
- `allot` ( n -- )
- `create`, `variable`, `constant`
- `load-base` ( -- addr ) - After loading a file
- `load-size` ( -- n ) - Size of loaded file

## PowerBook G4 Specific Information

### Display
- Native resolution: 1280x854
- Color depth: 32-bit (4 bytes per pixel)
- Framebuffer format: ARGB (Alpha, Red, Green, Blue)
- Byte order: Big-endian (PowerPC)

### Boot Control
- `auto-boot?` - Controls automatic booting
- `boot-command` - Command executed at boot
- `boot-device` - Default boot device
- nvramrc scripting for persistent modifications

### Device Aliases
Common device paths accessible via:
```forth
devalias
```
Typical aliases:
- `hd` - Primary hard drive
- `cd` - CD/DVD drive  
- `usb0`, `usb1` - USB devices
- `kbd` - Keyboard
- `screen` - Display

## Additional Resources

### Apple Developer Archive
Base URL: https://developer.apple.com/library/archive/
- Contains various OpenFirmware technical notes
- Historical Mac development documentation

### Community Resources
- OpenBIOS Project: Implementation examples
- NetBSD OpenFirmware documentation: Practical usage
- Various forums with PowerPC Mac OpenFirmware tips

## Quick Reference Commands

```forth
\ Enter OpenFirmware
Cmd+Option+O+F at startup

\ Common commands
dev /           \ Go to root of device tree
ls              \ List devices at current node
.properties     \ Show properties of current device
words           \ List all available Forth words
see word-name   \ Decompile a Forth word
```

## Notes on Apple's Implementation

- Based on IEEE 1275-1994 but with Apple-specific extensions
- Graphics support varies by model and video card
- PowerBook G4 has OpenFirmware 3.x
- Direct framebuffer access is possible via `frame-buffer-adr`
- File paths use backslash (\) as separator
- Case-insensitive command interpreter