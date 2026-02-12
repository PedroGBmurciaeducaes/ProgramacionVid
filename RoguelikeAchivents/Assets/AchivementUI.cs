using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class AchievementUI : MonoBehaviour
{
    public UIDocument UIDoc;
    private VisualElement root;

    private void OnEnable()
    {
        root = UIDoc.rootVisualElement;
        AchievementManager.OnUnlockAchievement += ShowAchievement;
    }

    private void OnDisable()
    {
        AchievementManager.OnUnlockAchievement -= ShowAchievement;
    }

    private void ShowAchievement(Achievement ach)
    {
        VisualElement popup = new VisualElement();
        popup.style.width = 300;
        popup.style.height = 80;
        popup.style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0.85f));
        popup.style.position = Position.Absolute;
        popup.style.top = 20;
        popup.style.left = 20;
        popup.style.paddingTop = 10;
        popup.style.paddingBottom = 10;
        popup.style.paddingLeft = 10;
        popup.style.paddingRight = 10;
        popup.style.borderTopLeftRadius = 8;
        popup.style.borderTopRightRadius = 8;
        popup.style.borderBottomLeftRadius = 8;
        popup.style.borderBottomRightRadius = 8;

        Label label = new Label($"LOGRO DESBLOQUEADO\n{ach.Title}");
        label.style.color = Color.white;
        label.style.unityTextAlign = TextAnchor.MiddleCenter;

        popup.Add(label);
        root.Add(popup);

        StartCoroutine(HidePopup(popup, 3f));
    }

    private IEnumerator HidePopup(VisualElement popup, float delay)
    {
        yield return new WaitForSeconds(delay);
        popup.RemoveFromHierarchy();
    }
}
