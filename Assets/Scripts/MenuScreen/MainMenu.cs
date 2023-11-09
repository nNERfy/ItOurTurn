using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField]Text dialogText;
    [SerializeField]GameObject dialogImage;
    public void PlayGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void Help(){
        dialogImage.SetActive(true);
        StartCoroutine(TextShowUp());
    
    }

    public void DisableIMAGE(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            dialogImage.SetActive(false);
        }
    }

    public IEnumerator TextShowUp(){
        string A = "Welcome, all who have found themselves here. In this realm, you are but a mere manifestation of a fading memory, devoid of recollections. Yet, even without the anchor of remembrance, you possess an inherent strength—the resilience to confront adversity. Utilize this power to seek the lost memories, and ultimately, to find your way out of this realm. The destiny lies within your hands.";
        yield return TypeDialog(A);
        yield return new WaitForSeconds(5f);
        dialogImage.SetActive(false);
    }

    public void ExitGame(){
        Application.Quit();
    }

    public IEnumerator TypeDialog(string dialog){
        dialogText.text = "";
        foreach(var letter in dialog.ToCharArray()){
            dialogText.text+= letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
    }
}
