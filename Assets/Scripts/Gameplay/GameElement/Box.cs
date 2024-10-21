using UnityEngine;

public class Box : MonoBehaviour
{
	[SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField] private float _fallSpeed;
    protected Rigidbody2D rb;
    private Collider2D[] results = new Collider2D[3];

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        int length = Physics2D.OverlapBoxNonAlloc(_groundCheckPoint.position, _groundCheckSize, 0, results, _groundLayer);
        if (length <= 1 && rb.velocity.y < 0.1f)
		{
            rb.velocity = new Vector2(rb.velocity.x, _fallSpeed);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
	}
}
