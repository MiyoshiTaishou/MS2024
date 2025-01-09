using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraZoom : NetworkBehaviour
{
    GameObject player1;
    GameObject player2;
    GameObject boss;

    [SerializeField, Tooltip("��ԃY�[���������ɂǂꂮ�炢������")] float minZ;
    [SerializeField, Tooltip("�Y�[���{��")] float zoomConf;
    [SerializeField, Header("�������J�����������Ȃ�")] float zoomLimit;
    [SerializeField, Header("��ԋ߂�����炱�̒l�����͈���")] float zeroRangezoom;
    public override void FixedUpdateNetwork()
    {
       

    }
    public override void Render()
    {
        if (player1 == null || player2 == null || boss == null)
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject[] objs = scene.GetRootGameObjects();

            foreach (var obj in objs)
            {
                if (player1 == null && obj.name == "Player(Clone)")
                {
                    player1 = obj;
                    Debug.Log("Player�o�^");
                }
                else if (player2 == null && obj.name == "Player(Clone)" && obj != player1)
                {
                    player2 = obj;
                    Debug.Log("Player2�o�^");
                }
                else if (boss == null && obj.name == "Boss2D")
                {
                    boss = obj;
                    Debug.Log("Boss2D�o�^");
                }
            }
        }
        if (player1 && player2)//�����ɏ���
        {
            Vector3 pos1 = player1.transform.position;
            Vector3 pos2 = player2.transform.position;
            Vector3 pos3 = boss.transform.position;

            float minX = Mathf.Min(pos1.x,  pos2.x, pos3.x);
            float maxX = Mathf.Max(pos1.x,  pos2.x, pos3.x);
            float minY=Mathf.Min(pos1.y, pos2.y, pos3.y);
            float maxY=Mathf.Max(pos1.y, pos2.y, pos3.y);
            float minZ1 = Mathf.Min(pos1.z, pos2.z, pos3.z);
            float maxZ1 = Mathf.Max(pos1.z, pos2.z, pos3.z);


            Vector2 max = new Vector2(maxX, maxZ1);
            Vector2 min = new Vector2(minX, minZ1);
            float dd = Vector2.Distance(max, min);
            //float rangeX= Vector2.Distance(new Vector2(minX,maxZ),new Vector2(maxX,minZ));
            float newY = (Mathf.Abs(minY - maxY)/2)+8.2f;
            float rangeX = maxX - minX;
            float center;
            //float newZ = Mathf.Lerp(minZ, maxZ, dd / zoomConf); // �u10f�v�͔͈͂̃X�P�[���ɉ����Ē����\
            //float newZ = minZ1 - (dd * zoomConf) - minZ;
            float newZ = minZ1 - Mathf.Abs(rangeX);
            if(Mathf.Abs(newZ-minZ1)<zeroRangezoom)
            {
                newZ -= zeroRangezoom;
            }
            if(newZ<zoomLimit)
            {
                newZ=zoomLimit;
            }
            center = (minX + maxX) / 2;
            Vector3 pos = transform.position;
            pos.x = center;
            pos.y = newY;
            pos.z = newZ;
            //transform.position = pos;
            Vector3 targetPosition = new Vector3(center, newY, newZ);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
            //Debug.Log("����" + minX + "����" + maxX);
        }
    }
}
