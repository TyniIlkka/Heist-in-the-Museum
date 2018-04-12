using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Floor : ObjectBase
    {
        protected override void Activated()
        {
            GetMouseController.MoveCursor();
        }
    }
}