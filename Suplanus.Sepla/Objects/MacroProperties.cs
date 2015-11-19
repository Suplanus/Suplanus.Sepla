using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;
using Eplan.EplApi.DataModel.MasterData;

namespace Suplanus.Sepla.Objects
{
    public class MacroProperties
    {
        #region GetProperties
        public static void GetProperties(WindowMacro macro, List<ListBoxPropertiesItem> listBoxPropertiesItems)
        {
            foreach (Placement placement in macro.Objects)
            {
                GetValue(listBoxPropertiesItems, placement);
            }
        }

        public static void GetProperties(PageMacro macro, List<ListBoxPropertiesItem> listBoxPropertiesItems)
        {
            foreach (Page page in macro.Pages)
            {
                MultiLangString multiLangString = page.Properties.PAGE_NOMINATIOMN.ToMultiLangString();
                MatchCollection matches =
                    Regex.Matches(multiLangString.GetStringToDisplay(ISOCode.Language.L_de_DE),
                        "<#(.*?)#>");
                foreach (Match match in matches)
                {
                    var newItem = new ListBoxPropertiesItem
                    {
                        Description = match.Value.Replace("<#", "").Replace("#>", "")
                    };
                    listBoxPropertiesItems.Add(newItem);
                }

                foreach (Placement placement in page.AllPlacements)
                {
                    GetValue(listBoxPropertiesItems, placement);
                }

            }
        }

        public static void GetProperties(SymbolMacro macro, List<ListBoxPropertiesItem> listBoxPropertiesItems)
        {
            foreach (Placement placement in macro.Objects)
            {
                GetValue(listBoxPropertiesItems, placement);
            }
        }

        private static void GetValue(List<ListBoxPropertiesItem> listBoxPropertiesItems, Placement placement)
        {
            if (placement is Text)
            {
                MultiLangString multiLangString = ((Text) placement).Contents;
                MatchCollection matches =
                    Regex.Matches(multiLangString.GetStringToDisplay(ISOCode.Language.L_de_DE),
                        "<#(.*?)#>");
                foreach (Match match in matches)
                {
                    var newItem = new ListBoxPropertiesItem
                    {
                        Description = match.Value.Replace("<#", "").Replace("#>", "")
                    };
                    listBoxPropertiesItems.Add(newItem);
                }
            }
        }

        #endregion

        #region SetProperties
        public static void SetProperties(WindowMacro macro, List<ListBoxPropertiesItem> listBoxPropertiesItems)
        {
            foreach (Placement placement in macro.Objects)
            {
                SetValue(listBoxPropertiesItems, placement);
            }
        }

        private static void SetValue(List<ListBoxPropertiesItem> listBoxPropertiesItems, Placement placement)
        {
            if (placement is Text)
            {
                MultiLangString multiLangString = ((Text) placement).Contents;
                MatchCollection matches =
                    Regex.Matches(multiLangString.GetStringToDisplay(ISOCode.Language.L_de_DE),
                        "<#(.*?)#>");
                foreach (Match match in matches)
                {
                    foreach (var listBoxPropertiesItem in listBoxPropertiesItems)
                    {
                        string value = match.Value.Replace("<#", "").Replace("#>", "");
                        if (listBoxPropertiesItem.Description.Equals(value))
                        {
                            if (!string.IsNullOrEmpty(listBoxPropertiesItem.Value))
                            {
                                SetProperty(listBoxPropertiesItem.Value, placement);
                            }
                        }
                    }
                }
            }
        }

        public static SymbolMacro SetProperties(SymbolMacro macro, List<ListBoxPropertiesItem> listBoxPropertiesItems)
        {
            SymbolMacro newMacro = macro;
            foreach (Placement placement in newMacro.Objects)
            {
                SetValue(listBoxPropertiesItems, placement);
            }
            return newMacro;
        }

        public static void SetProperties(PageMacro macro, List<ListBoxPropertiesItem> listBoxPropertiesItems)
        {
            foreach (var page in macro.Pages)
            {
                MultiLangString multiLangString = page.Properties.PAGE_NOMINATIOMN.ToMultiLangString();
                MatchCollection matches =
                    Regex.Matches(multiLangString.GetStringToDisplay(ISOCode.Language.L_de_DE),
                        "<#(.*?)#>");
                foreach (Match match in matches)
                {
                    foreach (var listBoxPropertiesItem in listBoxPropertiesItems)
                    {
                        string value = match.Value.Replace("<#", "").Replace("#>", "");
                        if (listBoxPropertiesItem.Description.Equals(value))
                        {
                            if (!string.IsNullOrEmpty(listBoxPropertiesItem.Value))
                            {
                                SetProperty(listBoxPropertiesItem.Value, page.Properties.PAGE_NOMINATIOMN);
                            }
                        }
                    }

                }

                foreach (var placement in page.AllPlacements)
                {
                    SetValue(listBoxPropertiesItems, placement);
                }
            }

        }

        private static void SetProperty(string value, PropertyValue property)
        {
            MultiLangString newMultiLangString = new MultiLangString();
            newMultiLangString.AddString(ISOCode.Language.L_de_DE, value);
            property.Set(newMultiLangString);
        }

        private static void SetProperty(string value, Placement placement)
        {
            MultiLangString newMultiLangString = new MultiLangString();
            newMultiLangString.AddString(ISOCode.Language.L_de_DE, value);
            ((Text)placement).Contents = newMultiLangString;
        }


        #endregion

        public static void GetPlaceholder(IEnumerable<PlaceHolder> placeHolders, List<ListBoxPlaceholderItem> listBoxPlaceholderItems)
        {
            listBoxPlaceholderItems.AddRange(placeHolders.Select(placeHolder => new ListBoxPlaceholderItem(placeHolder)));
        }

        public static void SetPlaceholder(IEnumerable<PlaceHolder> placeHolders, List<ListBoxPlaceholderItem> listBoxPlaceholderItems)
        {
            foreach (var placeHolderMacro in placeHolders)
            {
                foreach (var listBoxPlaceholderItem in listBoxPlaceholderItems)
                {
                    if (placeHolderMacro.Equals(listBoxPlaceholderItem.PlaceHolderObject))
                    {
                        placeHolderMacro.ApplyRecord(listBoxPlaceholderItem.SelectedRecord);
                    }
                }
            }
        }


    }


}
