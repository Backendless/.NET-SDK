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
    [Register ("AcquaintanceCell")]
    partial class AcquaintanceCell
    {
        [Outlet]
        UIKit.UILabel CompanyLabel { get; set; }


        [Outlet]
        UIKit.UILabel JobTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel NameLabel { get; set; }


        [Outlet]
        UIKit.UIImageView ProfilePhotoImageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CompanyLabel != null) {
                CompanyLabel.Dispose ();
                CompanyLabel = null;
            }

            if (JobTitleLabel != null) {
                JobTitleLabel.Dispose ();
                JobTitleLabel = null;
            }

            if (NameLabel != null) {
                NameLabel.Dispose ();
                NameLabel = null;
            }

            if (ProfilePhotoImageView != null) {
                ProfilePhotoImageView.Dispose ();
                ProfilePhotoImageView = null;
            }
        }
    }
}