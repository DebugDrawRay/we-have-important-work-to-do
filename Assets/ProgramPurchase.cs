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

        [SerializeField] private SuperTextMesh m_adPrice;
        [SerializeField] private SuperTextMesh m_predictionPrice;
        [SerializeField] private SuperTextMesh m_consolidatorPrice;
        [SerializeField] private SuperTextMesh m_timePrice;
        [SerializeField] private SuperTextMesh m_resizePrice;
        [SerializeField] private SuperTextMesh m_ramPrice;


        private void Awake()
        {
            m_adPrice.text = Settings.AntiAdCost.ToString() + " CeleryBucks";
            m_predictionPrice.text = Settings.PredictionCost.ToString() + " CeleryBucks";
            m_consolidatorPrice.text = Settings.ConsolidatorCost.ToString() + " CeleryBucks";
            m_timePrice.text = Settings.TimeCost.ToString() + " CeleryBucks";
            m_resizePrice.text = Settings.ResizeCost.ToString() + " CeleryBucks";
            m_ramPrice.text = Settings.RamCost.ToString() + " CeleryBucks";
        }
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
            m_adPurchase.SetActive(ProgramManager.instance.AntiAdAvailable);
            m_predictionPurchase.SetActive(ProgramManager.instance.PredictionAvailable);
            m_consolidatorPurchase.SetActive(ProgramManager.instance.ConsolidatorAvailable);
            m_timePurchase.SetActive(ProgramManager.instance.TimeAvailable);
            m_resizePurchase.SetActive(ProgramManager.instance.ResizeAvailable);
        }

        public void PurchaseConfirm(bool success)
        {
            if (success)
            {
                StartCoroutine(PurchasePrompt());
                GameManager.instance.PromptPurchase();
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
