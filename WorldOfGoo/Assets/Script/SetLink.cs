using UnityEngine;

public class SetLink : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private float _radiusSphere;
    [SerializeField] private float _radiusSphereVerif;

    [SerializeField] private int _numberLink;
    [SerializeField] private float _frequencyLink;

    public void AddLink(GameObject objTake)
    {
        Vector2 position = _camera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] node = Physics2D.OverlapCircleAll(position, _radiusSphere, 1 << LayerMask.NameToLayer("Node"));
        SortCollider(node, objTake.transform.position);

        if (node.Length < 2 || Physics2D.OverlapCircleAll(position, _radiusSphereVerif).Length > 1)
        {
            ResetObject(objTake);
            return;
        }

        for (int i = 0; i < node.Length; i++)
        {
            if (objTake.GetComponent<List_Link>()._links.Count >= _numberLink)
                break;

            //Add link
            objTake.GetComponent<List_Link>()._links.Add(node[i].transform);
            node[i].GetComponent<List_Link>()._links.Add(objTake.transform);

            //Add joint
            SpringJoint2D joint = objTake.AddComponent<SpringJoint2D>();
            joint.connectedBody = node[i].GetComponent<Rigidbody2D>();
            joint.frequency = _frequencyLink;
            joint.autoConfigureDistance = false;
        }

        objTake.layer = 7;
        objTake.GetComponent<Collider2D>().isTrigger = false;
    }
    private void ResetObject(GameObject obj)
    {
        MoveMonsters moveMonsters = obj.GetComponent<MoveMonsters>();
        obj.transform.position = moveMonsters._currentNode.position;
        moveMonsters.enabled = true;
    }

    private void SortCollider(Collider2D[] node, Vector2 posObj)
    {
        for (int i = 0; i < node.Length - 1; i++)
        {
            for (int j = 0; j < node.Length - 1; j++)
            {
                if (Vector2.Distance(posObj, node[j].transform.position) >
                    Vector2.Distance(posObj, node[j + 1].transform.position))
                {
                    (node[j + 1], node[j]) = (node[j], node[j + 1]);
                }
            }
        }
    }
    //private void SetTransformLink(Collider2D closestNode, GameObject linkPrefab, GameObject objTake)
    //{
    //    //Set position and scale
    //    Vector3 newPos = (objTake.transform.position - closestNode.transform.position) / 2 + closestNode.transform.position;
    //    linkPrefab.transform.position = newPos;

    //    float scaleX = Vector2.Distance(closestNode.transform.position, objTake.transform.position) - linkPrefab.transform.localScale.x;
    //    linkPrefab.transform.localScale += new Vector3(scaleX, 0, 0);

    //    //Rotate
    //    Vector3 dir = objTake.transform.position - linkPrefab.transform.position;
    //    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    //    linkPrefab.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, linkPrefab.transform.rotation.y, 1));
    //}
}