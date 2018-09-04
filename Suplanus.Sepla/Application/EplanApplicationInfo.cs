using System;
using System.Diagnostics;
using System.IO;
using Eplan.EplApi.Base;

namespace Suplanus.Sepla.Application
{
  public class EplanApplicationInfo
  {
    public static int GetActiveEplanVersion()
    {
      string eplanVersion = "0"; //default value = 0 to ensure, that EPLAN-version is correctly recognized 

      //try new variable $(EPLAN_VERSION) first, if not valid, no possibility to get active get active EPLAN-version
      if (PathMap.SubstitutePath("$(EPLAN_VERSION)") != "$(EPLAN_VERSION)")
      {
        eplanVersion = PathMap.SubstitutePath("$(EPLAN_VERSION)");
      }
      else
      {
        //try different method to get version of executing eplan, in case the actual version doesn't support $(EPLAN_VERSION)
        string dllFilename = Path.Combine(PathMap.SubstitutePath("$(BIN)"), "Eplan.EplApi.Baseu.dll");
        FileInfo fileInfo = new FileInfo(dllFilename);
        if (fileInfo.Exists)
        {
          var versionInfo = FileVersionInfo.GetVersionInfo(dllFilename);
          //return main-version-infos (without build number)
          if (versionInfo.ProductVersion.Length >= 5)
          {
            eplanVersion = versionInfo.ProductVersion.Substring(0, 5);
          }
        }
      }

      if (eplanVersion == "0" || eplanVersion == "$(EPLAN_VERSION)")
      {
        MultiLangString multiLangErrorText = new MultiLangString();
        multiLangErrorText.AddString(ISOCode.Language.L_de_DE, "Die aktuelle EPLAN-Version konnte nicht ermittelt werden.");
        multiLangErrorText.AddString(ISOCode.Language.L_en_US, "Unable to get actual EPLAN-version.");
        ISOCode.Language guiLanguage = new Languages().GuiLanguage.GetNumber();
        string errorText = multiLangErrorText.GetStringToDisplay(guiLanguage);
        if (String.IsNullOrEmpty(errorText))
        {
          //if actual GUI-language is not defined in multi-language-string, use en_US-text-version
          errorText = multiLangErrorText.GetStringToDisplay(ISOCode.Language.L_en_US);
        }
        new BaseException(errorText, MessageLevel.Warning).FixMessage(); 
        eplanVersion = "0";
      }
      return Convert.ToInt16(eplanVersion.Replace(".", string.Empty));
    }
  }
}
