# Release Version 1.0 - Roadmap

## Redeems

- [x] [TTI Weather: Help](#tti-weather-help)
- [x] [TTI Weather: Harm](#tti-weather-harm) 
- [x] [TTI Weather: Aurora](#tti-weather-aurora) 
- [x] [TTI Weather: Day/Night Toggle](#tti-weather-day-night-toggle)
- [x] [TTI Status: Help](#tti-status-help) 
- [x] [TTI Status: Harm](#tti-status-harm)
- [x] [TTI Status: Afflictions](#tti-status-afflictions)
- [x] [TTI Status: Affliction Cure](#tti-status-affliction-cure)
- [x] [TTI Status: Bleeding](#tti-status-bleeding)
- [x] [TTI Status: Sprain](#tti-status-sprain)
- [x] [TTI Status: Frostbite](#tti-status-frostbite)
- [x] [TTI Status: Stink](#tti-status-stink)
- [x] [TTI Inventory: Team NoPants](#tti-inventory-team-nopants)
- [x] [TTI Inventory: Drop Torch](#tti-inventory-drop-torch) 
- [x] [TTI Inventory: Drop Random Item](#tti-inventory-drop-random-item)
- [x] [TTI Inventory: Stepped on Stim](#tti-inventory-stepped-on-stim)
- [x] [TTI Inventory: Weapon](#tti-inventory-weapon)
- [x] [TTI Animal: Big Game](#tti-animal-big-game) 
- [x] [TTI Animal: T-Wolf Pack](#tti-animal-t-wolf-pack)
- [x] [TTI Animal: Stalking Wolf](#tti-animal-stalking-wolf)
- [x] [TTI Animal: Bunny Explosion](#tti-animal-bunny-explosion)
- [x] [TTI Misc: Happy 420](#tti-misc-happy-420)

## Details

### TTI Weather: Help
- changes the ingame weather
- possible userInputs: clear, fog, snow, cloudy
- can be enabled / disabled individually in settings

### TTI Weather: Harm 
- changes the ingame weather
- possible userInputs: blizzard, fog, snow
- can be enabled / disabled individually in settings

### TTI Weather: Aurora
- instantly makes it midnight and starts an aurora (aurora fading in)

### TTI Weather: Day Night Toggle
- adds 12h to the ingame time

### TTI Status: Help 
- changes specified meter to settings help value
- possible userInput: cold, fatigue, thirst, hunger
- configurable value in settings
- can be enabled / disabled individually in settings

### TTI Status: Harm
- changes specified meter to settings harm value
- possible userInput: cold, fatigue, thirst, hunger
- configurable value in settings
- can be enabled / disabled individually in settings

### TTI Status: Afflictions
- gives a random affliction
- possibe outcomes: foodpoisoning, dysentery, cabinfever, parasites, hypothermia
- can be enabled / disabled individually in settings
- refunds points if player already has rolled affliction
- if gamemode doesnt allow affliction, but it is not disabled in mod settings, viewers will loose points on it

### TTI Status: Affliction Cure
- instantly cures all afflictions

### TTI Status: Bleeding
- gives bleeding at random body part
- refunds points if player already has 4 or more bleedings

### TTI Status: Sprain
- randomly sprains wrist or ankle
- wrist sprains can be disabled separately (if you want to keep the fun, but not have it interfer too much with holding a weapon)
- refunds points if player already has 2 or more sprained wrists and TTI rolles 'wrist'
- refunds points if player already has 2 or more sprained ankles and TTI rolles 'ankle'

### TTI Status: Frostbite
- gives frostbite at random body part
- refunds points if player already has 4 or more frostbites

### TTI Status: Stink
- immediately adds stink 
- configurable time and intensity in mod settings

### TTI Inventory: Team NoPants
- drops all pants to the ground

### TTI Inventory: Drop Torch
- if holding torch (lit or unlit) player will drop that
- if not, TTI will check for best torch and drop that
- if there is no torch, TTI will look for a red flare
- if there is no red flare, TTI will look for a blue flare
- refunds points if player doesnt have torch or flare in inventory

### TTI Inventory: Drop Random Item
- drop random item in inventory

### TTI Inventory: Stepped on Stim
- immediatly activates the stim effect (doesnt consume from inventory)

### TTI Inventory: Weapon
- adds weapon
- possible user inputs: bow, rifle, revolver, flaregun
- can be enabled / disabled individually in settings
- amount of ammo configurable individually in settings

### TTI Animal: Big Game
- spawns bear or moose depending on userInput
- defaults to bear if enabled, defaults to moose if bear is not enabled
- can be enabled / disabled individually in settings
- configurable spawn distance in settings
- spawns aurorabear during an aurora
- delays moose spawn in case of aurora until its over

### TTI Animal: T-Wolf Pack
- spawns between 2-5 t-wolves depending on userInput
- configurable spawn distance in settings
- if userInput cannot be parsed, spawns a pack of 5
- spawns aurora t-wolves during an aurora

### TTI Animal: Stalking Wolf
- spawns wolf behind (opposite direction camera is facing)
- configurable spawn distance in settings
- spawns aurora wolf during an aurora

### TTI Animal: Bunny Explosion
- spawns x amount of bunnies
- configurable bunny count in settings

### TTI Misc: Happy 420
- plays the suffocate sound
