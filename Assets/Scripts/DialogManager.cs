using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;

    private string[] lines;
    private int index;
    private bool dialogActive = false;

    private Player player;  // ← ambil script Player

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        player = FindObjectOfType<Player>(); // ← cari Player di scene
    }

    void Update()
    {
        if (dialogActive && Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    public void StartDialog(string[] dialogLines)
    {
        lines = dialogLines;
        index = 0;
        dialogActive = true;

        dialogPanel.SetActive(true);
        dialogText.text = lines[index];

        if (player != null)
            player.SetCanMove(false); // ⛔ Kunci player
    }

    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            dialogText.text = lines[index];
        }
        else
        {
            EndDialog();
        }
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);
        dialogActive = false;

        if (player != null)
            player.SetCanMove(true); // ✔ Buka kunci player
    }
}
