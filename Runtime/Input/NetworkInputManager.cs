using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System;
using UnityEngine;

namespace PRN2Corgi {

    /*NetworkInputManager needed to provide overrides to the various set movement methods because
     the base methods are inteded for and require a mobile input flag to be set  */
    public class NetworkInputManager : InputManager {

        public override void SetMovement() {
            base.SetMovement();
        }

        public override void SetMovement(Vector2 movement) {
            base.SetMovement(movement);
            _primaryMovement.x = movement.x;
            _primaryMovement.y = movement.y; 
            
        }

        public override void SetHorizontalMovement(float horizontalInput) {
            _primaryMovement.x = horizontalInput;
        }

        public override void SetVerticalMovement(float verticalInput) {
            _primaryMovement.y = verticalInput;
        }

        public virtual void SetJumpButtonState(MMInput.ButtonStates jumpButtonState) { 
            switch(jumpButtonState) {
                case MMInput.ButtonStates.ButtonUp:
                    JumpButton.State.ChangeState(MMInput.ButtonStates.ButtonUp);
                    break;
                case MMInput.ButtonStates.ButtonDown:
                    JumpButton.State.ChangeState(MMInput.ButtonStates.ButtonDown);
                    break;
                case MMInput.ButtonStates.ButtonPressed:
                    JumpButton.State.ChangeState(MMInput.ButtonStates.ButtonPressed);
                    break;
                case MMInput.ButtonStates.Off:
                    break;
                default:
                    throw new NotImplementedException
                ($"Unrecognized JumpButton MMInput.ButtonStates value: {jumpButtonState}");
            }
        }
    }
}
