using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSS
{
    public class ProgramPurchase : MonoBehaviour
    {
        [SerializeField] private GameObject m_adPurchase;
        [SerializeField] private GameObject m_predictionPurchase;
        [SerializeField] private GameObject m_consolidatorPurchase;
        [SerializeField] private GameObject m_timePurchase;
        [SerializeField] private GameObject m_resizePurchase;

        [SerializeField] private GameObject m_purchaseMessage;
        [SerializeField] private GameObject m_noMoneyMessage;


        public void Purchase(int program)
        {
            bool success = ProgramManager.instance.Purchase(program);
            PurchaseConfirm(success);
        }

        public void OnEnable()
        {
            m_purchaseMessage.SetActive(false);
            m_noMoneyMessage.SetActive(false);
        }
        public void Update()
        {
            m_adPurchase.SetActive(!ProgramManager.instance.AntiAdPurchased);
            m_predictionPurchase.SetActive(!ProgramManager.instance.PredictionPurchased);
            m_consolidatorPurchase.SetActive(!ProgramManager.instance.ConsolidatorPurchased);
            m_timePurchase.SetActive(!ProgramManager.instance.TimePurchased);
            m_resizePurchase.SetActive(!ProgramManager.instance.ResizePurchased);
        }

        public void PurchaseConfirm(bool success)
        {
            if (success)
            {
                StartCoroutine(PurchasePrompt());
            }
            else
            {
                StartCoroutine(FailedPrompt());
            }
        }

        public IEnumerator PurchasePrompt()
        {
            m_purchaseMessage.SetActive(true);
            yield return new WaitForSeconds(3f);
            m_purchaseMessage.SetActive(false);
        }

        public IEnumerator FailedPrompt()
        {
            m_noMoneyMessage.SetActive(true);
            yield return new WaitForSeconds(3f);
            m_noMoneyMessage.SetActive(false);
        }
    }
}
