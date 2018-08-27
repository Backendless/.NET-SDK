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
    [Register ("SetupViewController")]
    partial class SetupViewController
    {
        [Outlet]
        UIKit.UIButton ContinueButton { get; set; }


        [Outlet]
        UIKit.UITextField DataPartitionPhraseEntry { get; set; }


        [Outlet]
        UIKit.UILabel InstructionsLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ContinueButton != null) {
                ContinueButton.Dispose ();
                ContinueButton = null;
            }

            if (DataPartitionPhraseEntry != null) {
                DataPartitionPhraseEntry.Dispose ();
                DataPartitionPhraseEntry = null;
            }

            if (InstructionsLabel != null) {
                InstructionsLabel.Dispose ();
                InstructionsLabel = null;
            }
        }
    }
}