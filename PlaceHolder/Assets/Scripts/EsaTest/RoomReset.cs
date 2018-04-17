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
                        GameManager.instance.refItems[0].Collected = false;
                        GameManager.instance.usedlevers[0] = false;

                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[0]))
                            inventory.RemoveItem(GameManager.instance.refItems[0]);
                        break;

                    case 1:
                        GameManager.instance.usedlevers[1] = false;
                        break;

                    case 2:
                        GameManager.instance.usedlevers[2] = false;
                        break;
                  
                    default:
                        Debug.LogError("Nothing to reset in current phase");
                        break;
                } 
            }
            else if (currentState.SceneName == "Room1")
            {
                switch (GameManager.instance.currentPhase)
                {
                    case 1:
                        GameManager.instance.refItems[2].Collected = false;
                        GameManager.instance.refItems[5].Collected = false;
                        GameManager.instance.refItems[9].Collected = false;
                        GameManager.instance.openedVitrines[1] = false;
                        GameManager.instance.keyItems[1].Collected = false;

                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[2]))
                            inventory.RemoveItem(GameManager.instance.refItems[2]);
                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[5]))
                            inventory.RemoveItem(GameManager.instance.refItems[5]);
                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[9]))
                            inventory.RemoveItem(GameManager.instance.refItems[9]);
                        break;

                    case 3:
                        GameManager.instance.usedlevers[3] = false;
                        break;

                    default:
                        Debug.LogError("Nothing to reset in current phase");
                        break;
                }
            }
            else if (currentState.SceneName == "Room2")
            {                
                switch (GameManager.instance.currentPhase)
                {
                    case 2:
                        GameManager.instance.refItems[3].Collected = false;
                        GameManager.instance.refItems[6].Collected = false;
                        GameManager.instance.refItems[10].Collected = false;
                        GameManager.instance.openedVitrines[2] = false;
                        GameManager.instance.keyItems[2].Collected = false;

                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[3]))
                            inventory.RemoveItem(GameManager.instance.refItems[3]);
                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[6]))
                            inventory.RemoveItem(GameManager.instance.refItems[6]);
                        if (inventory.InventoryItems.Contains(GameManager.instance.refItems[10]))
                            inventory.RemoveItem(GameManager.instance.refItems[10]);                        
                        break;

                    default:
                        Debug.LogError("Nothing to reset in current phase");
                        break;
                }
            }
        }
    }
}