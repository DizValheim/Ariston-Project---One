namespace Ariston
{
    public class PlayerStateFactory
    {
        PlayerController _context;

        public PlayerStateFactory(PlayerController currentContext)
        {
            _context = currentContext;
        }

        public PlayerBaseState Idle() 
        {
            return new PlayerIdleState(_context, this);
        }
        public PlayerBaseState Walk() 
        {
            return new PlayerWalkState(_context, this);
        }
        public PlayerBaseState Run() 
        {
            return new PlayerRunState(_context, this);
        }
        public PlayerBaseState Jump() 
        {
            return new PlayerJumpState(_context, this);
        }
        public PlayerBaseState Grounded() 
        {
            return new PlayerGroundedState(_context, this);
        }
        public PlayerBaseState Fall()
        {
            return new PlayerFallState(_context, this);
        }
        
    }

}