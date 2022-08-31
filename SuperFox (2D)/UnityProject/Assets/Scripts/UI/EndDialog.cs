using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndDialog : MonoBehaviour
{
    new Collider2D collider2D;
    private GameObject enterDialog;

    private void Awake()
    {
        collider2D = GetComponent<BoxCollider2D>();
        enterDialog = GameObject.Find("LevelCanvas").gameObject.transform.Find("EndDialog").gameObject;
    }

    private void Update()
    {
        EndGame();
    }

    private void EndGame()
    {
        if (enterDialog.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CheckoutDialog.DialogShow(SceneManager.GetActiveScene().buildIndex);
                enterDialog.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enterDialog.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enterDialog.SetActive(false);
        }
    }
}
