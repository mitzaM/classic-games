using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D arena;

    private void Start() {
        this.randomizePosition();
    }

    private void randomizePosition() {
        Bounds bounds = this.arena.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(
            Mathf.Round(x), Mathf.Round(y), 0.0f
        );
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            GetComponent<AudioSource>().Play();
            this.randomizePosition();
        }
    }
}
