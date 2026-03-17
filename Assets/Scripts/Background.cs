using UnityEngine;

public class Background : MonoBehaviour
{
    private Material material;

    [SerializeField] private float speed = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }
}
