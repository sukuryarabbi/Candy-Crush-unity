using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    public GameObject[] Candys;

    public int Horizontal, Height, HorizontalSpacing, HeightSpacing;

    public static Candy[,] GameCandys;

    public static CandySpawner Instance;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);

            
        }
    }
    void Start()

    {
        GameCandys = new Candy[Horizontal, Height];

        for (int x= 0; x < Horizontal; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Candy_Spawn(x, y);
            }
        }
    }

    

    public GameObject Random_Candy_Spawner()
    {
        int random = Random.Range(0, Candys.Length);
        return Candys[random];

    }
    
    public void Candy_Spawn(int x , int y)
    {

        
        if (CandySpawner.GameCandys[x, y] == null) // Eðer hedef hücre boþsa
        {
            GameObject Random_Candy_Objects = Random_Candy_Spawner();

            GameObject newCandy = GameObject.Instantiate(Random_Candy_Objects, new Vector3(x, y + 15, 0), Quaternion.identity);

            Candy candy = newCandy.GetComponent<Candy>();

            candy.color = Random_Candy_Objects.name;

            candy.New_Candy_Location(x, y);

            GameCandys[x, y] = candy;
        }

        

    }

    public void RespawnCandy(int x , int y)
    {
        Candy_Spawn(x,y);
    }
}
