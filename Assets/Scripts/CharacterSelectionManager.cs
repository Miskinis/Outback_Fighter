using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager main;
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
            foreach (var selectedCharacter in _selectedCharacters)
            {
                Instantiate(selectedCharacter.Item1, selectedCharacter.Item2.Position, selectedCharacter.Item2.Rotation);
            }
            menuPanel.SetActive(false);
            hudPanel.SetActive(true);
            _selectedCharacters = new Tuple<GameObject, InstantiationParameters>[2];
        }
    }
}