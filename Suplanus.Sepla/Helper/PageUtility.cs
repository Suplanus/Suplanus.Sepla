using System;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
  public class PageUtility
  {
    public enum PageCounterAlphabetical
    {
      A = 0,
      B = 1,
      C = 2,
      D = 3,
      E = 4,
      F = 5,
      G = 6,
      H = 7,
      I = 8,
      J = 9,
      K = 10,
      L = 11,
      M = 12,
      N = 13,
      P = 14,
    }

    public static void OpenPageAndSyncInNavigator(Page page)
    {
      new Edit().OpenPageWithName(page.Project.ProjectLinkFilePath, page.IdentifyingName); // Open in GED
      new CommandLineInterpreter().Execute("XGedSelectPageAction"); // Select page
      new CommandLineInterpreter().Execute("XEsSyncPDDsAction"); // Sync selection
      new CommandLineInterpreter().Execute("XGedEscapeAction"); // Escape (page selection)
    }

    public static WindowMacro.Enums.RepresentationType GetMacroRepresentationTypeFromPage(Page page)
    {
      switch (page.PageType)
      {
        case DocumentTypeManager.DocumentType.CircuitFluid: return WindowMacro.Enums.RepresentationType.Fluid_MultiLine;
        case DocumentTypeManager.DocumentType.Circuit: return WindowMacro.Enums.RepresentationType.MultiLine;
        case DocumentTypeManager.DocumentType.CircuitSingleLine: return WindowMacro.Enums.RepresentationType.SingleLine;
        case DocumentTypeManager.DocumentType.Overview: return WindowMacro.Enums.RepresentationType.Overview;
        case DocumentTypeManager.DocumentType.Graphics: return WindowMacro.Enums.RepresentationType.Graphics;
        case DocumentTypeManager.DocumentType.ProcessAndInstrumentationDiagram: return WindowMacro.Enums.RepresentationType.PI_FlowChart;
        case DocumentTypeManager.DocumentType.PanelLayout: return WindowMacro.Enums.RepresentationType.ArticlePlacement;
        case DocumentTypeManager.DocumentType.Topology: return WindowMacro.Enums.RepresentationType.Cabling;
        case DocumentTypeManager.DocumentType.Planning: return WindowMacro.Enums.RepresentationType.Planning;
        default: return WindowMacro.Enums.RepresentationType.Default;
      }
    }

    public static DocumentTypeManager.DocumentType GetPageRepresentationTypeFromMacro(WindowMacro.Enums.RepresentationType representationType)
    {
      switch (representationType)
      {
        case WindowMacro.Enums.RepresentationType.Default: return DocumentTypeManager.DocumentType.Undefined;
        case WindowMacro.Enums.RepresentationType.Neutral: return DocumentTypeManager.DocumentType.Undefined;
        case WindowMacro.Enums.RepresentationType.MultiLine: return DocumentTypeManager.DocumentType.Circuit;
        case WindowMacro.Enums.RepresentationType.SingleLine: return DocumentTypeManager.DocumentType.CircuitSingleLine;
        case WindowMacro.Enums.RepresentationType.PairCrossReference: return DocumentTypeManager.DocumentType.PairCrossReference;
        case WindowMacro.Enums.RepresentationType.Overview: return DocumentTypeManager.DocumentType.Overview;
        case WindowMacro.Enums.RepresentationType.Graphics: return DocumentTypeManager.DocumentType.Graphics;
        case WindowMacro.Enums.RepresentationType.ArticlePlacement: return DocumentTypeManager.DocumentType.PanelLayout;
        case WindowMacro.Enums.RepresentationType.PI_FlowChart: return DocumentTypeManager.DocumentType.ProcessAndInstrumentationDiagram;
        case WindowMacro.Enums.RepresentationType.Fluid_MultiLine: return DocumentTypeManager.DocumentType.CircuitFluid;
        case WindowMacro.Enums.RepresentationType.Cabling: return DocumentTypeManager.DocumentType.Topology;
        case WindowMacro.Enums.RepresentationType.ArticlePlacement3D: return DocumentTypeManager.DocumentType.ModelView;
        case WindowMacro.Enums.RepresentationType.Functional: return DocumentTypeManager.DocumentType.Functional;
        case WindowMacro.Enums.RepresentationType.Planning: return DocumentTypeManager.DocumentType.Planning;
        default: throw new ArgumentOutOfRangeException();
      }
    }

    public static PointD[] GetLogicalAreaofPage(Page page)
    {
      PlotFrame frame = page.PlotFrame;
      PointD ptSize = frame.Size;
      int xStart = frame.Properties.FRAME_EVALUATION_AREA_START_POINT_X;
      double xEnd = xStart + ptSize.X - xStart;
      int yStart = frame.Properties.FRAME_EVALUATION_AREA_START_POINT_Y;
      double yEnd = yStart + ptSize.Y - yStart - 5; // 5mm fix of plotframe

      PointD lowerLeft = new PointD(xStart, yStart);
      PointD upperRight = new PointD(xEnd, yEnd);
      return new[] { lowerLeft, upperRight };
    }
  }
}