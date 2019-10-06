using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace StrategyGame.Game_Controllers
{
    class MapController
    {

        MapCell[,] mapCells;


        public MapCell[,] MapCells { get { return mapCells; } }




        public  Canvas GameField;


        public void Generate(Canvas canvas, double height, double width, int xCount = 10, int yCount = 10, double gap = 2)
        {
            mapCells = new MapCell[xCount, yCount];
            GameField = canvas;

            double sizeX = width / xCount;  //сторона квадрата по ширине
            double sizeY = height / yCount;//сторона квадрата по высоте
                                           //операция сравнения - условие в if
                                           //либо true, либо false
            double size = (sizeX <= sizeY) ? sizeX : sizeY;



            size -= gap;
            double offsetX = (width - xCount * (gap + size)) / 2;
            double offsetY = (height - yCount * (gap + size)) / 2;

            Point point = new Point(offsetX, offsetY);
            //double size = 20;
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = size;
                    rectangle.Height = size;
                    //rectangle.Fill = new SolidColorBrush(Colors.Cornsilk);
                    rectangle.MouseDown += GameController.Instance.Cell_Interaction_Mouse_Down;
                    canvas.Children.Add(rectangle);
                    Canvas.SetLeft(rectangle, point.X); //-установка позиции
                    Canvas.SetTop(rectangle, point.Y);
                    point.X += size + gap;

                    mapCells[i, j] = new MapCell(new Point(i, j), rectangle);
                }
                point.Y += size + gap;
                point.X = offsetX;//offset=0
            }
        }

        /// <summary>
        ///  Поиск ячейки  в матрице поля по прямоугольнику
        /// </summary>
        /// <param name="r"> Прямоугольник, на который нажали</param>
        /// <returns> Возвращает найденную ячейку поля. Если она не найдена, то null </returns>
        public MapCell GetMapCell(Rectangle r)
        {
            MapCell mc = null;

            for (int i = 0; i < mapCells.GetLength(0); i++)
            {
                for (int j = 0; j < mapCells.GetLength(1); j++)
                {

                    /// найти ячеку карты из матрицы, с которой связан прямоугольник r

                    if (r == mapCells[i,j].MapRectangle)
                    {
                        mc = mapCells[i, j];
                        break;
                    }
                }
            }

             return mc;

        }



       
    }
}
