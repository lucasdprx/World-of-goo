using UnityEngine;
using UnityEngine.InputSystem;

public class TakeObject : MonoBehaviour
{
    [SerializeField] private SetLink _setLink;

    private Camera _camera;

    [HideInInspector] public GameObject _objectTake;
    [HideInInspector] public bool _isTake = false;
    [SerializeField] private LayerMask _layerMask;

    private void Start()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        if (_isTake)
        {
            FollowMouse();
        }
    }
    public void FollowMouse()
    {
        Vector2 position = _camera.ScreenToWorldPoint(Input.mousePosition);
        _objectTake.transform.position = position;
       if (_objectTake != null)
            _setLink.AddLinkPreview(_objectTake);
    }

    public void LeftClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, (_layerMask));
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
