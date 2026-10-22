# Roblox Multi-Client Lite (Windows)

A small, source-first Windows utility for experimenting with Roblox multi-instance behavior. It does **not** store credentials, read browser data, or automate sign-in.

## Status and limitations

- Holds a named Windows mutex (`ROBLOX_singletonEvent`) while enabled, then releases it when disabled or the app exits.
- This is a compatibility experiment, **not a guarantee** that current Roblox clients will permit multiple independent sessions. Roblox updates can make this approach ineffective.
- It does not patch Roblox binaries, inject code, disable anti-cheat, extract authentication tickets, or request `.ROBLOSECURITY`.
- Log in manually in each client. To join the same experience, open the same game page and use Roblox's normal UI in each client. Whether the clients can maintain separate sessions depends on the current Roblox build and its policies.
- Use at your own risk and stop if Roblox displays a warning or blocks the launch.

## Build

Requirements: Windows 10/11 and .NET 8 SDK.

```powershell
dotnet build .\RobloxMultiClientLite.csproj -c Release
dotnet run --project .\RobloxMultiClientLite.csproj
```

## Use

1. Close all Roblox clients before testing.
2. Start the app normally; administrator rights are not required.
3. Click **Enable multi-instance**, then launch Roblox normally from its official website.
4. Sign in manually. Repeat only if the current Roblox client allows another instance.
5. Click **Disable** when finished, or close the utility to release the mutex.

The app only displays the number of running Roblox player processes; that count does not prove each window has a different account or session.

## Safety

Never paste or save your `.ROBLOSECURITY` cookie in this app, a script, or a chat. That cookie is a session credential. This project intentionally has no account manager or credential storage.
