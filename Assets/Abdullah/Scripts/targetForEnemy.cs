using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetForEnemy : MonoBehaviour
{
    [SerializeField] float distance = 3f; // Oluþturulan noktanýn objeden olan uzaklýðý
    public GameObject pointObject;
    [SerializeField] Enemy_AI Enemy_AI;



    private void Start()
    {
      
    }

    public void GenerateRandomPoint(GameObject enemy)
    {
 
        if (Vector3.Distance(transform.position, enemy.transform.position) < Enemy_AI.distanceEnemy)
        {
            // Objeye olan uzaklýk vektörü
            Vector3 offset = Random.onUnitSphere * distance;

            // Oluþturulan noktanýn konumu
            Vector3 randomPoint = transform.position + offset;
            randomPoint.y = enemy.transform.position.y; // Y koordinatýný sýfýr olarak ayarla

            // Noktayý yerleþtir
            pointObject = new GameObject("EmptyObject");
            pointObject.transform.position = randomPoint;

        }
    }
}
