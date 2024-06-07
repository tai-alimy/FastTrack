using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseACarController : MonoBehaviour
{
    [SerializeField] CarCarousel carCarousel = null;
    [SerializeField] Button selectCarButton = null;
    [SerializeField] TextMeshProUGUI chooseACarText = null;

    private void Awake()
    {
        chooseACarText.SetText("Select a car, " + NavigationManager.SceneData["username"] + "!");
    }

    private void OnEnable()
    {
        selectCarButton.onClick.AddListener(HandleSelectCarButtonClicked);
    }

    private void OnDisable()
    {
        selectCarButton.onClick.RemoveListener(HandleSelectCarButtonClicked);
    }

    public void HandleSelectCarButtonClicked()
    {
        Events.CarChosen?.Invoke(carCarousel.GetCurrentItemIndex());
        NavigationManager.LoadScene(Scenes.MAIN_MENU);
    }
}
