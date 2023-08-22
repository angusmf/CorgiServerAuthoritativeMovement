using MoreMountains.Tools;
using PRN;
using System;
using System.Collections.Generic;

namespace PRN2Corgi {
    public class PlayerInputProvider : IInputProvider<NetworkPlayerMovementInput> {
        
        private NetworkPlayerMovementInput input;
        private NetworkInputManager inputManager;

        private const string JumpButtonID = "Jump";

        private Dictionary<string, MMInput.ButtonStates> currentStates = new Dictionary<string, MMInput.ButtonStates>() {
                { JumpButtonID, MMInput.ButtonStates.Off },
            };

        public PlayerInputProvider(NetworkInputManager inputManager) {
            this.inputManager = inputManager;            
        }

        public virtual NetworkPlayerMovementInput GetInput(TimeSpan deltaTime) {
            input.primaryMovement = inputManager.PrimaryMovement;
                currentStates[JumpButtonID] = GetButtonState(inputManager.JumpButton, (float)deltaTime.TotalSeconds, JumpButtonID);
            input.jumpButtonState = currentStates[JumpButtonID];
            return input;
        }

        protected virtual MMInput.ButtonStates GetButtonState(MMInput.IMButton button, float deltaTime, string buttonId) {                
                return (currentStates[buttonId] != MMInput.ButtonStates.ButtonDown && button.ButtonDownRecently(deltaTime)) ? MMInput.ButtonStates.ButtonDown :
                button.State.CurrentState;
            }
        }
    }