# windows-telemetry-utility

## Features
A small utility designed to remove the annoyances out of windows 10 such as:
- Disable Windows 11 Upgrade Notifications *You can update manually whenever desired through the windows update window, but the annoying popups should not come up anymore*
- Disable Bing Search in Windows Search
- Disable Web Suggestions
- Revoke Cortana Permissions

## Requirements
Before you can run the *make.sh* located within this repository you should have installed the following tools:
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)
- [Git Bash](https://git-scm.com/downloads) *To obviously clone this repo **and execute the make.sh** on windows*

## Compiling
- Simply execute the **make.sh** by either double-clicking the file or executing it from the terminal.
- You can find the compiled binaries in the **<path_to_repo>/builds/telemetry-utility/** directory.
- The main binary is called **WTU.MainApp.exe**
- You only should need this program again whenever you want to change your settings there's no requirement for auto-start.

## Notes
- This tool has to be run as administrator, because otherwise you're not allowed to make changes to the windows registry.
- I made this tool on a relatively relaxed weekend, because my personal most hated feature is the additional web suggestions directly built into the system search.. especially if you attempt to search for system settings or an app from the system settings panel.
- Currently there are no additional updates planned and you can build the code on your own and do with it whatever you want, i just wanted to ensure that theres atleast one clean tool out on the globe working, accessible and being transparent for getting these things away even for non system administrators aka a simple user.