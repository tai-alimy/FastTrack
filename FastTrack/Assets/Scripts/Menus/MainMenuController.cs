using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Button raceButton = null;
    [SerializeField] Button garageButton = null;
    [SerializeField] Button shopButton = null;
    [SerializeField] Button quitButton = null;

    [SerializeField] List<CarController> carList = null;


    private void Awake()
    {
        foreach (CarController car in carList)
        {
            car.DisplayCar(false);
        }

        CarController activeCar = carList[(GameManager.GetCurrentActiveCarIndex())];
        activeCar.SetCarColorAndRims();
        activeCar.DisplayCar(true, true);
    }

    private void OnEnable()
    {
        raceButton.onClick.AddListener(HandleRaceButtonClicked);
       // garageButton.onClick.AddListener(HandleGarageButtonClicked);
      //  shopButton.onClick.AddListener(HandleShopButtonClicked);

        quitButton.onClick.AddListener(HandleQuitButtonClicked);

    }

    private void OnDisable()
    {
        raceButton.onClick.RemoveListener(HandleRaceButtonClicked);
      //  garageButton.onClick.RemoveListener(HandleGarageButtonClicked);
       // shopButton.onClick.RemoveListener(HandleShopButtonClicked);
        quitButton.onClick.RemoveListener(HandleQuitButtonClicked);

    }

    void HandleRaceButtonClicked()
    {
        Events.OkButtonClicked?.Invoke();
        NavigationManager.LoadScene(Scenes.SELECT_A_TRACK);

    }

    void HandleGarageButtonClicked()
    {
        Events.OkButtonClicked?.Invoke();
      //  NavigationManager.LoadScene(Scenes.GARAGE);
    }

    void HandleQuitButtonClicked()
    {
       // Debug.Log("loaded?");
        Events.OkButtonClicked?.Invoke();
        NavigationManager.LoadScene(Scenes.WELCOME);
    }

    void HandleShopButtonClicked()
    {
        Events.OkButtonClicked?.Invoke();
       // NavigationManager.LoadScene(Scenes.SHOP);
    }


}
