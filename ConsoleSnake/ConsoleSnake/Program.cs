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

Console.WriteLine("Game over!");
Console.WriteLine($"Your score: {game.score}  |  High score: {game.highScore}");