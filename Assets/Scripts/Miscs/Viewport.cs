using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    private float minX, minY, maxX, maxY;   //屏幕左下角和右上角位置

    private void Start()
    {
        Camera mainCamera = Camera.main;
        //转为世界坐标
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }

    /// <summary>
    /// 获得Player可移动位置
    /// </summary>
    /// <param name="playerPosition">Player位置</param>
    /// <param name="paddingX">x方向边距</param>
    /// <param name="paddingY">y方向边距</param>
    /// <returns></returns>
    public Vector3 PlayerMovablePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        //限制player坐标在min~max之间
        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);

        return position;
    }
}
