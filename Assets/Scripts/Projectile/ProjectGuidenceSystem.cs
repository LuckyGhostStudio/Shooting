using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectGuidenceSystem : MonoBehaviour
{
    [SerializeField] private Projectile projectile;

    [SerializeField] private float minBallisticAngle = 50f;   //��С�����Ƕ�
    [SerializeField] private float maxBallisticAngle = 75f;   //��󵯵��Ƕ�

    private float ballisticAngle;   //�����Ƕ�

    private Vector3 targetDirection; //Ŀ�귽������

    /// <summary>
    /// �ӵ�׷��
    /// </summary>
    /// <param name="target">Ŀ��</param>
    /// <returns></returns>
    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);    //��������Ƕ�

        while (gameObject.activeSelf)
        {
            if (target.activeSelf)  //Ŀ��δ��ʧ
            {
                targetDirection = target.transform.position - transform.position;   //Ŀ�귽��

                //����Ŀ�귽���x��нǣ��ӵ���ʼ����Ϊx+�� ->  �ӵ�������z����ʱ����ת��Ӧ�Ƕ�
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);

                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle); //��ת����Ƕȣ�ʵ�ֻ���׷�٣�

                projectile.Move();  //�ƶ�
            }
            else
            {
                projectile.Move();  //ԭ�����ƶ�
            }

            yield return null;
        }
    }
}
