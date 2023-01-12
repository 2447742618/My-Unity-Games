using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    private void Start()
    {
        SwitchConfinerShape();    
    }

    private void SwitchConfinerShape()
    {
        PolygonCollider2D cofinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D = cofinerShape;

        //用于清除缓存
        confiner.InvalidatePathCache();
    }
}
