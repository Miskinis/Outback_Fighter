using UnityEngine;
using UnityEngine.UI;

public class FlagButton : Button
{
    protected override void Awake()
    {
        base.Awake();
        
        onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        var localColors = colors;
        localColors.disabledColor = Color.white;
        colors = localColors;
    }
}