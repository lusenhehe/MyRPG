using UnityEngine;
public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private UI ui;

    public string checkpointID;

    public bool activated;

    private void Awake(){
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        ui = GameManager.instance.ui;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckpoint();
            SaveManager.instance.SaveGame();
        }
    }
    public void ActivateCheckpoint()
    {
        GameManager.instance.lastActivatedCheckpointID = checkpointID;
        AudioManager.instance.PlaySFX(5, transform);

        if (activated)
        {
            return;
        }

        anim.SetBool("Active", true);
        activated = true;
    }

    [ContextMenu("Generate checkpoint ID")]
    private void GenerateCheckpointID()
    {
        checkpointID = System.Guid.NewGuid().ToString();
    }
    private void Update(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach(var hit in colliders){
            if (hit.GetComponent<Player>() != null)
            {
                if(Input.GetKeyDown(KeyCode.Z)){
                    ui.SwitchToMenu(ui.checkpoint_UI);
                }
            }
        }
    }
}
