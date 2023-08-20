using System;
using PRN;
using Unity.Netcode;
using UnityEngine;

namespace NewAntfarm {
	public class NetworkCorgiPlayable : NetworkBehaviour {

		private NetworkPlayerProcessor processor;
		private PlayerInputProvider inputProvider;
		private PlayerStateChecker consistencyChecker;

		[SerializeField]
		private NetworkCorgiController controller;
        [SerializeField]
        private NetworkInputManager inputManager;
        [SerializeField] 
		private NetworkCharacter character;

        [SerializeField]
        float framesPerSecond = 60f;

		private Ticker ticker;
		private NetworkHandler<NetworkPlayerMovementInput, NetworkPlayerMovementState> networkHandler;
        private NetworkRole role;

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            SetRole();
            Initialize();
        }

        private void SetRole() {
            if (IsServer) {
                role = IsOwner ? NetworkRole.HOST : NetworkRole.SERVER;
            }
            else {
                role = IsOwner ? NetworkRole.OWNER : NetworkRole.GUEST;
            }
        }

        private void Initialize() {
            ticker = new Ticker(TimeSpan.FromSeconds(1 / framesPerSecond));
            consistencyChecker = new PlayerStateChecker();

            InitInputManager();

            processor = new NetworkPlayerProcessor(controller, inputManager, character);

            networkHandler = new NetworkHandler<NetworkPlayerMovementInput, NetworkPlayerMovementState>(
                role: role,
                ticker: ticker,
                processor: processor,
                inputProvider: inputProvider,
                consistencyChecker: consistencyChecker
            );
            networkHandler.onSendInputToServer += SendInputServerRpc;
            networkHandler.onSendStateToClient += SendStateClientRpc;
        }

        private void InitInputManager() {
            inputProvider = new PlayerInputProvider(inputManager);
            if (role == NetworkRole.SERVER || role == NetworkRole.OWNER || role == NetworkRole.HOST) {
                character.SetInputManager(inputManager);
            }
            else {
                character.SetInputManager(null);
            }
        }

        private void FixedUpdate() {
			ticker.OnTimePassed(TimeSpan.FromSeconds(Time.fixedDeltaTime));
		}

		[ServerRpc]
		private void SendInputServerRpc(NetworkPlayerMovementInput input) {
			networkHandler.OnOwnerInputReceived(input);
		}

		[ClientRpc]
		private void SendStateClientRpc(NetworkPlayerMovementState state) {
			networkHandler.OnServerStateReceived(state);
		}

		public override void OnDestroy() {
			base.OnDestroy();
            networkHandler.onSendInputToServer -= SendInputServerRpc;
            networkHandler.onSendStateToClient -= SendStateClientRpc;
            networkHandler.Dispose();
		}
	}
}
