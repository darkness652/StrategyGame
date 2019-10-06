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
using StrategyGame.Troops;
using StrategyGame.Game_Controllers;
namespace StrategyGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MapController mapGenerator = new MapController();
        GameController gameController = new GameController();
        public MainWindow()
        {
            InitializeComponent();
            gameController.MapCtrl = mapGenerator;
            GameController.Instance = gameController;




        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            double width = field.ActualWidth;
            double height = field.ActualHeight;
            mapGenerator.Generate(field, height, width, gap: 1, xCount:20, yCount:20);
            GameController.Instance.AddUnitsByDefault();
            //  Добавить отряды по умолчанию
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameController.Instance.ChangePlayer();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Clean_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_ChangePlayer_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BuyingItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
