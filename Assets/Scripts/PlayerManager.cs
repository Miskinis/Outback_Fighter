using ECS.Components.Combat;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private Transform _playerTransform1, _playerTransform2;
    private Entity _playerEntity1, _playerEntity2;
    private bool _registered;
    private bool _player1Dead, _player2Dead;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void RegisterPlayer(Transform playerTransform, Entity playerEntity)
    {
        if (_playerTransform1 == null)
        {
            _playerTransform1 = playerTransform;
            _playerEntity1 = playerEntity;
        }
        else if (_playerTransform2 == null)
        {
            _playerTransform2 = playerTransform;
            _playerEntity2 = playerEntity;
        }

        _registered = _playerTransform1 != null && _playerTransform2 != null;
    }

    public void PlayerKilled(Transform player)
    {
        _player1Dead = player == _playerTransform1;
        _player2Dead = player == _playerTransform2;

        var entityCommandBuffer = World.Active.GetExistingSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
        
        if (_player1Dead && _player2Dead)
        {
            
        }
        else if (_player1Dead)
        {
            entityCommandBuffer.AddComponent(_playerEntity2, new Victory());
            print("player2 Win");
        }
        else if (_player2Dead)
        {
            entityCommandBuffer.AddComponent(_playerEntity1, new Victory());
            print("player1 Win");
        }
    }

    private void Update()
    {
        if(_registered)
        {
            RotatePlayer(_playerTransform1, _playerTransform2);
            RotatePlayer(_playerTransform2, _playerTransform1);
        }
    }

    private static void RotatePlayer(Transform player1, Transform player2)
    {
        var angles = Quaternion.LookRotation(player2.position - player1.position, Vector3.up).eulerAngles;
        angles.x             = 0f;
        angles.z             = 0f;
        player1.eulerAngles = angles;
    }
}