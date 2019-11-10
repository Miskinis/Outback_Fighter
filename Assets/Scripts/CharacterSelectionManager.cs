using System;
using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager main;
    public CinemachineTargetGroup cinemachineTargetGroup;
    public float characterRadius = 0.5f;
    public float delayTillCountdown = 1f;
    public float countInterval = 1f;
    public GameObject countdownPanel;
    public Image countdownImage;
    public Sprite[] countDownSprites;
    public GameObject menuPanel;
    public GameObject hudPanel;
    private Tuple<GameObject, InstantiationParameters>[] _selectedCharacters = new Tuple<GameObject, InstantiationParameters>[2];
    private readonly CinemachineTargetGroup.Target[] _instantiatedCharacters = new CinemachineTargetGroup.Target[2];

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            hudPanel.SetActive(false);
        }
    }

    public void ConfirmSelection(GameObject character, InstantiationParameters instantiationParameters, int playerNumber)
    {
        _selectedCharacters[playerNumber] = Tuple.Create(character, instantiationParameters);
        
        if (_selectedCharacters[0] != null && _selectedCharacters[1] != null)
        {
            StartCoroutine(StartDuel());
        }
    }

    private IEnumerator StartDuel()
    {
        yield return new WaitForSeconds(delayTillCountdown);

        menuPanel.SetActive(false);
        countdownPanel.SetActive(true);
        
        var delayTime = new WaitForSeconds(countInterval);

        for (int i = 0; i < countDownSprites.Length; i++)
        {
            countdownImage.sprite = countDownSprites[i];
            yield return delayTime;
        }
        
        for (var i = 0; i < _selectedCharacters.Length; i++)
        {
            var (character, instantiationParameters) = _selectedCharacters[i];

            _instantiatedCharacters[i] = new CinemachineTargetGroup.Target
                {
                    target = Instantiate(character, instantiationParameters.Position, instantiationParameters.Rotation).transform,
                    weight = 0.5f,
                    radius = characterRadius
                };

            cinemachineTargetGroup.m_Targets = _instantiatedCharacters;
        }

        countdownPanel.SetActive(false);
        hudPanel.SetActive(true);
        _selectedCharacters = new Tuple<GameObject, InstantiationParameters>[2];
        yield return null;
    }
}