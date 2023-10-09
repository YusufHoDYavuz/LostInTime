using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private float textSpeed = 40;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private GameObject textPanelObject;
    private bool isPlayingDialogue = false;

    [SerializeField] private Image endGamePicture;

    void ShowTextBox(string[] dialogues)
    {
        textPanelObject.SetActive(true);
        textBox.text = "";

        textPanelObject.transform.DOScale(Vector3.one, 0.25f)
            .OnComplete(() => StartCoroutine(StartDialogue(dialogues)));
    }

    void CloseTextBox()
    {
        textPanelObject.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            textPanelObject.SetActive(false);
            isPlayingDialogue = false;

            if (Singleton.Instance.gameFinished)
            {
                endGamePicture.enabled = true;
                endGamePicture.DOColor(Color.white, 1.5f).OnComplete(() => Invoke(nameof(GoMainMenu), 2f));
            }
        });
    }

    void GoMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayOldMan()
    {
        if (isPlayingDialogue)
            return;
        var singleton = Singleton.Instance;

        if (!(singleton.pastPuzzlesScrollCount[0] && singleton.pastPuzzlesScrollCount[1] &&
              singleton.pastPuzzlesScrollCount[2] && singleton.pastPuzzlesScrollCount[3]))
            return;

        isPlayingDialogue = true;
        if (!Singleton.Instance.gameFinished)
        {
            ShowTextBox(new string[]
            {
                "Yaşlı Adam: Seni gördüğüme sevindim. Uzun zamandır gelmeni bekliyorum. Umarım senin için gelmek kolay olmuştur. Ben senin içine bulunduğun Anakrobotun mucidiyim. Bu robotu oğlum için yapmıştım fakat onun ruhunu Anakrobota aktarmayı başaramadım. Sen bir mucizesin.",
                "Anakrobot: Ben senin kadar görüştüğümüze sevinmedim. Bir robotun bedenine hapsoldum ve peşimde beni öldürmek isteyen askerler var.",
                "Yaşlı Adam: Demek T.S örgütü nesiller boyu varlığına devam edebilmiş. O robotun içine girmek isteyen ne kadar fazla kişi olduğuna inanamazsın. Şu an kendini şansız hissediyor olabilirsin fakat elinde çok büyük bir güç var. Peşindeki askerler de bu güce sahip olmak isteyen T.S örgütünün askerleri.",
                "Anakrobot: Ben ne bu gücü ne de bu robottan bedeni istiyorum!",
                "Yaşlı Adam: Bu robotun içine girdiğin zaman zamanda bir kırılma yarattın ve insan bedeninin olduğu zamanları bu boyuttan sildin. Eski bedenine geri kavuşmak için zamanda bir kırılma oluşturup robotun içine hapsolmadan önceki zamanına geri gitmen gerek.",
                "Anakrobot: Zamanda nasıl bir kırılma oluşturabilirim?",
                "Yaşlı Adam: Her zaman kırılmasında bir taş oluşur. Senin yaşadığın şimdiki zamanda, benim yaşadığım zamanda ve gelecek zamanda bir taş bulunmakta. Benim yaşadığım zamanda bu taş kalenin içinde insanların çözemeyeceği lanetli bir şifreyle korunmakta. Bu üç taşı bulup bana getirirsen seni yeniden bedenine kavuşturabilirim.",
                "Anakrobot: Tüm taşları bulup geri döneceğim."
            });
        }
        else
        {
            ShowTextBox(new string[]
            {
                "Yaşlı Adam: Tekrardan hoş geldin. İstediğim taşları toplayabildin mi?",
                "Anakrobot: Evet, üçünü de istediğin gibi topladım. Lütfen bedenime ve eski hayatıma tekrar kavuşmama yardımcı ol.",
                "Yaşlı Adam: Tekrar bedenine geri dönmek mi? Elinde bu kadar güç varken tek istediğin bu mu? Tekrar eski hayatına dönebileceğini de nereden çıkardın. Bu taşlara sahip olan kişi tüm evrendeki zamanı kontrol etme gücüne sahip olur. Sen bunların hiçbirini haketmiyorsun. Ölen çocuğumu bile yeniden hayata getirebilirim ve istediğim herkesi yok edebilirim. Artık tüm evrenin sahibi benim.",
                "Anakrobot: Evrenin kaderini senin ellerine bırakmayacağım."
            });
        }
    }

    IEnumerator StartDialogue(string[] dialogueStrings, int dialogueIndex = 0)
    {
        if (dialogueIndex == dialogueStrings.Length)
        {
            yield return new WaitForSeconds(2);
            CloseTextBox();
        }
        else
        {
            string text = "";
            float dialogueTime = dialogueStrings[dialogueIndex].Length / textSpeed;
            DOTween.To(() => text, x => text = x, dialogueStrings[dialogueIndex], dialogueTime).SetEase(Ease.Linear)
                .OnUpdate(() => { textBox.text = text; });

            yield return new WaitForSeconds(dialogueTime + 1);
            StartCoroutine(StartDialogue(dialogueStrings, dialogueIndex + 1));
        }
    }
}