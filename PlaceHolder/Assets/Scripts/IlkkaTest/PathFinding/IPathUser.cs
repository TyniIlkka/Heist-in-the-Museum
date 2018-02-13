using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief.PathFinding {
    public interface IPathUser 
    {
        Node CurrentWaypoint { get; }
        Vector3 Position { get; set; }
    }
}
