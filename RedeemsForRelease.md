# Release Version 1.0 - Roadmap

## Redeems

- [ ] [TTI Weather: Help](#tti-weather-help)
- [ ] [TTI Weather: Harm](#tti-weather-harm) 
- [ ] [TTI Weather: Aurora](#tti-weather-aurora) 
- [ ] [TTI Status: Help](#tti-status-help) 
- [ ] [TTI Status: Harm](#tti-status-harm)
- [ ] [TTI Status: Cabin Fever](#tti-status-cabin-fever)
- [ ] [TTI Status: Dysentery](#tti-status-dysentery)
- [ ] [TTI Status: Food Poisoning](#tti-status-food-poisoning)
- [ ] [TTI Status: Hypothermia](#tti-status-hypothermia)
- [ ] [TTI Status: Bleeding](#tti-status-bleeding)
- [ ] [TTI Status: Sprain](#tti-status-sprain)
- [ ] [TTI Status: Stink](#tti-status-stink)
- [ ] [TTI Inventory: Team NoPants](#tti-inventory-team-nopants)
- [ ] [TTI Inventory: Drop Torch](#tti-inventory-drop-torch) 
- [ ] [TTI Inventory: Drop Random Item](#tti-inventory-drop-random-item)
- [ ] [TTI Inventory: Stepped on Stim](#tti-inventory-stepped-on-stim)
- [ ] [TTI Inventory: Bow](#tti-inventory-bow)
- [ ] [TTI Animal: Big Game](#tti-animal-big-game) 
- [ ] [TTI Animal: T-Wolf Pack](#tti-animal-t-wolf-pack)
- [ ] [TTI Animal: Stalking Wolf](#tti-animal-stalking-wolf)
- [ ] [TTI Animal: Bunny Explosion](#tti-animal-bunny-explosion)
- [ ] [TTI Misc: Time of day](#tti-misc-time-of-day)
- [ ] [TTI Misc: Teleport](#tti-misc-teleport)
- [ ] [TTI Sound: Happy 420](#tti-sound-happy-420)

## Details

- userInput order reflects fallback order in case userInput cannot be parsed. for example for the weather help redeem, if userInput is 'someUnidentifiedUserInput' TTI executes clear. if that is disabled, it executes fog, and so on

### TTI Weather: Help
- [ ] imeplement userInput: clear, fog, snow, cloudy
- [x] implement settings: enable each weather type individually
- [x] changes the ingame weather

### TTI Weather: Harm 
- [ ] implement userInput: fog, snow, blizzard
- [x] imeplement settings: enable each weather type individually
- [x] changes the ingame weather

### TTI Weather: Aurora
- [ ] instantly makes it midnight and starts an aurora (aurora fading in)
- [x] imeplement settings: enable / disable

### TTI Status: Help 
- [ ] implement userInput: fatigue, cold, hunger, thirst
- [x] imeplement settings: actual value the redeem will set the meter to, enable each meter individually
- [x] changes the specified meter to the specified value

### TTI Status: Harm
- [ ] implement userInput: fatigue, cold, hunger, thirst
- [x] imeplement settings: actual value the redeem will set the meter to, enable each meter individually
- [x] changes the specified meter to the specified value

### TTI Status: Cabin Fever
- [x] gives player cabin fever
- [ ] imeplement settings: enable / disable

### TTI Status: Dysentery
- [x] gives the player dysentery
- [ ] imeplement settings: enable / disable

### TTI Status: Food Poisoning
- [x] gives the player food poisoning
- [ ] imeplement settings: enable / disable

### TTI Status: Hypothermia
- [x] gives the player hypothermia
- [ ] imeplement settings: enable / disable

### TTI Status: Bleeding
- [ ] implement userInput: handleft, handright, footleft, footright 
- [ ] imeplement settings: enable / disable
- [ ] gives the player bleeding

### TTI Status: Sprain
- [ ] implement userInput: handleft, handright, footleft, footright 
- [ ] imeplement settings: enable / disable / disabling wrists
- [ ] gives the player sprain

### TTI Status: Stink
- [ ] immediately adds 3 stink lines 
- [ ] imeplement settings: enable / disable

### TTI Inventory: Team NoPants
- [ ] drops all pants
- [ ] imeplement settings: enable / disable

### TTI Inventory: Drop Torch
- [ ] if you are holding one it drops or best torch in the inventory 
- [ ] imeplement settings: enable / disable

### TTI Inventory: Drop Random Item
- [ ] drop random item in inventory (cannot pick back up)
- [ ] imeplement settings: enable / disable

### TTI Inventory: Stepped on Stim
- [ ] immediatly activates the stim effect (doesnt consume from inventory)
- [ ] imeplement settings: enable / disable

### TTI Inventory: Bow
- [ ] adds bow
- [ ] adds x arrows
- [ ] imeplement settings: enable / disable / arrow amount

### TTI Animal: Big Game
- [ ] implemenet userInput: bear, moose
- [x] implement settings: spawn distance / enable / disable
- [x] spawns in front
- [x] spawns aurorabear during an aurora, delays moose spawn in case of aurora until its over

### TTI Animal: T-Wolf Pack
- [ ] implement userInput: 2-5
- [ ] implement settings: spawn distance / enable / disable
- [x] spawns in front

### TTI Animal: Stalking Wolf
- [ ] implement settings: spawn distance / enable / disable
- [x] spawns behind (oposite direction camera is looking at at time of redeem)

### TTI Animal: Bunny Explosion
- [ ] settings: amount of bunnies (for performance sake) / enable / disable
- [x] spawns x amount of bunnies slightly in front

### TTI Misc: Teleport
- [ ] implement userInput: [some locations that still have to be decided on. thinking summit, goldmine, monolithlake. something along those lines]
- [ ] imeplement settings: enable / disable
- [ ] teleports the player to the specified location

### TTI Misc: Time of day
- [ ] implement userInput: an actual time of day, like 12:00, 17:42, 01:55
- [ ] imeplement settings: enable / disable
- [ ] sets the in game time of day to the specified time

### TTI Sound: Happy 420
- [x] plays the suffocate sound
- [ ] imeplement settings: enable / disable