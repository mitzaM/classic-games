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
        Vector2 input = this.GetInput();
        if (input != Vector2.zero) {
            this._direction = input;
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

    private Vector2 GetInput() {
        Vector2 input = Vector2.zero;
        if (this._direction.x != 0.0f) {
            if (Input.GetKeyDown(KeyCode.W)) {
                input = Vector2.up;
            } else if (Input.GetKeyDown(KeyCode.S)) {
                input = Vector2.down;
            }
        } else if (this._direction.y != 0.0f) {
            if (Input.GetKeyDown(KeyCode.A)) {
                input = Vector2.left;
            } else if (Input.GetKeyDown(KeyCode.D)) {
                input = Vector2.right;
            }
        }
        return input;
    }

    public bool Occupies(float x, float y) {
        foreach (Transform segment in this._segments) {
            if (segment.position.x == x && segment.position.y == y) {
                return true;
            }
        }
        return false;
    }
}
