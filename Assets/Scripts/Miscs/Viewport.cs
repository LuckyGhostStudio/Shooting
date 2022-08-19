using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    private float minX, minY, maxX, maxY;   //��Ļ���½Ǻ����Ͻ�λ��
    private float middleX;  //�ӿ��м�Xֵ

    private void Start()
    {
        Camera mainCamera = Camera.main;
        //תΪ��������
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)).x;
    }

    /// <summary>
    /// ���Player���ƶ�λ��
    /// </summary>
    /// <param name="playerPosition">Playerλ��</param>
    /// <param name="paddingX">x����߾�</param>
    /// <param name="paddingY">y����߾�</param>
    /// <returns></returns>
    public Vector3 PlayerMovablePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        //����player������min~max֮��
        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);

        return position;
    }

    /// <summary>
    /// �������Enemy����λ��
    /// </summary>
    /// <param name="paddingX"></param>
    /// <param name="paddingY"></param>
    /// <returns></returns>
    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        return new Vector3(maxX + paddingX, Random.Range(minY + paddingY, maxY - paddingY));
    }

    /// <summary>
    /// �������Enemy�ƶ�λ�ã���Ļ�Ұ��
    /// </summary>
    /// <param name="paddingX"></param>
    /// <param name="paddingY"></param>
    /// <returns></returns>
    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {
        return new Vector3(Random.Range(middleX, maxX - paddingX), Random.Range(minY + paddingY, maxY - paddingY));
    }
}
