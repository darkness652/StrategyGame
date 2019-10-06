using StrategyGame.Troops;
using StrategyGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StrategyGame.Game_Controllers
{
    class GameController
    {
        public static GameController Instance;
        // 1) информация об игроках
        // 2) их отряды
        // чья очередь


        int playersMadeMoves = 0;
        public int MaxMovesCount = 4;
        public   MapController MapCtrl;
        Player playerOne=new Player() { Color = Colors.CornflowerBlue };
        Player playerTwo=new Player() { Color = Colors.Yellow };

        Player playerWithTurn;

        List<MapCell> _possibleCells;

        public GameController()
        {
            playerWithTurn = playerOne;
            playerOne.OnPlayerOutOfMoves += ChangePlayer;
            playerTwo.OnPlayerOutOfMoves += ChangePlayer;

         

        }

        public void AddUnitsByDefault()
        {
           
            for (int i = 0; i < MapCtrl.MapCells.GetLength(1); i++)
            {
                addUnitToField(playerOne, new Archer(), MapCtrl.MapCells[0, i]);
                addUnitToField(playerTwo, new Archer(), MapCtrl.MapCells[MapCtrl.MapCells.GetLength(0) - 1, i]);

            }


        }

        // начало раунда нужно где то вызвать
        void roundStarted()
        {
            playerOne.MovesCount = 0;
            playerTwo.MovesCount = 0;
            //ChangePlayer();
        }

        public void ChangePlayer()
        {
            unselecting(playerWithTurn);
            if (playerWithTurn == playerOne)
                playerWithTurn = playerTwo;
            else
                playerWithTurn = playerOne;

            playersMadeMoves++;
            if (playersMadeMoves == 2)
            {
                roundStarted();
            }
        }

        //при нажатии на клетку можем отряд ставить, либо указать, что на нее отряд переместить
        public   void Cell_Interaction_Mouse_Down(object sender, MouseButtonEventArgs e)
        {
            if (!playerWithTurn.CanMove)
                return;
            if (e.LeftButton == MouseButtonState.Pressed)
            //этот метод будет вызываться 
            // задача добавить на квадрат отряд
            {
                Rectangle rect = sender as Rectangle;
                //не добавлять отряд, если ячейка занята
                MapCell mc = MapCtrl.GetMapCell(rect);
                if (mc != null)
                {
                    if (!mc.IsTaken)
                    {
                        //  return
                        moveUnit(mc);
                        //  вернемся сюда
                    }

                }
            }

           else if (e.RightButton == MouseButtonState.Pressed)
                unselecting(playerWithTurn);


            //dfdfdfsdfsdf
            ///sdf
        }


        //при нажатии на квадратик отряда можно его
        //1) выбрать
        //2) снять выделение
        //3) атаковать
        //4) наложить заклинание, как положительное для своих, так и отрицательное для врага
        void unit_rect_interaction_mousedown(object sender, MouseButtonEventArgs e)
        {
            MouseButtonState mouseButtonState = e.RightButton;
            MouseButtonState mouseButtonState1 = e.LeftButton;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Rectangle rectangle = sender as Rectangle;
                // u - Тот отряд, на который нажали
                Unit u = getUnit(playerWithTurn, rectangle); //??
                selectingUnit(u, playerWithTurn);
                if (u != null)
                {
                    _possibleCells = PathFinder.GetPossibleCells(u.Cell, MapCtrl.MapCells, u.MaxCellCount);
                    FieldUtils.PaintCells(_possibleCells, true);
                }
            }
            else if (e.RightButton == MouseButtonState.Pressed)
                unselecting(playerWithTurn);
        }


        void addUnitToField(Player p,  Unit u, MapCell mc)
        {
            mc.IsTaken = true;
            u.Cell = mc;
            double left = Canvas.GetLeft(mc.MapRectangle);
            double top = Canvas.GetTop(mc.MapRectangle);

            p.AddUnit(u);
            
            UnitPosition unitPosition = Unit.GetUnitPosition(u, mc.MapRectangle.Width, mc.MapRectangle.Height);

            Rectangle r = new Rectangle();
            r.Width = unitPosition.Width;
            r.Height = unitPosition.Height;
            r.Fill = new SolidColorBrush(p.Color);
            r.MouseDown += unit_rect_interaction_mousedown;
            MapCtrl.GameField.Children.Add(r);

            u.UnitRectangle = r;
            Canvas.SetLeft(r, left + unitPosition.OffsetX);
            Canvas.SetTop(r, top + unitPosition.OffsetY);
          //  selectingUnit(u, p);
        }



        Unit getUnit(Player p, Rectangle r)
        {
            Unit unit = null;
            foreach (var u in p.units)
            {
                if (u.UnitRectangle == r)
                {
                    unit = u;
                    break;
                }

            }
            return unit;

        }



        void selectingUnit(Unit u, Player p)
        {

            if (u != null)
            {
                unselecting(p);
                if (p.SelectedUnit != null)
                {
                    p.SelectedUnit.UnitRectangle.StrokeThickness = 0;
                }
                p.SelectedUnit = u;
                u.UnitRectangle.Stroke = new SolidColorBrush(Colors.Green);
                u.UnitRectangle.StrokeThickness = 1;
            }
        }

        void unselecting(Player p)
        {
            if (p.SelectedUnit != null)
            {
                // Можно перекрасить радиус хода отряда
                FieldUtils.PaintCells(_possibleCells, false);
                p.SelectedUnit.UnitRectangle.StrokeThickness = 0;
                p.SelectedUnit = null;
            }
        }




        void unitStopMove(Unit u)
        {

        }




        void moveUnit(MapCell mc)
        {

            if (playerWithTurn.SelectedUnit != null)
            //   addUnitToField(playerWithTurn, new Archer(), mc);
            //else
            {

                // не можем перемещать отряд или атаковать им, если этим отрядом уже сделан ход в раунде
                // начать перемещение
                //if(playerWithTurn.SelectedUnit.MadeMove)

                List<MapCell> route = PathFinder.FindPath(MapCtrl.MapCells, playerWithTurn.SelectedUnit.Cell, mc);

                if (route == null)
                    return;


                FieldUtils.PaintCells(_possibleCells, false);

                mc.IsTaken = true;
                playerWithTurn.SelectedUnit.Cell.IsTaken = false;
                playerWithTurn.SelectedUnit.Cell = mc;


                playerWithTurn.SelectedUnit.StartMove(route);

            }
        }
    }
}
