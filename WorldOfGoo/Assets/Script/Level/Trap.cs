using UnityEngine;

public class Trap : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private float _speed;
    [SerializeField] private TakeObject _takeObject;
    [SerializeField] private SetLink _setLink;
    void Start()
    {
        _transform = transform;
    }
    void FixedUpdate()
    {
        _transform.Rotate(0, 0, 1 * _speed * Time.fixedDeltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            for (int i = 0; i < _setLink._listLink.Count; i++)
            {
                if (_setLink._listLink[i] == null)
                    continue;

                if (_setLink._listLink[i].GetComponent<LinkVisual>().node1 == collision.transform)
                {
                    _setLink._listLink[i].SetActive(false);
                }

                if (_setLink._listLink[i].GetComponent<LinkVisual>().node2 == collision.transform)
                {
                    _setLink._listLink[i].SetActive(false);
                }
            }
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("enter");
        if (collision.gameObject.layer == 6)
        {
            _takeObject._objectTake = null;
            _takeObject._isTake = false;
            Destroy(collision.gameObject);
            _setLink.EnableLinkPreview(false);
        }
        else if (collision.gameObject.layer == 0)
        {
            DeleteLink(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
    private void DeleteLink(GameObject obj)
    {
        Destroy(obj.GetComponent<LinkVisual>()._joint);
        List_Link list_Link1 = obj.GetComponent<LinkVisual>().node1.GetComponent<List_Link>();
        List_Link list_Link2 = obj.GetComponent<LinkVisual>().node2.GetComponent<List_Link>();
        for (int i = 0; i < list_Link1._links.Count; i++)
        {
            if (list_Link1._links[i] == list_Link2.transform)
            {
                list_Link1._links[i] = null;
                list_Link1._links.RemoveAt(i);
            }
        }
        for (int i = 0; i < list_Link2._links.Count; i++)
        {
            if (list_Link2._links[i] == list_Link1.transform)
            {
                list_Link2._links[i] = null;
                list_Link2._links.RemoveAt(i);
            }
        }
    }
}
