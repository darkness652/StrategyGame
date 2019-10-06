using StrategyGame.Game_Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace StrategyGame.Troops
{
    public enum UnitSize { small=40, medium=60, large=80}
    class UnitPosition
    {
        public double OffsetX;
        public double OffsetY;
        public double Width;
        public double Height;
    }

     abstract  class Unit
    {
        public event Action<bool> OnActionMade;

        public int Count { get; set; }
        public int CurrentHP {get;set;}
        public int AttackPoints    {get;set;}
        public int DefencePonts   {get;set;}
        public int MaxCellCount { get; set; } = 3;
        public int AttackRadius { get; set;}
        public MapCell Cell { get; set; }
        public bool MadeMove { get; set; }

        public virtual UnitSize UnitSize { get { return UnitSize.small; } }

        public Rectangle UnitRectangle;

        int _currentRouteIndex = 0;
        List<MapCell> _route;
        Point _targetPoint;

        Point _interval;


        System.Windows.Threading.DispatcherTimer dt;


        public virtual void TakeDamage(int damage)
        {
            CurrentHP -= damage;

        }


        // передаем маршрут
        public virtual void StartMove(List<MapCell> route)
        {
            //  не складывает методы --  null
            //if(OnMadeAction!=null)
            //OnMadeAction();
            OnActionMade?.Invoke(true);

           
            if (dt!=null)
            dt.Stop();
            _route = route;
            Utils.FieldUtils.PaintCells(_route, true);
            if (route.Count > 0)
            {

                _currentRouteIndex = 0;
                _targetPoint = GetUnitRectanglePosition(this, 
                    _route[0].MapRectangle.Width,
                    _route[0].MapRectangle.Height, 
                    _route[0].RectangleCoords.X, 
                    _route[0].RectangleCoords.Y);

                double left = Canvas.GetLeft(UnitRectangle);
                double top = Canvas.GetTop(UnitRectangle);

                _interval.X = (_targetPoint.X - left)/40;
                _interval.Y = (_targetPoint.Y - top)/40;


                dt = new System.Windows.Threading.DispatcherTimer();
                dt.Interval = new TimeSpan(0, 0, 0, 0, 10);
                dt.Tick += Move;
                dt.Start();
            }
            //ThreadStart threadStart = new ThreadStart(Move);
            //Thread t = new Thread(threadStart);
            //t.Start();
        }
            
        public virtual void Move(object sender, EventArgs e)
        {
            // в класс Unit передать маршрут, то есть List<MapCell>
            //  можно сделать движение между 2 соседними клетками в маршруте

            //   пока текущие координаты не станут равны конечным, то цикл будет продолжаться
            double left = Canvas.GetLeft(UnitRectangle);
            double top = Canvas.GetTop(UnitRectangle);

            //double dx = _targetPoint.X - left;
            //double dy = _targetPoint.Y - top;

            left += _interval.X;
            top += _interval.Y;

            //if (left < _targetPoint.X)
            //    left += 2;
            //else if (left > _targetPoint.X)
            //    left -= 2;
            //if (top < _targetPoint.Y)
            //    top += 2;
            //else if (top > _targetPoint.Y)
            //    top -= 2;
            Canvas.SetLeft(UnitRectangle, left);
            Canvas.SetTop(UnitRectangle, top);

            if (Math.Abs(left - _targetPoint.X)<5 && Math.Abs(top - _targetPoint.Y) <5)
            {
                //StopMove();

                if (_currentRouteIndex+1 >= _route.Count)
                    StopMove();
                else {
                    _currentRouteIndex++;
                    _targetPoint =   GetUnitRectanglePosition(this,
                    _route[_currentRouteIndex].MapRectangle.Width,
                    _route[_currentRouteIndex].MapRectangle.Height,
                    _route[_currentRouteIndex].RectangleCoords.X,
                    _route[_currentRouteIndex].RectangleCoords.Y);
                    _interval.X = (_targetPoint.X-left)/40;
                    _interval.Y = (_targetPoint.Y - top) / 40;

                }
            }
            //   while (left!=_targetPoint.X  && top!=_targetPoint.Y)
            // {
            //  Thread.Sleep(1000);

            //setCoords(3, 3);
            // }
        }


        public void StopMove()
        {
        

            Utils.FieldUtils.PaintCells(_route, false);
            dt.Stop();
            OnActionMade?.Invoke(false);

        }
    
        public virtual void Attack()
        { }


        // left top width height
        public static UnitPosition GetUnitPosition(Unit u, double cellSizeX, double cellSizeY)
        {
            UnitPosition position = new UnitPosition();

            UnitSize unitSize = u.UnitSize;

            double w = (int)unitSize * cellSizeX / 100;
            double h = (int)unitSize * cellSizeY / 100;


            double offsetX = 0;
            double offsetY= 0;

            offsetX = (cellSizeX - w) / 2;
            offsetY = (cellSizeY - h) / 2;


            position.Height = h;
            position.Width = w;
            position.OffsetX = offsetX;
            position.OffsetY = offsetY;
            return position;
        }

        public static Point GetUnitRectanglePosition(Unit u, double cellSizeX, double cellSizeY, double left, double top)
        {

            UnitPosition unitPosition = GetUnitPosition(u, cellSizeX, cellSizeY);

            Point p = new Point(unitPosition.OffsetX + left, unitPosition.OffsetY + top);

            return p;
            
        }

    }
}
