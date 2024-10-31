using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BotanGameServices.AdsPackage.Local
{
    public class ConsentPopup : MonoBehaviour
    {
        [SerializeField] Text popupText;
        UnityAction consentPopupClosed;

        public void Initialize(string popupText, UnityAction consentPopupClosed)
        {
            this.popupText.text = popupText;
            this.consentPopupClosed = consentPopupClosed;
        }


        public void Accept()
        {
            AdsPackage.API.SetGDPRConsent(true);
            Close();
        }

        public void Reject()
        {
            AdsPackage.API.SetGDPRConsent(false);
            Close();
        }


        public void Close()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            consentPopupClosed?.Invoke();
        }
    }
}