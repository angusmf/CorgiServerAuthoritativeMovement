using Unity.Netcode;
using UnityEngine;
using PRN;
using MoreMountains.Tools;

public struct InterfaceTest : IInterfaceTest {
    public int GetTick() {
        throw new System.NotImplementedException();
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        throw new System.NotImplementedException();
    }

    public void SetTick(int tick) {
        throw new System.NotImplementedException();
    }
}

public interface IInterfaceTest : IInput, INetworkSerializable { }

public struct NetworkPlayerMovementInput : IInput, INetworkSerializable
{
    public int tick;
    public Vector2 primaryMovement;
    public MMInput.ButtonStates jumpButtonState;


    // You need to implement those 2 methods
    public void SetTick(int tick) => this.tick = tick;
    public int GetTick() => tick;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref tick); 
        serializer.SerializeValue(ref primaryMovement);
        serializer.SerializeValue(ref jumpButtonState);
    }

}
