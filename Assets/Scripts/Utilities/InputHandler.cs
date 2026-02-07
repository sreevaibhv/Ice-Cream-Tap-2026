using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private PipeController currentPipeController;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
           
            OnMouseDown();
        }

       

        //Vector2 mousePosition = Input.mousePosition;
        //Vector3 woldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //Debug.DrawRay(woldPosition, Vector3.forward, Color.red);
    }

    private void OnMouseDown()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 woldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        RaycastHit hit;
        Physics.Raycast(woldPosition, Vector3.forward,out hit);
        if(hit.transform != null)
        {
            if(hit.collider.GetComponent<PipeController>() != null)
            {
                currentPipeController = hit.collider.GetComponent<PipeController>();
                currentPipeController.SetPouring(true);

            }
        }

    }
}
