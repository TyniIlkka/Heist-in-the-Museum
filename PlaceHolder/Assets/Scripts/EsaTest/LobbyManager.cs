using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class LobbyManager : MonoBehaviour
    {
        // TODO Guard patrol routese and states
        

        public void ResetLobby()
        {
            GameObject player = GameManager.instance.player;
            player.transform.position = GameManager.instance.lastPosition;

            // TODO guard status & position reset.
        }
    }
}