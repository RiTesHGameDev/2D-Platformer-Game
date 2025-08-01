using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeController : MonoBehaviour
{
    [SerializeField] private int life;
    [SerializeField] private int maxLife;
    [SerializeField] private Image[] fullHearts;
    [SerializeField] private Image[] emptyHearts;

    public PlayerController playerController;

    public void ReduceLife()
    {
        ChangeLife(-1);
    }
    public void ChangeLife(int amount)
    {
        life += amount;
        life = Mathf.Clamp(life, 0, maxLife); // Keep life between 0 and maxLife
        Life();

        if (life <= 0)
        {
            playerController.PlayerDeath();
        }
    }
    private void Life()
    {
        for (int i = 0; i < emptyHearts.Length; i++)
        {
            if(i < life)
            {
                fullHearts[i].enabled = true;
            }
            else
            {
                fullHearts[i].enabled = false;
            }
            if (i < maxLife)
            {
                emptyHearts[i].enabled = true;
            }
            else
            {
                emptyHearts[i].enabled = false;
            }
        }

    }
    void Start()
    {
       life = maxLife;
       Life();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
