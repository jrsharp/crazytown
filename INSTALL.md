# Crazytown Installation Guide

## Prerequisites
- PowerBook G4 or compatible OpenFirmware Mac
- Access to OpenFirmware (Command+Option+O+F at startup)
- Your custom boot screen: `rd_boot.bmp` (1280x854, 24-bit BMP)

## Installation Steps

1. **Prepare the files**
   - Copy `crazytown-fast.fs` to the root of your hard drive
   - Copy `rd_boot.bmp` to the root of your hard drive
   - Optional: Copy `bitmap-direct.fs` for additional bitmap utilities

2. **Enter OpenFirmware**
   - Restart your PowerBook G4
   - Hold Command+Option+O+F immediately after the startup chime
   - You'll see the OpenFirmware prompt: `ok`

3. **Load Crazytown**
   ```forth
   load hd:\crazytown-fast.fs
   ```

4. **Test the lock screen**
   ```forth
   test-fast
   ```
   Password: `forth`

5. **Install permanently** (Advanced users only!)
   ```forth
   install-lock
   setenv auto-boot? true
   reset-all
   ```

## Password Configuration

The default password is "openfirmware". To change it:

1. Calculate XOR values for your password with key `0xDEADBEEF`
2. Edit the `stored-password` array in `crazytown.fs`
3. Update `stored-password-length` to match

## Troubleshooting

- **Black screen**: Framebuffer initialization failed. Try different display depths.
- **No keyboard input**: Some USB keyboards may need initialization. Try PS/2.
- **Can't load files**: Check file paths and device aliases with `devalias`

## Removing Crazytown

From OpenFirmware:
```forth
setenv boot-command boot
reset-all
```

## Security Note

This is a demonstration project. The password protection can be bypassed by:
- Resetting NVRAM (Command+Option+P+R)
- Accessing OpenFirmware directly
- Booting from external media

For real security, use FileVault or firmware passwords.