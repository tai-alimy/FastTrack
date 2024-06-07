using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LapText : FormattedText
{

    private int value;

    public void SetValue(int value)
    {
        this.value = value;
        FormatText();
    }

    protected override void FormatText()
    {
        formattedText.SetText(prefix + " " + value);
    }

}