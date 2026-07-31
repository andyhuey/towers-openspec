using TowersOfHanoi.Core;

var diskCount = StartScreen.Run();
var game = new Game(diskCount);
GameplayScreen.Run(game);
EndScreen.Run(game);
