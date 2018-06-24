using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALMS.ViewModels
{
    public class EditToolBar
    {
        private string clickEventName;
        public string ClickEventName
        {
            get
            {
                return string.IsNullOrWhiteSpace(clickEventName) ? "ToolBarClick" : clickEventName;
            }
            set
            {
                clickEventName = value;
            }
        }

        private bool showAdd = true;
        public bool ShowAdd
        {
            get
            {
                return showAdd;
            }
            set
            {
                showAdd = value;
            }
        }

        private bool showEdit = true;
        public bool ShowEdit
        {
            get
            {
                return showEdit;
            }
            set
            {
                showEdit = value;
            }
        }

        private bool showDelete = true;
        public bool ShowDelete
        {
            get
            {
                return showDelete;
            }
            set
            {
                showDelete = value;
            }
        }
    }
}

