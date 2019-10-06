using StrategyGame.Troops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StrategyGame.Game_Controllers
{
     class Player
    {

      public event Action OnPlayerOutOfMoves;
      public Color Color;
      public  List<Unit> units = new List<Unit>();
      public Unit SelectedUnit;
      public int MovesCount;
        public bool CanMove { get; set; } = true;

        public void AddUnit(Unit u)
        {
            units.Add(u);
            u.OnActionMade += onUnitMadeAction;
        }
        void onUnitMadeAction(bool startMove)
        {
            CanMove = !startMove;

            if (!startMove)
            {
                MovesCount++;
                if (MovesCount >= GameController.Instance.MaxMovesCount)
                {//обратиться к GameController 
                 //либо сменит игрока, либо проведет смену раунда

                    OnPlayerOutOfMoves?.Invoke();
                }
            }
            
            
        }

    
    }
}
