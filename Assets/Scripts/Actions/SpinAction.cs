using UnityEngine;

public class SpinAction : BaseAction
{
    float totalSpinHappened = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        float spinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);

        totalSpinHappened += spinAmount;
        if (totalSpinHappened >= 360)
        {
            isActive = false;
            totalSpinHappened = 0;
        }
    }

    public void Spin()
    {
        isActive = true;
    }
}
