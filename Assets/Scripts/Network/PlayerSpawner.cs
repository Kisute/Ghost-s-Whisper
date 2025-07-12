using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
    }
}
