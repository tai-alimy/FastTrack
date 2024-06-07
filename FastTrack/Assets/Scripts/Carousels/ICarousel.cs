using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarousel
{
    void ShowNextItem();
    void ShowPreviousItem();
    int GetCurrentItemIndex();

}
