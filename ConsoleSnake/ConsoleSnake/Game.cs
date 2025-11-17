using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public struct PlayerPos
    {
        public PlayerPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public PlayerPos(PlayerPos pos)
        {
            this.x = pos.x;
            this.y = pos.y;
        }

        public int x;
        public int y;
    }

    public class Game
    {
        Tile[,] board = new Tile[25, 50];

        List<PlayerPos> player = new List<PlayerPos>();

        PlayerPos playerFrontPos = new PlayerPos(25, 12);

        int score = 0;

        public Direction direction = Direction.Up;

        char c = 'w';
        char oldC = 'w';

        Direction prevDirection;

        Thread inputThread;

        public bool running = false;

        public Game() { }

        public void InitialiseBoard()
        {
            for (int y = 0; y < 25; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    board[y, x] = new Tile(y == 0 || y == 24 || x == 0 || x == 49);
                }
            }
        }

        public void RunGame()
        {
            running = true;

            inputThread = new Thread(new ThreadStart(GameLoop));
            inputThread.IsBackground = true;
            inputThread.Start();


        }

        public void GameLoop()
        {
            while (running)
            {
                c = Console.ReadKey(intercept: true).KeyChar;

                if (c != oldC)
                {
                    SetPlayerDirection(c);
                    oldC = c;
                }
            }
        }

        public void UpdateBoard()
        {
            foreach (PlayerPos player in player)
            {
                board[player.y, player.x].state = TileState.Snake;
            }
        }

        public void PrintBoard()
        {
            Console.Clear();

            for (int y = 0; y < 25; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    board[y, x].Print();
                }

                Console.Write("\n");
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void InitilisePlayer()
        {
            player.Add(new PlayerPos(25, 12));
        }

        public void SetPlayerDirection(char direction)
        {
            PlayerPos prevPos = playerFrontPos;
            prevDirection = this.direction;

            switch (direction)
            {
                case 'w':
                    //playerFrontPos.y--;
                    this.direction = Direction.Up;
                    break;

                case 'a':
                    //playerFrontPos.x--;
                    this.direction = Direction.Left;
                    break;

                case 's':
                    //playerFrontPos.y++;
                    this.direction = Direction.Down;
                    break;

                case 'd':
                    //playerFrontPos.x++;
                    this.direction = Direction.Right;
                    break;

                default:
                    break;

            }
        }

        public void AddPlayerMovement()
        {
            PlayerPos prevPos = playerFrontPos;


            switch (direction)
            {
                case Direction.Up:
                    playerFrontPos.y--;
                    break;

                case Direction.Left:
                    playerFrontPos.x--;
                    break;

                case Direction.Down:
                    playerFrontPos.y++;
                    break;

                case Direction.Right:
                    playerFrontPos.x++;
                    break;

                default:
                    break;

            }

            if (player.Count() > 1)
            {
                if (player[1].x == playerFrontPos.x && player[1].y == playerFrontPos.y)
                {
                    playerFrontPos = prevPos;
                    direction = prevDirection;
                    return;
                }
            }

            if (board[playerFrontPos.y, playerFrontPos.x].state == TileState.Free ||
                board[playerFrontPos.y, playerFrontPos.x].state == TileState.Apple)
            {
                if (board[playerFrontPos.y, playerFrontPos.x].state == TileState.Apple)
                {
                    score++;
                    Console.Title = $"Score: {score}";

                    player.Insert(0, new PlayerPos(playerFrontPos));
                    board[playerFrontPos.y, playerFrontPos.x].state = TileState.Snake;
                    PlaceApple();
                }
                else
                {
                    PlayerPos last = player.Last();
                    board[last.y, last.x].state = TileState.Free;
                    player.Remove(player.Last());

                    last = playerFrontPos;

                    player.Insert(0, last);
                }


            }
            else
            {
                playerFrontPos = prevPos;
                GameOver();
            }
        }


        public void PlaceApple()
        {
            bool placingApple = true;

            do
            {
                int y = new Random().Next(23);
                int x = new Random().Next(48);

                if (board[y, x].state == TileState.Free)
                {
                    board[y, x].state = TileState.Apple;
                    placingApple = false;
                }

            } while (placingApple);
        }

        public void GameOver()
        {
            //inputThread.Abort();
            running = false;
        }
    }
}
