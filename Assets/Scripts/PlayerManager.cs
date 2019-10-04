using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    private Transform _player1, _player2;
    private bool _registered;
    private bool _player1Dead, _player2Dead;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void RegisterPlayer(Transform player)
    {
        if (_player1 == null)
        {
            _player1 = player;
        }
        else if (_player2 == null)
        {
            _player2 = player;
        }

        _registered = _player1 != null && _player2 != null;
    }

    public void PlayerKilled(Transform player)
    {
        _player1Dead = player == _player1;
        _player2Dead = player == _player2;
        
        if (_player1Dead && _player2Dead)
        {
            
        }
        else if (_player1Dead)
        {
            
        }
        else if (_player2Dead)
        {
            
        }
    }

    private void Update()
    {
        if(_registered)
        {
            RotatePlayer(_player1, _player2);
            RotatePlayer(_player2, _player1);
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