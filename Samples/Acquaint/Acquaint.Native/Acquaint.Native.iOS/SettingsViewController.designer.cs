// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Acquaint.Native.iOS
{
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        UIKit.UITextField BackendUrlEntry { get; set; }


        [Outlet]
        UIKit.UISwitch ClearImageCacheSwitch { get; set; }


        [Outlet]
        UIKit.UITextField DataPartitionPhraseEntry { get; set; }


        [Outlet]
        UIKit.UITextField ImageCacheDurationEntry { get; set; }


        [Outlet]
        UIKit.UISwitch ResetToDefaultsSwitch { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackendUrlEntry != null) {
                BackendUrlEntry.Dispose ();
                BackendUrlEntry = null;
            }

            if (ClearImageCacheSwitch != null) {
                ClearImageCacheSwitch.Dispose ();
                ClearImageCacheSwitch = null;
            }

            if (DataPartitionPhraseEntry != null) {
                DataPartitionPhraseEntry.Dispose ();
                DataPartitionPhraseEntry = null;
            }

            if (ImageCacheDurationEntry != null) {
                ImageCacheDurationEntry.Dispose ();
                ImageCacheDurationEntry = null;
            }

            if (ResetToDefaultsSwitch != null) {
                ResetToDefaultsSwitch.Dispose ();
                ResetToDefaultsSwitch = null;
            }
        }
    }
}