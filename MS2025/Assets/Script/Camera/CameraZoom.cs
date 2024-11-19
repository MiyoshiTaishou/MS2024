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

    [SerializeField, Tooltip("àÍî‘ÉYÅ[ÉÄÇµÇΩéûÇ…Ç«ÇÍÇÆÇÁÇ¢ó£Ç∑Ç©")] float minZ;
    [SerializeField, Tooltip("ÉYÅ[ÉÄî{ó¶")] float zoomConf;

    public override void FixedUpdateNetwork()
    {
        if (player1 == null || player2 == null || boss == null)
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject[] objs = scene.GetRootGameObjects();

            foreach(var obj in objs)
            {
                if(player1==null&&obj.name=="Player(Clone)")
                {
                    player1= obj;
                    Debug.Log("Playerìoò^");
                }
                else if(player2==null&&obj.name=="Player(Clone)"&&obj!=player1)
                {
                    player2= obj;
                    Debug.Log("Player2ìoò^");
                }
                else if(boss==null&&obj.name== "Boss2D")
                {
                    boss= obj;
                    Debug.Log("Boss2Dìoò^");
                }
            }
        }        
        if(player1&&player2)//Ç±Ç±Ç…èàóù
        {
            Vector3 pos1= player1.transform.position;
            Vector3 pos2= player2.transform.position;
            Vector3 pos3= boss.transform.position;

            float minX = Mathf.Min(pos1.x, pos2.x, pos3.x);
            float maxX = Mathf.Max(pos1.x, pos2.x, pos3.x);
            float minZ1 = Mathf.Min(pos1.z, pos2.z);
            float maxZ1= Mathf.Max(pos1.z, pos2.z, pos3.z);

            Vector2 max= new Vector2(maxX, maxZ1);
            Vector2 min= new Vector2(minX, minZ1);
            float dd=Vector2.Distance(max,min);
            //float rangeX= Vector2.Distance(new Vector2(minX,maxZ),new Vector2(maxX,minZ));

            float rangeX= maxX-minX;
            float center;
            //float newZ = Mathf.Lerp(minZ, maxZ, dd / zoomConf); // Åu10fÅvÇÕîÕàÕÇÃÉXÉPÅ[ÉãÇ…âûÇ∂Çƒí≤êÆâ¬î\
            float newZ = minZ1-(dd * zoomConf)-minZ;
            center =(minX+maxX)/2;
            Vector3 pos = transform.position;
            pos.x = center;
            pos.z = newZ;
            transform.position = pos;
            Debug.Log("ãóó£" + minX + "Ç¡Ç±" + maxX);
        }

    }
}
