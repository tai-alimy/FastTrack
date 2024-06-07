using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Events
{
    public static Action<string> UsernameSubmitted;
    public static Action<int> CarChosen;
    public static Action BackButtonPressed;
    public static Action<Color, float, float> CarColorChosen; //Action is invoked when the player customizes the color of the car
    //    base
    //          metallicness
    //                  smoothness

    public static Action<Material> RimColorChosen;


    public static Action OkButtonClicked;
    public static Action CancelButtonClicked;
  //  public static Action PurchaseButtonClicked;
    public static Action LeftOrRightButtonClicked;
    public static Action SaveSlotClicked;
    public static Action RaceStarted;
    public static Action<int> LapCompleted;
    public static Action RaceCompleted;
    public static Action<CarController> CarSpawnedToTrack;










}