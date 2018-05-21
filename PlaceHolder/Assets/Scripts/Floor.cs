using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Floor : ObjectBase
    {
        protected override void Activated()
        {
            if (GameManager.instance.mouseMovemet)
                GetMouseController.MoveCursor();
        }
    }
}