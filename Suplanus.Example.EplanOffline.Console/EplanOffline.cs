using Eplan.EplApi.System;

namespace Suplanus.Example.EplanOffline.Console
{
  /// <summary>
  ///   EplanOffline class to use EPLAN Offline Application
  /// </summary>
  public class EplanOffline
  {
    /// <summary>
    ///   Returns if the EPLAN-Application is running
    /// </summary>
    public bool IsRunning
    {
      get
      {
        var running = Application != null;
        return running;
      }
    }

    /// <summary>
    ///   EPLAN Application
    /// </summary>
    public EplApplication Application;

    /// <summary>
    ///   EPLAN Bin path e.g.: C:\Program Files\EPLAN\Platform\2.6.3\Bin
    /// </summary>
    public string BinPath;

    /// <summary>
    ///   LicenseFile to start with given package
    /// </summary>
    public string LicenseFile;

    /// <summary>
    ///   SystemConfiguration with environment paths
    /// </summary>
    public string SystemConfiguration;

    /// <summary>
    ///   Init EPLAN with given bin path and license file (optional)
    /// </summary>
    /// <param name="binPath"></param>
    /// <param name="systemConfiguration"></param>
    /// <param name="licenseFile"></param>
    public EplanOffline(string binPath, string systemConfiguration, string licenseFile = null)
    {
      BinPath = binPath;
      SystemConfiguration = systemConfiguration;
      LicenseFile = licenseFile;
    }

    /// <summary>
    ///   Starts EPLAN in ConsoleApplication
    ///   You have to set Attribute <c>[STAThread]</c> for Main void
    /// </summary>
    public void StartWithoutGui()
    {
      Start();
    }

    /// <summary>
    ///   Starts the application without Gui
    /// </summary>
    private void Start()
    {
      if (!IsRunning)
      {
        try
        {
          EplApplication eplApplication = new EplApplication();
          eplApplication.EplanBinFolder = BinPath;
          if (!string.IsNullOrEmpty(SystemConfiguration))
          {
            eplApplication.SystemConfiguration = SystemConfiguration;
          }
          if (!string.IsNullOrEmpty(LicenseFile))
          {
            eplApplication.LicenseFile = LicenseFile; // Set specific license
          }
          eplApplication.QuietMode = EplApplication.QuietModes.ShowAllDialogs;
          eplApplication.Init("", true, true);
          eplApplication.QuietMode = EplApplication.QuietModes.ShowNoDialogs;
          Application = eplApplication;
        }
        catch
        {
          Application = null;
        }
      }
    }

    /// <summary>
    ///   Release all objects
    ///   <note type="caution">
    ///     Needed for eplan runtime exceptions, there is a known issue (T1094381), EPLAN says Microsoft should fix this
    ///     problem
    ///     Workaround: Enable native code debugging in visual studio
    ///   </note>
    /// </summary>
    public void Close()
    {
      // T1094381: There is a known problem with console applications, that visual studio not quit the debugging session, workaround: enable native code debugging in project
      if (Application != null)
      {
        try
        {
          Application.Exit();
        }
        finally
        {
          Application = null;
        }
      }
    }
  }
}