using UnityEngine;

public class Food : MonoBehaviour {
    public BoxCollider2D arena;
    private Snake snake;

    private void Awake() {
        snake = FindObjectOfType<Snake>();
    }

    private void Start() {
        this.randomizePosition();
    }

    private void randomizePosition() {
        Bounds bounds = this.arena.bounds;
        float x, y;

        do {
            x = Random.Range(bounds.min.x, bounds.max.x);
            y = Random.Range(bounds.min.y, bounds.max.y);
        } while (snake.Occupies(x, y));

        this.transform.position = new Vector3(
            Mathf.Round(x), Mathf.Round(y), 0.0f
        );
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            GetComponent<AudioSource>().Play();
            this.randomizePosition();
        }
    }
}
