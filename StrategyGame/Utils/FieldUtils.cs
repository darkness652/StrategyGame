using StrategyGame.Game_Controllers;
using StrategyGame.Troops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace StrategyGame.Utils
{
  public static  class FieldUtils
    {


        public static void  PaintCells(List<Game_Controllers.MapCell> cells, bool selected)
        {

            foreach (var item in cells)
            {
                item.ColorMapCell(selected);
            }
        }


        //public static Unit GetUnit(Player p, Rectangle r)
        //{
        //    Unit unit = null;
        //    foreach (var u in p.units)
        //    {
        //        if (u.UnitRectangle == r)
        //        {
        //            unit = u;
        //            break;
        //        }

        //    }
        //    return unit;

        //}



        //public static void selectingUnit(Unit u, Player p)
        //{

        //    if (u != null)
        //    {
        //        unselecting(p);
        //        if (p.SelectedUnit != null)
        //        {
        //            p.SelectedUnit.UnitRectangle.StrokeThickness = 0;
        //        }
        //        p.SelectedUnit = u;
        //        u.UnitRectangle.Stroke = new SolidColorBrush(Colors.Green);
        //        u.UnitRectangle.StrokeThickness = 1;
        //    }
        //}

        //public static void unselecting(Player p, List<MapCell> possibleCells)
        //{
        //    if (p.SelectedUnit != null)
        //    {
        //        // Можно перекрасить радиус хода отряда
        //        FieldUtils.PaintCells(possibleCells, false);
        //        p.SelectedUnit.UnitRectangle.StrokeThickness = 0;
        //        p.SelectedUnit = null;
        //    }
        //}

    }
}
