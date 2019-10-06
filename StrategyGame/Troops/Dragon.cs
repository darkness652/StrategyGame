using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Troops
{
    class Dragon:Unit
    {
        public override UnitSize UnitSize { get { return UnitSize.large; } }
        public override void Attack()
        {
            base.Attack();
        }

        //public override void Move()
        //{
        //    base.Move();
        //}
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);


        }
    }
}
