using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// Parallax scrolling for background elements

public class ScrollingScript : MonoBehaviour
{

    public Vector2 speed = new Vector2(0, 0);
    public Vector2 direction = new Vector2(-1, 0);
    public bool isLinkedToCamera = false;
    public bool isLooping = false;
    private List<SpriteRenderer> backgroundPart;


    void Start()
    {
        // For infinite background
        if (isLooping)
        {
            // Get all the children of the layer with a renderer
            backgroundPart = new List<SpriteRenderer>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                SpriteRenderer r = child.GetComponent<SpriteRenderer>();

                // Add only the visible children
                if (r != null)
                {
                    backgroundPart.Add(r);
                }
            }

            // Sort by position.
            backgroundPart = backgroundPart.OrderBy(
              t => t.transform.position.x
            ).ToList();
        }
    }

    void Update()
    {
        // Movement
        Vector3 movement = new Vector3(
          speed.x * direction.x,
          speed.y * direction.y,
          0);

        movement *= Time.deltaTime;
        transform.Translate(movement);


        if (isLooping)
        {
            // Get the first object.
            SpriteRenderer firstChild = backgroundPart.FirstOrDefault();

            if (firstChild != null)
            {
                // Check if the child is already (partly) before the camera.
                if (firstChild.transform.position.x < Camera.main.transform.position.x)
                {
                    // If the child is already on the left of the camera
                    if (firstChild.IsVisibleFrom(Camera.main) == false)
                    {
                        // Get the last child position.
                        SpriteRenderer lastChild = backgroundPart.LastOrDefault();

                        Vector3 lastPosition = lastChild.transform.position;
                        Vector3 lastSize = (lastChild.bounds.max - lastChild.bounds.min);

                        // Set the position of the recyled one to be AFTER the last child.
                        firstChild.transform.position = new Vector3(lastPosition.x + lastSize.x, firstChild.transform.position.y, firstChild.transform.position.z);

                        // Set the recycled child to the last position
                        // of the backgroundPart list.
                        backgroundPart.Remove(firstChild);
                        backgroundPart.Add(firstChild);
                    }
                }
            }
        }
    }
}