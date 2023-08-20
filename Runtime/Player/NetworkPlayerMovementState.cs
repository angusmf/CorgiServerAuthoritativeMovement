using PRN;
using Unity.Netcode;
using UnityEngine;

namespace PRN2Corgi {
	public struct NetworkPlayerMovementState : IState, INetworkSerializable {

		public int tick;
		public Vector3 position;
		public bool isFacingRight;

        // You need to implement those 2 methods
        public void SetTick(int tick) => this.tick = tick;
		public int GetTick() => tick;

		public NetworkPlayerMovementState(NetworkCharacter character) {
			position = character.transform.position;
			isFacingRight = character.IsFacingRight;
			tick = 0;
		}

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
			serializer.SerializeValue(ref tick);
			serializer.SerializeValue(ref position);
			serializer.SerializeValue(ref isFacingRight);

        }
	}
}