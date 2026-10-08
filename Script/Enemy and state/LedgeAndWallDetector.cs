using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LedgeAndWallDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _edgeForwardOffset;
    private Enemy _enemy;
    // Start is called before the first frame update
    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }
    public bool IsAtEdge(float direction)
    {
        Vector2 origin = new Vector2(_enemy.transform.position.x + direction * _edgeForwardOffset, _enemy.transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _groundCheckDistance, _groundMask);
        if (hit.collider == null) 
        {
            Debug.Log("Hitting edge");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsHittingWall(float direction)
    {
        Vector2 directionVector = new Vector2(direction, 0);
        RaycastHit2D hit = Physics2D.Raycast(_enemy.transform.position, directionVector, _wallCheckDistance, _groundMask);
        if (hit.collider == null)
        {
           
            return false;
            
        }
        else
        {
            Debug.Log("Hitting wall");
            return true;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Vector2 start = transform.position;
        Vector2 end = start + Vector2.down * _groundCheckDistance;

        Gizmos.DrawLine(start, end);

        Gizmos.DrawLine(start, start + _enemy.facingDirection * _wallCheckDistance);
    }
}
