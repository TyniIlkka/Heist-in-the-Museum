using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class PlayerSpawner : MonoBehaviour
    {
        public void SpawnPlayer(Vector3 positon, Quaternion rotation)
        {
            GameObject player = GameManager.instance.player;

            Instantiate(player, positon, rotation);
        }
    }
}