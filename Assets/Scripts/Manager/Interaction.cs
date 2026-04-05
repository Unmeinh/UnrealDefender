using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{

    public Slider slider;
    public TextMeshProUGUI myDialog;
    public GameObject queenObject;
    public GameObject backgroundObject;
    public GameObject dialogObject;
    public GameObject buttonObject;
    public List<QueenEmote> queenEmotes = new();
    Dialog[] introduceDialog, winnerDialog;
    Dialog[] curDialog;
    int curDialogIndex = 0;
    bool isIntroduced, isWinner;

    void Start()
    {
        isIntroduced = false;
        isWinner = false;
        CreateIntroduceDialog();
        CreateWinnerDialog();
        gameObject.GetComponent<Button>().onClick.AddListener(NextDialog);
    }

    void Update()
    {
        if (!isIntroduced && introduceDialog.Length > 0)
        {
            isIntroduced = true;
            SetDialog(introduceDialog);
        }
        if (Human.All.Count == 0 && !isWinner && winnerDialog.Length > 0 && slider.value == 1)
        {
            isWinner = true;
            SetDialog(winnerDialog);
        }
    }

    void SetDialog(Dialog[] dialogs)
    {
        queenObject.SetActive(true);
        backgroundObject.SetActive(true);
        dialogObject.SetActive(true);
        curDialog = dialogs;
        curDialogIndex = 0;
        Time.timeScale = 0;
        ShowDialog(0);
    }

    public void ShowDialog(int index)
    {
        if (index < 0 || index >= curDialog.Length)
            return;

        myDialog.text = curDialog[index].text;
        queenObject.GetComponent<Image>().sprite = curDialog[index].sprite;
    }

    public void NextDialog()
    {
        curDialogIndex++;

        if (curDialogIndex >= curDialog.Length && !isWinner)
        {
            curDialogIndex = -1;
            queenObject.SetActive(false);
            backgroundObject.SetActive(false);
            dialogObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        if (isWinner && curDialogIndex == curDialog.Length - 1)
        {
            buttonObject.SetActive(true);
        }

        ShowDialog(curDialogIndex);
    }


    void CreateIntroduceDialog()
    {
        introduceDialog = new Dialog[]
        {
        new(Emote.talk, "Xin chào đằng đó.", queenEmotes),
        new(Emote.calm, "Ta là nữ hoàng dark elf.\nTình hình là lũ con người đang tiến đến nơi đây để đánh chiếm nốt lâu đài của ta!", queenEmotes),
        new(Emote.mad, "Nếu lâu đài của ta thất thủ thì thế giới kì ảo này sẽ hoàn toàn nằm dưới quyền cai trị của loài người!", queenEmotes),
        new(Emote.sad, "Nhưng hiện tại ma lực của ta đang quá yếu...", queenEmotes),
        new(Emote.smile, "Thật may mắn là có những đồng minh đã đến đây và giúp đỡ chúng ta phòng thủ cuộc tấn công của lũ loài người.", queenEmotes),
        new(Emote.talk, "Ta cũng có tạo 5 chiếc trụ phòng thủ như là phòng tuyến cuối cùng của chúng ta.", queenEmotes),
        new(Emote.talk, "Hãy giúp ta phòng thủ cuộc tấn công này!\nHãy cố gắng cầm cự cho đến khi ta hồi phục đủ ma lực!", queenEmotes),
        new(Emote.mad, "Một khi ta đã hồi phục đủ ma lực.. Lũ loài người sẽ biết tay!", queenEmotes),
        new(Emote.smile, "Vậy nhé, ta phải lui vào trong lâu đài rồi. Chúc ngươi may mắn!", queenEmotes),
        };
    }

    void CreateWinnerDialog()
    {
        winnerDialog = new Dialog[]
        {
        new(Emote.shock, "Ô!?", queenEmotes),
        new(Emote.shock, "Ngươi đã tiêu diệt được hết bọn chúng rồi à.", queenEmotes),
        new(Emote.sad, "Ta còn chưa kịp hồi phục đủ ma lực........", queenEmotes),
        new(Emote.talk, "Thôi thì ta cũng chưa hồi phục đủ ma lực để cho ngươi chứng kiến cái kết sẽ đến của lũ loài người.", queenEmotes),
        new(Emote.calm, "Mà tên làm game hắn cũng không có làm nhiều level cho trò chơi này.", queenEmotes),
        new(Emote.smile, "Nên là nếu muốn thì ngươi bấm vào nút chơi lại ở đây nha.\nCảm ơn vì đã ủng hộ chúng ta!", queenEmotes),
        };
    }
}

[System.Serializable]
public enum Emote
{
    talk,
    shock,
    calm,
    smile,
    sad,
    mad
}

[System.Serializable]
public class QueenEmote
{
    public Sprite queenSprite;
    public Emote emote;
}

[System.Serializable]
public class Dialog
{
    public Sprite sprite;
    public string text;

    public Dialog(Emote emote, string text, List<QueenEmote> emoteList)
    {
        this.text = text;
        sprite = GetSprite(emote, emoteList);
    }

    Sprite GetSprite(Emote emote, List<QueenEmote> list)
    {
        foreach (var e in list)
        {
            if (e.emote == emote)
                return e.queenSprite;
        }

        return list.Find(q => q.emote == Emote.talk).queenSprite;
    }
}