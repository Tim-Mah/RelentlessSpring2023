using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMoveScript : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    private bool active;
    [SerializeField] private Vector2 move = new Vector2(0, 0);
   private Vector2 original;
    // Update is called once per frame
    private void Start()
    {
        original = vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            active = true;

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
           active = false;


        }
    }

    private void Update()
    {
        if (active)
        {
            vcam.gameObject.SetActive(true);
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = move;

        }
        
    }
}
