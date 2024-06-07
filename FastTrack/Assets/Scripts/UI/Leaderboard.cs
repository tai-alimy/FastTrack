using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] List<LeaderBoardSlot> leaderboardSlots = null;
    [SerializeField] Color highlightColor = Color.gray;
    [SerializeField] PlaceText placeText = null;

    public void UpdateLeaderboard(List<CarController> cars)
    {
        for (int i = 0; i < cars.Count; i++)
        {
            print(i);
            string name = cars[i].GetCarLabel();
            int place = i + 1;
            leaderboardSlots[i].nameText.SetText(name);
            leaderboardSlots[i].placeText.SetValue(place);
            bool isPlayerCar = name.Equals(GameManager.GetUsername());
            Color color = isPlayerCar ? highlightColor : Color.gray;
            SetSlotColor(leaderboardSlots[i], color);
            if (isPlayerCar && placeText != null)
            {
                placeText.SetValue(place);
            }
        }
    }

    void SetSlotColor(LeaderBoardSlot slot, Color color)
    {
        Image slotImage = slot.GetComponent<Image>();

        if (slotImage != null)
        {
            slotImage.color = color;
        }
    }
}
