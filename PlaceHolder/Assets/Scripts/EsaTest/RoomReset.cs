using UnityEngine;
using ProjectThief.States;

namespace ProjectThief
{
    public class RoomReset : MonoBehaviour
    {
        public void ResetRoom()
        {
            GameStateBase currentState = GameStateController.CurrentState;
            Inventory inventory = GameManager.instance.levelController.Inventory;
            int pos = GameManager.instance.levelController.ListPos;

            if (GameManager.instance.levelController.JustCleared)
            {
                GameManager.instance.currentPhase--;
                GameManager.instance.clearedRooms[pos] = false;
            }

            if (currentState.SceneName == "Lobby")
            {
                switch (GameManager.instance.currentPhase)
                {
                    case 0:
                        GameManager.instance.refItems[1].Collected = false;
                        GameManager.instance.usedlevers[1] = false;

                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[1]))
                            inventory.RemoveItem(GameManager.instance.refItems[1]);
                        break;

                    case 1:
                        GameManager.instance.usedlevers[2] = false;
                        break;

                    case 2:
                        GameManager.instance.usedlevers[3] = false;
                        break;
                  
                    default:
                        Debug.Log("Nothing to reset in current phase");
                        break;
                } 
            }
            else if (currentState.SceneName == "Room1")
            {
                switch (GameManager.instance.currentPhase)
                {
                    case 1:
                        GameManager.instance.refItems[3].Collected = false;
                        GameManager.instance.refItems[6].Collected = false;
                        GameManager.instance.refItems[10].Collected = false;
                        GameManager.instance.openedVitrines[1] = false;
                        GameManager.instance.keyItems[1].Collected = false;

                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[3]))
                            inventory.RemoveItem(GameManager.instance.refItems[3]);
                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[6]))
                            inventory.RemoveItem(GameManager.instance.refItems[6]);
                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[10]))
                            inventory.RemoveItem(GameManager.instance.refItems[10]);
                        break;

                    case 3:
                        GameManager.instance.usedlevers[4] = false;
                        break;

                    default:
                        Debug.Log("Nothing to reset in current phase");
                        break;
                }
            }
            else if (currentState.SceneName == "Room2")
            {                
                switch (GameManager.instance.currentPhase)
                {
                    case 2:
                        GameManager.instance.refItems[4].Collected = false;
                        GameManager.instance.refItems[7].Collected = false;
                        GameManager.instance.refItems[11].Collected = false;
                        GameManager.instance.openedVitrines[2] = false;
                        GameManager.instance.keyItems[2].Collected = false;

                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[4]))
                            inventory.RemoveItem(GameManager.instance.refItems[4]);
                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[7]))
                            inventory.RemoveItem(GameManager.instance.refItems[7]);
                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[11]))
                            inventory.RemoveItem(GameManager.instance.refItems[11]);                        
                        break;

                    default:
                        Debug.Log("Nothing to reset in current phase");
                        break;
                }
            }
        }
    }
}