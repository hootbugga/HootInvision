# Hoot-Invision

A small FiveM server resource that defers player connections and checks a player's whitelist status against an Invision Community (IPS) forum API.

**Contents**
- `Main.cs`: Core resource implementation (class `HootInvision`).
- `HootInvision.csproj`: Project file with NuGet references.
- `fxmanifest.lua`: FiveM resource manifest.
- `config.json`: API keys and allowed group IDs (example included).

**Requirements**
- .NET Framework 4.7.1 Developer Pack / Targeting Pack (for `net471`).
- `dotnet` SDK installed for CLI builds (used for `dotnet build`).
- A FiveM server to deploy the resource.

Getting started
--------------

1. Edit `config.json` with your forum API settings and allowed group IDs:

```json
{
  "ApiUrl": "https://yourforum.com/api",
  "ApiKey": "your_api_key_here",
  "AllowedGroups": [4, 8],
  "DeferMessage": "Checking whitelist with the community's whitelist database, please wait...",
  "KickMessage": "You are not whitelisted on our forums. Join at yourforum.com"
}
```

2. Build the project from the repo root:

```powershell
dotnet build "HootInvision.csproj"
```

Output DLL will be at `bin/Debug/net471/HootInvision.dll` (or `bin/Release/...` for release builds).

3. Deploy to FiveM server:
- Copy the entire resource folder (this repository) to your server's `resources` directory.
- Ensure `fxmanifest.lua` is present and references the built DLL if needed.
- Add the resource to your server's `server.cfg` (e.g., `ensure Hoot-Invision`).

Configuration notes
-------------------
- `ApiUrl`: Base API URL for Invision Community. The plugin calls `core/members/{memberId}`.
- `ApiKey`: Your IPS API key, passed as `IPS-API-Key` header.
- `AllowedGroups`: Numeric group IDs to allow. `PrimaryGroup.Id` and `SecondaryGroups[].Id` are checked.

Troubleshooting
---------------
- Error: Missing runtime binder (`CS0656`) — solved by adding `Microsoft.CSharp` dependency (already included).
- If you target a runtime without the .NET targeting pack, install the .NET Framework 4.7.1 Developer Pack.
- Check `Main.cs` logs for debug messages; FiveM server console will show messages from `Debug.WriteLine`.

Contributing
------------
PRs welcome. Keep changes focused and include build verification. Run `dotnet build` before opening PRs.

License
-------
Include your license here (e.g., MIT). If none, add one before publishing.

Files of interest
- [HootInvision.csproj](HootInvision.csproj)
- [Main.cs](Main.cs)
- [config.json](config.json)

Questions or help?
------------------
Open an issue on GitHub with build logs and the output of `dotnet --info`.

Installation (from Releases)
----------------------------
The Releases page will provide a zip containing a single folder named `HootInvision` with these files:

- `HootInvision/Config.json`
- `HootInvision/fxmanifest.lua`
- `HootInvision/HootInvision.dll`

To install the released package on your FiveM server:

1. Download the zip from Releases and extract the `HootInvision` folder into your server's `resources` directory.
2. Ensure the folder layout on the server is exactly `resources/HootInvision/` with the three files above inside it.
3. Add the resource to your `server.cfg` (for example):

```ini
ensure HootInvision
```

4. Start or restart the server; the resource will run using the included `HootInvision.dll`.

Manual build and packaging (for maintainers/contributors)
-------------------------------------------------------
If you prefer to build the DLL yourself and produce the release package:

1. Edit `config.json` as needed.
2. Build the project from the repo root:

```powershell
dotnet build "HootInvision.csproj"
```

3. After a successful build, copy the produced DLL from the build output into a `HootInvision` folder for release packaging. Example output path:

```
bin\Debug\net471\HootInvision.dll
```

4. Create a zip containing the `HootInvision` folder with `Config.json`, `fxmanifest.lua`, and `HootInvision.dll` and upload that zip to the Releases page.

License & Distribution
----------------------
This repository is published under the **Hootbugga License**. See `LICENSE` for the full terms. In short:

- You may download, run, and modify the code for your own use.
- You may redistribute the original, unmodified release package (the files as published in Releases).
- You are NOT permitted to redistribute edited/modified versions of the code or compiled binaries derived from edited code — whether for free or for sale.
- All redistributions of the original release must include the `LICENSE` file and retain attribution.

If you are unsure about allowed uses, open an issue to discuss.
