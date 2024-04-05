using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private Speaker[] speakers;
    [SerializeField]
    private DialogData[] dialogs;
    [SerializeField]
    private bool isAutoStart = true;
    private bool isFirst = true;
    private int currentDialogIndex = -1;
    private int currentSpeakerIndex = 0;
    private float typingSpeed = 0.1f;
    private bool isTypingEffect = false;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        for(int i=0; i<speakers.Length; ++i)
        {
            SetActiveObjects(speakers[i], false);

            speakers[i].spriteRenderer.gameObject.SetActive(true);
        }
    }
    
    public bool UpdateDialog()
    {
        if(isFirst == true)
        {
            Setup();

            if (isAutoStart) SetNextDialog();

            isFirst = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //�ؽ�Ʈ Ÿ���� ȿ�� ������϶� ���콺 ���� Ŭ���� Ÿ����ȿ������
            if(isTypingEffect == true)
            {
                isTypingEffect = false;

                //Ÿ����ȿ�� �����ϰ�,���� ��� ��ü ��� github���� �׽�Ʈ
                StopCoroutine("OnTypingText");
                speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
                //��簡 �Ϸ�Ǿ����� ��µǴ� Ŀ��Ȱ��ȭ
                speakers[currentSpeakerIndex].objectArrow.SetActive(true);

                return false;
            }
            Debug.Log("diaLogLengthss:" + dialogs.Length);
            Debug.Log("currentDialoindex:" + (currentDialogIndex + 1));
            if (dialogs.Length > currentDialogIndex + 1)
            {
                SetNextDialog();
            }
            else
            {
                //���������̾Ʒαױ��� �� ��¿Ϸ��� ���Ľ����� ��� speakers�� Ȱ������ ��� ����(ȭ���Ҹ������)
                for(int i=0; i<speakers.Length; ++i)
                {
                    SetActiveObjects(speakers[i], false);
                    speakers[i].spriteRenderer.gameObject.SetActive(false);
                }
                return true;
            }
        }
        return false;
    }

    private void SetNextDialog()
    {
        SetActiveObjects(speakers[currentSpeakerIndex], false);

        currentDialogIndex++;

        currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;

        SetActiveObjects(speakers[currentSpeakerIndex], true);
        speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
        //speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
        StartCoroutine("OnTypingText");
    }

    private void SetActiveObjects(Speaker speaker, bool visible)
    {
        speaker.imageDialog.gameObject.SetActive(visible);
        speaker.textName.gameObject.SetActive(visible);
        speaker.textDialogue.gameObject.SetActive(visible);

        speaker.objectArrow.SetActive(false);

        Color color = speaker.spriteRenderer.color;
        color.a = visible == true ? 1 : 0.2f;
        speaker.spriteRenderer.color = color;
    }

    private IEnumerator OnTypingText()
    {
        int index = 0;
        isTypingEffect = true;

        //�ؽ�Ʈ�� �ѱ��ھ� Ÿ����ġ�� ���
        while( index < dialogs[currentDialogIndex].dialogue.Length)
        {
            speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue.Substring(0, index);

            index++;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;

        //��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
        speakers[currentSpeakerIndex].objectArrow.SetActive(true);
    }
}

[System.Serializable]
public struct Speaker
{
    public SpriteRenderer spriteRenderer;
    public Image imageDialog;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDialogue;
    public GameObject objectArrow;
}

[System.Serializable]
public struct DialogData
{
    public int speakerIndex;
    public string name;
    [TextArea(3, 5)]
    public string dialogue;
}