using UnityEngine;
using PRN;

namespace PRN2Corgi {
	public class PlayerStateChecker : IStateConsistencyChecker<NetworkPlayerMovementState> {

		public bool IsConsistent(NetworkPlayerMovementState serverState, NetworkPlayerMovementState ownerState) =>
			Vector3.Distance(serverState.position, ownerState.position) <= .01f;
	}
}
