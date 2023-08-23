using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Candy : MonoBehaviour
{
    float x, y;

    private bool isFall;
    public bool Swap = false;


    GameObject ClickObject;

    public static Candy FirstClickedCandy;
    public static Candy SecondClickedCandy;

    public Vector3 TargetLocation;

    public List<Candy> Candy_X;
    public List<Candy> Candy_Y;

    public string color;

    public Animator anim;

   

    
    void Start()
    {

        ClickObject = GameObject.FindGameObjectWithTag("click");



        isFall = true;

        anim = GetComponent<Animator>();

        

    }


    void Update()
    {
        if (isFall)
        {

            Swap = false;

            if (transform.position.y - y < 0.02f)
            {
                isFall = false;
                transform.position = new Vector3(x, y, 0);


            }

            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), Time.deltaTime * 3f);

            
        }

        if (Swap == true)
        {
            CandySwap();
        }

        CheckAndReplaceBrokenCandies();

        Destroy(CandySpawner.GameCandys[0,0]);
    }

    public void New_Candy_Location(float _X, float _Y)
    {

        CandySpawner.GameCandys[(int)x, (int)y] = null;

        x = _X;
        y = _Y;

        CandySpawner.GameCandys[(int)x, (int)y] = this;

        isFall = true;
    }

    private void OnMouseDown()
    {


        ClickObject.transform.position = transform.position;

        CandySwapControl();

    }

    private void CandySwapControl()
    {
        if (FirstClickedCandy == null)
        {
            FirstClickedCandy = this;

        }
        else
        {

            SecondClickedCandy = this;

            if (FirstClickedCandy != SecondClickedCandy)
            {
                float dif_x = Mathf.Abs(FirstClickedCandy.x - SecondClickedCandy.x);
                float dif_y = Mathf.Abs(FirstClickedCandy.y - SecondClickedCandy.y);

                if (dif_x + dif_y == 1)
                {


                    FirstClickedCandy.TargetLocation = SecondClickedCandy.transform.position;
                    SecondClickedCandy.TargetLocation = FirstClickedCandy.transform.position;

                    FirstClickedCandy.Swap = true;
                    SecondClickedCandy.Swap = true;

                    VariablesSwap();

                    FirstClickedCandy.X_Variables_Control();
                    FirstClickedCandy.Y_Variables_Control();

                    SecondClickedCandy.X_Variables_Control();
                    SecondClickedCandy.Y_Variables_Control();

                    StartCoroutine(FirstClickedCandy.Destroy());
                    StartCoroutine(SecondClickedCandy.Destroy());

                    FirstClickedCandy = null;

                }

                else
                {

                    FirstClickedCandy = SecondClickedCandy;

                }
            }



            SecondClickedCandy = null;

        }
    }

    private void CandySwap()
    {
        transform.position = Vector3.Lerp(transform.position, TargetLocation, 0.5f);
    }

    private void VariablesSwap()
    {
        CandySpawner.GameCandys[(int)FirstClickedCandy.x, (int)FirstClickedCandy.y] = SecondClickedCandy;
        CandySpawner.GameCandys[(int)SecondClickedCandy.x, (int)SecondClickedCandy.y] = FirstClickedCandy;


        float first_x = FirstClickedCandy.x;
        float first_y = FirstClickedCandy.y;

        FirstClickedCandy.x = SecondClickedCandy.x;
        FirstClickedCandy.y = SecondClickedCandy.y;

        SecondClickedCandy.x = first_x;
        SecondClickedCandy.y = first_y;
    }

    private void X_Variables_Control()
    {
        for (int i = (int)x + 1; i < CandySpawner.GameCandys.GetLength(0); i++)
        {
            Candy candy_right = CandySpawner.GameCandys[i, (int)y];


            if (candy_right != null)
            {
                if (color == candy_right.color)
                {
                    Candy_X.Add(candy_right);
                }

                else
                {
                    break;
                }
            }

            else
            {
                break;
            }

        }

        for (int i = (int)x - 1; i >= 0; i--)
        {
            Candy candy_left = CandySpawner.GameCandys[i, (int)y];


            if (candy_left != null)
            {
                if (color == candy_left.color)
                {
                    Candy_X.Add(candy_left);
                }

                else
                {
                    break;
                }
            }

            else
            {
                break;
            }

        }
    }

    private void Y_Variables_Control()
    {
        for (int i = (int)y + 1; i < CandySpawner.GameCandys.GetLength(1); i++)
        {
            Candy candy_up = CandySpawner.GameCandys[(int)x, i];


            if (candy_up != null)
            {
                if (color == candy_up.color)
                {
                    Candy_Y.Add(candy_up);
                }

                else
                {
                    break;
                }
            }

            else
            {
                break;
            }

        }

        for (int i = (int)y - 1; i >= 0; i--)
        {
            Candy candy_down = CandySpawner.GameCandys[(int)x, i];

            if (candy_down != null)
            {
                if (color == candy_down.color)
                {
                    Candy_Y.Add(candy_down);
                }

                else
                {
                    break;
                }
            }

            else
            {
                break;
            }

        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.3f);

        if (Candy_X.Count >= 2 || Candy_Y.Count >= 2)
        {
            if (Candy_X.Count >= 2)
            {

                foreach (var item in Candy_X)
                {
                    item.anim.SetBool("Destroy", true);

                    
                    
                }
                anim.SetBool("Destroy", true);

                

            }

            else
            {

                foreach (var item in Candy_Y)
                {

                    item.anim.SetBool("Destroy", true);

                    
                    

                }
                anim.SetBool("Destroy", true);

                


            }

           
        }
    }

    private void AnimationDestroy()
    {

        Destroy(gameObject);


    }

    private void CheckAndReplaceBrokenCandies()
    {
        int emptyCellY = -1;
        for (int i = (int)y; i >= 0; i--)
        {
            if (CandySpawner.GameCandys[(int)x, i] == null)
            {
                emptyCellY = i;

                CandySpawner.Instance.RespawnCandy((int)x, (int)emptyCellY);

                isFall = true;

                break;
            }
        }

        if (emptyCellY != -1)
        {
            if (CandySpawner.GameCandys[(int)x, (int)emptyCellY] == null)
            {

                CandySpawner.GameCandys[(int)x, (int)y] = null;

                y = emptyCellY;

                CandySpawner.GameCandys[(int)x, (int)y] = this;

                isFall = true;
            }
        }
    }
}
    








