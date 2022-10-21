using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {
    public float speed = 20.0f;
    public float speedMultiplier = 1.0f;
    public int initialSize = 4;
    public Transform segmentPrefab;

    private Vector2 _direction = Vector2.right;
    private float _nextUpdate;
    private List<Transform> _tail = new List<Transform>();
    private bool _shouldGrow = false;

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
        if (Time.time < this._nextUpdate) {
            return;
        }
        this.Move();
        this._nextUpdate = Time.time + (1.0f / (this.speed * this.speedMultiplier));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Food")) {
            this._shouldGrow = true;
        } else if (other.gameObject.CompareTag("Obstacle")) {
            GetComponent<AudioSource>().Play();
            this.ResetState();
        }
    }

    private void Move() {
        Vector3 headPosition = this.transform.position;
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + this._direction.x,
            Mathf.Round(this.transform.position.y) + this._direction.y,
            0.0f
        );
        if (this._tail.Count < this.initialSize) {
            this._shouldGrow = true;
        }
        if (this._shouldGrow) {
            Transform segment = Instantiate(this.segmentPrefab, headPosition, Quaternion.identity);
            this._tail.Insert(0, segment);
            this._shouldGrow = false;
        } else {
            for (int i = this._tail.Count - 1; i >= 0; i--) {
                Vector3 newPosition = i == 0 ? headPosition : this._tail[i - 1].position;
                this._tail[i].position = newPosition;
            }
        }
    }

    private void ResetState() {
        for (int i = 0; i < this._tail.Count; i++) {
            Destroy(this._tail[i].gameObject);
        }
        this._tail.Clear();
        this._direction = Vector2.right;
        this.transform.position = Vector3.zero;
    }

    private Vector2 GetInput() {
        Vector2 input = Vector2.zero;
        if (this._direction == Vector2.left || this._direction == Vector2.right) {
            if (Input.GetKeyDown(KeyCode.W)) {
                input = Vector2.up;
            } else if (Input.GetKeyDown(KeyCode.S)) {
                input = Vector2.down;
            }
        } else if (this._direction == Vector2.up || this._direction == Vector2.down) {
            if (Input.GetKeyDown(KeyCode.A)) {
                input = Vector2.left;
            } else if (Input.GetKeyDown(KeyCode.D)) {
                input = Vector2.right;
            }
        }
        return input;
    }

    public bool Occupies(float x, float y) {
        foreach (Transform segment in this._tail) {
            if (segment.position.x == x && segment.position.y == y) {
                return true;
            }
        }
        return this.transform.position.x == x && this.transform.position.y == y;
    }
}
