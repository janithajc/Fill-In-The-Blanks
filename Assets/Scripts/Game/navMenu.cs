using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class navMenu : MonoBehaviour, IPointerClickHandler
{	
	public void OnPointerClick(PointerEventData eventData) {
		Application.LoadLevel ("MainMenu");
	}
}
