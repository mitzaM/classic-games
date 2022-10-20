using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {
    public float speed = 20.0f;
    public float speedMultiplier = 1.0f;
    public int initialSize = 4;
    public Transform segmentPrefab;

    private Vector2 _direction = Vector2.right;
    private float _nextUpdate;
    private List<Transform> _segments = new List<Transform>();

    private void Start() {
        this.ResetState();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            this._direction = Vector2.up;
        } else if (Input.GetKeyDown(KeyCode.A)) {
            this._direction = Vector2.left;
        } else if (Input.GetKeyDown(KeyCode.S)) {
            this._direction = Vector2.down;
        } else if (Input.GetKeyDown(KeyCode.D)) {
            this._direction = Vector2.right;
        }
    }

    private void FixedUpdate() {
        // if (Time.time < this._nextUpdate) {
        //     return;
        // }

        for (int i = this._segments.Count - 1; i > 0; i--) {
            this._segments[i].position = this._segments[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + this._direction.x,
            Mathf.Round(this.transform.position.y) + this._direction.y,
            0.0f
        );
        this._nextUpdate = Time.time + 1.0f / (this.speed * this.speedMultiplier);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Food")) {
            this.Grow();
        } else if (other.gameObject.CompareTag("Obstacle")) {
            GetComponent<AudioSource>().Play();
            this.ResetState();
        }
    }

    private void Grow() {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = this._segments[this._segments.Count - 1].position;
        this._segments.Add(segment);
    }

    private void ResetState() {
        for (int i = 1; i < this._segments.Count; i++) {
            Destroy(this._segments[i].gameObject);
        }
        this._segments.Clear();
        this._segments.Add(this.transform);

        for (int i = 1; i < initialSize; i++) {
            _segments.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector3.zero;
        this._direction = Vector2.right;
    }
}
