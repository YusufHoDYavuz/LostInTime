using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetForEnemy : MonoBehaviour
{
    [SerializeField] float distance = 3f; // Oluþturulan noktanýn objeden olan uzaklýðý
    public GameObject pointObject; 


    private void Start()
    {
      
    }

    public void GenerateRandomPoint()
    {
     
        // Objeye olan uzaklýk vektörü
        Vector3 offset = Random.onUnitSphere * distance;

        // Oluþturulan noktanýn konumu
        Vector3 randomPoint = transform.position + offset;
        randomPoint.y = 0f; // Y koordinatýný sýfýr olarak ayarla

        // Noktayý yerleþtir
        pointObject = new GameObject("EmptyObject");
        pointObject.transform.position = randomPoint;


    }
}
