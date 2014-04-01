// Copyright (c) 2014, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using WebsitePanel.Providers.SharePoint;

namespace WebsitePanel.Portal
{
    public partial class HostedSharePointStorageUsage : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindGrid();
        }

        protected void btnRecalculateDiscSpace_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            int errorCode;
            try
            {
                SharePointSiteDiskSpace[] sharePointSiteDiskSpace =
                    ES.Services.HostedSharePointServers.CalculateSharePointSitesDiskSpace(PanelRequest.ItemID,
                                                                                          out errorCode);


                if (errorCode < 0)
                {
                    messageBox.ShowResultMessage(errorCode);
                    return;
                }
                
                if (sharePointSiteDiskSpace != null && sharePointSiteDiskSpace.Length == 1 && string.IsNullOrEmpty(sharePointSiteDiskSpace[0].Url))
                {
                    gvStorageUsage.DataSource = null;
                    gvStorageUsage.DataBind();
                    lblTotalItems.Text = "0";
                    lblTotalSize.Text = "0";
                    return;
                }
                
                gvStorageUsage.DataSource = sharePointSiteDiskSpace;
                gvStorageUsage.DataBind();

                if (sharePointSiteDiskSpace != null)
                {
                    lblTotalItems.Text = sharePointSiteDiskSpace.Length.ToString();

                    long total = 0;
                    foreach (SharePointSiteDiskSpace current in sharePointSiteDiskSpace)
                    {
                        total += current.DiskSpace;
                    }

                    lblTotalSize.Text = total.ToString();
                }
            }
            catch(Exception ex)
            {
                messageBox.ShowErrorMessage("HOSTED_SHAREPOINT_RECALCULATE_SIZE", ex);   
            }
        }
    }
}