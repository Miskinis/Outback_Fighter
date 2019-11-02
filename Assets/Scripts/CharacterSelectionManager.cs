using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager main;
    public float duelStartDelay = 2f;
    public GameObject menuPanel;
    public GameObject hudPanel;
    private Tuple<GameObject, InstantiationParameters>[] _selectedCharacters = new Tuple<GameObject, InstantiationParameters>[2];

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
        yield return new WaitForSeconds(duelStartDelay);
        foreach (var (character, instantiationParameters) in _selectedCharacters)
        {
            Instantiate(character, instantiationParameters.Position, instantiationParameters.Rotation);
        }
        menuPanel.SetActive(false);
        hudPanel.SetActive(true);
        _selectedCharacters = new Tuple<GameObject, InstantiationParameters>[2];
        yield return null;
    }
}