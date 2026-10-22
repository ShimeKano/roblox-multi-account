# Roblox Multi-Client Lite (Windows)

A lightweight Windows utility for launching Roblox game pages and toggling a legacy multi-instance compatibility technique. It does **not** manage accounts or automate sign-in.

## Features implemented in the app

These describe what the application code is designed to do; they do not imply that Roblox will allow multiple independent clients.

- **Enable / disable toggle:** creates and holds a named Windows event, `ROBLOX_singletonEvent`, while enabled; disposes it when disabled or when the app closes.
- **Roblox process counter:** checks for processes named `RobloxPlayerBeta` and refreshes the displayed count every 1.5 seconds.
- **Open a game page:** accepts an optional HTTP(S) URL only when its host is `roblox.com` or `www.roblox.com`. If the field is empty, opens the official Roblox games page in the default browser.
- **Input validation:** shows a warning when the entered URL is not a valid URL on the allowed Roblox host.
- **No admin prompt by design:** the app does not explicitly request administrator privileges.
- **No credential handling:** sign-in is manual. The app does not ask for, read, or save Roblox passwords, browser data, or `.ROBLOSECURITY` cookies.
- **Automatic cleanup:** closing the app releases the named event handle.

## Important limitations

- **Multiple independent Roblox sessions are NOT confirmed.** The toggle only holds a named event object; current Roblox clients may ignore this legacy technique or behave differently after updates.
- **No promise of five clients.** The app has no five-account launcher or session-isolation system. Whether multiple clients can open and stay signed into different accounts is unverified and depends on Roblox's current client and policies.
- **The process counter is not an account check.** It counts matching Windows processes only; it cannot tell whether they belong to different accounts or sessions.
- **Opening a game page is not a join automation feature.** Each client must be signed in and joined using Roblox's normal UI.
- The app does not patch Roblox binaries, inject code, bypass anti-cheat, extract authentication tickets, or store credentials.
- Stop testing if Roblox displays a warning or blocks the launch. Use the tool at your own risk.

## Requirements

- Windows 10 or Windows 11
- .NET 8 SDK to build from source

## Run from source

Open PowerShell in the repository directory:

```powershell
dotnet build .\RobloxMultiClientLite.csproj -c Release
dotnet run --project .\RobloxMultiClientLite.csproj
```

## How to use

1. Close Roblox clients before testing.
2. Start the app normally; administrator rights are not intentionally requested.
3. Click **Enable multi-instance**. The status changes to ON if the named event can be created; this is not proof that multiple clients will work.
4. Optionally paste an official Roblox game-page URL and click **Open game page**. If the field is empty, the official games page opens.
5. Sign in manually and use Roblox's normal UI in each client. Do not assume separate accounts will work.
6. Click **Disable multi-instance** when finished, or close the utility to release the event handle.

## Download

For a prebuilt Windows executable, check the repository's [Releases](https://github.com/ShimeKano/roblox-multi-account/releases) page. If no release is published yet, see [GitHub Actions](https://github.com/ShimeKano/roblox-multi-account/actions/workflows/release-windows.yml) for build artifacts.

## Safety

Never paste or save your `.ROBLOSECURITY` cookie in this app, a script, or a chat. It is a session credential. This project intentionally has no account manager or credential storage.
