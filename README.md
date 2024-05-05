# TLD-Twitch-Integration
Mod for The Long Dark that let's viewers of a twitch live stream interact with the game.
The channel using this mod needs to be at least affiliate with twitch, so custom redeems are enabled.

## What can we do

### Changing the weather
- TTI: Blizzard
- TTI: Clear
- TTI: Light Fog
- TTI: Dense Fog
- TTI: Partly Cloudy
- TTI: Cloudy
- TTI: Light Snow
- TTI: Heavy Snow

### Spawning animals
- TTI: Bear
- TTI: Moose
- TTI: T-Wolf Pack (5)
- TTI: Stalking Wolf
- TTI: Bunny Explosion

### Changing status
- TTI: Hungry
- TTI: Thirst
- TTI: Tired
- TTI: Freezing
- TTI: Full
- TTI: Not Thirsty
- TTI: Awake
- TTI: Warm

### Player sounds
- (TTI Sound: Dev Check)
- TTI Sound: Hydrate
- TTI Sound: Happy 420
- TTI Sound: Good Night
- TTI Sound: Hello

## Redeems
- TTI will create all twitch redeems on initial load (game start)
- if there is any naming conflicts with redeems TTI wants to create and redeems you already have, TTI will skip creation and retry on next load
- TTI checks regularly and tries recreating redeems that are missing on twitch. deleting a TTI redeem in your twitch dashboard will trigger TTI to recreate its default.
- all TTI created redeems can be editted in any way (including title, cost, color, icons and prompts).
- changing the redeems enabled status in the twitch dashboard will be overwritten by availability setting in mod settings in game
- if you uninstall TTI, but want to preserve your redeems on twitch and want to relink them at a later point, keep the file /TLD-Twitch-Integration/redeems.json from your Mods folder.

**limitation: twitch allows for a maximum of 50 custom rewards (redeems) which includes both enabled and disabled**

## Requirements
- if you haven't done so already, install MelonLoader by downloading and running [MelonLoader.Installer.exe](https://github.com/HerpDerpinstine/MelonLoader/releases/latest/download/MelonLoader.Installer.exe)
- download and place [ModSettings.dll](https://github.com/zeobviouslyfakeacc/ModSettings/releases) in your /Mods/ folder
- install TTI

## Install (Development Build)
- go to twitch developer console and [register new application](https://dev.twitch.tv/docs/authentication/register-app/)
- check out development branch
- edit CleintId and ClientSecret properties in TLD-Twitch-Integration/AuthService.cs
- build solution -> copy TLD-Twitch-Integration.dll to your Mods folder

### Uninstall 
- remove TLD-Twitch-Integration.dll from your /Mods/ folder
- remove /Mods/TLD_Twitch_Integration/ and everything in it (preserve redeems.json if you wish to keep your redeem settings for later)

## Getting started
- start the game
- follow the authorization url from the melon loader console
- go to mod settings and configure which redeems to allow
- go to twitch and configure redeem appearence, cost and alerts
- go live and have fun

## Logging out

To log out for example to connect a different twitch account go to the mods data folder /Mods/TLD_Twitch_Integration/ and delete the file login.json

## Credits

[ChefMaria](https://www.twitch.tv/chefmaria) banger ideas

[NeilsClark](https://www.youtube.com/@ProfNeils) art work

[TLD Modding discord](https://discord.gg/nb2jQez) general awesomeness

[DigitalZombie](https://github.com/DigitalzombieTLD) super helpful pointers
