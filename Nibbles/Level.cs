using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nibbles
{
    public class Level
    {

        #region Fields

        private static Random rnd = new Random();
        private int _numPlayers = 1;
        private Snake[] _snakes = new Snake[2];
        private int[] _scores;
        private int _delayTime = 200;
        private const int TimeDiff = 25;
        private const int MinTime = 75;

        private bool _increaseSpeed = false;
        private Point _foodPoint;
        private bool _needFoodPoint = true;
        private Arena _arena;
        private int _sizeIncrease = 3;
        private int _maxScore = 20;
        private int _scorePerFoodPoint = 4;
        private bool _wantsToPlayMore = true;


        #endregion

        #region Properties

        public int NumPlayers
        {
            get { return _numPlayers; }
            set
            {
                if (value < 1)
                    _numPlayers = 1;
                if (value > 2)
                    _numPlayers = 2;
                _numPlayers = value;
            }
        }

        public int DelayTime { get { return _delayTime; } set { _delayTime = value; } }
        public bool IncreaseSpeed { get { return _increaseSpeed; } set { _increaseSpeed = value; } }
        public int MaxScore { get { return _maxScore; } set { _maxScore = value; } }
        public int ScorePerFoodPoint { get { return _scorePerFoodPoint; } set { _scorePerFoodPoint = value; } }


        #endregion

        #region Construction / Deconstruction

        public Level(string levelFile)
        {
            _arena = Arena.FromFile(levelFile);
            _snakes = new Snake[2];
            _scores = new int[] { 0, 0 };

            _snakes[0] = new Snake(
                GetRandomPoint(_arena, null),
                SnakeDirection.Left,
                _arena.BackgroundColor,
                ConsoleColor.Yellow);

            _snakes[1] = new Snake(
                GetRandomPoint(_arena, null),
                SnakeDirection.Left,
                _arena.BackgroundColor,
                ConsoleColor.Red);
        }

        #endregion

        #region Public Methods

        public bool Play()
        {
            bool exitLoop = false;

            _arena.DrawArena();

            SoundPlayer sp = new SoundPlayer("gamesound.wav");
            SoundPlayer spw = new SoundPlayer("level_complete.wav");
            
            //sp.Play();

            SoundPlayer spe = new SoundPlayer("select.wav");

            while (!exitLoop)
            {
                if (_needFoodPoint)
                {
                    _needFoodPoint = false;
                    _foodPoint = GenerateFoodPoint(_arena, _snakes);
                    Console.SetCursorPosition(_foodPoint.X, _foodPoint.Y);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("*");
                }

                for (int i = 0; i < _numPlayers; i++)
                {
                    if (_snakes[i].IsDead)
                    {
                        SoundPlayer spd = new SoundPlayer("death.wav");
                        spd.Play();
                        Dialog.Show("Player " + (i + 1) + " died");
                        exitLoop = true;
                        break;
                    }

                    var newHead = _snakes[i].UpdatePosition();

                    //Check if it is colliding with itself.
                    if (_snakes[i].BodyCollides(newHead))
                        _snakes[i].KillSnake();

                    //Check if it is colliding with the arena
                    if (_arena.HasBlockAt(newHead))
                        _snakes[i].KillSnake();

                    //Check if it is colliding with the other snake
                    if (_snakes[i == 0 ? 1 : 0].HeadCollides(newHead))
                        _snakes[i].KillSnake();

                    if (_snakes[i].HeadCollides(_foodPoint))
                    {
                        spe.Play();
                        _snakes[i].IncreaseSize(_sizeIncrease);
                        _foodPoint = GenerateFoodPoint(_arena, _snakes);
                        _needFoodPoint = true;

                        if (_increaseSpeed && _delayTime > MinTime)
                            _delayTime -= TimeDiff;

                        _scores[i] += _scorePerFoodPoint;

                        if (_scores[i] > _maxScore)
                        {
                            spw.Play();
                            Dialog.Show("Player " + (i + 1) + " wins!");
                            exitLoop = true;
                            break;
                        }
                    }

                    _snakes[i].DrawSnake();
                }

                Thread.Sleep(_delayTime);

                if (UpdateSnakeDirections())
                    exitLoop = true;
            }

            return _wantsToPlayMore;
        }

        #endregion

        #region Private Methods

        private bool UpdateSnakeDirections()
        {
            if (!Console.KeyAvailable)
                return false;

            var input = Console.ReadKey();

            //Do player 1 input...
            if (input.Key == ConsoleKey.UpArrow)
                _snakes[0].ChangeDirection(SnakeDirection.Up);
            if (input.Key == ConsoleKey.DownArrow)
                _snakes[0].ChangeDirection(SnakeDirection.Down);
            if (input.Key == ConsoleKey.LeftArrow)
                _snakes[0].ChangeDirection(SnakeDirection.Left);
            if (input.Key == ConsoleKey.RightArrow)
                _snakes[0].ChangeDirection(SnakeDirection.Right);

            //Do player 2 input...
            if (input.Key == ConsoleKey.W)
                _snakes[1].ChangeDirection(SnakeDirection.Up);
            if (input.Key == ConsoleKey.S)
                _snakes[1].ChangeDirection(SnakeDirection.Down);
            if (input.Key == ConsoleKey.A)
                _snakes[1].ChangeDirection(SnakeDirection.Left);
            if (input.Key == ConsoleKey.D)
                _snakes[1].ChangeDirection(SnakeDirection.Right);

            if (input.Key == ConsoleKey.Q)
            {
                _wantsToPlayMore = false;
                return true;
            }

            return false;
        }

        private static Point GenerateFoodPoint(Arena arena, Snake[] snakes)
        {
            Point p = new Point() { X = rnd.Next(Arena.WindowWidth), Y = rnd.Next(Arena.WindowHeight) };

            while (arena.HasBlockAt(p) ||
                   snakes.Any(sn => sn.BodyCollides(p)))
            {
                p = new Point() { X = rnd.Next(Arena.WindowWidth), Y = rnd.Next(Arena.WindowHeight) };
            }

            return p;
        }

        private static Point GetRandomPoint(Arena arena, Snake otherSnake)
        {
            Point p = new Point()
            {
                X = rnd.Next(Arena.WindowWidth),
                Y = rnd.Next(Arena.WindowHeight)
            };

            while (arena.HasBlockAt(p))
            {
                p = new Point()
                {
                    X = rnd.Next(Arena.WindowWidth),
                    Y = rnd.Next(Arena.WindowHeight)
                };
            }

            return p;
        }
        
        #endregion
    }
        
}
