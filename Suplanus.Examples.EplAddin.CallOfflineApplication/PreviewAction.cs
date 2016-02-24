using Eplan.EplApi.ApplicationFramework;

namespace Suplanus.Examples.EplAddin.Preview
{
	class PreviewAction : IEplAction
	{
		public bool OnRegister(ref string name, ref int ordinal)
		{
			name = "PreviewAction";
			ordinal = 20;
			return true;
		}

		public bool Execute(ActionCallingContext oActionCallingContext)
		{
			Preview.MainWindow mainWindow = new Preview.MainWindow();
			mainWindow.ShowDialog();
			return true;
		}

		public void GetActionProperties(ref ActionProperties actionProperties)
		{

		}
	}
}
