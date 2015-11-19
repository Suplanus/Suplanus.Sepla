using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;

namespace Suplanus.Sepla.Objects
{
    public class MacroSepla
    {
        public MacroSepla(Project currentProject)
        {
            // Variant
            VariantsCombinations = new ObservableCollection<VariantCombination>();

            // Type
            //string extension = System.IO.Path.GetExtension(item.Path);
            //switch (extension)
            //{
            //    case ".ems":
            //        Type = MacroType.Symbol;
            //        SymbolMacro symbolMacro = new SymbolMacro();
            //        symbolMacro.Open(item.Path, currentProject);
            //        Object = symbolMacro;

            //        GetVariantCombinations(symbolMacro);

            //        break;

            //    case ".ema":
            //        Type = MacroType.Window;
            //        WindowMacro windowMacro = new WindowMacro();
            //        windowMacro.Open(item.Path, currentProject);
            //        Object = windowMacro;

            //        GetVariantCombinations(windowMacro);

            //        break;

            //    case ".emp":
            //        Type = MacroType.Page;
            //        PageMacro pageMacro = new PageMacro();
            //        pageMacro.Open(item.Path, currentProject);
            //        Object = pageMacro;
            //        GetVariantCombinations(pageMacro);
            //        break;

            //    default:
            //        Type = MacroType.None;
            //        break;
            //}

            //CurrentVariantCombination = 0;
            //Path = item.Path;
            //Name = item.Header;
        }

        private void GetVariantCombinations(PageMacro pageMacro)
        {

            //pageMacro.ChangeCurrentVariant(representationType, index);
            VariantCombination variantCombination = new VariantCombination();
            variantCombination.Description =
                CleanUp.DescriptionFromHeaderChars(pageMacro.Description.GetAsString());
            variantCombination.Index = 0;
            variantCombination.RepresentationType = WindowMacro.Enums.RepresentationType.Default;
            VariantsCombinations.Add(variantCombination);


        }

        private void GetVariantCombinations(WindowMacro windowMacro)
        {
            foreach (var representationType in windowMacro.RepresentationTypes)
            {
                foreach (int index in windowMacro.GetVariants(representationType).ToList())
                {
                    windowMacro.ChangeCurrentVariant(representationType, index);
                    VariantCombination variantCombination = new VariantCombination();
                    variantCombination.Description =
                        CleanUp.DescriptionFromHeaderChars(windowMacro.Description.GetAsString());
                    variantCombination.Index = index;
                    variantCombination.RepresentationType = representationType;
                    VariantsCombinations.Add(variantCombination);
                }
            }
        }

        private void GetVariantCombinations(SymbolMacro symbolMacro)
        {
            foreach (var representationType in symbolMacro.RepresentationTypes)
            {
                foreach (int index in symbolMacro.GetVariants(representationType).ToList())
                {
                    symbolMacro.ChangeCurrentVariant(representationType, index);
                    VariantCombination variantCombination = new VariantCombination();
                    variantCombination.Description =
                        CleanUp.DescriptionFromHeaderChars(symbolMacro.Description.GetAsString());
                    variantCombination.Index = index;
                    variantCombination.RepresentationType = representationType;
                    VariantsCombinations.Add(variantCombination);
                }
            }
        }

        public ObservableCollection<VariantCombination> VariantsCombinations { get; set; }

        public ObservableCollection<ListBoxPropertiesItem> TextPlaceHolder { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public object Object { get; set; }

        public MacroType Type { get; set; }


        private int _currentVariantCombination;
        public int CurrentVariantCombination
        {
            get { return _currentVariantCombination; }
            set
            {
                if (value != _currentVariantCombination)
                {
                    _currentVariantCombination = value;
                }
            }
        }

        public struct VariantCombination
        {
            public WindowMacro.Enums.RepresentationType RepresentationType { get; set; }
            public int Index { get; set; }
            public string Description { get; set; }
        }

        public enum MacroType
        {
            Page,
            Window,
            Symbol,
            None
        }
    }

    class CleanUp
    {
        public static string DescriptionFromHeaderChars(string description)
        {
            string[] slDescriptions = description.Split(';');
            foreach (string s in slDescriptions)
            {
                if (s.StartsWith("??_??@"))
                {
                    description = s;
                    break;
                }
                if (s.StartsWith("de_DE@"))
                {
                    description = s;
                    break;
                }
                if (!s.Equals(""))
                {
                    description = s;
                }
            }

            // Sprachenkenner entfernen
            if (description.Length >= 6)
            {
                description = description.Remove(0, 6);
            }
            return description;
        }
    }






}
