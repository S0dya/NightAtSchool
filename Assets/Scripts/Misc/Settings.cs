using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    //game save logic
    public static bool firstTime;
    public static float[] soundVolume = new float[3];

    //UI
    public static string[] interactionNames = { "Open", "Pick up", "Open", "Open", "Hide" };

    //uiInteraction
    public static int[][] lockPassword = new int[][]
    {
        new int[] { 2, 4, 1, 8 },
        new int[] { 2, 4, 1, 8 }
    };
}
