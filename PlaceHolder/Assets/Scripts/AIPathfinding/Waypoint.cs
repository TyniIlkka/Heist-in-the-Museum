using UnityEngine;
using System.Collections;

namespace ProjectThief.WaypointSystem
{
	public class Waypoint : MonoBehaviour
	{
		public Vector3 Position
		{
			get { return transform.position; }
		}
	}
}
