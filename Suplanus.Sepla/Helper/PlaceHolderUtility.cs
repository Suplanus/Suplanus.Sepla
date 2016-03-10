using System.Collections.Generic;
using System.Linq;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;

namespace Suplanus.Sepla.Helper
{
	public class PlaceHolderUtility
	{
		/// <summary>
		/// Apply a record to placeholder by name
		/// </summary>
		/// <param name="placeHolders"></param>
		/// <param name="placeHolderName"></param>
		/// <param name="recordName"></param>
		/// <returns></returns>
		public bool ApplyRecord(PlaceHolder[] placeHolders, string placeHolderName, string recordName)
		{
			List<PlaceHolder> foundPlaceHolder = placeHolders
				.Where(placeHolder => placeHolder.Name.Equals(placeHolderName)) // name
				.Where(placeHolder => placeHolder.FindRecord(recordName) != -1) // record
				.ToList();

			using (Transaction transaction = new TransactionManager().CreateTransaction())
			{
				foreach (PlaceHolder placeHolder in foundPlaceHolder)
				{
					placeHolder.ApplyRecord(recordName, true); // apply (with page data)
					transaction.Commit(); // needed if not placed in project
				}
			}

			return foundPlaceHolder.Any(); // true == found | false == not found
		}

		/// <summary>
		/// Create a record to a placeHolder by name and apply a record
		/// </summary>
		/// <param name="placeHolders"></param>
		/// <param name="placeHolderName"></param>
		/// <param name="recordName"></param>
		/// <param name="variableName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool CreateRecordWithValueAndApply(PlaceHolder[] placeHolders, string placeHolderName, string recordName, string variableName, string value)
		{
			List<PlaceHolder> foundPlaceHolder = placeHolders
				.Where(placeHolder => placeHolder.Name.Equals(placeHolderName)) // name
				.ToList();

			foreach (PlaceHolder placeHolder in foundPlaceHolder)
			{
				placeHolder.AddRecord(recordName);
				placeHolder.set_Value(recordName, variableName, value);
				using (Transaction transaction = new TransactionManager().CreateTransaction())
				{
					placeHolder.ApplyRecord(recordName, true); // apply (with page data)
					transaction.Commit(); // needed if not placed in project
				}
			}

			return foundPlaceHolder.Any(); // true == found | false == not found
		}
	}
}
