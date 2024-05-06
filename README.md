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

### Afflictions
- TTI: Cabin Fever
- TTI: Dysentery
- TTI: Food Poisoning
- TTI: Hypothermia
- TTI: Parasites

### Player sounds
- (TTI Sound: Dev Check)
- TTI Sound: Hydrate
- TTI Sound: Happy 420
- TTI Sound: Good Night
- TTI Sound: Hello

## Planned redeems for release version 1.0

### TTI: Good Weather
- userInput: fog, snow, clear, cloudy
- settings: enable each weather type individually

### TTI: Bad Weather 
- userInput: fog, snow, blizzard
- settings: enable each weather type individually

### TTI: Aurora
- instantly makes it midnight and starts an aurora (aurora fading in)

### TTI: Status Help 
- userInput: fatigue, cold, hunger, thirst
- settings: actual value the redeem will set the meter to

### TTI: Status Harm
- userInput: fatigue, cold, hunger, thirst
- settings: actual value the redeem will set the meter to

### TTI: Cabin Fever

### TTI: Dysentery

### TTI: Food Poisoning

### TTI: Hypothermia

### TTI: Bleeding
- userInput: handleft, handright, footleft, footright 

### TTI: Sprain
- userInput: handleft, handright, footleft, footright 

### TTI: Team NoPants
- drops all pants

### TTI: Stink
- immediately adds 3 stink lines 

### TTI: Drop Torch
- if you are holding one it drops or best torch in the inventory 

### TTI: Drop Random Item
- drop random item in inventory 

### TTI: Teleport
- userInput: [some locations that still have to be decided on. thinking summit, goldmine, monolithlake. something along those lines]

### TTI: Stepped on Stim
- immediatly activates the stim effect (doesnt consume from inventory)

### TTI: Bow
- also adds 10 arrows

### TTI: Time of day
- userInput: an actual time of day, like 12:00, 17:42, 01:55

### TTI: Big Game
- userInput: bear, moose
- settings: spawn distance
- spawns in front
- spawns aurorabear during an aurora, delays moose spawn in case of aurora until its over

### TTI: T-Wolf Pack
- userInput: 2-5
- settings: spawn distance
- spawns in front

### TTI: Stalking Wolf
- settings: spawn distance
- spawns behind (oposite direction camera is looking at at time of redeem)

### TTI: Bunny Explosion
- settings: amount of bunnies (for performance sake)
- spawns x amount of bunnies slightly in front

### TTI Sound: Happy 420
- plays the suffocate sound

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
