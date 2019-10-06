using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StrategyGame.Game_Controllers
{
   public class MapCell
    {
        // при подготовке боя, учесть, что игроки расставляют свои отряды в определенных зонах карты
        //  эти зоны будем передавать игроку из MapController  в виде списка клеток ,либо в виде  матрицы
         bool isTaken;
     
        public bool IsTaken
        {
            get { return isTaken; }
             set { isTaken = value; }
        }


        // отступ от канвы
        public Point RectangleCoords { get; private set; }

        //номера в матрице 
        public Point Coords {  get; private set; }
        public Rectangle MapRectangle { get; private set; }

        Color regularColor= Colors.Cornsilk;
        Color selectedColor = Colors.BlanchedAlmond;


        public void ColorMapCell(bool selected)
        {
            if(selected)
            MapRectangle.Fill = new SolidColorBrush(selectedColor);
            else
                MapRectangle.Fill = new SolidColorBrush(regularColor);

        }

        public MapCell(Point coords, Rectangle r)
        {
            Coords = coords;
            MapRectangle = r;
            RectangleCoords = new Point(Canvas.GetLeft(r), Canvas.GetTop(r));
            MapRectangle.Fill = new SolidColorBrush(regularColor);
        }

    }
}
