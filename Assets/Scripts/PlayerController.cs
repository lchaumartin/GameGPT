using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int gridSize = 5;
    private Vector3 targetPosition;
    public GameObject gom;

    private void Start()
    {
        // Set initial position of player
        transform.position = new Vector3(0, 0, 0);
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePlayer(-Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePlayer(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MovePlayer(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePlayer(-Vector3.forward);
        }
    }

    private void MovePlayer(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction;
        if (IsPositionValid(newPosition))
        {
            targetPosition = newPosition;
            transform.position = targetPosition;
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        return (Mathf.Abs(position.x) <= gridSize / 2) && (Mathf.Abs(position.z) <= gridSize / 2);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            gom.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
