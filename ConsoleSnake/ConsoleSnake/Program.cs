using ConsoleSnake;

Game game = new Game();

game.Initialise();
game.Run();

do
{
    game.AddPlayerMovement();
    game.UpdateBoard();
    game.PrintBoard();
    Thread.Sleep(50);
} while (game.running);