using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;

enum MyButtons
{
    Forward = 0,
    Backward = 1,
    Left = 2,
    Right = 3,
    Jump = 4,
}

public struct PlayerInput : INetworkInput
{
    public NetworkButtons Buttons;
    public Vector3 aimDirection;
}