using UnityEngine;
public class Coin : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.SetTrigger("Spawn");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.GetCoins();
            anim.SetTrigger("Collected");
            FindObjectOfType<AudioManager>().coinCollect.Play();
            //Destroy(gameObject, 1.5f);
        }
    }
}
