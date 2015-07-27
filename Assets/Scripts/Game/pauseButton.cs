using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class pauseButton : MonoBehaviour, IPointerClickHandler
{
	public GameObject pauseMenu;
	private float timeScale;

	void Start(){
		timeScale = Time.timeScale;
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (Time.timeScale == timeScale) {
			pauseMenu.SetActive (true);
			Time.timeScale = 0f;
		} else {
			pauseMenu.SetActive (false);
			Time.timeScale = timeScale;
		}
	}
}

