﻿using System.Collections.Generic;
using WebsitePanel.WebDavPortal.Models.Common;

namespace WebsitePanel.WebDavPortal.Models.FileSystem
{
    public class DeleteFilesModel : AjaxModel
    {
        public DeleteFilesModel()
        {
            DeletedFiles = new List<string>();
        }

        public List<string> DeletedFiles { get; set; }
    }
}