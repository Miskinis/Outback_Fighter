using System;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public Transform player1SpawnLocation;
    public Transform player2SpawnLocation;
    public GameObject player1ReadyPanel;
    public GameObject player2ReadyPanel;
    public Transform characterIconsObject;
    public Transform player1FlagsObject;
    public Transform player2FlagsObject;

    public Sprite player1Flag;
    public Sprite player2Flag;

    private async void Awake()
    {
        player1ReadyPanel.SetActive(false);
        player2ReadyPanel.SetActive(false);
        
        var handle = await new UniTask<Tuple<IReadOnlyList<GameObject>, IReadOnlyList<GameObject>,IReadOnlyList<Sprite>, IReadOnlyList<Sprite>>>(GameAssetManager.main.LoadAllAsync);
        var (characters1, characters2, icons1, icons2) = handle;

        var images = characterIconsObject.GetComponentsInChildren<Image>();
        for (var i = 0; i < icons1.Count; i++)
        {
            images[i].sprite = icons1[i];
        }
        
        SetButtons(player1FlagsObject, characters1, icons1, player1SpawnLocation, player1ReadyPanel, player1Flag, 0);
        SetButtons(player2FlagsObject, characters2, icons2, player2SpawnLocation, player2ReadyPanel, player2Flag, 1);
    }

    private static void SetButtons(Component buttonParent, IReadOnlyList<GameObject> characters, IReadOnlyList<Sprite> characterIcons, Transform spawnLocation, GameObject readyPanel, Sprite playerFlag, int playerIndex)
    {
        var flagButtons = buttonParent.GetComponentsInChildren<FlagButton>();

        int index = 0;
        for (; index < characters.Count; index++)
        {
            int characterIndex = index;
            var character = characters[characterIndex];
            var flagButton = flagButtons[characterIndex];
            flagButton.image.sprite = playerFlag;

            void OnFlagClick()
            {
                CharacterSelectionManager.main.ConfirmSelection(character, new InstantiationParameters(spawnLocation.position, spawnLocation.rotation, null), playerIndex);
                readyPanel.GetComponent<Image>().sprite = characterIcons[characterIndex];
                readyPanel.SetActive(true);
                for (int i = 0; i < flagButtons.Length; i++)
                {
                    flagButtons[i].interactable = false;
                }
            }

            flagButton.onClick.AddListener(OnFlagClick);
        }
        
        for (; index < flagButtons.Length; index++)
        {
            flagButtons[index].interactable = false;
        }
    }
}