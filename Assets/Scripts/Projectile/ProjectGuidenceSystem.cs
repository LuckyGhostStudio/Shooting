using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectGuidenceSystem : MonoBehaviour
{
    [SerializeField] private Projectile projectile;

    [SerializeField] private float minBallisticAngle = 50f;   //最小弹道角度
    [SerializeField] private float maxBallisticAngle = 75f;   //最大弹道角度

    private float ballisticAngle;   //弹道角度

    private Vector3 targetDirection; //目标方向向量

    /// <summary>
    /// 子弹追踪
    /// </summary>
    /// <param name="target">目标</param>
    /// <returns></returns>
    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);    //随机弹道角度

        while (gameObject.activeSelf)
        {
            if (target.activeSelf)  //目标未消失
            {
                targetDirection = target.transform.position - transform.position;   //目标方向

                //计算目标方向和x轴夹角（子弹初始方向为x+） ->  子弹方向沿z轴逆时针旋转相应角度
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);

                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle); //旋转随机角度（实现弧线追踪）

                projectile.Move();  //移动
            }
            else
            {
                projectile.Move();  //原方向移动
            }

            yield return null;
        }
    }
}
