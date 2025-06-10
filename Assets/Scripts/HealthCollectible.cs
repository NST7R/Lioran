using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private int healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Lioran")
        {
            collision.GetComponent<LioranHealth>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}
