using System;
using System.Collections;
using ECS.Components.Combat;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject gameOverPanel;

    private Transform _playerTransform1, _playerTransform2;
    private Entity _playerEntity1, _playerEntity2;
    private bool _registered;
    private bool _player1Dead, _player2Dead;
    private bool _gameOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameOverPanel.SetActive(false);
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
        if (player == _playerTransform1)
            _player1Dead = true;
        if (player == _playerTransform2)
            _player2Dead = true;

        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        _gameOver = true;

        yield return new WaitForSeconds(2f);

        var entityCommandBuffer = World.Active.GetExistingSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
        string message = string.Empty;

        if (_player1Dead && _player2Dead)
        {
            message = $"Game Over{Environment.NewLine}Tie";
        }
        else if (_player1Dead)
        {
            entityCommandBuffer.AddComponent(_playerEntity2, new Victory());
            message = $"Game Over{Environment.NewLine}Player 2 Wins";
        }
        else if (_player2Dead)
        {
            entityCommandBuffer.AddComponent(_playerEntity1, new Victory());
            message = $"Game Over{Environment.NewLine}Player 1 Wins";
        }

        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponentInChildren<TextMeshProUGUI>().text = message;

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
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