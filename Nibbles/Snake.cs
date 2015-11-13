using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibbles
{
    public class Snake
    {
        private const int MaxBodyLength = 100;

        private List<Point> _body;
        private List<Point> _removePoints;
        private List<Point> _addPoints;
        private int size = 5;
        private Point _head;
        private SnakeDirection _direction;
        private ConsoleColor _backgroundColor;
        private ConsoleColor _snakeColor;
        private const char SnakeBlock = '█';
        private bool _killingSnake = false;

        public bool IsDead { get; private set; }

        public Snake(Point startPosition, SnakeDirection startDirection, ConsoleColor backgroundColor, ConsoleColor snakeColor)
        {
            _body = new List<Point>();
            _removePoints = new List<Point>();
            _addPoints = new List<Point>();
            _head = startPosition;
            _direction = startDirection;
            _backgroundColor = backgroundColor;
            _addPoints.Add(_head);
            _snakeColor = snakeColor;
        }

        public Point UpdatePosition()
        {
            if (_killingSnake)
            {
                KillSnakeAnimation();
                return _head;
            }

            _body.Add(_head);

            switch (_direction)
            {
                case SnakeDirection.Up:
                    _head.Y -= 1;
                    if (_head.Y < 0)
                        _head.Y = Arena.WindowHeight - 1;
                    break;
                case SnakeDirection.Down:
                    _head.Y += 1;
                    if (_head.Y >= Arena.WindowHeight)
                        _head.Y = 0;
                    break;
                case SnakeDirection.Left:
                    _head.X -= 1;
                    if (_head.X < 0)
                        _head.X = Arena.WindowWidth - 1;
                    break;
                case SnakeDirection.Right:
                    _head.X += 1;
                    if (_head.X >= Arena.WindowWidth)
                        _head.X = 0;
                    break;
                default:
                    break;
            }

            _addPoints.Add(_head);

            while (_body.Count > size)
            {
                _removePoints.Add(_body[0]);
                _body.RemoveAt(0);
            }

            return _head;
        }

        public void ChangeDirection(SnakeDirection direction)
        {
            //Do not allow direct reversals
            if (direction == SnakeDirection.Up && _direction == SnakeDirection.Down)
                return;
            if (direction == SnakeDirection.Down && _direction == SnakeDirection.Up)
                return;
            if (direction == SnakeDirection.Left && _direction == SnakeDirection.Right)
                return;
            if (direction == SnakeDirection.Right && _direction == SnakeDirection.Left)
                return;

            _direction = direction;
        }

        public void IncreaseSize(int change)
        {
            size += change;
        }

        public bool BodyCollides(Point p)
        {
            if (_body.Any(b => b == p))
                return true;

            return false;
        }

        public bool HeadCollides(Point p)
        {
            if (_head == p)
                return true;
            return false;
        }

        public void KillSnake()
        {
            if (_killingSnake)
                return;

            _addPoints.Clear();
            _head = _body[_body.Count - 1];
            _body.RemoveAt(_body.Count - 1);
            _killingSnake = true;
        }

        private void KillSnakeAnimation()
        {
            if (_body.Count < 2)
            {
                _removePoints.AddRange(_body);
                _body.Clear();
                IsDead = true;
                return;
            }

            _removePoints.Add(_body[0]);
            _removePoints.Add(_body[_body.Count - 1]);
            _body.RemoveAt(_body.Count - 1);
            _body.RemoveAt(0);
        }

        public void DrawSnake()
        {
            Console.ForegroundColor = _snakeColor;
            foreach (var p in _addPoints)
            {
                Console.SetCursorPosition(p.X, p.Y);
                Console.Write(SnakeBlock);
            }

            Console.ForegroundColor = _backgroundColor;
            foreach (var p in _removePoints)
            {
                Console.SetCursorPosition(p.X, p.Y);
                Console.Write(SnakeBlock);
            }

            _addPoints.Clear();
            _removePoints.Clear();
        }
    }
}
