using UnityEngine;

public class Back : MonoBehaviour
{
    public GameObject canvasOff;    // 关闭的画布


    void Start()
    {

    }

    /// <summary>
    /// 返回（关闭指定画布）
    /// </summary>
    public void GoBack()
    {
        if (canvasOff != null)
        {
            canvasOff.SetActive(false);
        }
    }

    /// <summary>
    /// 返回并销毁预制件实例
    /// </summary>
    public void BackAndDestroy(GameObject obj)
    {
        if (obj == null) return;

        // 更新管理器中的窗口状态
        if (PopupWindowManager.instance != null)
        {
            PopupWindowManager.instance.windowActive = false;
        }

        // 播放关闭动画（可选）
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Close");
            // 延迟销毁
            StartCoroutine(DestroyAfterAnimation(obj, 0.3f));
        }
        else
        {
            // 直接销毁
            Destroy(obj,0.05f);
        }
    }

    private System.Collections.IEnumerator DestroyAfterAnimation(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            Destroy(obj);
        }
    }
}