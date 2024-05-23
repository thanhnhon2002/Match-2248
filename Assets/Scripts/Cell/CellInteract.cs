using DarkcupGames;
using UnityEngine;
using UnityEngine.Events;

public class CellInteract: MonoBehaviour
{
    public UnityEvent eventInteract;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Loading"))
        {
            GetComponent<CellInteractEffect>().PlaySound();
            GetComponent<BounceOnClick>().Bounce();
            eventInteract?.Invoke();
        }
    }
}
