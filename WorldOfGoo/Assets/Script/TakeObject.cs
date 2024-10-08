using UnityEngine;
using UnityEngine.InputSystem;

public class TakeObject : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private GameObject _objectTake;
    private bool _isTake = false;
    void Update()
    {
        if (_isTake)
        {
            Vector3 position = _camera.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            _objectTake.transform.position = position;
        }
    }

    public void LeftClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.layer == 6 && hit.collider.GetComponent<MoveMonsters>().enabled)
            {
                hit.collider.GetComponent<MoveMonsters>().enabled = false;
                _objectTake = hit.collider.gameObject;
                _isTake = true;
            }
        }
        else if (ctx.canceled && _objectTake != null)
        {
            _objectTake = null;
            _isTake = false;
        }
    }
}
