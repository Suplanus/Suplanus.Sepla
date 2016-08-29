using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
   public class SearchUtility
   {
      public static void SearchAndReplaceText(Search search, string searchText, string replaceText, Project project)
      {
         // Init search
         search.ClearSearchDB(project);
         search.Project(project, searchText);

         // Get objects
         StorableObject[] foundObjects = search.GetAllSearchDBEntries(project);
         foreach (var foundObject in foundObjects)
         {
            // Filter only text objects
            // todo: EPLAN fix (2.6) T1085938
            var existingValues = foundObject.Properties.ExistingValues
               .Where(p => !p.Definition.IsInternal &&
               (p.Definition.Type == PropertyDefinition.PropertyType.MultilangString ||
               p.Definition.Type == PropertyDefinition.PropertyType.String)).ToList();
            List<PropertyValue> existingValuesWithoutEmpty = new List<PropertyValue>();
            foreach (var propertyValue in existingValues)
            {
               if (propertyValue.Definition.IsIndexed)
               {
                  foreach (int index in propertyValue.Indexes)
                  {
                     if (!propertyValue[index].IsEmpty)
                     {
                        existingValuesWithoutEmpty.Add(propertyValue[index]);
                     }
                  }
               }
               else
               {
                  if (!propertyValue.IsEmpty)
                  {
                     existingValuesWithoutEmpty.Add(propertyValue);
                  }
               }
            }

            existingValues.Clear(); // todo: needed?
            existingValues = existingValuesWithoutEmpty;

            // Replace
            foreach (PropertyValue propertyValue in existingValues)
            {
               switch (propertyValue.Definition.Type)
               {
                  // MultiLanguageString
                  case PropertyDefinition.PropertyType.MultilangString:
                     MultiLangString multiLangString = propertyValue;
                     var valueMultiLangString = multiLangString.GetAsString();
                     if (valueMultiLangString.Contains(searchText))
                     {
                        string newValue = valueMultiLangString.Replace(searchText, replaceText); // All languages
                        multiLangString.SetAsString(newValue);
                        propertyValue.Set(newValue);
                     }
                     break;

                  // String
                  case PropertyDefinition.PropertyType.String:
                     var value = propertyValue.ToString();
                     if (value.Contains(searchText))
                     {
                        string newValue = value.Replace(searchText, replaceText);
                        propertyValue.Set(newValue);
                     }
                     break;
               }

            }
         }
      }
   }
}
