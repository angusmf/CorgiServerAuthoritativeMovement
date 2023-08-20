using PRN;

namespace PRN2Corgi {
    public class PlayerInputProvider : IInputProvider<NetworkPlayerMovementInput> {
        
        private NetworkPlayerMovementInput input;
        private NetworkInputManager inputManager;

        public PlayerInputProvider(NetworkInputManager inputManager) {
            this.inputManager = inputManager;
        }

        public NetworkPlayerMovementInput GetInput() {
            input.primaryMovement = inputManager.PrimaryMovement;
            input.jumpButtonState = inputManager.JumpButton.State.CurrentState;
            return input;
        }
    }
}
