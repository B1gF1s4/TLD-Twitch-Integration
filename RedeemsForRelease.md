# Release Version 1.0 - Roadmap

## Redeems

- [x] [TTI Weather: Help](#tti-weather-help)
- [x] [TTI Weather: Harm](#tti-weather-harm) 
- [x] [TTI Weather: Aurora](#tti-weather-aurora) 
- [x] [TTI Status: Help](#tti-status-help) 
- [x] [TTI Status: Harm](#tti-status-harm)
- [ ] [TTI Status: Afflictions](#tti-status-afflictions)
- [ ] [TTI Status: Bleeding](#tti-status-bleeding)
- [ ] [TTI Status: Sprain](#tti-status-sprain)
- [ ] [TTI Status: Frostbite](#tti-status-frostbite)
- [ ] [TTI Status: Stink](#tti-status-stink)
- [ ] [TTI Inventory: Team NoPants](#tti-inventory-team-nopants)
- [ ] [TTI Inventory: Drop Torch](#tti-inventory-drop-torch) 
- [ ] [TTI Inventory: Drop Random Item](#tti-inventory-drop-random-item)
- [ ] [TTI Inventory: Stepped on Stim](#tti-inventory-stepped-on-stim)
- [ ] [TTI Inventory: Bow](#tti-inventory-bow)
- [x] [TTI Animal: Big Game](#tti-animal-big-game) 
- [x] [TTI Animal: T-Wolf Pack](#tti-animal-t-wolf-pack)
- [x] [TTI Animal: Stalking Wolf](#tti-animal-stalking-wolf)
- [x] [TTI Animal: Bunny Explosion](#tti-animal-bunny-explosion)
- [ ] [TTI Misc: Time of day](#tti-misc-time-of-day)
- [ ] [TTI Misc: Teleport](#tti-misc-teleport)
- [ ] [TTI Sound: Happy 420](#tti-sound-happy-420)

## Details

- userInput order reflects fallback order in case userInput cannot be parsed. for example for the weather help redeem, if userInput is 'someUnidentifiedUserInput' TTI executes clear. if that is disabled, it executes fog, and so on

### TTI Weather: Help
- changes the ingame weather
- possible userInputs: clear, fog, snow, cloudy (this is also the order of fallback when disabled)
- can be enabled / disabled individually in settings

### TTI Weather: Harm 
- changes the ingame weather
- possible userInputs: blizzard, fog, snow (this is also the order of fallback when disabled)
- can be enabled / disabled individually in settings

### TTI Weather: Aurora
- instantly makes it midnight and starts an aurora (aurora fading in)
- this currently also works in the dam ...

### TTI Status: Help 
- changes specified meter to settings help value
- possible userInput: cold, fatigue, thirst, hunger (this is also the order of fallback when disabled)
- configurable value in settings
- can be enabled / disabled individually in settings

### TTI Status: Harm
- changes specified meter to settings harm value
- possible userInput: cold, fatigue, thirst, hunger (this is also the order of fallback when disabled)
- configurable value in settings
- can be enabled / disabled individually in settings

### TTI Status: Afflictions
- gives a random affliction
- possibe outcomes: foodpoisoning, dysentery, cabinfever, parasites, hypothermia
- can be enabled / disabled individually in settings

### TTI Status: Bleeding
- gives bleeding at specified body part
- possible userInput: handleft, handright, footleft, footright 

### TTI Status: Sprain
- gives a sprain at specified body part
- possible userInput:  handleft, handright, footleft, footright 
- wrist sprains can be disabled separately in settings

### TTI Status: Frostbite
- gives frostbite at random body part

### TTI Status: Stink
- immediately adds 3 stink lines 
- configurable time and intensity in settings

### TTI Inventory: Team NoPants
- [ ] drops all pants
- [x] imeplement settings: enable / disable

### TTI Inventory: Drop Torch
- [ ] if you are holding one it drops or best torch in the inventory 
- [x] imeplement settings: enable / disable

### TTI Inventory: Drop Random Item
- [ ] drop random item in inventory (cannot pick back up)
- [x] imeplement settings: enable / disable

### TTI Inventory: Stepped on Stim
- [ ] immediatly activates the stim effect (doesnt consume from inventory)
- [x] imeplement settings: enable / disable

### TTI Inventory: Bow
- [ ] adds bow
- [ ] adds x arrows
- [x] imeplement settings: enable / disable / arrow amount

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

### TTI Misc: Teleport
- [ ] implement userInput: [some locations that still have to be decided on. thinking summit, goldmine, monolithlake. something along those lines]
- [x] imeplement settings: enable / disable
- [ ] teleports the player to the specified location

### TTI Misc: Time of day
- [ ] implement userInput: an actual time of day, like 12:00, 17:42, 01:55
- [x] imeplement settings: enable / disable
- [ ] sets the in game time of day to the specified time

### TTI Sound: Happy 420
- [x] plays the suffocate sound
- [x] imeplement settings: enable / disable