using PRN;
using System.Collections.Generic;
using System;

namespace NewAntfarm {

    public interface IProcessState {
        void ProcessState();
    }

    public class NetworkPlayerProcessor : IProcessor<NetworkPlayerMovementInput, NetworkPlayerMovementState> {

        private  NetworkInputManager inputManager;
        private readonly NetworkCorgiController controller;
        private readonly NetworkCharacter character;
        private List<IProcessState> processStates = new List<IProcessState>();

        public NetworkPlayerProcessor(NetworkCorgiController controller,
            NetworkInputManager inputManager,
            NetworkCharacter character) {
            this.inputManager = inputManager;
            this.controller = controller;
            this.character = character;

            processStates.Add(controller);
            processStates.Add(character);
        }

        public NetworkPlayerMovementState Process(NetworkPlayerMovementInput input, TimeSpan deltaTime) {

            //apply new delta time
            character.SetNetworkDeltaTime((float)deltaTime.TotalSeconds);
            controller.SetNetworkDeltaTime((float)deltaTime.TotalSeconds); 

            //apply new input state
            inputManager.SetMovement(input.primaryMovement);
            inputManager.SetJumpButtonState(input.jumpButtonState);

            //process to generate new state
            foreach (var state in processStates) {
                state.ProcessState();
            }

            //get new state from character model
            return new NetworkPlayerMovementState(character);
        }

        public void Rewind(NetworkPlayerMovementState state) {
            character.SetFacingRight(state.isFacingRight);
            controller.SetTransformPosition(state.position);
        }                
    }
}