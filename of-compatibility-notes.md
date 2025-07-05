# OpenFirmware Compatibility Analysis

## Major Issues Found

### 1. **Framebuffer Access**
- `address` method might not exist - use `frame-buffer-adr` instead
- Resolution setting via `set-resolution` is non-standard
- Most OF implementations require nvram settings: `setenv output-device screen:1280x854x32`

### 2. **File I/O**
- `read-file`, `seek-file`, `w@`, `l@` don't exist in standard OF
- Must use device methods: `read`, `seek`, `load`
- Word/long access must be built from `c@`

### 3. **Memory Management**
- `alloc-mem` exists but `free-mem` often doesn't
- Limited heap space (often < 1MB)
- Memory persists until reset

### 4. **Graphics**
- Direct framebuffer writes work but are slow
- No built-in bitmap support
- Must handle endianness (PowerPC is big-endian)

### 5. **Boot Interception**
- `to boot-command` syntax varies
- Must use nvramrc for persistence
- Requires `use-nvramrc? true`

## Recommended Testing Approach

1. **Start with `crazytown-safe.fs`** - Uses only console I/O
2. **Test framebuffer detection** separately
3. **Build up graphics gradually** after confirming FB access
4. **Test on real hardware** incrementally

## OpenFirmware Versions

PowerBook G4 likely has OF 3.x which includes:
- IEEE 1275-1994 core words
- Enhanced device tree
- Better framebuffer support
- USB keyboard support (usually)

## Debug Commands

```forth
\ Show OF version
dev /openprom
.properties

\ List all words
words

\ Check for specific word
see frame-buffer-adr

\ Show memory map
show-devs

\ Display properties
dev screen
.properties
```