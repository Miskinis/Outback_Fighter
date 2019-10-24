using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager main;
    public GameObject menuPanel;
    public GameObject hudPanel;
    private Dictionary<GameObject, InstantiationParameters> _selectedCharacters = new Dictionary<GameObject, InstantiationParameters>();

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            hudPanel.SetActive(false);
        }
    }

    public void ConfirmSelection(GameObject character, InstantiationParameters instantiationParameters)
    {
        _selectedCharacters.Add(character, instantiationParameters);
        
        if (_selectedCharacters.Count > 1)
        {
            foreach (var selectedCharacter in _selectedCharacters)
            {
                Instantiate(selectedCharacter.Key, selectedCharacter.Value.Position, selectedCharacter.Value.Rotation);
                menuPanel.SetActive(false);
                hudPanel.SetActive(true);
            }
        }
    }
}