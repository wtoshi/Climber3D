using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private void Update()
    {
		OnTouch();

	}

    void OnTouch()
    {
		if (Input.GetMouseButtonDown(0))
		{
			if (GameManager.Instance.GameState != GameManager.GameStates.Started)
				return;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Raycast to climb point
			RaycastHit hit = new RaycastHit();

			
			if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask(Consts.Layers.Touchable)))
			{
				ClimbPoint climbPoint = hit.collider.GetComponent<ClimbPoint>();

                if (climbPoint != null)
                {
					EventManager.OnTouchEvent?.Invoke(new TouchableData(climbPoint));
				}
			}
			
		}
	}
}
