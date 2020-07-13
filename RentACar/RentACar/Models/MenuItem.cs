using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RentACar.Models
{
    public class MenuItem
    {
        public MenuItem()
        {

        }

        public MenuItem(Type viewModelType)
        {
            TypeName = Type.AssemblyQualifiedName;
        }

        public int MenuItemId { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; } // [HtmlId]
        public MenuItem ParentMenu { get; set; }
        public int MenuItemType { get; set; }

        public string Url { get; set; }
        public int PageId { get; set; }

        public string Title { get; set; }
        public string TitleKey { get; set; }
        public string TooltipKey { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public int SortOrder { get; set; }
        public int ApplicationTypeId { get; set; }
        public string AuthorizationRule { get; set; }
        public Type Type { get; set; }
        public string TypeName { get; set; }

        public object Data { get; set; }
        public Action Action { get; set; }
        public object Icon { get; set; }
        public string Parametar { get; set; }

        public Command Command { get; set; }
    }
    
}
