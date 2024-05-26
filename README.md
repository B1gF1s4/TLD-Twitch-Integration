# TLD-Twitch-Integration
Mod for The Long Dark that let's viewers of a twitch live stream interact with the game.
The channel using this mod needs to be at least affiliate with twitch, so custom redeems are enabled.

## Install

- download and install [dotnet 6.0 runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- install [MelonLoader](https://github.com/HerpDerpinstine/MelonLoader/releases/latest/download/MelonLoader.Installer.exe) **v0.6.1**
- download and place [AudioCore.dll](https://github.com/DigitalzombieTLD/AudioCore/releases) in your **[TLD]/Plugins/** folder
- download and place [AudioManager.dll](https://github.com/DigitalzombieTLD/AudioManager/releases) in your **[TLD]/Mods/** folder
- download and place [ModSettings.dll](https://github.com/zeobviouslyfakeacc/ModSettings/releases) in your **[TLD]/Mods/** folder
- download and place [DevConsole.dll](https://github.com/FINDarkside/TLD-Developer-Console/releases) in your **[TLD]/Mods/** folder
- download and place [TLD-Twitch-Integration.dll](https://github.com/B1gF1s4/TLD-Twitch-Integration/releases) in your **[TLD]/Mods/** folder

### Getting started
- start the game
- follow the authorization url from the melon loader console
	- **TTI does not ask for password! Some users report a password dialog opening after authorizing. You can just exit this, and try the auth url again. TTI does not ask for your twitch password!**
- go to mod settings and configure which redeems to allow
- go to twitch and configure redeem appearence, cost and alerts
- go live and have fun

### Uninstall 
- remove TLD-Twitch-Integration.dll from your /Mods/ folder
- remove /Mods/TLD_Twitch_Integration/ and everything in it (preserve redeems.json if you wish to keep your redeem settings for later)

## Redeems
- TTI will create all twitch redeems on initial load (game start)
- if there is any naming conflicts with redeems TTI wants to create and redeems you already have, TTI will skip creation and retry on next load
- TTI checks regularly and tries recreating redeems that are missing on twitch. deleting a TTI redeem in your twitch dashboard will trigger TTI to recreate its default.
- all TTI created redeems can be editted in any way (including title, cost, color, icons and prompts).
- changing the redeems enabled status in the twitch dashboard will be overwritten by availability setting in mod settings in game
- if you uninstall TTI, but want to preserve your redeems on twitch and want to relink them at a later point, keep the file /TLD-Twitch-Integration/redeems.json from your Mods folder.

**limitation: TTI currently comes with 23 redeems. Twitch allows for a maximum of 50 custom rewards (redeems) which includes both enabled and disabled**

## Logging out

To log out for example to connect a different twitch account go to the mods data folder /Mods/TLD_Twitch_Integration/ and delete the file login.json

## Credits

[B1gF1s4](https://www.twitch.tv/b1gf1s4) streams this regularly on twitch

[ChefMaria](https://www.twitch.tv/chefmaria) banger ideas

[NeilsClark](https://www.youtube.com/@ProfNeils) art work

[MustardGlove](https://www.twitch.tv/mustardglove) being persistent about something to use channelpoints on

[TLD Modding discord](https://discord.gg/nb2jQez) general awesomeness

[DigitalZombie](https://github.com/DigitalzombieTLD) super helpful pointers
