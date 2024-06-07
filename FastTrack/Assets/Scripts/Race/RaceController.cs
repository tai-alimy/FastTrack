using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] List<CarController> playerCarList = null;
    [SerializeField] TimeText timeText = null;
    [SerializeField] LapText lapText = null;
    [SerializeField] List<CheckpointTrigger> checkpointTriggers = null;
    [SerializeField] PlaceText placeText = null;
    [SerializeField] Leaderboard leaderboard = null;
    [SerializeField] FinishedRaceModal finishedRaceModal = null;
    List<CarController> listOfCarsInRace;
    float raceTime = 0;
    bool raceStarted = false;

    CarController playerCar;
    int place;


    private void Awake()
    {
        listOfCarsInRace = new List<CarController>();
        timeText.SetValue(0f);
        lapText.SetValue(0);
        playerCar = playerCarList[GameManager.GetCurrentActiveCarIndex()];
        int currentTrack = int.Parse(NavigationManager.SceneData["track"]);
        bool showHeadLights = currentTrack % 2 == 0;
        playerCar.DisplayCar(true, true, showHeadLights);
        listOfCarsInRace.Add(playerCar);
        Events.RaceStarted += HandleRaceStarted;
        Events.LapCompleted += HandleLapUpdated;
        Events.CarSpawnedToTrack += HandleCarAddedToRace;
        Events.RaceCompleted += HandleRaceCompleted;
    }

    private void OnDestroy()
    {
        Events.RaceStarted -= HandleRaceStarted;
        Events.LapCompleted -= HandleLapUpdated;
        Events.CarSpawnedToTrack -= HandleCarAddedToRace;
        Events.RaceCompleted -= HandleRaceCompleted;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && raceStarted)
        {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }

        UpdateCarList();
        UpdatePlace();
        leaderboard.UpdateLeaderboard(listOfCarsInRace);
    }

    private void FixedUpdate()
    {
        if (raceStarted)
        {
            raceTime += Time.fixedDeltaTime;
            timeText.SetValue(raceTime);
        }
    }

    private void HandleRaceStarted()
    {
        raceStarted = true;
    }

    private void HandleLapUpdated(int lapNum)
    {
        lapText.SetValue(lapNum);
    }

    int SortListOfCarsInRace(CarController car1, CarController car2)
    {
        CarModel car1Model = car1.GetComponent<CarModel>();
        CarModel car2Model = car2.GetComponent<CarModel>();
        int compareValue = car1Model.GetNumberOfCheckpointsHit().CompareTo(
                                                                car2Model.GetNumberOfCheckpointsHit());
        if (compareValue != 0) return compareValue;
        float distanceOfC1FromNextCheckpoint = Vector3.Distance(
                                                            car1Model.gameObject.transform.position,
                                                             checkpointTriggers[car1Model.GetNumberOfCheckpointsHit() + 1].gameObject
                                                             .transform.position);
        float distanceOfC2FromNextCheckpoint = Vector3.Distance(
                                                            car2Model.gameObject.transform.position,
                                                             checkpointTriggers[car2Model.GetNumberOfCheckpointsHit() + 1].gameObject
                                                             .transform.position);

        int returnValue = distanceOfC1FromNextCheckpoint > distanceOfC2FromNextCheckpoint ? -1 : 1;
        return returnValue;


    }


    private void UpdatePlace()
    {
        place = listOfCarsInRace.IndexOf(playerCar) + 1;
        placeText.SetValue(place);
    }

    private void UpdateCarList()
    {
        listOfCarsInRace.Sort(SortListOfCarsInRace);
        listOfCarsInRace.Reverse();
    }

    private void HandleCarAddedToRace(CarController car)
    {
        listOfCarsInRace.Add(car);
    }

    private void HandleRaceCompleted()
    {
        finishedRaceModal.Init(HandleMainMenuButtonClicked);
        finishedRaceModal.UpdateFinalLeaderboard(listOfCarsInRace);
        finishedRaceModal.ShowModal();
        finishedRaceModal.ShowFirstPlaceCelebration(place == 1);
    }

    private void HandleMainMenuButtonClicked()
    {
        NavigationManager.LoadScene(Scenes.MAIN_MENU);
    }
}

