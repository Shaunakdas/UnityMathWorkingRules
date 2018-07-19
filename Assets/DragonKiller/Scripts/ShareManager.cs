using UnityEngine;
using System.Collections;
using Rezero;
#if NATIVE_PLUGINS_LITE_VERSION
using VoxelBusters.NativePlugins;
#endif

namespace Rezero
{
    public class ShareManager : MonoBehaviour {

        public string Message;
        public string URL;

        void Start () {
        
        }
        
        void Update () {
        
        }

        public void Share()
        {
    #if NATIVE_PLUGINS_LITE_VERSION && USES_SHARING
                // Create share sheet
                ShareSheet _shareSheet = new ShareSheet();    
                _shareSheet.Text = Message;
                _shareSheet.URL = URL;

                // Show composer at last touch point
                NPBinding.UI.SetPopoverPointAtLastTouchPosition();
                NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
    #endif
        }

    #if NATIVE_PLUGINS_LITE_VERSION && USES_SHARING
        void FinishedSharing(eShareResult _result)
        {
            Debug.Log("Finished sharing");
            Debug.Log("Share Result = " + _result);
        }
    #endif
    }
}