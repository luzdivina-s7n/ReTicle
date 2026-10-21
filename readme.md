# rtc

lightweight crosshair overlay for Windows

## tray menu

- **size:** tiny / small / medium
- **shape:** dot / cross / ring / square
- **color:** white / red / blue / green
- **hide:** temporarily hide or show the crosshair
- **exit**

## settings

the selected size, shape, and color are stored in the current user's registry under `HKCU\Software\rtc` and are restored at the next launch. no files are created and the executable is not modified.

## download

download the latest release from the [releases page](https://github.com/luzdivina-s7n/ReTicle/releases/).

## performance

- memory usage: ~2 MB working set, ~10 MB private
- CPU usage: 0% at idle

## build

requires Windows and .NET Framework 4.0 or later.

```
build.bat
```

the build produces `rtc.exe`.

## run

requires Windows and .NET Framework 4.0 or later. run `rtc.exe`.
