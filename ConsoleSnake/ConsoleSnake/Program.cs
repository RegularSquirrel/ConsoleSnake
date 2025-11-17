using ConsoleSnake;

Console.WriteLine("Hello, World!");

Game game = new Game();

game.InitialiseBoard();
game.PrintBoard();
game.InitilisePlayer();
game.UpdateBoard();



game.PlaceApple();

game.PrintBoard();
game.RunGame();

do
{
    //char c = Console.ReadKey().KeyChar;
    //game.SetPlayerDirection(c);
    game.AddPlayerMovement();
    game.UpdateBoard();
    game.PrintBoard();
    Thread.Sleep(50);
} while (game.running);