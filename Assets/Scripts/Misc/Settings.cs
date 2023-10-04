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


    /*info


    100 - main exit lock
    101 - security room lock
    102 - music class lock
    103 - math class lock
    104 - battery holder
    105 - literature class lock
    106 - art class lock
    107 - teachers locker room lock

    */
}
