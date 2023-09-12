using UnityEngine;

public class Drive : MonoBehaviour {

    // Storage for the camera
    Camera cam;
    // cube movement speed
    public float speed = 10.0f;
    // cube rotation speed
    public float rotationSpeed = 20.0f;

    void Start() {

        // Get hold of the camera
        cam = this.GetComponentInChildren<Camera>();
        // Make the camera look at the cube
        cam.gameObject.transform.LookAt(this.transform.position);
    }

    void Update() {

        // Get input from A, S, D, W and arrow keys and store it
        float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float translation2 = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        // Translate the cube
        this.transform.Translate(0.0f, 0.0f, translation);
        this.transform.Translate(translation2, 0.0f, 0.0f);

        // Check for left rotation
        if (Input.GetKey(KeyCode.Z)) {

            transform.Rotate(0.0f, -rotationSpeed * Time.deltaTime, 0.0f);
        }

        // Check for right rotation
        if (Input.GetKey(KeyCode.C)) {

            transform.Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f);
        }


        // Zoom in and check we are greater than 5.0 in Y (values may defer depending on your set up)
        if (Input.GetKey(KeyCode.R) && cam.gameObject.transform.position.y > 5.0f) {

            cam.gameObject.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
        }

        // Zoom out and check we are less than 22.0 in the Y (values may defer depending on your set up)
        if (Input.GetKey(KeyCode.F) && cam.gameObject.transform.position.y < 2.0f) {

            cam.gameObject.transform.Translate(0.0f, 0.0f, -speed * Time.deltaTime);
        }

        // Get the angle between the camera and the cube
        float angle = Vector3.Angle(cam.gameObject.transform.forward, Vector3.up);
        // Debug.Log(angle);

        // Move up and look at cube and check we are less than 175 degrees
        if (Input.GetKey(KeyCode.T) && angle < 175.0f) {

            cam.gameObject.transform.Translate(Vector3.up);
            cam.gameObject.transform.LookAt(this.transform.position);
        }

        // Move down and look at cube and check we're greater thatn 95 degrees
        if (Input.GetKey(KeyCode.G) & angle > 95.0f) {

            cam.gameObject.transform.Translate(-Vector3.up);
            cam.gameObject.transform.LookAt(this.transform.position);
        }
    }
}
