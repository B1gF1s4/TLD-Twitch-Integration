# TLD-Twitch-Integration
Mod for The Long Dark that let's viewers of a twitch live stream interact with the game.
The channel using this mod needs to be at least affiliate with twitch, so custom redeems are enabled.

## What can we do
- TTI: Blizzard
- TTI: Clear
- TTI: Light Fog
- TTI: Dense Fog
- TTI: Partly Cloudy
- TTI: Cloudy
- TTI: Light Snow
- TTI: Heavy Snow
- TTI Sound: Hydrate
- TTI Sound: Happy 420
- TTI Sound: Good Night
- TTI Sound: Hello

## Redeems
- TTI will create all twitch redeems on initial load (game start)
- if there is any naming conflicts with redeems TTI wants to create and redeems you already have, TTI will skip creation and retry on next load
- TTI checks regularly and tries recreating redeems that are missing on twitch. deleting a TTI redeem in your twitch dashboard will trigger TTI to recreate its default.
- all TTI created redeems can be editted in any way (including title, cost, color, icons and prompts).
- changing the redeems enabled status in the twitch dashboard will be overwritten by availabilty setting in mod settings in game
- if you uninstall TTI, but want to preserve your redeems on twitch and want to relink them at a later point, keep the file /TLD-Twitch-Integration/redeems.json from your Mods folder.

- !!!! limitation: twitch allows for a maximum of 50 custom rewards (redeems) which includes both enabled and disabled redeems

## How to install
- install melon
- install mod settings mod
- install TTI

## Getting started
- start the game
- follow the authorization url from the melon loader console
- go to mod settings and configure which redeems to allow
- go to twitch and configure redeem appearence, cost and alerts
- go live and have fun

## Development build
- go to twitch devloper console and create new application (clientId and clientSecret)
- check out development branch
- add CleintId and ClientSecret in AuthService.cs
- build solution -> copy TLD-Twitch-Integration.dll to your Mods folder

## Logging out

To log out for example to connect a different twitch account go to the mods data folder /Mods/TLD_Twitch_Integration and delete the file login.json

