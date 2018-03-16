namespace ProjectThief.States
{
    public class VaultState : GameStateBase
    {
        public override string SceneName
        {
            get
            {
                return "RoomVault";
            }
        }

        public override GameStateType StateType
        {
            get { return GameStateType.Vault; }
        }

        public VaultState()
        {
            AddTargetState(GameStateType.MainMenu);            
            AddTargetState(GameStateType.Lobby);
            AddTargetState(GameStateType.Vault);
        }        
    }
}