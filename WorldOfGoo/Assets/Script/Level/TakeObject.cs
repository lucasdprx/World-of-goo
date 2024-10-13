using UnityEngine;
using UnityEngine.InputSystem;

public class TakeObject : MonoBehaviour
{
    [SerializeField] private SetLink _setLink;

    private GameObject _objectTake;
    private bool _isTake = false;
    void Update()
    {
        if (_isTake)
        {
            FollowMouse();
        }
    }
    public void FollowMouse()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        _objectTake.transform.position = position;
       if (_objectTake != null)
            _setLink.AddLinkPreview(_objectTake);
    }

    public void LeftClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider == null || hit.collider.gameObject.layer != 6)
                return;

            if (hit.collider.GetComponent<MoveMonsters>() == null)
                return;
            
            if (!hit.collider.GetComponent<MoveMonsters>().enabled)
                return;

            hit.collider.GetComponent<MoveMonsters>().enabled = false;
            _objectTake = hit.collider.gameObject;
            _isTake = true;
        }
        else if (ctx.canceled && _objectTake != null)
        {
            _setLink.AddLink(_objectTake);
            _objectTake = null;
            _isTake = false;
        }
    }
}
