using UnityEngine;

public class Gold : MonoBehaviour
{
    public float dropToYPos;
    public float speed = 1.5f;

    private void Start()
    {
        // dropToYPos = Random.Range(-2f, 1f);
        Destroy(gameObject, Random.Range(8, 12));
    }

    private void Update()
    {
        if (transform.position.y > dropToYPos)
        {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }
        
    }

}
