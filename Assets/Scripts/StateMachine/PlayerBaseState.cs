namespace Ariston
{
    public abstract class PlayerBaseState
    {

        private bool _isRootState = false;
        private PlayerController _ctx;
        private PlayerStateFactory _factory;
        private PlayerBaseState _currentSubState;
        private PlayerBaseState _currentSuperState;

        protected bool IsRootState { set {_isRootState = value; } }
        protected PlayerController Ctx { get { return _ctx; } }
        protected PlayerStateFactory Factory { get { return _factory; } }
        public PlayerBaseState(PlayerController currentContext, PlayerStateFactory playerStateFactory)
        {
            _ctx = currentContext;
            _factory = playerStateFactory;
        }

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CheckSwitchState();
        public abstract void InitializeSubState();

        public void UpdateStates() 
        {
            UpdateState();
            _currentSubState?.UpdateStates(); 
            
        }
        protected void SwitchState(PlayerBaseState newState) 
        {
            //Exits the current state
            ExitState();
            
            //Enters the new state
            newState.EnterState();

            if(_isRootState)
            {
                // change current state of context
                _ctx.CurrentState = newState;
            }
            else
            {
                _currentSuperState?.SetSubState(newState);
            }

        }
        protected void SetSuperState(PlayerBaseState newSuperState) 
        {
            _currentSuperState = newSuperState;
        }
        protected void SetSubState(PlayerBaseState newSubState) 
        {
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);
        }

    }
}