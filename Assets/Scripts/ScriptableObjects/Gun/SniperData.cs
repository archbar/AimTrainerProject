using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSniperData", menuName = "GunType/SniperData")]
public class SniperData : GunData
{
    [Tooltip("The maximum sight distance that can zoomed into")]
    public float maxZoomDistance;
    [Tooltip("The increment the zoom increases by when zooming in each time")]
    public float zoomDistanceIncrement;
    [Tooltip("The current sight distance zoomed into")]
    public float currentZoomDistance;
    [Tooltip("The total number of times the zoom can be increased")]
    public int numberOfZooms;
}
