using System.Collections.Generic;
using UnityEngine;

public class MoveMonsters : MonoBehaviour
{
    [SerializeField] private int _speed;
    private Transform _transform;
    [SerializeField] private Transform _currentNode;
    private void Start()
    {
        _transform = transform;
    }
    void FixedUpdate()
    {
        MoveToNextBase();
    }
    private void MoveToNextBase()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _currentNode.position, _speed * Time.fixedDeltaTime);
        if (_transform.position == _currentNode.position)
        {
            List<Transform> _links = _currentNode.GetComponent<Link>()._links;
            _currentNode = _links[Random.Range(0, _links.Count)];
        }
    }
}
