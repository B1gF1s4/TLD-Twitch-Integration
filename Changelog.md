# Changelog

## PRE-RELEASE-0.2.0

- adds optional kill counter
- adds moddata dependency
- further lowered accepted height offset for animal spawns
- adds distance traveled display
- adds animals in range counter
- adds animal cleanup outside of range

## PRE-RELEASE-0.1.2

- fixed an issue where drop item redeem was broken after rolling water
- fixed an issue where afflictions cure redeem was enabled while afflictions were turned off
- fixed an issue where manually rejecting or completing a redeem while in main menu would crash the game
- separated syncing redeems enabled status from checking for deleted redeems and lowered its interval to 3s
	- this will make redeems on twitch appear/disappear faster after enabling/disabling in settings
	- deleted redeems on twitch side will still be recreated every 30s
- added 10min default cooldown to most redeems

## PRE-RELEASE-0.1.1

- added changelog
- cleaned up AssemblyInfo
- fixed typos in settings
- fixed an issue where execution of a redeem could get stuck in a loop when something went wrong during processing
- fixed an issue where manually completing or refunding a redeem could cause TTI to get stuck
- added refunding of all open redeems in main menu. new runs now start fresh
- fixed an issue with mod settings of status harm redeem were not applied correctly
- slightly lowered accepted height offset for animal spawns