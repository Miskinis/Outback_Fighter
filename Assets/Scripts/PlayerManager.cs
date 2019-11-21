using System;
using System.Collections;
using ECS.Components.Combat;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    //public GameObject gameOverPanel;
    public float preDeathScreenDelay = 2f;
    public float postDeathScreenDelay = 3f;

    private Transform _playerTransform1, _playerTransform2;
    private Entity _playerEntity1, _playerEntity2;
    private bool _registered;
    private bool _player1Dead, _player2Dead;
    private bool _gameOver;

    public GameObject gameOverPrefab1;
    public GameObject gameOverPrefab2;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //gameOverPanel.SetActive(false);
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

        if(_gameOver == false)
        {
            StartCoroutine(GameOver());
        }
    }

    private IEnumerator GameOver()
    {
        _gameOver = true;

        yield return new WaitForSeconds(preDeathScreenDelay);

        var entityCommandBuffer = World.Active.GetExistingSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
        //string message = string.Empty;

        /*if (_player1Dead && _player2Dead)
        {
            //message = $"Game Over{Environment.NewLine}Tie";
        }
        else*/ if (_player1Dead)
        {
            entityCommandBuffer.AddComponent(_playerEntity2, new Victory());
            Instantiate(gameOverPrefab2, transform);
            //message = $"Game Over{Environment.NewLine}Player 2 Wins";
        }
        else if (_player2Dead)
        {
            entityCommandBuffer.AddComponent(_playerEntity1, new Victory());
            Instantiate(gameOverPrefab1, transform);
            //message = $"Game Over{Environment.NewLine}Player 1 Wins";
        }

        //gameOverPanel.SetActive(true);
        //gameOverPanel.GetComponentInChildren<TextMeshProUGUI>().text = message;

        var sceneLoader = SceneManager.LoadSceneAsync(0);
        sceneLoader.allowSceneActivation = false;
        yield return new WaitForSeconds(postDeathScreenDelay);
        
       Restart(sceneLoader);
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

    public void Restart(AsyncOperation sceneLoader)
    {
        var entityManager = World.Active.EntityManager;
        entityManager.DestroyEntity(entityManager.GetAllEntities());
        sceneLoader.allowSceneActivation = true;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}