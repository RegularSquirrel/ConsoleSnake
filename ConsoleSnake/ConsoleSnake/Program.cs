using ConsoleSnake;

Game game = new Game();

game.Initialise();
game.Begin();

do
{
    game.Update();
    Thread.Sleep(75);
} while (game.running);

Console.WriteLine("Game over!");
Console.WriteLine($"Your score: {game.score}  |  High score: {game.highScore}");