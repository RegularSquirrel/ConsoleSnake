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
        const int SIZE_X = 25;
        const int SIZE_Y = 25;

        const string HIGHSCORE_FILE_NAME = "HighScore.txt";

        //Game state
        public bool running = false;

        //Game board
        Tile[,] board = new Tile[SIZE_Y, SIZE_X];

        //Input
        Thread inputThread;
        char c = 'w';
        char oldC = 'w';
        public Direction direction = Direction.Up;
        Direction prevDirection;

        //Player
        List<PlayerPos> player = new List<PlayerPos>();
        PlayerPos playerFrontPos;

        //Score
        public int score = 0;
        public int highScore = 0;

        public Game() { }

        #region Game functions

        public void Initialise()
        {
            StreamReader sr;

            if(!File.Exists(HIGHSCORE_FILE_NAME))
            {
                FileStream fs = File.Create(HIGHSCORE_FILE_NAME);
                fs.Close();
                sr = new StreamReader("HighScore.txt");

            }
            else
            {
               sr = new StreamReader("HighScore.txt");
            }

            string s = sr.ReadLine();

            try
            {
                highScore = int.Parse(s);
            }catch { highScore = 0;  }

            sr.Close();



            for (int y = 0; y < SIZE_Y; y++)
            {
                for (int x = 0; x < SIZE_X; x++)
                {
                    board[y, x] = new Tile(y == 0 || y == SIZE_Y - 1 || x == 0 || x == SIZE_X - 1);
                }
            }

            playerFrontPos = new PlayerPos(SIZE_Y / 2, SIZE_X / 2);
            player.Add(new PlayerPos(playerFrontPos));
            UpdateBoard();

            PlaceApple();
        }

        public void Begin()
        {
            Console.Title = "Controls: W, A, S, D";
            running = true;


            inputThread = new Thread(new ThreadStart(InputLoop));
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        public void Update()
        {
            AddPlayerMovement();
            UpdateBoard();
            PrintBoard();
        }

        public void GameOver()
        {
            //inputThread.Abort();

            StreamWriter sw = new StreamWriter(HIGHSCORE_FILE_NAME);

            sw.WriteLine(highScore.ToString());
            sw.Close();

            running = false;
        }
        #endregion

        #region Input functions
        public void InputLoop()
        {
            while (running)
            {
                c = Console.ReadKey(intercept: true).KeyChar;

                if (c != oldC)
                {
                    SetPlayerDirection(c);
                    //oldC = c;
                }
            }
        }

        public void SetPlayerDirection(char direction)
        {
            PlayerPos prevPos = playerFrontPos;
            prevDirection = this.direction;

            switch (direction)
            {
                case 'w':
                    this.direction = Direction.Up;
                    oldC = 'w';
                    break;

                case 'a':
                    this.direction = Direction.Left;
                    oldC = 'a';
                    break;

                case 's':
                    this.direction = Direction.Down;
                    oldC = 's';
                    break;

                case 'd':
                    this.direction = Direction.Right;
                    oldC = 'd';
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
                    if (score > highScore)
                    {
                        highScore = score;
                    }

                    Console.Title = $"Score: {score} | High Score: {highScore}";

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
        #endregion

        #region Board functions
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

            for (int y = 0; y < SIZE_Y; y++)
            {
                for (int x = 0; x < SIZE_X; x++)
                {
                    board[y, x].Print();
                }

                Console.Write("\n");
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void PlaceApple()
        {
            bool placingApple = true;

            do
            {
                int y = new Random().Next(SIZE_Y - 1);
                int x = new Random().Next(SIZE_X - 1);

                if (board[y, x].state == TileState.Free)
                {
                    board[y, x].state = TileState.Apple;
                    placingApple = false;
                }

            } while (placingApple);
        }
        #endregion
    }
}
