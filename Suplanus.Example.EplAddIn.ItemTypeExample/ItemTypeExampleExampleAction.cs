using System;
using System.Linq;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplSDK.WPF.EEvent;

namespace Suplanus.Example.EplAddIn.ItemTypeExample
{
    class ItemTypeExampleExampleAction : IEplAction
    {
        public static string ItemType = null;

        public void GetActionProperties(ref ActionProperties actionProperties) { }

        public bool OnRegister(ref string name, ref int ordinal)
        {
            name = nameof(ItemTypeExampleExampleAction);
            return true;
        }

        public bool Execute(ActionCallingContext actionCallingContext)
        {
            // Sent events to WPF control from base action
            string itemType = string.Empty;
            string action = string.Empty;
            string key = string.Empty;
            actionCallingContext.GetParameter("itemtype", ref itemType);
            actionCallingContext.GetParameter("action", ref action);
            actionCallingContext.GetParameter("key", ref key);

            // Check specific itemType
            if (itemType != ItemType)
            {
                return true;
            }

            WPFDialogEventManager wpfDialogEventManager = new WPFDialogEventManager();

            switch (action)
            {
                case "SelectItem":
                    wpfDialogEventManager.send("XPartsManagementDialog", action, key);
                    break;

                case "SaveItem":
                    wpfDialogEventManager.send("XPartsManagementDialog", "SaveItem", key);
                    break;

                case "GetRootLevel":
                    Data.Load();
                    actionCallingContext.AddParameter("text", ItemType);
                    actionCallingContext.AddParameter("key", "0");
                    break;

                case "GetNextLevel":

                    string keys = string.Empty;
                    string texts = string.Empty;
                    foreach (var itemTypeObject in Data.ItemTypeObjects.OrderBy(obj=>obj.Text))
                    {
                        if (keys != string.Empty)
                        {
                            keys += "\n";
                        }
                        if (texts != string.Empty)
                        {
                            texts += "\n";
                        }
                        keys += itemTypeObject.Key;
                        texts += itemTypeObject.Text;
                    }
                    actionCallingContext.AddParameter("key", keys);
                    actionCallingContext.AddParameter("text", texts);
                    break;

                case "PreShowTab":
                    if (string.IsNullOrEmpty(key) || key == "0") // Dont show on root level
                    {
                        actionCallingContext.AddParameter("show", "0");
                    }
                    else
                    {
                        actionCallingContext.AddParameter("show", "1");
                    }
                    break;

                case "DeleteItem":
                    ItemTypeObject itemTypObjectToRemove = Data.ItemTypeObjects.First(obj => obj.Key.Equals(key));
                    Data.ItemTypeObjects.Remove(itemTypObjectToRemove);
                    Data.Save();
                    break;

                case "CopyItem":
                    string newKeyCopy = Guid.NewGuid().ToString();
                    string sourceKey = string.Empty;
                    actionCallingContext.GetParameter("sourcekey", ref sourceKey);
                    ItemTypeObject sourceItemTypeObject = Data.ItemTypeObjects.First(obj => obj.Key.Equals(sourceKey));

                    ItemTypeObject newItemTypeObjectCopy = new ItemTypeObject()
                    {
                        Key = newKeyCopy,
                        Text = sourceItemTypeObject.Text,
                    };
                    Data.ItemTypeObjects.Add(newItemTypeObjectCopy);
                    key = newKeyCopy;
                    actionCallingContext.AddParameter("key", key);
                    Data.Save();
                    break;

                case "NewItem":
                    string newKey = Guid.NewGuid().ToString();
                    ItemTypeObject newItemTypeObject = new ItemTypeObject()
                    {
                        Key = newKey,
                        Text = "New item"
                    };
                    Data.ItemTypeObjects.Add(newItemTypeObject);
                    key = newKey;
                    actionCallingContext.AddParameter("key", key);
                    Data.Save();
                    break;
            }

            return true;
        }
    }
}
