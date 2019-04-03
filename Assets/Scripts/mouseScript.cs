using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class mouseScript : MonoBehaviour {

	[SerializeField] GameObject allCanvas;

	Vector3 oldMousePos;
	float mouseTimer;
	float mouseStillTime = 4f; //seconds for mouse to be still until stuff fades


    GraphicRaycaster gr;
    PointerEventData m_PointerEventData;
    EventSystem es;
    Canvas canvas;

    [Space(10)]
    [SerializeField] float cameraSpeed;
    [SerializeField] GameObject cameraPivot;
    [SerializeField] GameObject joystickRange;
    [SerializeField] GameObject joystick;
    [SerializeField] Text label;
    bool dragging;
    float joystickRangeRadius;
    float joystickRadius;

    // Use this for initialization
    void Start () {
		mouseTimer = 0f;

        canvas = GetComponent<Canvas>();
        gr = GetComponent<GraphicRaycaster>();
        es = GetComponent<EventSystem>();

        dragging = false;
        joystickRangeRadius = joystickRange.GetComponent<RectTransform>().rect.width * canvas.scaleFactor * 0.5f;
        joystickRadius = joystick.GetComponent<RectTransform>().rect.width * joystick.transform.localScale.x * canvas.scaleFactor * 0.5f;
    }
	
	// Update is called once per frame
	void Update () {

        //hover shit -----------------------------------------------------

		RaycastHit hit = new RaycastHit ();
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
			GameObject objectHit = hit.transform.gameObject;
			if (objectHit.tag == "person") {
				objectHit.GetComponent<person> ().hover();
			}
        }

        //activity shit -----------------------------------------------------

        Vector3 mousePos = Input.mousePosition;
		if (mousePos == oldMousePos) {
			mouseTimer += Time.deltaTime;
		} else {
			mouseTimer = 0;
			allCanvas.SetActive (true);
		}
		oldMousePos = mousePos;

		if (mouseTimer > mouseStillTime) {
			allCanvas.SetActive (false);
		}

        //joystick shit -----------------------------------------------------

        if (Input.GetMouseButtonDown(0)) {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(es);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            gr.Raycast(m_PointerEventData, results);
            
            if (results[0].gameObject == joystick) {
                dragging = true;
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            dragging = false;
        }

        if (dragging) {
            Vector3 dragPosition = Input.mousePosition;
            Vector3 direction = dragPosition - joystickRange.transform.position; //keep it inside circle
            if (direction.magnitude > joystickRangeRadius-joystickRadius) {
                dragPosition = joystickRange.transform.position + direction.normalized * (joystickRangeRadius-joystickRadius);
            }
            joystick.transform.position = dragPosition;
        }
        Vector3 joystickDirection = joystick.transform.position-joystickRange.transform.position;

        float inputH1 = Vector3.Dot(joystickDirection, new Vector3(1, 0, 0)) / joystickRangeRadius;
        float inputV1 = Vector3.Dot(joystickDirection, new Vector3(0, 1, 0)) / joystickRangeRadius;

        cameraPivot.transform.RotateAround(cameraPivot.transform.position, Vector3.up, -inputH1 * cameraSpeed * Time.deltaTime);
        cameraPivot.transform.RotateAround(cameraPivot.transform.position, cameraPivot.transform.right, inputV1 * cameraSpeed * Time.deltaTime);
    }
}
