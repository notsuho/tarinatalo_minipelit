using UnityEngine;

// Assings a random material to the object's MeshRenderer from its list of materials.
public class RandomMaterial : MonoBehaviour
{
    [SerializeField] Material[] materialList;
    private Material material;

    public Material GetMaterial()
    {
        return material;
    }

    void Awake()
    {
        if (materialList.Length == 0)
        {
            Debug.Log("Jam material list is empty.");
            return;
        }
        
        if (!gameObject.GetComponent<MeshRenderer>())
        {
            Debug.Log("Game object is missing a mesh renderer");
            return;
        }

        material = materialList[Random.Range(0, materialList.Length)];

        gameObject.GetComponent<MeshRenderer>().material = material;
    }
}