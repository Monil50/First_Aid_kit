using UnityEngine;

public class ChangeMat : MonoBehaviour
{

    public Material M2;
    public GameObject[] objectsArray; 

    public void ChangeMaterial()
    {
        foreach (GameObject obj in objectsArray)
        {
            obj.GetComponent<Renderer>().material = M2;
        }
    }
}
