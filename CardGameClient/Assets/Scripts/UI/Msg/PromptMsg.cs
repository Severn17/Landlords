using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PromptMsg
{
    public string Text;
    public Color Color;

    public PromptMsg()
    {

    }
    public PromptMsg(string text,Color color)
    {
        Text = text;
        Color = color;
    }

    public void Change(string text,Color color)
    {
        this.Text = text;
        this.Color = color;
    }
}
