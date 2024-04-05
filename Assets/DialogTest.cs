using System.Collections;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DialogTest : MonoBehaviour
{
    [SerializeField]
    private DialogSystem dialogSystem01;
    [SerializeField]
    private TextMeshProUGUI textCountdown;
    [SerializeField]
    private DialogSystem dialogSystem02;

    private IEnumerator Start()
    {
        textCountdown.gameObject.SetActive(false);

        //첫 번째 대사분기 시작
        yield return new WaitUntil(() => dialogSystem01.UpdateDialog());

        //대사분기 사이에 원하는 행동 추가가능.
        //캐릭터 움직이거나 아이템획득등의,현재 5-4-3-2-1 카운트다운실행
        textCountdown.gameObject.SetActive(true);
        int count = 5;
        while (count > 0)
        {
            textCountdown.text = count.ToString();
            count--;

            yield return new WaitForSeconds(1);
        }
        textCountdown.gameObject.SetActive(false);

        //두번째 대사분기 시작
        yield return new WaitUntil(() => dialogSystem02.UpdateDialog());

        textCountdown.gameObject.SetActive(true);
        textCountdown.text = "The End";

        yield return new WaitForSeconds(2);

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
        #endif
    }
}
