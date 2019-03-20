using DocumentFormat.OpenXml.Packaging;
using Ap = DocumentFormat.OpenXml.ExtendedProperties;
using Vt = DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Wp = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using Pic = DocumentFormat.OpenXml.Drawing.Pictures;
using A14 = DocumentFormat.OpenXml.Office2010.Drawing;
using Wps = DocumentFormat.OpenXml.Office2010.Word.DrawingShape;
using V = DocumentFormat.OpenXml.Vml;
using Ovml = DocumentFormat.OpenXml.Vml.Office;
using M = DocumentFormat.OpenXml.Math;
using Ds = DocumentFormat.OpenXml.CustomXmlDataProperties;
using System;

namespace Wiki.Areas.PZ.Models
{
    public class GeneratedTablesOreder
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        int[] Id;
        int count;

        public GeneratedTablesOreder(int[] Id)
        {
            this.Id = Id;
        }
        // Creates a WordprocessingDocument.
        public void CreatePackage(string filePath, int count)
        {
            this.count = count;
            using (WordprocessingDocument package = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                CreateParts(package);
            }
        }

        // Adds child parts and generates content of the specified part.
        private void CreateParts(WordprocessingDocument document)
        {
            ExtendedFilePropertiesPart extendedFilePropertiesPart1 = document.AddNewPart<ExtendedFilePropertiesPart>("rId3");
            GenerateExtendedFilePropertiesPart1Content(extendedFilePropertiesPart1);

            MainDocumentPart mainDocumentPart1 = document.AddMainDocumentPart();
            GenerateMainDocumentPart1Content(mainDocumentPart1);

            ImagePart imagePart1 = mainDocumentPart1.AddNewPart<ImagePart>("image/png", "rId8");
            GenerateImagePart1Content(imagePart1);

            DocumentSettingsPart documentSettingsPart1 = mainDocumentPart1.AddNewPart<DocumentSettingsPart>("rId3");
            GenerateDocumentSettingsPart1Content(documentSettingsPart1);

            documentSettingsPart1.AddExternalRelationship("http://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate", new System.Uri("file:///C:\\Users\\myi\\source\\repos\\Portal\\Wiki\\Areas\\PZ\\Contant\\dotx\\Табличка_заказ.dotx", System.UriKind.Absolute), "rId1");
            ImagePart imagePart2 = mainDocumentPart1.AddNewPart<ImagePart>("image/png", "rId7");
            GenerateImagePart2Content(imagePart2);

            StyleDefinitionsPart styleDefinitionsPart1 = mainDocumentPart1.AddNewPart<StyleDefinitionsPart>("rId2");
            GenerateStyleDefinitionsPart1Content(styleDefinitionsPart1);

            CustomXmlPart customXmlPart1 = mainDocumentPart1.AddNewPart<CustomXmlPart>("application/xml", "rId1");
            GenerateCustomXmlPart1Content(customXmlPart1);

            CustomXmlPropertiesPart customXmlPropertiesPart1 = customXmlPart1.AddNewPart<CustomXmlPropertiesPart>("rId1");
            GenerateCustomXmlPropertiesPart1Content(customXmlPropertiesPart1);

            ImagePart imagePart3 = mainDocumentPart1.AddNewPart<ImagePart>("image/png", "rId6");
            GenerateImagePart3Content(imagePart3);

            ImagePart imagePart4 = mainDocumentPart1.AddNewPart<ImagePart>("image/png", "rId5");
            GenerateImagePart4Content(imagePart4);

            ThemePart themePart1 = mainDocumentPart1.AddNewPart<ThemePart>("rId10");
            GenerateThemePart1Content(themePart1);

            WebSettingsPart webSettingsPart1 = mainDocumentPart1.AddNewPart<WebSettingsPart>("rId4");
            GenerateWebSettingsPart1Content(webSettingsPart1);

            FontTablePart fontTablePart1 = mainDocumentPart1.AddNewPart<FontTablePart>("rId9");
            GenerateFontTablePart1Content(fontTablePart1);

            SetPackageProperties(document);
        }

        // Generates content of extendedFilePropertiesPart1.
        private void GenerateExtendedFilePropertiesPart1Content(ExtendedFilePropertiesPart extendedFilePropertiesPart1)
        {
            Ap.Properties properties1 = new Ap.Properties();
            properties1.AddNamespaceDeclaration("vt", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
            Ap.Template template1 = new Ap.Template();
            template1.Text = "Табличка_заказ.dotx";
            Ap.TotalTime totalTime1 = new Ap.TotalTime();
            totalTime1.Text = "1";
            Ap.Pages pages1 = new Ap.Pages();
            pages1.Text = "1";
            Ap.Words words1 = new Ap.Words();
            words1.Text = "48";
            Ap.Characters characters1 = new Ap.Characters();
            characters1.Text = "277";
            Ap.Application application1 = new Ap.Application();
            application1.Text = "Microsoft Office Word";
            Ap.DocumentSecurity documentSecurity1 = new Ap.DocumentSecurity();
            documentSecurity1.Text = "0";
            Ap.Lines lines1 = new Ap.Lines();
            lines1.Text = "2";
            Ap.Paragraphs paragraphs1 = new Ap.Paragraphs();
            paragraphs1.Text = "1";
            Ap.ScaleCrop scaleCrop1 = new Ap.ScaleCrop();
            scaleCrop1.Text = "false";

            Ap.HeadingPairs headingPairs1 = new Ap.HeadingPairs();

            Vt.VTVector vTVector1 = new Vt.VTVector() { BaseType = Vt.VectorBaseValues.Variant, Size = (UInt32Value)2U };

            Vt.Variant variant1 = new Vt.Variant();
            Vt.VTLPSTR vTLPSTR1 = new Vt.VTLPSTR();
            vTLPSTR1.Text = "Название";

            variant1.Append(vTLPSTR1);

            Vt.Variant variant2 = new Vt.Variant();
            Vt.VTInt32 vTInt321 = new Vt.VTInt32();
            vTInt321.Text = "1";

            variant2.Append(vTInt321);

            vTVector1.Append(variant1);
            vTVector1.Append(variant2);

            headingPairs1.Append(vTVector1);

            Ap.TitlesOfParts titlesOfParts1 = new Ap.TitlesOfParts();

            Vt.VTVector vTVector2 = new Vt.VTVector() { BaseType = Vt.VectorBaseValues.Lpstr, Size = (UInt32Value)1U };
            Vt.VTLPSTR vTLPSTR2 = new Vt.VTLPSTR();
            vTLPSTR2.Text = "";

            vTVector2.Append(vTLPSTR2);

            titlesOfParts1.Append(vTVector2);
            Ap.Company company1 = new Ap.Company();
            company1.Text = "SPecialiST RePack";
            Ap.LinksUpToDate linksUpToDate1 = new Ap.LinksUpToDate();
            linksUpToDate1.Text = "false";
            Ap.CharactersWithSpaces charactersWithSpaces1 = new Ap.CharactersWithSpaces();
            charactersWithSpaces1.Text = "324";
            Ap.SharedDocument sharedDocument1 = new Ap.SharedDocument();
            sharedDocument1.Text = "false";
            Ap.HyperlinksChanged hyperlinksChanged1 = new Ap.HyperlinksChanged();
            hyperlinksChanged1.Text = "false";
            Ap.ApplicationVersion applicationVersion1 = new Ap.ApplicationVersion();
            applicationVersion1.Text = "15.0000";

            properties1.Append(template1);
            properties1.Append(totalTime1);
            properties1.Append(pages1);
            properties1.Append(words1);
            properties1.Append(characters1);
            properties1.Append(application1);
            properties1.Append(documentSecurity1);
            properties1.Append(lines1);
            properties1.Append(paragraphs1);
            properties1.Append(scaleCrop1);
            properties1.Append(headingPairs1);
            properties1.Append(titlesOfParts1);
            properties1.Append(company1);
            properties1.Append(linksUpToDate1);
            properties1.Append(charactersWithSpaces1);
            properties1.Append(sharedDocument1);
            properties1.Append(hyperlinksChanged1);
            properties1.Append(applicationVersion1);

            extendedFilePropertiesPart1.Properties = properties1;
        }

        // Generates content of mainDocumentPart1.
        private void GenerateMainDocumentPart1Content(MainDocumentPart mainDocumentPart1)
        {
            Document document1 = new Document() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15 wp14" } };
            document1.AddNamespaceDeclaration("wpc", "http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas");
            document1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            document1.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
            document1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            document1.AddNamespaceDeclaration("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
            document1.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
            document1.AddNamespaceDeclaration("wp14", "http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing");
            document1.AddNamespaceDeclaration("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
            document1.AddNamespaceDeclaration("w10", "urn:schemas-microsoft-com:office:word");
            document1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            document1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            document1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");
            document1.AddNamespaceDeclaration("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
            document1.AddNamespaceDeclaration("wpi", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");
            document1.AddNamespaceDeclaration("wne", "http://schemas.microsoft.com/office/word/2006/wordml");
            document1.AddNamespaceDeclaration("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");

            Body body1 = new Body();

            Paragraph paragraph1 = new Paragraph() { RsidParagraphAddition = "006E2D0E", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize1 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "24" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(spacingBetweenLines1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run() { RsidRunProperties = "004F2046" };

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize2 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "24" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            text1.Text = "Металл, пластик";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines2 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize3 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "24" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(spacingBetweenLines2);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run() { RsidRunProperties = "004F2046" };

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            NoProof noProof1 = new NoProof();
            FontSize fontSize4 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "24" };
            Languages languages1 = new Languages() { EastAsia = "ru-RU" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(noProof1);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            runProperties2.Append(languages1);

            Drawing drawing1 = new Drawing();

            Wp.Inline inline1 = new Wp.Inline() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)0U, DistanceFromRight = (UInt32Value)0U, AnchorId = "64E5FA2D", EditId = "2EA8FD0C" };
            Wp.Extent extent1 = new Wp.Extent() { Cx = 1809524L, Cy = 666667L };
            Wp.EffectExtent effectExtent1 = new Wp.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 635L, BottomEdge = 635L };
            Wp.DocProperties docProperties1 = new Wp.DocProperties() { Id = (UInt32Value)1U, Name = "Рисунок 1" };

            Wp.NonVisualGraphicFrameDrawingProperties nonVisualGraphicFrameDrawingProperties1 = new Wp.NonVisualGraphicFrameDrawingProperties();

            A.GraphicFrameLocks graphicFrameLocks1 = new A.GraphicFrameLocks() { NoChangeAspect = true };
            graphicFrameLocks1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            nonVisualGraphicFrameDrawingProperties1.Append(graphicFrameLocks1);

            A.Graphic graphic1 = new A.Graphic();
            graphic1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            A.GraphicData graphicData1 = new A.GraphicData() { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" };

            Pic.Picture picture1 = new Pic.Picture();
            picture1.AddNamespaceDeclaration("pic", "http://schemas.openxmlformats.org/drawingml/2006/picture");

            Pic.NonVisualPictureProperties nonVisualPictureProperties1 = new Pic.NonVisualPictureProperties();
            Pic.NonVisualDrawingProperties nonVisualDrawingProperties1 = new Pic.NonVisualDrawingProperties() { Id = (UInt32Value)1U, Name = "" };
            Pic.NonVisualPictureDrawingProperties nonVisualPictureDrawingProperties1 = new Pic.NonVisualPictureDrawingProperties();

            nonVisualPictureProperties1.Append(nonVisualDrawingProperties1);
            nonVisualPictureProperties1.Append(nonVisualPictureDrawingProperties1);

            Pic.BlipFill blipFill1 = new Pic.BlipFill();
            A.Blip blip1 = new A.Blip() { Embed = "rId5" };

            A.Stretch stretch1 = new A.Stretch();
            A.FillRectangle fillRectangle1 = new A.FillRectangle();

            stretch1.Append(fillRectangle1);

            blipFill1.Append(blip1);
            blipFill1.Append(stretch1);

            Pic.ShapeProperties shapeProperties1 = new Pic.ShapeProperties();

            A.Transform2D transform2D1 = new A.Transform2D();
            A.Offset offset1 = new A.Offset() { X = 0L, Y = 0L };
            A.Extents extents1 = new A.Extents() { Cx = 1809524L, Cy = 666667L };

            transform2D1.Append(offset1);
            transform2D1.Append(extents1);

            A.PresetGeometry presetGeometry1 = new A.PresetGeometry() { Preset = A.ShapeTypeValues.Rectangle };
            A.AdjustValueList adjustValueList1 = new A.AdjustValueList();

            presetGeometry1.Append(adjustValueList1);

            shapeProperties1.Append(transform2D1);
            shapeProperties1.Append(presetGeometry1);

            picture1.Append(nonVisualPictureProperties1);
            picture1.Append(blipFill1);
            picture1.Append(shapeProperties1);

            graphicData1.Append(picture1);

            graphic1.Append(graphicData1);

            inline1.Append(extent1);
            inline1.Append(effectExtent1);
            inline1.Append(docProperties1);
            inline1.Append(nonVisualGraphicFrameDrawingProperties1);
            inline1.Append(graphic1);

            drawing1.Append(inline1);

            run2.Append(runProperties2);
            run2.Append(drawing1);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "00FA438E" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines3 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize5 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "24" };
            Languages languages2 = new Languages() { EastAsia = "ru-RU" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);
            paragraphMarkRunProperties3.Append(languages2);

            paragraphProperties3.Append(spacingBetweenLines3);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run();

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Times New Roman", ComplexScript = "Times New Roman" };
            NoProof noProof2 = new NoProof();
            FontSize fontSize6 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "24" };
            Languages languages3 = new Languages() { EastAsia = "ru-RU" };

            runProperties3.Append(runFonts6);
            runProperties3.Append(noProof2);
            runProperties3.Append(fontSize6);
            runProperties3.Append(fontSizeComplexScript6);
            runProperties3.Append(languages3);

            Drawing drawing2 = new Drawing();

            Wp.Inline inline2 = new Wp.Inline() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)0U, DistanceFromRight = (UInt32Value)0U };
            Wp.Extent extent2 = new Wp.Extent() { Cx = 7067550L, Cy = 5114925L };
            Wp.EffectExtent effectExtent2 = new Wp.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 9525L };
            Wp.DocProperties docProperties2 = new Wp.DocProperties() { Id = (UInt32Value)2U, Name = "Рисунок 2" };

            Wp.NonVisualGraphicFrameDrawingProperties nonVisualGraphicFrameDrawingProperties2 = new Wp.NonVisualGraphicFrameDrawingProperties();

            A.GraphicFrameLocks graphicFrameLocks2 = new A.GraphicFrameLocks() { NoChangeAspect = true };
            graphicFrameLocks2.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            nonVisualGraphicFrameDrawingProperties2.Append(graphicFrameLocks2);

            A.Graphic graphic2 = new A.Graphic();
            graphic2.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            A.GraphicData graphicData2 = new A.GraphicData() { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" };

            Pic.Picture picture2 = new Pic.Picture();
            picture2.AddNamespaceDeclaration("pic", "http://schemas.openxmlformats.org/drawingml/2006/picture");

            Pic.NonVisualPictureProperties nonVisualPictureProperties2 = new Pic.NonVisualPictureProperties();
            Pic.NonVisualDrawingProperties nonVisualDrawingProperties2 = new Pic.NonVisualDrawingProperties() { Id = (UInt32Value)0U, Name = "Picture 3" };

            Pic.NonVisualPictureDrawingProperties nonVisualPictureDrawingProperties2 = new Pic.NonVisualPictureDrawingProperties();
            A.PictureLocks pictureLocks1 = new A.PictureLocks() { NoChangeAspect = true, NoChangeArrowheads = true };

            nonVisualPictureDrawingProperties2.Append(pictureLocks1);

            nonVisualPictureProperties2.Append(nonVisualDrawingProperties2);
            nonVisualPictureProperties2.Append(nonVisualPictureDrawingProperties2);

            Pic.BlipFill blipFill2 = new Pic.BlipFill();

            A.Blip blip2 = new A.Blip() { Embed = "rId6" };

            A.BlipExtensionList blipExtensionList1 = new A.BlipExtensionList();

            A.BlipExtension blipExtension1 = new A.BlipExtension() { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" };

            A14.UseLocalDpi useLocalDpi1 = new A14.UseLocalDpi() { Val = false };
            useLocalDpi1.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");

            blipExtension1.Append(useLocalDpi1);

            blipExtensionList1.Append(blipExtension1);

            blip2.Append(blipExtensionList1);
            A.SourceRectangle sourceRectangle1 = new A.SourceRectangle();

            A.Stretch stretch2 = new A.Stretch();
            A.FillRectangle fillRectangle2 = new A.FillRectangle();

            stretch2.Append(fillRectangle2);

            blipFill2.Append(blip2);
            blipFill2.Append(sourceRectangle1);
            blipFill2.Append(stretch2);

            Pic.ShapeProperties shapeProperties2 = new Pic.ShapeProperties() { BlackWhiteMode = A.BlackWhiteModeValues.Auto };

            A.Transform2D transform2D2 = new A.Transform2D();
            A.Offset offset2 = new A.Offset() { X = 0L, Y = 0L };
            A.Extents extents2 = new A.Extents() { Cx = 7067550L, Cy = 5114925L };

            transform2D2.Append(offset2);
            transform2D2.Append(extents2);

            A.PresetGeometry presetGeometry2 = new A.PresetGeometry() { Preset = A.ShapeTypeValues.Rectangle };
            A.AdjustValueList adjustValueList2 = new A.AdjustValueList();

            presetGeometry2.Append(adjustValueList2);
            A.NoFill noFill1 = new A.NoFill();

            A.Outline outline1 = new A.Outline();
            A.NoFill noFill2 = new A.NoFill();

            outline1.Append(noFill2);

            shapeProperties2.Append(transform2D2);
            shapeProperties2.Append(presetGeometry2);
            shapeProperties2.Append(noFill1);
            shapeProperties2.Append(outline1);

            picture2.Append(nonVisualPictureProperties2);
            picture2.Append(blipFill2);
            picture2.Append(shapeProperties2);

            graphicData2.Append(picture2);

            graphic2.Append(graphicData2);

            inline2.Append(extent2);
            inline2.Append(effectExtent2);
            inline2.Append(docProperties2);
            inline2.Append(nonVisualGraphicFrameDrawingProperties2);
            inline2.Append(graphic2);

            drawing2.Append(inline2);

            run3.Append(runProperties3);
            run3.Append(drawing2);

            Run run4 = new Run() { RsidRunAddition = "004F2046" };

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Times New Roman", ComplexScript = "Times New Roman" };
            NoProof noProof3 = new NoProof();
            FontSize fontSize7 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "24" };
            Languages languages4 = new Languages() { EastAsia = "ru-RU" };

            runProperties4.Append(runFonts7);
            runProperties4.Append(noProof3);
            runProperties4.Append(fontSize7);
            runProperties4.Append(fontSizeComplexScript7);
            runProperties4.Append(languages4);

            AlternateContent alternateContent1 = new AlternateContent();

            AlternateContentChoice alternateContentChoice1 = new AlternateContentChoice() { Requires = "wps" };

            Drawing drawing3 = new Drawing();

            Wp.Anchor anchor1 = new Wp.Anchor() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)114300U, DistanceFromRight = (UInt32Value)114300U, SimplePos = false, RelativeHeight = (UInt32Value)251660288U, BehindDoc = false, Locked = false, LayoutInCell = true, AllowOverlap = true };
            Wp.SimplePosition simplePosition1 = new Wp.SimplePosition() { X = 0L, Y = 0L };

            Wp.HorizontalPosition horizontalPosition1 = new Wp.HorizontalPosition() { RelativeFrom = Wp.HorizontalRelativePositionValues.Column };
            Wp.PositionOffset positionOffset1 = new Wp.PositionOffset();
            positionOffset1.Text = "3127375";

            horizontalPosition1.Append(positionOffset1);

            Wp.VerticalPosition verticalPosition1 = new Wp.VerticalPosition() { RelativeFrom = Wp.VerticalRelativePositionValues.Paragraph };
            Wp.PositionOffset positionOffset2 = new Wp.PositionOffset();
            positionOffset2.Text = "4371340";

            verticalPosition1.Append(positionOffset2);
            Wp.Extent extent3 = new Wp.Extent() { Cx = 866775L, Cy = 1400175L };
            Wp.EffectExtent effectExtent3 = new Wp.EffectExtent() { LeftEdge = 0L, TopEdge = 38100L, RightEdge = 47625L, BottomEdge = 28575L };
            Wp.WrapNone wrapNone1 = new Wp.WrapNone();
            Wp.DocProperties docProperties3 = new Wp.DocProperties() { Id = (UInt32Value)8U, Name = "Прямая со стрелкой 8" };
            Wp.NonVisualGraphicFrameDrawingProperties nonVisualGraphicFrameDrawingProperties3 = new Wp.NonVisualGraphicFrameDrawingProperties();

            A.Graphic graphic3 = new A.Graphic();
            graphic3.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            A.GraphicData graphicData3 = new A.GraphicData() { Uri = "http://schemas.microsoft.com/office/word/2010/wordprocessingShape" };

            Wps.WordprocessingShape wordprocessingShape1 = new Wps.WordprocessingShape();
            Wps.NonVisualConnectorProperties nonVisualConnectorProperties1 = new Wps.NonVisualConnectorProperties();

            Wps.ShapeProperties shapeProperties3 = new Wps.ShapeProperties();

            A.Transform2D transform2D3 = new A.Transform2D() { VerticalFlip = true };
            A.Offset offset3 = new A.Offset() { X = 0L, Y = 0L };
            A.Extents extents3 = new A.Extents() { Cx = 866775L, Cy = 1400175L };

            transform2D3.Append(offset3);
            transform2D3.Append(extents3);

            A.PresetGeometry presetGeometry3 = new A.PresetGeometry() { Preset = A.ShapeTypeValues.StraightConnector1 };
            A.AdjustValueList adjustValueList3 = new A.AdjustValueList();

            presetGeometry3.Append(adjustValueList3);

            A.Outline outline2 = new A.Outline();

            A.SolidFill solidFill1 = new A.SolidFill();
            A.SchemeColor schemeColor1 = new A.SchemeColor() { Val = A.SchemeColorValues.Text1 };

            solidFill1.Append(schemeColor1);
            A.TailEnd tailEnd1 = new A.TailEnd() { Type = A.LineEndValues.Triangle };

            outline2.Append(solidFill1);
            outline2.Append(tailEnd1);

            shapeProperties3.Append(transform2D3);
            shapeProperties3.Append(presetGeometry3);
            shapeProperties3.Append(outline2);

            Wps.ShapeStyle shapeStyle1 = new Wps.ShapeStyle();

            A.LineReference lineReference1 = new A.LineReference() { Index = (UInt32Value)1U };
            A.SchemeColor schemeColor2 = new A.SchemeColor() { Val = A.SchemeColorValues.Accent1 };

            lineReference1.Append(schemeColor2);

            A.FillReference fillReference1 = new A.FillReference() { Index = (UInt32Value)0U };
            A.SchemeColor schemeColor3 = new A.SchemeColor() { Val = A.SchemeColorValues.Accent1 };

            fillReference1.Append(schemeColor3);

            A.EffectReference effectReference1 = new A.EffectReference() { Index = (UInt32Value)0U };
            A.SchemeColor schemeColor4 = new A.SchemeColor() { Val = A.SchemeColorValues.Accent1 };

            effectReference1.Append(schemeColor4);

            A.FontReference fontReference1 = new A.FontReference() { Index = A.FontCollectionIndexValues.Minor };
            A.SchemeColor schemeColor5 = new A.SchemeColor() { Val = A.SchemeColorValues.Text1 };

            fontReference1.Append(schemeColor5);

            shapeStyle1.Append(lineReference1);
            shapeStyle1.Append(fillReference1);
            shapeStyle1.Append(effectReference1);
            shapeStyle1.Append(fontReference1);
            Wps.TextBodyProperties textBodyProperties1 = new Wps.TextBodyProperties();

            wordprocessingShape1.Append(nonVisualConnectorProperties1);
            wordprocessingShape1.Append(shapeProperties3);
            wordprocessingShape1.Append(shapeStyle1);
            wordprocessingShape1.Append(textBodyProperties1);

            graphicData3.Append(wordprocessingShape1);

            graphic3.Append(graphicData3);

            anchor1.Append(simplePosition1);
            anchor1.Append(horizontalPosition1);
            anchor1.Append(verticalPosition1);
            anchor1.Append(extent3);
            anchor1.Append(effectExtent3);
            anchor1.Append(wrapNone1);
            anchor1.Append(docProperties3);
            anchor1.Append(nonVisualGraphicFrameDrawingProperties3);
            anchor1.Append(graphic3);

            drawing3.Append(anchor1);

            alternateContentChoice1.Append(drawing3);

            AlternateContentFallback alternateContentFallback1 = new AlternateContentFallback();

            Picture picture3 = new Picture();

            V.Shapetype shapetype1 = new V.Shapetype() { Id = "_x0000_t32", CoordinateSize = "21600,21600", Oned = true, Filled = false, OptionalNumber = 32, EdgePath = "m,l21600,21600e" };
            shapetype1.SetAttribute(new OpenXmlAttribute("w14", "anchorId", "http://schemas.microsoft.com/office/word/2010/wordml", "7EEDE81C"));
            V.Path path1 = new V.Path() { AllowFill = false, ShowArrowhead = true, ConnectionPointType = Ovml.ConnectValues.None };
            Ovml.Lock lock1 = new Ovml.Lock() { Extension = V.ExtensionHandlingBehaviorValues.Edit, ShapeType = true };

            shapetype1.Append(path1);
            shapetype1.Append(lock1);

            V.Shape shape1 = new V.Shape() { Id = "Прямая со стрелкой 8", Style = "position:absolute;margin-left:246.25pt;margin-top:344.2pt;width:68.25pt;height:110.25pt;flip:y;z-index:251660288;visibility:visible;mso-wrap-style:square;mso-wrap-distance-left:9pt;mso-wrap-distance-top:0;mso-wrap-distance-right:9pt;mso-wrap-distance-bottom:0;mso-position-horizontal:absolute;mso-position-horizontal-relative:text;mso-position-vertical:absolute;mso-position-vertical-relative:text", OptionalString = "_x0000_s1026", StrokeColor = "black [3213]", StrokeWeight = ".5pt", Type = "#_x0000_t32", EncodedPackage = "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\nBQYAAAAABAAEAPMAAACABQAAAAA=\n" };
            V.Stroke stroke1 = new V.Stroke() { JoinStyle = V.StrokeJoinStyleValues.Miter, EndArrow = V.StrokeArrowValues.Block };

            shape1.Append(stroke1);

            picture3.Append(shapetype1);
            picture3.Append(shape1);

            alternateContentFallback1.Append(picture3);

            alternateContent1.Append(alternateContentChoice1);
            alternateContent1.Append(alternateContentFallback1);

            run4.Append(runProperties4);
            run4.Append(alternateContent1);

            Run run5 = new Run() { RsidRunAddition = "004F2046" };

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Times New Roman", ComplexScript = "Times New Roman" };
            NoProof noProof4 = new NoProof();
            FontSize fontSize8 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "24" };
            Languages languages5 = new Languages() { EastAsia = "ru-RU" };

            runProperties5.Append(runFonts8);
            runProperties5.Append(noProof4);
            runProperties5.Append(fontSize8);
            runProperties5.Append(fontSizeComplexScript8);
            runProperties5.Append(languages5);

            AlternateContent alternateContent2 = new AlternateContent();

            AlternateContentChoice alternateContentChoice2 = new AlternateContentChoice() { Requires = "wps" };

            Drawing drawing4 = new Drawing();

            Wp.Anchor anchor2 = new Wp.Anchor() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)114300U, DistanceFromRight = (UInt32Value)114300U, SimplePos = false, RelativeHeight = (UInt32Value)251659264U, BehindDoc = false, Locked = false, LayoutInCell = true, AllowOverlap = true };
            Wp.SimplePosition simplePosition2 = new Wp.SimplePosition() { X = 0L, Y = 0L };

            Wp.HorizontalPosition horizontalPosition2 = new Wp.HorizontalPosition() { RelativeFrom = Wp.HorizontalRelativePositionValues.Column };
            Wp.PositionOffset positionOffset3 = new Wp.PositionOffset();
            positionOffset3.Text = "1479549";

            horizontalPosition2.Append(positionOffset3);

            Wp.VerticalPosition verticalPosition2 = new Wp.VerticalPosition() { RelativeFrom = Wp.VerticalRelativePositionValues.Paragraph };
            Wp.PositionOffset positionOffset4 = new Wp.PositionOffset();
            positionOffset4.Text = "4371340";

            verticalPosition2.Append(positionOffset4);
            Wp.Extent extent4 = new Wp.Extent() { Cx = 1152525L, Cy = 857250L };
            Wp.EffectExtent effectExtent4 = new Wp.EffectExtent() { LeftEdge = 0L, TopEdge = 38100L, RightEdge = 47625L, BottomEdge = 19050L };
            Wp.WrapNone wrapNone2 = new Wp.WrapNone();
            Wp.DocProperties docProperties4 = new Wp.DocProperties() { Id = (UInt32Value)7U, Name = "Прямая со стрелкой 7" };
            Wp.NonVisualGraphicFrameDrawingProperties nonVisualGraphicFrameDrawingProperties4 = new Wp.NonVisualGraphicFrameDrawingProperties();

            A.Graphic graphic4 = new A.Graphic();
            graphic4.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            A.GraphicData graphicData4 = new A.GraphicData() { Uri = "http://schemas.microsoft.com/office/word/2010/wordprocessingShape" };

            Wps.WordprocessingShape wordprocessingShape2 = new Wps.WordprocessingShape();
            Wps.NonVisualConnectorProperties nonVisualConnectorProperties2 = new Wps.NonVisualConnectorProperties();

            Wps.ShapeProperties shapeProperties4 = new Wps.ShapeProperties();

            A.Transform2D transform2D4 = new A.Transform2D() { VerticalFlip = true };
            A.Offset offset4 = new A.Offset() { X = 0L, Y = 0L };
            A.Extents extents4 = new A.Extents() { Cx = 1152525L, Cy = 857250L };

            transform2D4.Append(offset4);
            transform2D4.Append(extents4);

            A.PresetGeometry presetGeometry4 = new A.PresetGeometry() { Preset = A.ShapeTypeValues.StraightConnector1 };
            A.AdjustValueList adjustValueList4 = new A.AdjustValueList();

            presetGeometry4.Append(adjustValueList4);

            A.Outline outline3 = new A.Outline();

            A.SolidFill solidFill2 = new A.SolidFill();
            A.SchemeColor schemeColor6 = new A.SchemeColor() { Val = A.SchemeColorValues.Text1 };

            solidFill2.Append(schemeColor6);
            A.TailEnd tailEnd2 = new A.TailEnd() { Type = A.LineEndValues.Triangle };

            outline3.Append(solidFill2);
            outline3.Append(tailEnd2);

            shapeProperties4.Append(transform2D4);
            shapeProperties4.Append(presetGeometry4);
            shapeProperties4.Append(outline3);

            Wps.ShapeStyle shapeStyle2 = new Wps.ShapeStyle();

            A.LineReference lineReference2 = new A.LineReference() { Index = (UInt32Value)1U };
            A.SchemeColor schemeColor7 = new A.SchemeColor() { Val = A.SchemeColorValues.Accent1 };

            lineReference2.Append(schemeColor7);

            A.FillReference fillReference2 = new A.FillReference() { Index = (UInt32Value)0U };
            A.SchemeColor schemeColor8 = new A.SchemeColor() { Val = A.SchemeColorValues.Accent1 };

            fillReference2.Append(schemeColor8);

            A.EffectReference effectReference2 = new A.EffectReference() { Index = (UInt32Value)0U };
            A.SchemeColor schemeColor9 = new A.SchemeColor() { Val = A.SchemeColorValues.Accent1 };

            effectReference2.Append(schemeColor9);

            A.FontReference fontReference2 = new A.FontReference() { Index = A.FontCollectionIndexValues.Minor };
            A.SchemeColor schemeColor10 = new A.SchemeColor() { Val = A.SchemeColorValues.Text1 };

            fontReference2.Append(schemeColor10);

            shapeStyle2.Append(lineReference2);
            shapeStyle2.Append(fillReference2);
            shapeStyle2.Append(effectReference2);
            shapeStyle2.Append(fontReference2);
            Wps.TextBodyProperties textBodyProperties2 = new Wps.TextBodyProperties();

            wordprocessingShape2.Append(nonVisualConnectorProperties2);
            wordprocessingShape2.Append(shapeProperties4);
            wordprocessingShape2.Append(shapeStyle2);
            wordprocessingShape2.Append(textBodyProperties2);

            graphicData4.Append(wordprocessingShape2);

            graphic4.Append(graphicData4);

            anchor2.Append(simplePosition2);
            anchor2.Append(horizontalPosition2);
            anchor2.Append(verticalPosition2);
            anchor2.Append(extent4);
            anchor2.Append(effectExtent4);
            anchor2.Append(wrapNone2);
            anchor2.Append(docProperties4);
            anchor2.Append(nonVisualGraphicFrameDrawingProperties4);
            anchor2.Append(graphic4);

            drawing4.Append(anchor2);

            alternateContentChoice2.Append(drawing4);

            AlternateContentFallback alternateContentFallback2 = new AlternateContentFallback();

            Picture picture4 = new Picture();

            V.Shape shape2 = new V.Shape() { Id = "Прямая со стрелкой 7", Style = "position:absolute;margin-left:116.5pt;margin-top:344.2pt;width:90.75pt;height:67.5pt;flip:y;z-index:251659264;visibility:visible;mso-wrap-style:square;mso-wrap-distance-left:9pt;mso-wrap-distance-top:0;mso-wrap-distance-right:9pt;mso-wrap-distance-bottom:0;mso-position-horizontal:absolute;mso-position-horizontal-relative:text;mso-position-vertical:absolute;mso-position-vertical-relative:text", OptionalString = "_x0000_s1026", StrokeColor = "black [3213]", StrokeWeight = ".5pt", Type = "#_x0000_t32", EncodedPackage = "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\nBQYAAAAABAAEAPMAAACABQAAAAA=\n" };
            shape2.SetAttribute(new OpenXmlAttribute("w14", "anchorId", "http://schemas.microsoft.com/office/word/2010/wordml", "23FCBB85"));
            V.Stroke stroke2 = new V.Stroke() { JoinStyle = V.StrokeJoinStyleValues.Miter, EndArrow = V.StrokeArrowValues.Block };

            shape2.Append(stroke2);

            picture4.Append(shape2);

            alternateContentFallback2.Append(picture4);

            alternateContent2.Append(alternateContentChoice2);
            alternateContent2.Append(alternateContentFallback2);

            run5.Append(runProperties5);
            run5.Append(alternateContent2);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);
            paragraph3.Append(run4);
            paragraph3.Append(run5);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines4 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize9 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "24" };
            Languages languages6 = new Languages() { EastAsia = "ru-RU" };

            paragraphMarkRunProperties4.Append(runFonts9);
            paragraphMarkRunProperties4.Append(fontSize9);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript9);
            paragraphMarkRunProperties4.Append(languages6);

            paragraphProperties4.Append(spacingBetweenLines4);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            paragraph4.Append(paragraphProperties4);

            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "a3" };
            TableWidth tableWidth1 = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };

            TableBorders tableBorders1 = new TableBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideHorizontalBorder insideHorizontalBorder1 = new InsideHorizontalBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideVerticalBorder insideVerticalBorder1 = new InsideVerticalBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };

            tableBorders1.Append(topBorder1);
            tableBorders1.Append(leftBorder1);
            tableBorders1.Append(bottomBorder1);
            tableBorders1.Append(rightBorder1);
            tableBorders1.Append(insideHorizontalBorder1);
            tableBorders1.Append(insideVerticalBorder1);
            TableLook tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableBorders1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "4957" };

            tableGrid1.Append(gridColumn1);

            TableRow tableRow1 = new TableRow() { RsidTableRowAddition = "004F2046", RsidTableRowProperties = "004F2046" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "4957", Type = TableWidthUnitValues.Dxa };

            tableCellProperties1.Append(tableCellWidth1);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "004F2046", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();

            Tabs tabs1 = new Tabs();
            TabStop tabStop1 = new TabStop() { Val = TabStopValues.Left, Position = 6300 };
            TabStop tabStop2 = new TabStop() { Val = TabStopValues.Left, Position = 7020 };

            tabs1.Append(tabStop1);
            tabs1.Append(tabStop2);
            Indentation indentation1 = new Indentation() { Right = "-6" };
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", EastAsia = "Times New Roman", ComplexScript = "Arial" };
            Bold bold1 = new Bold();
            FontSize fontSize10 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "28" };
            Languages languages7 = new Languages() { EastAsia = "ru-RU" };

            paragraphMarkRunProperties5.Append(runFonts10);
            paragraphMarkRunProperties5.Append(bold1);
            paragraphMarkRunProperties5.Append(fontSize10);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript10);
            paragraphMarkRunProperties5.Append(languages7);

            paragraphProperties5.Append(tabs1);
            paragraphProperties5.Append(indentation1);
            paragraphProperties5.Append(justification1);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run6 = new Run() { RsidRunProperties = "004F2046" };

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", EastAsia = "Times New Roman", ComplexScript = "Arial" };
            Bold bold2 = new Bold();
            FontSize fontSize11 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript() { Val = "28" };
            Languages languages8 = new Languages() { EastAsia = "ru-RU" };

            runProperties6.Append(runFonts11);
            runProperties6.Append(bold2);
            runProperties6.Append(fontSize11);
            runProperties6.Append(fontSizeComplexScript11);
            runProperties6.Append(languages8);
            Text text2 = new Text();
            text2.Text = "Сделано в Республике Беларусь";

            run6.Append(runProperties6);
            run6.Append(text2);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run6);

            Paragraph paragraph6 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();

            Tabs tabs2 = new Tabs();
            TabStop tabStop3 = new TabStop() { Val = TabStopValues.Left, Position = 6300 };
            TabStop tabStop4 = new TabStop() { Val = TabStopValues.Left, Position = 7020 };

            tabs2.Append(tabStop3);
            tabs2.Append(tabStop4);
            Indentation indentation2 = new Indentation() { Right = "-6" };
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", EastAsia = "Times New Roman", ComplexScript = "Arial" };
            Bold bold3 = new Bold();
            FontSize fontSize12 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript() { Val = "28" };
            Languages languages9 = new Languages() { EastAsia = "ru-RU" };

            paragraphMarkRunProperties6.Append(runFonts12);
            paragraphMarkRunProperties6.Append(bold3);
            paragraphMarkRunProperties6.Append(fontSize12);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript12);
            paragraphMarkRunProperties6.Append(languages9);

            paragraphProperties6.Append(tabs2);
            paragraphProperties6.Append(indentation2);
            paragraphProperties6.Append(justification2);
            paragraphProperties6.Append(paragraphMarkRunProperties6);

            Run run7 = new Run() { RsidRunProperties = "004F2046" };

            RunProperties runProperties7 = new RunProperties();
            RunFonts runFonts13 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", EastAsia = "Times New Roman", ComplexScript = "Arial" };
            Bold bold4 = new Bold();
            FontSize fontSize13 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript13 = new FontSizeComplexScript() { Val = "28" };
            Languages languages10 = new Languages() { EastAsia = "ru-RU" };

            runProperties7.Append(runFonts13);
            runProperties7.Append(bold4);
            runProperties7.Append(fontSize13);
            runProperties7.Append(fontSizeComplexScript13);
            runProperties7.Append(languages10);
            Text text3 = new Text();
            text3.Text = "СЭЗ Минск (+37517)3669067";

            run7.Append(runProperties7);
            run7.Append(text3);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run7);

            Paragraph paragraph7 = new Paragraph() { RsidParagraphMarkRevision = "004F2046", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties7 = new ParagraphProperties();

            Tabs tabs3 = new Tabs();
            TabStop tabStop5 = new TabStop() { Val = TabStopValues.Left, Position = 6300 };
            TabStop tabStop6 = new TabStop() { Val = TabStopValues.Left, Position = 7020 };

            tabs3.Append(tabStop5);
            tabs3.Append(tabStop6);
            Indentation indentation3 = new Indentation() { Right = "-6" };
            Justification justification3 = new Justification() { Val = JustificationValues.Both };

            ParagraphMarkRunProperties paragraphMarkRunProperties7 = new ParagraphMarkRunProperties();
            RunFonts runFonts14 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", EastAsia = "Times New Roman", ComplexScript = "Arial" };
            Bold bold5 = new Bold();
            FontSize fontSize14 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript14 = new FontSizeComplexScript() { Val = "28" };
            Languages languages11 = new Languages() { EastAsia = "ru-RU" };

            paragraphMarkRunProperties7.Append(runFonts14);
            paragraphMarkRunProperties7.Append(bold5);
            paragraphMarkRunProperties7.Append(fontSize14);
            paragraphMarkRunProperties7.Append(fontSizeComplexScript14);
            paragraphMarkRunProperties7.Append(languages11);

            paragraphProperties7.Append(tabs3);
            paragraphProperties7.Append(indentation3);
            paragraphProperties7.Append(justification3);
            paragraphProperties7.Append(paragraphMarkRunProperties7);

            Run run8 = new Run();

            RunProperties runProperties8 = new RunProperties();
            RunFonts runFonts15 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", EastAsia = "Times New Roman", ComplexScript = "Arial" };
            Bold bold6 = new Bold();
            FontSize fontSize15 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript15 = new FontSizeComplexScript() { Val = "28" };
            Languages languages12 = new Languages() { EastAsia = "ru-RU" };

            runProperties8.Append(runFonts15);
            runProperties8.Append(bold6);
            runProperties8.Append(fontSize15);
            runProperties8.Append(fontSizeComplexScript15);
            runProperties8.Append(languages12);
            Text text4 = new Text() { Space = SpaceProcessingModeValues.Preserve };
            text4.Text = "               ";

            run8.Append(runProperties8);
            run8.Append(text4);

            Run run9 = new Run() { RsidRunProperties = "004F2046" };

            RunProperties runProperties9 = new RunProperties();
            RunFonts runFonts16 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", EastAsia = "Times New Roman", ComplexScript = "Arial" };
            Bold bold7 = new Bold();
            FontSize fontSize16 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript16 = new FontSizeComplexScript() { Val = "28" };
            Languages languages13 = new Languages() { EastAsia = "ru-RU" };

            runProperties9.Append(runFonts16);
            runProperties9.Append(bold7);
            runProperties9.Append(fontSize16);
            runProperties9.Append(fontSizeComplexScript16);
            runProperties9.Append(languages13);
            Text text5 = new Text() { Space = SpaceProcessingModeValues.Preserve };
            text5.Text = "Значок ";

            run9.Append(runProperties9);
            run9.Append(text5);
            ProofError proofError1 = new ProofError() { Type = ProofingErrorValues.GrammarStart };

            Run run10 = new Run() { RsidRunProperties = "004F2046" };

            RunProperties runProperties10 = new RunProperties();
            RunFonts runFonts17 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", EastAsia = "Times New Roman", ComplexScript = "Arial" };
            Bold bold8 = new Bold();
            FontSize fontSize17 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript17 = new FontSizeComplexScript() { Val = "28" };
            Languages languages14 = new Languages() { EastAsia = "ru-RU" };

            runProperties10.Append(runFonts17);
            runProperties10.Append(bold8);
            runProperties10.Append(fontSize17);
            runProperties10.Append(fontSizeComplexScript17);
            runProperties10.Append(languages14);
            Text text6 = new Text();
            text6.Text = "PCT,EAC";

            run10.Append(runProperties10);
            run10.Append(text6);
            ProofError proofError2 = new ProofError() { Type = ProofingErrorValues.GrammarEnd };

            Run run11 = new Run() { RsidRunProperties = "004F2046" };

            RunProperties runProperties11 = new RunProperties();
            RunFonts runFonts18 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", EastAsia = "Times New Roman", ComplexScript = "Arial" };
            Bold bold9 = new Bold();
            FontSize fontSize18 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript18 = new FontSizeComplexScript() { Val = "28" };
            Languages languages15 = new Languages() { EastAsia = "ru-RU" };

            runProperties11.Append(runFonts18);
            runProperties11.Append(bold9);
            runProperties11.Append(fontSize18);
            runProperties11.Append(fontSizeComplexScript18);
            runProperties11.Append(languages15);
            Text text7 = new Text();
            text7.Text = ",ISO 9001";

            run11.Append(runProperties11);
            run11.Append(text7);

            paragraph7.Append(paragraphProperties7);
            paragraph7.Append(run8);
            paragraph7.Append(run9);
            paragraph7.Append(proofError1);
            paragraph7.Append(run10);
            paragraph7.Append(proofError2);
            paragraph7.Append(run11);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph5);
            tableCell1.Append(paragraph6);
            tableCell1.Append(paragraph7);

            tableRow1.Append(tableCell1);

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);
            table1.Append(tableRow1);

            Paragraph paragraph8 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties8 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines5 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties8 = new ParagraphMarkRunProperties();
            RunFonts runFonts19 = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize19 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript19 = new FontSizeComplexScript() { Val = "24" };
            Languages languages16 = new Languages() { EastAsia = "ru-RU" };

            paragraphMarkRunProperties8.Append(runFonts19);
            paragraphMarkRunProperties8.Append(fontSize19);
            paragraphMarkRunProperties8.Append(fontSizeComplexScript19);
            paragraphMarkRunProperties8.Append(languages16);

            paragraphProperties8.Append(spacingBetweenLines5);
            paragraphProperties8.Append(paragraphMarkRunProperties8);

            paragraph8.Append(paragraphProperties8);

            Paragraph paragraph9 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties9 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines6 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties9 = new ParagraphMarkRunProperties();
            RunFonts runFonts20 = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize20 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript20 = new FontSizeComplexScript() { Val = "24" };
            Languages languages17 = new Languages() { EastAsia = "ru-RU" };

            paragraphMarkRunProperties9.Append(runFonts20);
            paragraphMarkRunProperties9.Append(fontSize20);
            paragraphMarkRunProperties9.Append(fontSizeComplexScript20);
            paragraphMarkRunProperties9.Append(languages17);

            paragraphProperties9.Append(spacingBetweenLines6);
            paragraphProperties9.Append(paragraphMarkRunProperties9);

            paragraph9.Append(paragraphProperties9);

            Paragraph paragraph10 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties10 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines7 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties10 = new ParagraphMarkRunProperties();
            RunFonts runFonts21 = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize21 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript21 = new FontSizeComplexScript() { Val = "24" };
            Languages languages18 = new Languages() { EastAsia = "ru-RU" };

            paragraphMarkRunProperties10.Append(runFonts21);
            paragraphMarkRunProperties10.Append(fontSize21);
            paragraphMarkRunProperties10.Append(fontSizeComplexScript21);
            paragraphMarkRunProperties10.Append(languages18);

            paragraphProperties10.Append(spacingBetweenLines7);
            paragraphProperties10.Append(paragraphMarkRunProperties10);

            paragraph10.Append(paragraphProperties10);

            Paragraph paragraph11 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties11 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines8 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties11 = new ParagraphMarkRunProperties();
            RunFonts runFonts22 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize22 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript22 = new FontSizeComplexScript() { Val = "24" };

            paragraphMarkRunProperties11.Append(runFonts22);
            paragraphMarkRunProperties11.Append(fontSize22);
            paragraphMarkRunProperties11.Append(fontSizeComplexScript22);

            paragraphProperties11.Append(spacingBetweenLines8);
            paragraphProperties11.Append(paragraphMarkRunProperties11);

            paragraph11.Append(paragraphProperties11);










            Paragraph paragraph30 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties30 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines9 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties21 = new ParagraphMarkRunProperties();
            RunFonts runFonts32 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize32 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript32 = new FontSizeComplexScript() { Val = "24" };

            paragraphMarkRunProperties21.Append(runFonts32);
            paragraphMarkRunProperties21.Append(fontSize32);
            paragraphMarkRunProperties21.Append(fontSizeComplexScript32);

            paragraphProperties30.Append(spacingBetweenLines9);
            paragraphProperties30.Append(paragraphMarkRunProperties21);

            paragraph30.Append(paragraphProperties30);

            Paragraph paragraph31 = new Paragraph() { RsidParagraphMarkRevision = "004F2046", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties31 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines10 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification13 = new Justification() { Val = JustificationValues.Right };

            ParagraphMarkRunProperties paragraphMarkRunProperties22 = new ParagraphMarkRunProperties();
            RunFonts runFonts33 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize33 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript33 = new FontSizeComplexScript() { Val = "24" };

            paragraphMarkRunProperties22.Append(runFonts33);
            paragraphMarkRunProperties22.Append(fontSize33);
            paragraphMarkRunProperties22.Append(fontSizeComplexScript33);

            paragraphProperties31.Append(spacingBetweenLines10);
            paragraphProperties31.Append(justification13);
            paragraphProperties31.Append(paragraphMarkRunProperties22);

            Run run21 = new Run() { RsidRunProperties = "004F2046" };

            RunProperties runProperties12 = new RunProperties();
            RunFonts runFonts34 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize34 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript34 = new FontSizeComplexScript() { Val = "24" };

            runProperties12.Append(runFonts34);
            runProperties12.Append(fontSize34);
            runProperties12.Append(fontSizeComplexScript34);
            Text text17 = new Text() { Space = SpaceProcessingModeValues.Preserve };
            text17.Text = "Итого: ";

            run21.Append(runProperties12);
            run21.Append(text17);
            BookmarkStart bookmarkStart1 = new BookmarkStart() { Name = "count", Id = "0" };

            Run run22 = new Run();

            RunProperties runProperties13 = new RunProperties();
            RunFonts runFonts35 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize35 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript35 = new FontSizeComplexScript() { Val = "24" };
            Languages languages19 = new Languages() { Val = "en-US" };

            runProperties13.Append(runFonts35);
            runProperties13.Append(fontSize35);
            runProperties13.Append(fontSizeComplexScript35);
            runProperties13.Append(languages19);
            Text text18 = new Text();
            text18.Text = count.ToString();

            run22.Append(runProperties13);
            run22.Append(text18);

            Run run23 = new Run() { RsidRunProperties = "004F2046" };

            RunProperties runProperties14 = new RunProperties();
            RunFonts runFonts36 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize36 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript36 = new FontSizeComplexScript() { Val = "24" };

            runProperties14.Append(runFonts36);
            runProperties14.Append(fontSize36);
            runProperties14.Append(fontSizeComplexScript36);
            Text text19 = new Text() { Space = SpaceProcessingModeValues.Preserve };
            text19.Text = " ";

            run23.Append(runProperties14);
            run23.Append(text19);
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = "0" };

            Run run24 = new Run() { RsidRunProperties = "004F2046" };

            RunProperties runProperties15 = new RunProperties();
            RunFonts runFonts37 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize37 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript37 = new FontSizeComplexScript() { Val = "24" };

            runProperties15.Append(runFonts37);
            runProperties15.Append(fontSize37);
            runProperties15.Append(fontSizeComplexScript37);
            Text text20 = new Text();
            text20.Text = "табл.";

            run24.Append(runProperties15);
            run24.Append(text20);

            paragraph31.Append(paragraphProperties31);
            paragraph31.Append(run21);
            paragraph31.Append(bookmarkStart1);
            paragraph31.Append(run22);
            paragraph31.Append(run23);
            paragraph31.Append(bookmarkEnd1);
            paragraph31.Append(run24);

            Paragraph paragraph32 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties32 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines11 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties23 = new ParagraphMarkRunProperties();
            RunFonts runFonts38 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize38 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript38 = new FontSizeComplexScript() { Val = "24" };

            paragraphMarkRunProperties23.Append(runFonts38);
            paragraphMarkRunProperties23.Append(fontSize38);
            paragraphMarkRunProperties23.Append(fontSizeComplexScript38);

            paragraphProperties32.Append(spacingBetweenLines11);
            paragraphProperties32.Append(paragraphMarkRunProperties23);

            paragraph32.Append(paragraphProperties32);

            Paragraph paragraph33 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties33 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines12 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties24 = new ParagraphMarkRunProperties();
            RunFonts runFonts39 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize39 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript39 = new FontSizeComplexScript() { Val = "24" };

            paragraphMarkRunProperties24.Append(runFonts39);
            paragraphMarkRunProperties24.Append(fontSize39);
            paragraphMarkRunProperties24.Append(fontSizeComplexScript39);

            paragraphProperties33.Append(spacingBetweenLines12);
            paragraphProperties33.Append(paragraphMarkRunProperties24);

            Run run25 = new Run() { RsidRunProperties = "004F2046" };

            RunProperties runProperties16 = new RunProperties();
            RunFonts runFonts40 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize40 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript40 = new FontSizeComplexScript() { Val = "24" };

            runProperties16.Append(runFonts40);
            runProperties16.Append(fontSize40);
            runProperties16.Append(fontSizeComplexScript40);
            Text text21 = new Text();
            text21.Text = "На расстоянии 5 мм от каждого края в углах нанести линии обозначения места сверления";

            run25.Append(runProperties16);
            run25.Append(text21);

            paragraph33.Append(paragraphProperties33);
            paragraph33.Append(run25);

            Paragraph paragraph34 = new Paragraph() { RsidParagraphMarkRevision = "004F2046", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties34 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines13 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties25 = new ParagraphMarkRunProperties();
            RunFonts runFonts41 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize41 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript41 = new FontSizeComplexScript() { Val = "24" };

            paragraphMarkRunProperties25.Append(runFonts41);
            paragraphMarkRunProperties25.Append(fontSize41);
            paragraphMarkRunProperties25.Append(fontSizeComplexScript41);

            paragraphProperties34.Append(spacingBetweenLines13);
            paragraphProperties34.Append(paragraphMarkRunProperties25);
            BookmarkStart bookmarkStart2 = new BookmarkStart() { Name = "_GoBack", Id = "1" };
            BookmarkEnd bookmarkEnd2 = new BookmarkEnd() { Id = "1" };

            paragraph34.Append(paragraphProperties34);
            paragraph34.Append(bookmarkStart2);
            paragraph34.Append(bookmarkEnd2);

            Paragraph paragraph35 = new Paragraph() { RsidParagraphMarkRevision = "004F2046", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties35 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines14 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties26 = new ParagraphMarkRunProperties();
            RunFonts runFonts42 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            FontSize fontSize42 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript42 = new FontSizeComplexScript() { Val = "24" };

            paragraphMarkRunProperties26.Append(runFonts42);
            paragraphMarkRunProperties26.Append(fontSize42);
            paragraphMarkRunProperties26.Append(fontSizeComplexScript42);

            paragraphProperties35.Append(spacingBetweenLines14);
            paragraphProperties35.Append(paragraphMarkRunProperties26);

            Run run26 = new Run() { RsidRunProperties = "003A207D" };

            RunProperties runProperties17 = new RunProperties();
            NoProof noProof5 = new NoProof();
            Languages languages20 = new Languages() { EastAsia = "ru-RU" };

            runProperties17.Append(noProof5);
            runProperties17.Append(languages20);

            Drawing drawing5 = new Drawing();

            Wp.Inline inline3 = new Wp.Inline() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)0U, DistanceFromRight = (UInt32Value)0U };
            Wp.Extent extent5 = new Wp.Extent() { Cx = 504825L, Cy = 495300L };
            Wp.EffectExtent effectExtent5 = new Wp.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 9525L, BottomEdge = 0L };
            Wp.DocProperties docProperties5 = new Wp.DocProperties() { Id = (UInt32Value)4U, Name = "Рисунок 4" };

            Wp.NonVisualGraphicFrameDrawingProperties nonVisualGraphicFrameDrawingProperties5 = new Wp.NonVisualGraphicFrameDrawingProperties();

            A.GraphicFrameLocks graphicFrameLocks3 = new A.GraphicFrameLocks() { NoChangeAspect = true };
            graphicFrameLocks3.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            nonVisualGraphicFrameDrawingProperties5.Append(graphicFrameLocks3);

            A.Graphic graphic5 = new A.Graphic();
            graphic5.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            A.GraphicData graphicData5 = new A.GraphicData() { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" };

            Pic.Picture picture5 = new Pic.Picture();
            picture5.AddNamespaceDeclaration("pic", "http://schemas.openxmlformats.org/drawingml/2006/picture");

            Pic.NonVisualPictureProperties nonVisualPictureProperties3 = new Pic.NonVisualPictureProperties();
            Pic.NonVisualDrawingProperties nonVisualDrawingProperties3 = new Pic.NonVisualDrawingProperties() { Id = (UInt32Value)0U, Name = "Рисунок 1" };

            Pic.NonVisualPictureDrawingProperties nonVisualPictureDrawingProperties3 = new Pic.NonVisualPictureDrawingProperties();
            A.PictureLocks pictureLocks2 = new A.PictureLocks() { NoChangeAspect = true, NoChangeArrowheads = true };

            nonVisualPictureDrawingProperties3.Append(pictureLocks2);

            nonVisualPictureProperties3.Append(nonVisualDrawingProperties3);
            nonVisualPictureProperties3.Append(nonVisualPictureDrawingProperties3);

            Pic.BlipFill blipFill3 = new Pic.BlipFill();

            A.Blip blip3 = new A.Blip() { Embed = "rId7", CompressionState = A.BlipCompressionValues.Print };

            A.BlipExtensionList blipExtensionList2 = new A.BlipExtensionList();

            A.BlipExtension blipExtension2 = new A.BlipExtension() { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" };

            A14.UseLocalDpi useLocalDpi2 = new A14.UseLocalDpi() { Val = false };
            useLocalDpi2.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");

            blipExtension2.Append(useLocalDpi2);

            blipExtensionList2.Append(blipExtension2);

            blip3.Append(blipExtensionList2);
            A.SourceRectangle sourceRectangle2 = new A.SourceRectangle();

            A.Stretch stretch3 = new A.Stretch();
            A.FillRectangle fillRectangle3 = new A.FillRectangle();

            stretch3.Append(fillRectangle3);

            blipFill3.Append(blip3);
            blipFill3.Append(sourceRectangle2);
            blipFill3.Append(stretch3);

            Pic.ShapeProperties shapeProperties5 = new Pic.ShapeProperties() { BlackWhiteMode = A.BlackWhiteModeValues.Auto };

            A.Transform2D transform2D5 = new A.Transform2D();
            A.Offset offset5 = new A.Offset() { X = 0L, Y = 0L };
            A.Extents extents5 = new A.Extents() { Cx = 504825L, Cy = 495300L };

            transform2D5.Append(offset5);
            transform2D5.Append(extents5);

            A.PresetGeometry presetGeometry5 = new A.PresetGeometry() { Preset = A.ShapeTypeValues.Rectangle };
            A.AdjustValueList adjustValueList5 = new A.AdjustValueList();

            presetGeometry5.Append(adjustValueList5);
            A.NoFill noFill3 = new A.NoFill();

            A.Outline outline4 = new A.Outline();
            A.NoFill noFill4 = new A.NoFill();

            outline4.Append(noFill4);

            shapeProperties5.Append(transform2D5);
            shapeProperties5.Append(presetGeometry5);
            shapeProperties5.Append(noFill3);
            shapeProperties5.Append(outline4);

            picture5.Append(nonVisualPictureProperties3);
            picture5.Append(blipFill3);
            picture5.Append(shapeProperties5);

            graphicData5.Append(picture5);

            graphic5.Append(graphicData5);

            inline3.Append(extent5);
            inline3.Append(effectExtent5);
            inline3.Append(docProperties5);
            inline3.Append(nonVisualGraphicFrameDrawingProperties5);
            inline3.Append(graphic5);

            drawing5.Append(inline3);

            run26.Append(runProperties17);
            run26.Append(drawing5);

            Run run27 = new Run() { RsidRunProperties = "009E19F7" };

            RunProperties runProperties18 = new RunProperties();
            NoProof noProof6 = new NoProof();
            Languages languages21 = new Languages() { EastAsia = "ru-RU" };

            runProperties18.Append(noProof6);
            runProperties18.Append(languages21);

            Drawing drawing6 = new Drawing();

            Wp.Inline inline4 = new Wp.Inline() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)0U, DistanceFromRight = (UInt32Value)0U };
            Wp.Extent extent6 = new Wp.Extent() { Cx = 571500L, Cy = 466725L };
            Wp.EffectExtent effectExtent6 = new Wp.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 9525L };
            Wp.DocProperties docProperties6 = new Wp.DocProperties() { Id = (UInt32Value)5U, Name = "Рисунок 5" };

            Wp.NonVisualGraphicFrameDrawingProperties nonVisualGraphicFrameDrawingProperties6 = new Wp.NonVisualGraphicFrameDrawingProperties();

            A.GraphicFrameLocks graphicFrameLocks4 = new A.GraphicFrameLocks() { NoChangeAspect = true };
            graphicFrameLocks4.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            nonVisualGraphicFrameDrawingProperties6.Append(graphicFrameLocks4);

            A.Graphic graphic6 = new A.Graphic();
            graphic6.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            A.GraphicData graphicData6 = new A.GraphicData() { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" };

            Pic.Picture picture6 = new Pic.Picture();
            picture6.AddNamespaceDeclaration("pic", "http://schemas.openxmlformats.org/drawingml/2006/picture");

            Pic.NonVisualPictureProperties nonVisualPictureProperties4 = new Pic.NonVisualPictureProperties();
            Pic.NonVisualDrawingProperties nonVisualDrawingProperties4 = new Pic.NonVisualDrawingProperties() { Id = (UInt32Value)0U, Name = "Picture 14" };

            Pic.NonVisualPictureDrawingProperties nonVisualPictureDrawingProperties4 = new Pic.NonVisualPictureDrawingProperties();
            A.PictureLocks pictureLocks3 = new A.PictureLocks() { NoChangeAspect = true, NoChangeArrowheads = true };

            nonVisualPictureDrawingProperties4.Append(pictureLocks3);

            nonVisualPictureProperties4.Append(nonVisualDrawingProperties4);
            nonVisualPictureProperties4.Append(nonVisualPictureDrawingProperties4);

            Pic.BlipFill blipFill4 = new Pic.BlipFill();

            A.Blip blip4 = new A.Blip() { Embed = "rId8", CompressionState = A.BlipCompressionValues.Print };

            A.BlipExtensionList blipExtensionList3 = new A.BlipExtensionList();

            A.BlipExtension blipExtension3 = new A.BlipExtension() { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" };

            A14.UseLocalDpi useLocalDpi3 = new A14.UseLocalDpi() { Val = false };
            useLocalDpi3.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");

            blipExtension3.Append(useLocalDpi3);

            blipExtensionList3.Append(blipExtension3);

            blip4.Append(blipExtensionList3);
            A.SourceRectangle sourceRectangle3 = new A.SourceRectangle();

            A.Stretch stretch4 = new A.Stretch();
            A.FillRectangle fillRectangle4 = new A.FillRectangle();

            stretch4.Append(fillRectangle4);

            blipFill4.Append(blip4);
            blipFill4.Append(sourceRectangle3);
            blipFill4.Append(stretch4);

            Pic.ShapeProperties shapeProperties6 = new Pic.ShapeProperties() { BlackWhiteMode = A.BlackWhiteModeValues.Auto };

            A.Transform2D transform2D6 = new A.Transform2D();
            A.Offset offset6 = new A.Offset() { X = 0L, Y = 0L };
            A.Extents extents6 = new A.Extents() { Cx = 571500L, Cy = 466725L };

            transform2D6.Append(offset6);
            transform2D6.Append(extents6);

            A.PresetGeometry presetGeometry6 = new A.PresetGeometry() { Preset = A.ShapeTypeValues.Rectangle };
            A.AdjustValueList adjustValueList6 = new A.AdjustValueList();

            presetGeometry6.Append(adjustValueList6);
            A.NoFill noFill5 = new A.NoFill();

            A.Outline outline5 = new A.Outline();
            A.NoFill noFill6 = new A.NoFill();

            outline5.Append(noFill6);

            shapeProperties6.Append(transform2D6);
            shapeProperties6.Append(presetGeometry6);
            shapeProperties6.Append(noFill5);
            shapeProperties6.Append(outline5);

            picture6.Append(nonVisualPictureProperties4);
            picture6.Append(blipFill4);
            picture6.Append(shapeProperties6);

            graphicData6.Append(picture6);

            graphic6.Append(graphicData6);

            inline4.Append(extent6);
            inline4.Append(effectExtent6);
            inline4.Append(docProperties6);
            inline4.Append(nonVisualGraphicFrameDrawingProperties6);
            inline4.Append(graphic6);

            drawing6.Append(inline4);

            run27.Append(runProperties18);
            run27.Append(drawing6);

            paragraph35.Append(paragraphProperties35);
            paragraph35.Append(run26);
            paragraph35.Append(run27);










            Table table2 = new Table();

            TableProperties tableProperties2 = new TableProperties();
            TableStyle tableStyle2 = new TableStyle() { Val = "a3" };
            TableWidth tableWidth2 = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };
            TableLayout tableLayout1 = new TableLayout() { Type = TableLayoutValues.Fixed };
            TableLook tableLook2 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties2.Append(tableStyle2);
            tableProperties2.Append(tableWidth2);
            tableProperties2.Append(tableLayout1);
            tableProperties2.Append(tableLook2);

            TableGrid tableGrid2 = new TableGrid();
            GridColumn gridColumn2 = new GridColumn() { Width = "2405" };
            GridColumn gridColumn3 = new GridColumn() { Width = "1848" };
            GridColumn gridColumn4 = new GridColumn() { Width = "945" };
            GridColumn gridColumn5 = new GridColumn() { Width = "756" };
            GridColumn gridColumn6 = new GridColumn() { Width = "1843" };
            GridColumn gridColumn7 = new GridColumn() { Width = "1180" };
            GridColumn gridColumn8 = new GridColumn() { Width = "598" };
            GridColumn gridColumn9 = new GridColumn() { Width = "916" };
            GridColumn gridColumn10 = new GridColumn() { Width = "567" };

            tableGrid2.Append(gridColumn2);
            tableGrid2.Append(gridColumn3);
            tableGrid2.Append(gridColumn4);
            tableGrid2.Append(gridColumn5);
            tableGrid2.Append(gridColumn6);
            tableGrid2.Append(gridColumn7);
            tableGrid2.Append(gridColumn8);
            tableGrid2.Append(gridColumn9);
            tableGrid2.Append(gridColumn10);

            TableRow tableRow2 = new TableRow() { RsidTableRowAddition = "004F2046", RsidTableRowProperties = "004F2046" };

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "2405", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellVerticalAlignment1);

            Paragraph paragraph12 = new Paragraph() { RsidParagraphMarkRevision = "006D06A1", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties12 = new ParagraphProperties();
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties12.Append(justification4);

            Run run12 = new Run() { RsidRunProperties = "006D06A1" };
            Text text8 = new Text();
            text8.Text = "НАИМЕНОВАНИЕ";

            run12.Append(text8);

            paragraph12.Append(paragraphProperties12);
            paragraph12.Append(run12);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph12);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "1848", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellVerticalAlignment2);

            Paragraph paragraph13 = new Paragraph() { RsidParagraphMarkRevision = "006D06A1", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties13 = new ParagraphProperties();
            Justification justification5 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties13.Append(justification5);

            Run run13 = new Run() { RsidRunProperties = "006D06A1" };
            Text text9 = new Text();
            text9.Text = "ТЕХ. ХАРАКТ";

            run13.Append(text9);

            paragraph13.Append(paragraphProperties13);
            paragraph13.Append(run13);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph13);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "945", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellVerticalAlignment3);

            Paragraph paragraph14 = new Paragraph() { RsidParagraphMarkRevision = "006D06A1", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties14 = new ParagraphProperties();
            Justification justification6 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties14.Append(justification6);

            Run run14 = new Run() { RsidRunProperties = "006D06A1" };
            Text text10 = new Text();
            text10.Text = "ЗАВ. НОМЕР";

            run14.Append(text10);

            paragraph14.Append(paragraphProperties14);
            paragraph14.Append(run14);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph14);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "756", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellVerticalAlignment4);

            Paragraph paragraph15 = new Paragraph() { RsidParagraphMarkRevision = "006D06A1", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties15 = new ParagraphProperties();
            Justification justification7 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties15.Append(justification7);

            Run run15 = new Run() { RsidRunProperties = "006D06A1" };
            Text text11 = new Text();
            text11.Text = "Частота";

            run15.Append(text11);

            paragraph15.Append(paragraphProperties15);
            paragraph15.Append(run15);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph15);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "1843", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment5 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellVerticalAlignment5);

            Paragraph paragraph16 = new Paragraph() { RsidParagraphMarkRevision = "006D06A1", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties16 = new ParagraphProperties();
            Justification justification8 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties16.Append(justification8);

            Run run16 = new Run() { RsidRunProperties = "006D06A1" };
            Text text12 = new Text();
            text12.Text = "ГОСТ";

            run16.Append(text12);

            paragraph16.Append(paragraphProperties16);
            paragraph16.Append(run16);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph16);

            TableCell tableCell7 = new TableCell();

            TableCellProperties tableCellProperties7 = new TableCellProperties();
            TableCellWidth tableCellWidth7 = new TableCellWidth() { Width = "1180", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment6 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties7.Append(tableCellWidth7);
            tableCellProperties7.Append(tableCellVerticalAlignment6);

            Paragraph paragraph17 = new Paragraph() { RsidParagraphMarkRevision = "006D06A1", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties17 = new ParagraphProperties();
            Justification justification9 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties17.Append(justification9);

            Run run17 = new Run() { RsidRunProperties = "006D06A1" };
            Text text13 = new Text();
            text13.Text = "МАССА";

            run17.Append(text13);

            paragraph17.Append(paragraphProperties17);
            paragraph17.Append(run17);

            tableCell7.Append(tableCellProperties7);
            tableCell7.Append(paragraph17);

            TableCell tableCell8 = new TableCell();

            TableCellProperties tableCellProperties8 = new TableCellProperties();
            TableCellWidth tableCellWidth8 = new TableCellWidth() { Width = "598", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment7 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties8.Append(tableCellWidth8);
            tableCellProperties8.Append(tableCellVerticalAlignment7);

            Paragraph paragraph18 = new Paragraph() { RsidParagraphMarkRevision = "006D06A1", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties18 = new ParagraphProperties();
            Justification justification10 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties18.Append(justification10);

            Run run18 = new Run() { RsidRunProperties = "006D06A1" };
            Text text14 = new Text();
            text14.Text = "ГОД";

            run18.Append(text14);

            paragraph18.Append(paragraphProperties18);
            paragraph18.Append(run18);

            tableCell8.Append(tableCellProperties8);
            tableCell8.Append(paragraph18);

            TableCell tableCell9 = new TableCell();

            TableCellProperties tableCellProperties9 = new TableCellProperties();
            TableCellWidth tableCellWidth9 = new TableCellWidth() { Width = "916", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment8 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties9.Append(tableCellWidth9);
            tableCellProperties9.Append(tableCellVerticalAlignment8);

            Paragraph paragraph19 = new Paragraph() { RsidParagraphMarkRevision = "006D06A1", RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties19 = new ParagraphProperties();
            Justification justification11 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties19.Append(justification11);

            Run run19 = new Run() { RsidRunProperties = "006D06A1" };
            Text text15 = new Text();
            text15.Text = "Свободное поле";

            run19.Append(text15);

            paragraph19.Append(paragraphProperties19);
            paragraph19.Append(run19);

            tableCell9.Append(tableCellProperties9);
            tableCell9.Append(paragraph19);

            TableCell tableCell10 = new TableCell();

            TableCellProperties tableCellProperties10 = new TableCellProperties();
            TableCellWidth tableCellWidth10 = new TableCellWidth() { Width = "567", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment9 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties10.Append(tableCellWidth10);
            tableCellProperties10.Append(tableCellVerticalAlignment9);

            Paragraph paragraph20 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "004F2046", RsidRunAdditionDefault = "004F2046" };

            ParagraphProperties paragraphProperties20 = new ParagraphProperties();
            Justification justification12 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties20.Append(justification12);

            Run run20 = new Run() { RsidRunProperties = "006D06A1" };
            Text text16 = new Text();
            text16.Text = "Кол-во";

            run20.Append(text16);

            paragraph20.Append(paragraphProperties20);
            paragraph20.Append(run20);

            tableCell10.Append(tableCellProperties10);
            tableCell10.Append(paragraph20);

            tableRow2.Append(tableCell2);
            tableRow2.Append(tableCell3);
            tableRow2.Append(tableCell4);
            tableRow2.Append(tableCell5);
            tableRow2.Append(tableCell6);
            tableRow2.Append(tableCell7);
            tableRow2.Append(tableCell8);
            tableRow2.Append(tableCell9);
            tableRow2.Append(tableCell10);
            table2.Append(tableProperties2);
            table2.Append(tableGrid2);
            table2.Append(tableRow2);

            foreach (var data in Id)
            {
                PZ_PlanZakaz pZ_PlanZakaz = new PZ_PlanZakaz();
                pZ_PlanZakaz = db.PZ_PlanZakaz.Find(data);

                TableRow tableRow3 = new TableRow() { RsidTableRowAddition = "004F2046", RsidTableRowProperties = "004F2046" };









                TableCell tableCell11 = new TableCell();

                TableCellProperties tableCellProperties11 = new TableCellProperties();
                TableCellWidth tableCellWidth11 = new TableCellWidth() { Width = "2405", Type = TableWidthUnitValues.Dxa };

                tableCellProperties11.Append(tableCellWidth11);

                Paragraph paragraph21 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "009A507F", RsidRunAdditionDefault = "004F2046" };

                ParagraphProperties paragraphProperties21 = new ParagraphProperties();

                ParagraphMarkRunProperties paragraphMarkRunProperties12 = new ParagraphMarkRunProperties();
                RunFonts runFonts23 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                FontSize fontSize23 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript23 = new FontSizeComplexScript() { Val = "24" };

                paragraphMarkRunProperties12.Append(runFonts23);
                paragraphMarkRunProperties12.Append(fontSize23);
                paragraphMarkRunProperties12.Append(fontSizeComplexScript23);

                paragraphProperties21.Append(paragraphMarkRunProperties12);
                
                Run runName = new Run() { RsidRunProperties = "006D06A1" };
                Text textName = new Text();
                textName.Text = pZ_PlanZakaz.Name;
                runName.Append(textName);
                paragraph21.Append(runName);


                paragraph21.Append(paragraphProperties21);
                tableCell11.Append(tableCellProperties11);
                tableCell11.Append(paragraph21);
                
                TableCell tableCell12 = new TableCell();

                TableCellProperties tableCellProperties12 = new TableCellProperties();
                TableCellWidth tableCellWidth12 = new TableCellWidth() { Width = "1848", Type = TableWidthUnitValues.Dxa };

                tableCellProperties12.Append(tableCellWidth12);

                Paragraph paragraph22 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "009A507F", RsidRunAdditionDefault = "004F2046" };

                ParagraphProperties paragraphProperties22 = new ParagraphProperties();

                ParagraphMarkRunProperties paragraphMarkRunProperties13 = new ParagraphMarkRunProperties();
                RunFonts runFonts24 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                FontSize fontSize24 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript24 = new FontSizeComplexScript() { Val = "24" };

                paragraphMarkRunProperties13.Append(runFonts24);
                paragraphMarkRunProperties13.Append(fontSize24);
                paragraphMarkRunProperties13.Append(fontSizeComplexScript24);

                paragraphProperties22.Append(paragraphMarkRunProperties13);

                Run runNameTU = new Run() { RsidRunProperties = "006D06A1" };
                Text textNameTU = new Text();
                textNameTU.Text = pZ_PlanZakaz.nameTU;
                runNameTU.Append(textNameTU);
                paragraph22.Append(runNameTU);

                paragraph22.Append(paragraphProperties22);

                tableCell12.Append(tableCellProperties12);
                tableCell12.Append(paragraph22);

                TableCell tableCell13 = new TableCell();

                TableCellProperties tableCellProperties13 = new TableCellProperties();
                TableCellWidth tableCellWidth13 = new TableCellWidth() { Width = "945", Type = TableWidthUnitValues.Dxa };

                tableCellProperties13.Append(tableCellWidth13);

                Paragraph paragraph23 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "009A507F", RsidRunAdditionDefault = "004F2046" };

                ParagraphProperties paragraphProperties23 = new ParagraphProperties();

                ParagraphMarkRunProperties paragraphMarkRunProperties14 = new ParagraphMarkRunProperties();
                RunFonts runFonts25 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                FontSize fontSize25 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript25 = new FontSizeComplexScript() { Val = "24" };

                paragraphMarkRunProperties14.Append(runFonts25);
                paragraphMarkRunProperties14.Append(fontSize25);
                paragraphMarkRunProperties14.Append(fontSizeComplexScript25);

                paragraphProperties23.Append(paragraphMarkRunProperties14);

                Run runNamePZ = new Run() { RsidRunProperties = "006D06A1" };
                Text textNamePZ = new Text();
                textNamePZ.Text = pZ_PlanZakaz.PlanZakaz.ToString();
                runNamePZ.Append(textNamePZ);
                paragraph23.Append(runNamePZ);

                paragraph23.Append(paragraphProperties23);

                tableCell13.Append(tableCellProperties13);
                tableCell13.Append(paragraph23);

                TableCell tableCell14 = new TableCell();

                TableCellProperties tableCellProperties14 = new TableCellProperties();
                TableCellWidth tableCellWidth14 = new TableCellWidth() { Width = "756", Type = TableWidthUnitValues.Dxa };

                tableCellProperties14.Append(tableCellWidth14);

                Paragraph paragraph24 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "009A507F", RsidRunAdditionDefault = "004F2046" };

                ParagraphProperties paragraphProperties24 = new ParagraphProperties();

                ParagraphMarkRunProperties paragraphMarkRunProperties15 = new ParagraphMarkRunProperties();
                RunFonts runFonts26 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                FontSize fontSize26 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript26 = new FontSizeComplexScript() { Val = "24" };

                paragraphMarkRunProperties15.Append(runFonts26);
                paragraphMarkRunProperties15.Append(fontSize26);
                paragraphMarkRunProperties15.Append(fontSizeComplexScript26);

                paragraphProperties24.Append(paragraphMarkRunProperties15);

                Run runName50 = new Run() { RsidRunProperties = "006D06A1" };
                Text textName50 = new Text();
                textName50.Text = "50 Гц";
                runName50.Append(textName50);
                paragraph24.Append(runName50);

                paragraph24.Append(paragraphProperties24);

                tableCell14.Append(tableCellProperties14);
                tableCell14.Append(paragraph24);

                TableCell tableCell15 = new TableCell();

                TableCellProperties tableCellProperties15 = new TableCellProperties();
                TableCellWidth tableCellWidth15 = new TableCellWidth() { Width = "1843", Type = TableWidthUnitValues.Dxa };

                tableCellProperties15.Append(tableCellWidth15);

                Paragraph paragraph25 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "009A507F", RsidRunAdditionDefault = "004F2046" };

                ParagraphProperties paragraphProperties25 = new ParagraphProperties();

                ParagraphMarkRunProperties paragraphMarkRunProperties16 = new ParagraphMarkRunProperties();
                RunFonts runFonts27 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                FontSize fontSize27 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript27 = new FontSizeComplexScript() { Val = "24" };

                paragraphMarkRunProperties16.Append(runFonts27);
                paragraphMarkRunProperties16.Append(fontSize27);
                paragraphMarkRunProperties16.Append(fontSizeComplexScript27);

                paragraphProperties25.Append(paragraphMarkRunProperties16);

                Run runGOST = new Run() { RsidRunProperties = "006D06A1" };
                Text textGOST = new Text();
                textGOST.Text = pZ_PlanZakaz.PZ_ProductType.tu;
                runGOST.Append(textGOST);
                paragraph25.Append(runGOST);

                paragraph25.Append(paragraphProperties25);

                tableCell15.Append(tableCellProperties15);
                tableCell15.Append(paragraph25);

                TableCell tableCell16 = new TableCell();

                TableCellProperties tableCellProperties16 = new TableCellProperties();
                TableCellWidth tableCellWidth16 = new TableCellWidth() { Width = "1180", Type = TableWidthUnitValues.Dxa };

                tableCellProperties16.Append(tableCellWidth16);

                Paragraph paragraph26 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "009A507F", RsidRunAdditionDefault = "004F2046" };

                ParagraphProperties paragraphProperties26 = new ParagraphProperties();

                ParagraphMarkRunProperties paragraphMarkRunProperties17 = new ParagraphMarkRunProperties();
                RunFonts runFonts28 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                FontSize fontSize28 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript28 = new FontSizeComplexScript() { Val = "24" };

                paragraphMarkRunProperties17.Append(runFonts28);
                paragraphMarkRunProperties17.Append(fontSize28);
                paragraphMarkRunProperties17.Append(fontSizeComplexScript28);

                paragraphProperties26.Append(paragraphMarkRunProperties17);

                paragraph26.Append(paragraphProperties26);

                tableCell16.Append(tableCellProperties16);
                tableCell16.Append(paragraph26);

                TableCell tableCell17 = new TableCell();

                TableCellProperties tableCellProperties17 = new TableCellProperties();
                TableCellWidth tableCellWidth17 = new TableCellWidth() { Width = "598", Type = TableWidthUnitValues.Dxa };

                tableCellProperties17.Append(tableCellWidth17);

                Paragraph paragraph27 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "009A507F", RsidRunAdditionDefault = "004F2046" };

                ParagraphProperties paragraphProperties27 = new ParagraphProperties();

                ParagraphMarkRunProperties paragraphMarkRunProperties18 = new ParagraphMarkRunProperties();
                RunFonts runFonts29 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                FontSize fontSize29 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript29 = new FontSizeComplexScript() { Val = "24" };

                paragraphMarkRunProperties18.Append(runFonts29);
                paragraphMarkRunProperties18.Append(fontSize29);
                paragraphMarkRunProperties18.Append(fontSizeComplexScript29);

                paragraphProperties27.Append(paragraphMarkRunProperties18);

                Run runYear = new Run() { RsidRunProperties = "006D06A1" };
                Text textYear = new Text();
                textYear.Text = DateTime.Now.Year.ToString();
                runYear.Append(textYear);
                paragraph27.Append(runYear);

                paragraph27.Append(paragraphProperties27);

                tableCell17.Append(tableCellProperties17);
                tableCell17.Append(paragraph27);

                TableCell tableCell18 = new TableCell();

                TableCellProperties tableCellProperties18 = new TableCellProperties();
                TableCellWidth tableCellWidth18 = new TableCellWidth() { Width = "916", Type = TableWidthUnitValues.Dxa };

                tableCellProperties18.Append(tableCellWidth18);

                Paragraph paragraph28 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "009A507F", RsidRunAdditionDefault = "004F2046" };

                ParagraphProperties paragraphProperties28 = new ParagraphProperties();

                ParagraphMarkRunProperties paragraphMarkRunProperties19 = new ParagraphMarkRunProperties();
                RunFonts runFonts30 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                FontSize fontSize30 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript30 = new FontSizeComplexScript() { Val = "24" };

                paragraphMarkRunProperties19.Append(runFonts30);
                paragraphMarkRunProperties19.Append(fontSize30);
                paragraphMarkRunProperties19.Append(fontSizeComplexScript30);

                paragraphProperties28.Append(paragraphMarkRunProperties19);

                paragraph28.Append(paragraphProperties28);

                tableCell18.Append(tableCellProperties18);
                tableCell18.Append(paragraph28);

                TableCell tableCell19 = new TableCell();

                TableCellProperties tableCellProperties19 = new TableCellProperties();
                TableCellWidth tableCellWidth19 = new TableCellWidth() { Width = "567", Type = TableWidthUnitValues.Dxa };

                tableCellProperties19.Append(tableCellWidth19);

                Paragraph paragraph29 = new Paragraph() { RsidParagraphAddition = "004F2046", RsidParagraphProperties = "009A507F", RsidRunAdditionDefault = "004F2046" };

                ParagraphProperties paragraphProperties29 = new ParagraphProperties();

                ParagraphMarkRunProperties paragraphMarkRunProperties20 = new ParagraphMarkRunProperties();
                RunFonts runFonts31 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                FontSize fontSize31 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript31 = new FontSizeComplexScript() { Val = "24" };

                paragraphMarkRunProperties20.Append(runFonts31);
                paragraphMarkRunProperties20.Append(fontSize31);
                paragraphMarkRunProperties20.Append(fontSizeComplexScript31);

                paragraphProperties29.Append(paragraphMarkRunProperties20);


                Run runCount = new Run() { RsidRunProperties = "006D06A1" };
                Text textCount = new Text();
                textCount.Text = "1";
                runCount.Append(textCount);
                paragraph29.Append(runCount);




                paragraph29.Append(paragraphProperties29);

                tableCell19.Append(tableCellProperties19);
                tableCell19.Append(paragraph29);

                tableRow3.Append(tableCell11);
                tableRow3.Append(tableCell12);
                tableRow3.Append(tableCell13);
                tableRow3.Append(tableCell14);
                tableRow3.Append(tableCell15);
                tableRow3.Append(tableCell16);
                tableRow3.Append(tableCell17);
                tableRow3.Append(tableCell18);
                tableRow3.Append(tableCell19);
                table2.Append(tableRow3);
            }
            


            SectionProperties sectionProperties1 = new SectionProperties() { RsidRPr = "004F2046", RsidR = "004F2046", RsidSect = "004F2046" };
            PageSize pageSize1 = new PageSize() { Width = (UInt32Value)11906U, Height = (UInt32Value)16838U };
            PageMargin pageMargin1 = new PageMargin() { Top = 340, Right = (UInt32Value)340U, Bottom = 340, Left = (UInt32Value)340U, Header = (UInt32Value)709U, Footer = (UInt32Value)709U, Gutter = (UInt32Value)0U };
            Columns columns1 = new Columns() { Space = "708" };
            DocGrid docGrid1 = new DocGrid() { LinePitch = 360 };

            sectionProperties1.Append(pageSize1);
            sectionProperties1.Append(pageMargin1);
            sectionProperties1.Append(columns1);
            sectionProperties1.Append(docGrid1);

            body1.Append(paragraph1);
            body1.Append(paragraph2);
            body1.Append(paragraph3);
            body1.Append(paragraph4);
            body1.Append(table1);
            body1.Append(paragraph8);
            body1.Append(paragraph9);
            body1.Append(paragraph10);
            body1.Append(paragraph11);
            body1.Append(table2);
            body1.Append(paragraph30);
            body1.Append(paragraph31);
            body1.Append(paragraph32);
            body1.Append(paragraph33);
            body1.Append(paragraph34);
            body1.Append(paragraph35);
            body1.Append(sectionProperties1);

            document1.Append(body1);

            mainDocumentPart1.Document = document1;
        }

        // Generates content of imagePart1.
        private void GenerateImagePart1Content(ImagePart imagePart1)
        {
            System.IO.Stream data = GetBinaryDataStream(imagePart1Data);
            imagePart1.FeedData(data);
            data.Close();
        }

        // Generates content of documentSettingsPart1.
        private void GenerateDocumentSettingsPart1Content(DocumentSettingsPart documentSettingsPart1)
        {
            Settings settings1 = new Settings() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15" } };
            settings1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            settings1.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
            settings1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            settings1.AddNamespaceDeclaration("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
            settings1.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
            settings1.AddNamespaceDeclaration("w10", "urn:schemas-microsoft-com:office:word");
            settings1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            settings1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            settings1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");
            settings1.AddNamespaceDeclaration("sl", "http://schemas.openxmlformats.org/schemaLibrary/2006/main");
            Zoom zoom1 = new Zoom() { Percent = "100" };
            ProofState proofState1 = new ProofState() { Spelling = ProofingStateValues.Clean, Grammar = ProofingStateValues.Clean };
            AttachedTemplate attachedTemplate1 = new AttachedTemplate() { Id = "rId1" };
            DefaultTabStop defaultTabStop1 = new DefaultTabStop() { Val = 708 };
            CharacterSpacingControl characterSpacingControl1 = new CharacterSpacingControl() { Val = CharacterSpacingValues.DoNotCompress };

            Compatibility compatibility1 = new Compatibility();
            CompatibilitySetting compatibilitySetting1 = new CompatibilitySetting() { Name = CompatSettingNameValues.CompatibilityMode, Uri = "http://schemas.microsoft.com/office/word", Val = "15" };
            CompatibilitySetting compatibilitySetting2 = new CompatibilitySetting() { Name = CompatSettingNameValues.OverrideTableStyleFontSizeAndJustification, Uri = "http://schemas.microsoft.com/office/word", Val = "1" };
            CompatibilitySetting compatibilitySetting3 = new CompatibilitySetting() { Name = CompatSettingNameValues.EnableOpenTypeFeatures, Uri = "http://schemas.microsoft.com/office/word", Val = "1" };
            CompatibilitySetting compatibilitySetting4 = new CompatibilitySetting() { Name = CompatSettingNameValues.DoNotFlipMirrorIndents, Uri = "http://schemas.microsoft.com/office/word", Val = "1" };
            CompatibilitySetting compatibilitySetting5 = new CompatibilitySetting() { Name = new EnumValue<CompatSettingNameValues>() { InnerText = "differentiateMultirowTableHeaders" }, Uri = "http://schemas.microsoft.com/office/word", Val = "1" };

            compatibility1.Append(compatibilitySetting1);
            compatibility1.Append(compatibilitySetting2);
            compatibility1.Append(compatibilitySetting3);
            compatibility1.Append(compatibilitySetting4);
            compatibility1.Append(compatibilitySetting5);

            Rsids rsids1 = new Rsids();
            RsidRoot rsidRoot1 = new RsidRoot() { Val = "00FA438E" };
            Rsid rsid1 = new Rsid() { Val = "004F2046" };
            Rsid rsid2 = new Rsid() { Val = "006E2D0E" };
            Rsid rsid3 = new Rsid() { Val = "00FA438E" };

            rsids1.Append(rsidRoot1);
            rsids1.Append(rsid1);
            rsids1.Append(rsid2);
            rsids1.Append(rsid3);

            M.MathProperties mathProperties1 = new M.MathProperties();
            M.MathFont mathFont1 = new M.MathFont() { Val = "Cambria Math" };
            M.BreakBinary breakBinary1 = new M.BreakBinary() { Val = M.BreakBinaryOperatorValues.Before };
            M.BreakBinarySubtraction breakBinarySubtraction1 = new M.BreakBinarySubtraction() { Val = M.BreakBinarySubtractionValues.MinusMinus };
            M.SmallFraction smallFraction1 = new M.SmallFraction() { Val = M.BooleanValues.Zero };
            M.DisplayDefaults displayDefaults1 = new M.DisplayDefaults();
            M.LeftMargin leftMargin1 = new M.LeftMargin() { Val = (UInt32Value)0U };
            M.RightMargin rightMargin1 = new M.RightMargin() { Val = (UInt32Value)0U };
            M.DefaultJustification defaultJustification1 = new M.DefaultJustification() { Val = M.JustificationValues.CenterGroup };
            M.WrapIndent wrapIndent1 = new M.WrapIndent() { Val = (UInt32Value)1440U };
            M.IntegralLimitLocation integralLimitLocation1 = new M.IntegralLimitLocation() { Val = M.LimitLocationValues.SubscriptSuperscript };
            M.NaryLimitLocation naryLimitLocation1 = new M.NaryLimitLocation() { Val = M.LimitLocationValues.UnderOver };

            mathProperties1.Append(mathFont1);
            mathProperties1.Append(breakBinary1);
            mathProperties1.Append(breakBinarySubtraction1);
            mathProperties1.Append(smallFraction1);
            mathProperties1.Append(displayDefaults1);
            mathProperties1.Append(leftMargin1);
            mathProperties1.Append(rightMargin1);
            mathProperties1.Append(defaultJustification1);
            mathProperties1.Append(wrapIndent1);
            mathProperties1.Append(integralLimitLocation1);
            mathProperties1.Append(naryLimitLocation1);
            ThemeFontLanguages themeFontLanguages1 = new ThemeFontLanguages() { Val = "ru-RU" };
            ColorSchemeMapping colorSchemeMapping1 = new ColorSchemeMapping() { Background1 = ColorSchemeIndexValues.Light1, Text1 = ColorSchemeIndexValues.Dark1, Background2 = ColorSchemeIndexValues.Light2, Text2 = ColorSchemeIndexValues.Dark2, Accent1 = ColorSchemeIndexValues.Accent1, Accent2 = ColorSchemeIndexValues.Accent2, Accent3 = ColorSchemeIndexValues.Accent3, Accent4 = ColorSchemeIndexValues.Accent4, Accent5 = ColorSchemeIndexValues.Accent5, Accent6 = ColorSchemeIndexValues.Accent6, Hyperlink = ColorSchemeIndexValues.Hyperlink, FollowedHyperlink = ColorSchemeIndexValues.FollowedHyperlink };

            ShapeDefaults shapeDefaults1 = new ShapeDefaults();
            Ovml.ShapeDefaults shapeDefaults2 = new Ovml.ShapeDefaults() { Extension = V.ExtensionHandlingBehaviorValues.Edit, MaxShapeId = 1026 };

            Ovml.ShapeLayout shapeLayout1 = new Ovml.ShapeLayout() { Extension = V.ExtensionHandlingBehaviorValues.Edit };
            Ovml.ShapeIdMap shapeIdMap1 = new Ovml.ShapeIdMap() { Extension = V.ExtensionHandlingBehaviorValues.Edit, Data = "1" };

            shapeLayout1.Append(shapeIdMap1);

            shapeDefaults1.Append(shapeDefaults2);
            shapeDefaults1.Append(shapeLayout1);
            DecimalSymbol decimalSymbol1 = new DecimalSymbol() { Val = "," };
            ListSeparator listSeparator1 = new ListSeparator() { Val = ";" };
            OpenXmlUnknownElement openXmlUnknownElement1 = OpenXmlUnknownElement.CreateOpenXmlUnknownElement("<w15:chartTrackingRefBased xmlns:w15=\"http://schemas.microsoft.com/office/word/2012/wordml\" />");

            OpenXmlUnknownElement openXmlUnknownElement2 = OpenXmlUnknownElement.CreateOpenXmlUnknownElement("<w15:docId w15:val=\"{A108810E-BFFC-4FAC-A2F5-A28C115DC11C}\" xmlns:w15=\"http://schemas.microsoft.com/office/word/2012/wordml\" />");

            settings1.Append(zoom1);
            settings1.Append(proofState1);
            settings1.Append(attachedTemplate1);
            settings1.Append(defaultTabStop1);
            settings1.Append(characterSpacingControl1);
            settings1.Append(compatibility1);
            settings1.Append(rsids1);
            settings1.Append(mathProperties1);
            settings1.Append(themeFontLanguages1);
            settings1.Append(colorSchemeMapping1);
            settings1.Append(shapeDefaults1);
            settings1.Append(decimalSymbol1);
            settings1.Append(listSeparator1);
            settings1.Append(openXmlUnknownElement1);
            settings1.Append(openXmlUnknownElement2);

            documentSettingsPart1.Settings = settings1;
        }

        // Generates content of imagePart2.
        private void GenerateImagePart2Content(ImagePart imagePart2)
        {
            System.IO.Stream data = GetBinaryDataStream(imagePart2Data);
            imagePart2.FeedData(data);
            data.Close();
        }

        // Generates content of styleDefinitionsPart1.
        private void GenerateStyleDefinitionsPart1Content(StyleDefinitionsPart styleDefinitionsPart1)
        {
            Styles styles1 = new Styles() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15" } };
            styles1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            styles1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            styles1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            styles1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            styles1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");

            DocDefaults docDefaults1 = new DocDefaults();

            RunPropertiesDefault runPropertiesDefault1 = new RunPropertiesDefault();

            RunPropertiesBaseStyle runPropertiesBaseStyle1 = new RunPropertiesBaseStyle();
            RunFonts runFonts43 = new RunFonts() { AsciiTheme = ThemeFontValues.MinorHighAnsi, HighAnsiTheme = ThemeFontValues.MinorHighAnsi, EastAsiaTheme = ThemeFontValues.MinorHighAnsi, ComplexScriptTheme = ThemeFontValues.MinorBidi };
            FontSize fontSize43 = new FontSize() { Val = "22" };
            FontSizeComplexScript fontSizeComplexScript43 = new FontSizeComplexScript() { Val = "22" };
            Languages languages22 = new Languages() { Val = "ru-RU", EastAsia = "en-US", Bidi = "ar-SA" };

            runPropertiesBaseStyle1.Append(runFonts43);
            runPropertiesBaseStyle1.Append(fontSize43);
            runPropertiesBaseStyle1.Append(fontSizeComplexScript43);
            runPropertiesBaseStyle1.Append(languages22);

            runPropertiesDefault1.Append(runPropertiesBaseStyle1);

            ParagraphPropertiesDefault paragraphPropertiesDefault1 = new ParagraphPropertiesDefault();

            ParagraphPropertiesBaseStyle paragraphPropertiesBaseStyle1 = new ParagraphPropertiesBaseStyle();
            SpacingBetweenLines spacingBetweenLines15 = new SpacingBetweenLines() { After = "160", Line = "259", LineRule = LineSpacingRuleValues.Auto };

            paragraphPropertiesBaseStyle1.Append(spacingBetweenLines15);

            paragraphPropertiesDefault1.Append(paragraphPropertiesBaseStyle1);

            docDefaults1.Append(runPropertiesDefault1);
            docDefaults1.Append(paragraphPropertiesDefault1);

            LatentStyles latentStyles1 = new LatentStyles() { DefaultLockedState = false, DefaultUiPriority = 99, DefaultSemiHidden = false, DefaultUnhideWhenUsed = false, DefaultPrimaryStyle = false, Count = 371 };
            LatentStyleExceptionInfo latentStyleExceptionInfo1 = new LatentStyleExceptionInfo() { Name = "Normal", UiPriority = 0, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo2 = new LatentStyleExceptionInfo() { Name = "heading 1", UiPriority = 9, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo3 = new LatentStyleExceptionInfo() { Name = "heading 2", UiPriority = 9, SemiHidden = true, UnhideWhenUsed = true, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo4 = new LatentStyleExceptionInfo() { Name = "heading 3", UiPriority = 9, SemiHidden = true, UnhideWhenUsed = true, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo5 = new LatentStyleExceptionInfo() { Name = "heading 4", UiPriority = 9, SemiHidden = true, UnhideWhenUsed = true, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo6 = new LatentStyleExceptionInfo() { Name = "heading 5", UiPriority = 9, SemiHidden = true, UnhideWhenUsed = true, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo7 = new LatentStyleExceptionInfo() { Name = "heading 6", UiPriority = 9, SemiHidden = true, UnhideWhenUsed = true, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo8 = new LatentStyleExceptionInfo() { Name = "heading 7", UiPriority = 9, SemiHidden = true, UnhideWhenUsed = true, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo9 = new LatentStyleExceptionInfo() { Name = "heading 8", UiPriority = 9, SemiHidden = true, UnhideWhenUsed = true, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo10 = new LatentStyleExceptionInfo() { Name = "heading 9", UiPriority = 9, SemiHidden = true, UnhideWhenUsed = true, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo11 = new LatentStyleExceptionInfo() { Name = "index 1", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo12 = new LatentStyleExceptionInfo() { Name = "index 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo13 = new LatentStyleExceptionInfo() { Name = "index 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo14 = new LatentStyleExceptionInfo() { Name = "index 4", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo15 = new LatentStyleExceptionInfo() { Name = "index 5", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo16 = new LatentStyleExceptionInfo() { Name = "index 6", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo17 = new LatentStyleExceptionInfo() { Name = "index 7", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo18 = new LatentStyleExceptionInfo() { Name = "index 8", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo19 = new LatentStyleExceptionInfo() { Name = "index 9", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo20 = new LatentStyleExceptionInfo() { Name = "toc 1", UiPriority = 39, SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo21 = new LatentStyleExceptionInfo() { Name = "toc 2", UiPriority = 39, SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo22 = new LatentStyleExceptionInfo() { Name = "toc 3", UiPriority = 39, SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo23 = new LatentStyleExceptionInfo() { Name = "toc 4", UiPriority = 39, SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo24 = new LatentStyleExceptionInfo() { Name = "toc 5", UiPriority = 39, SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo25 = new LatentStyleExceptionInfo() { Name = "toc 6", UiPriority = 39, SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo26 = new LatentStyleExceptionInfo() { Name = "toc 7", UiPriority = 39, SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo27 = new LatentStyleExceptionInfo() { Name = "toc 8", UiPriority = 39, SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo28 = new LatentStyleExceptionInfo() { Name = "toc 9", UiPriority = 39, SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo29 = new LatentStyleExceptionInfo() { Name = "Normal Indent", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo30 = new LatentStyleExceptionInfo() { Name = "footnote text", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo31 = new LatentStyleExceptionInfo() { Name = "annotation text", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo32 = new LatentStyleExceptionInfo() { Name = "header", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo33 = new LatentStyleExceptionInfo() { Name = "footer", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo34 = new LatentStyleExceptionInfo() { Name = "index heading", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo35 = new LatentStyleExceptionInfo() { Name = "caption", UiPriority = 35, SemiHidden = true, UnhideWhenUsed = true, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo36 = new LatentStyleExceptionInfo() { Name = "table of figures", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo37 = new LatentStyleExceptionInfo() { Name = "envelope address", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo38 = new LatentStyleExceptionInfo() { Name = "envelope return", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo39 = new LatentStyleExceptionInfo() { Name = "footnote reference", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo40 = new LatentStyleExceptionInfo() { Name = "annotation reference", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo41 = new LatentStyleExceptionInfo() { Name = "line number", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo42 = new LatentStyleExceptionInfo() { Name = "page number", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo43 = new LatentStyleExceptionInfo() { Name = "endnote reference", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo44 = new LatentStyleExceptionInfo() { Name = "endnote text", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo45 = new LatentStyleExceptionInfo() { Name = "table of authorities", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo46 = new LatentStyleExceptionInfo() { Name = "macro", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo47 = new LatentStyleExceptionInfo() { Name = "toa heading", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo48 = new LatentStyleExceptionInfo() { Name = "List", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo49 = new LatentStyleExceptionInfo() { Name = "List Bullet", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo50 = new LatentStyleExceptionInfo() { Name = "List Number", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo51 = new LatentStyleExceptionInfo() { Name = "List 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo52 = new LatentStyleExceptionInfo() { Name = "List 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo53 = new LatentStyleExceptionInfo() { Name = "List 4", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo54 = new LatentStyleExceptionInfo() { Name = "List 5", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo55 = new LatentStyleExceptionInfo() { Name = "List Bullet 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo56 = new LatentStyleExceptionInfo() { Name = "List Bullet 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo57 = new LatentStyleExceptionInfo() { Name = "List Bullet 4", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo58 = new LatentStyleExceptionInfo() { Name = "List Bullet 5", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo59 = new LatentStyleExceptionInfo() { Name = "List Number 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo60 = new LatentStyleExceptionInfo() { Name = "List Number 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo61 = new LatentStyleExceptionInfo() { Name = "List Number 4", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo62 = new LatentStyleExceptionInfo() { Name = "List Number 5", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo63 = new LatentStyleExceptionInfo() { Name = "Title", UiPriority = 10, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo64 = new LatentStyleExceptionInfo() { Name = "Closing", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo65 = new LatentStyleExceptionInfo() { Name = "Signature", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo66 = new LatentStyleExceptionInfo() { Name = "Default Paragraph Font", UiPriority = 1, SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo67 = new LatentStyleExceptionInfo() { Name = "Body Text", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo68 = new LatentStyleExceptionInfo() { Name = "Body Text Indent", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo69 = new LatentStyleExceptionInfo() { Name = "List Continue", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo70 = new LatentStyleExceptionInfo() { Name = "List Continue 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo71 = new LatentStyleExceptionInfo() { Name = "List Continue 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo72 = new LatentStyleExceptionInfo() { Name = "List Continue 4", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo73 = new LatentStyleExceptionInfo() { Name = "List Continue 5", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo74 = new LatentStyleExceptionInfo() { Name = "Message Header", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo75 = new LatentStyleExceptionInfo() { Name = "Subtitle", UiPriority = 11, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo76 = new LatentStyleExceptionInfo() { Name = "Salutation", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo77 = new LatentStyleExceptionInfo() { Name = "Date", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo78 = new LatentStyleExceptionInfo() { Name = "Body Text First Indent", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo79 = new LatentStyleExceptionInfo() { Name = "Body Text First Indent 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo80 = new LatentStyleExceptionInfo() { Name = "Note Heading", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo81 = new LatentStyleExceptionInfo() { Name = "Body Text 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo82 = new LatentStyleExceptionInfo() { Name = "Body Text 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo83 = new LatentStyleExceptionInfo() { Name = "Body Text Indent 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo84 = new LatentStyleExceptionInfo() { Name = "Body Text Indent 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo85 = new LatentStyleExceptionInfo() { Name = "Block Text", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo86 = new LatentStyleExceptionInfo() { Name = "Hyperlink", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo87 = new LatentStyleExceptionInfo() { Name = "FollowedHyperlink", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo88 = new LatentStyleExceptionInfo() { Name = "Strong", UiPriority = 22, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo89 = new LatentStyleExceptionInfo() { Name = "Emphasis", UiPriority = 20, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo90 = new LatentStyleExceptionInfo() { Name = "Document Map", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo91 = new LatentStyleExceptionInfo() { Name = "Plain Text", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo92 = new LatentStyleExceptionInfo() { Name = "E-mail Signature", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo93 = new LatentStyleExceptionInfo() { Name = "HTML Top of Form", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo94 = new LatentStyleExceptionInfo() { Name = "HTML Bottom of Form", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo95 = new LatentStyleExceptionInfo() { Name = "Normal (Web)", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo96 = new LatentStyleExceptionInfo() { Name = "HTML Acronym", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo97 = new LatentStyleExceptionInfo() { Name = "HTML Address", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo98 = new LatentStyleExceptionInfo() { Name = "HTML Cite", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo99 = new LatentStyleExceptionInfo() { Name = "HTML Code", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo100 = new LatentStyleExceptionInfo() { Name = "HTML Definition", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo101 = new LatentStyleExceptionInfo() { Name = "HTML Keyboard", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo102 = new LatentStyleExceptionInfo() { Name = "HTML Preformatted", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo103 = new LatentStyleExceptionInfo() { Name = "HTML Sample", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo104 = new LatentStyleExceptionInfo() { Name = "HTML Typewriter", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo105 = new LatentStyleExceptionInfo() { Name = "HTML Variable", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo106 = new LatentStyleExceptionInfo() { Name = "Normal Table", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo107 = new LatentStyleExceptionInfo() { Name = "annotation subject", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo108 = new LatentStyleExceptionInfo() { Name = "No List", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo109 = new LatentStyleExceptionInfo() { Name = "Outline List 1", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo110 = new LatentStyleExceptionInfo() { Name = "Outline List 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo111 = new LatentStyleExceptionInfo() { Name = "Outline List 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo112 = new LatentStyleExceptionInfo() { Name = "Table Simple 1", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo113 = new LatentStyleExceptionInfo() { Name = "Table Simple 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo114 = new LatentStyleExceptionInfo() { Name = "Table Simple 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo115 = new LatentStyleExceptionInfo() { Name = "Table Classic 1", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo116 = new LatentStyleExceptionInfo() { Name = "Table Classic 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo117 = new LatentStyleExceptionInfo() { Name = "Table Classic 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo118 = new LatentStyleExceptionInfo() { Name = "Table Classic 4", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo119 = new LatentStyleExceptionInfo() { Name = "Table Colorful 1", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo120 = new LatentStyleExceptionInfo() { Name = "Table Colorful 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo121 = new LatentStyleExceptionInfo() { Name = "Table Colorful 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo122 = new LatentStyleExceptionInfo() { Name = "Table Columns 1", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo123 = new LatentStyleExceptionInfo() { Name = "Table Columns 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo124 = new LatentStyleExceptionInfo() { Name = "Table Columns 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo125 = new LatentStyleExceptionInfo() { Name = "Table Columns 4", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo126 = new LatentStyleExceptionInfo() { Name = "Table Columns 5", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo127 = new LatentStyleExceptionInfo() { Name = "Table Grid 1", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo128 = new LatentStyleExceptionInfo() { Name = "Table Grid 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo129 = new LatentStyleExceptionInfo() { Name = "Table Grid 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo130 = new LatentStyleExceptionInfo() { Name = "Table Grid 4", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo131 = new LatentStyleExceptionInfo() { Name = "Table Grid 5", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo132 = new LatentStyleExceptionInfo() { Name = "Table Grid 6", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo133 = new LatentStyleExceptionInfo() { Name = "Table Grid 7", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo134 = new LatentStyleExceptionInfo() { Name = "Table Grid 8", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo135 = new LatentStyleExceptionInfo() { Name = "Table List 1", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo136 = new LatentStyleExceptionInfo() { Name = "Table List 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo137 = new LatentStyleExceptionInfo() { Name = "Table List 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo138 = new LatentStyleExceptionInfo() { Name = "Table List 4", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo139 = new LatentStyleExceptionInfo() { Name = "Table List 5", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo140 = new LatentStyleExceptionInfo() { Name = "Table List 6", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo141 = new LatentStyleExceptionInfo() { Name = "Table List 7", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo142 = new LatentStyleExceptionInfo() { Name = "Table List 8", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo143 = new LatentStyleExceptionInfo() { Name = "Table 3D effects 1", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo144 = new LatentStyleExceptionInfo() { Name = "Table 3D effects 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo145 = new LatentStyleExceptionInfo() { Name = "Table 3D effects 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo146 = new LatentStyleExceptionInfo() { Name = "Table Contemporary", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo147 = new LatentStyleExceptionInfo() { Name = "Table Elegant", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo148 = new LatentStyleExceptionInfo() { Name = "Table Professional", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo149 = new LatentStyleExceptionInfo() { Name = "Table Subtle 1", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo150 = new LatentStyleExceptionInfo() { Name = "Table Subtle 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo151 = new LatentStyleExceptionInfo() { Name = "Table Web 1", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo152 = new LatentStyleExceptionInfo() { Name = "Table Web 2", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo153 = new LatentStyleExceptionInfo() { Name = "Table Web 3", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo154 = new LatentStyleExceptionInfo() { Name = "Balloon Text", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo155 = new LatentStyleExceptionInfo() { Name = "Table Grid", UiPriority = 39 };
            LatentStyleExceptionInfo latentStyleExceptionInfo156 = new LatentStyleExceptionInfo() { Name = "Table Theme", SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo157 = new LatentStyleExceptionInfo() { Name = "Placeholder Text", SemiHidden = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo158 = new LatentStyleExceptionInfo() { Name = "No Spacing", UiPriority = 1, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo159 = new LatentStyleExceptionInfo() { Name = "Light Shading", UiPriority = 60 };
            LatentStyleExceptionInfo latentStyleExceptionInfo160 = new LatentStyleExceptionInfo() { Name = "Light List", UiPriority = 61 };
            LatentStyleExceptionInfo latentStyleExceptionInfo161 = new LatentStyleExceptionInfo() { Name = "Light Grid", UiPriority = 62 };
            LatentStyleExceptionInfo latentStyleExceptionInfo162 = new LatentStyleExceptionInfo() { Name = "Medium Shading 1", UiPriority = 63 };
            LatentStyleExceptionInfo latentStyleExceptionInfo163 = new LatentStyleExceptionInfo() { Name = "Medium Shading 2", UiPriority = 64 };
            LatentStyleExceptionInfo latentStyleExceptionInfo164 = new LatentStyleExceptionInfo() { Name = "Medium List 1", UiPriority = 65 };
            LatentStyleExceptionInfo latentStyleExceptionInfo165 = new LatentStyleExceptionInfo() { Name = "Medium List 2", UiPriority = 66 };
            LatentStyleExceptionInfo latentStyleExceptionInfo166 = new LatentStyleExceptionInfo() { Name = "Medium Grid 1", UiPriority = 67 };
            LatentStyleExceptionInfo latentStyleExceptionInfo167 = new LatentStyleExceptionInfo() { Name = "Medium Grid 2", UiPriority = 68 };
            LatentStyleExceptionInfo latentStyleExceptionInfo168 = new LatentStyleExceptionInfo() { Name = "Medium Grid 3", UiPriority = 69 };
            LatentStyleExceptionInfo latentStyleExceptionInfo169 = new LatentStyleExceptionInfo() { Name = "Dark List", UiPriority = 70 };
            LatentStyleExceptionInfo latentStyleExceptionInfo170 = new LatentStyleExceptionInfo() { Name = "Colorful Shading", UiPriority = 71 };
            LatentStyleExceptionInfo latentStyleExceptionInfo171 = new LatentStyleExceptionInfo() { Name = "Colorful List", UiPriority = 72 };
            LatentStyleExceptionInfo latentStyleExceptionInfo172 = new LatentStyleExceptionInfo() { Name = "Colorful Grid", UiPriority = 73 };
            LatentStyleExceptionInfo latentStyleExceptionInfo173 = new LatentStyleExceptionInfo() { Name = "Light Shading Accent 1", UiPriority = 60 };
            LatentStyleExceptionInfo latentStyleExceptionInfo174 = new LatentStyleExceptionInfo() { Name = "Light List Accent 1", UiPriority = 61 };
            LatentStyleExceptionInfo latentStyleExceptionInfo175 = new LatentStyleExceptionInfo() { Name = "Light Grid Accent 1", UiPriority = 62 };
            LatentStyleExceptionInfo latentStyleExceptionInfo176 = new LatentStyleExceptionInfo() { Name = "Medium Shading 1 Accent 1", UiPriority = 63 };
            LatentStyleExceptionInfo latentStyleExceptionInfo177 = new LatentStyleExceptionInfo() { Name = "Medium Shading 2 Accent 1", UiPriority = 64 };
            LatentStyleExceptionInfo latentStyleExceptionInfo178 = new LatentStyleExceptionInfo() { Name = "Medium List 1 Accent 1", UiPriority = 65 };
            LatentStyleExceptionInfo latentStyleExceptionInfo179 = new LatentStyleExceptionInfo() { Name = "Revision", SemiHidden = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo180 = new LatentStyleExceptionInfo() { Name = "List Paragraph", UiPriority = 34, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo181 = new LatentStyleExceptionInfo() { Name = "Quote", UiPriority = 29, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo182 = new LatentStyleExceptionInfo() { Name = "Intense Quote", UiPriority = 30, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo183 = new LatentStyleExceptionInfo() { Name = "Medium List 2 Accent 1", UiPriority = 66 };
            LatentStyleExceptionInfo latentStyleExceptionInfo184 = new LatentStyleExceptionInfo() { Name = "Medium Grid 1 Accent 1", UiPriority = 67 };
            LatentStyleExceptionInfo latentStyleExceptionInfo185 = new LatentStyleExceptionInfo() { Name = "Medium Grid 2 Accent 1", UiPriority = 68 };
            LatentStyleExceptionInfo latentStyleExceptionInfo186 = new LatentStyleExceptionInfo() { Name = "Medium Grid 3 Accent 1", UiPriority = 69 };
            LatentStyleExceptionInfo latentStyleExceptionInfo187 = new LatentStyleExceptionInfo() { Name = "Dark List Accent 1", UiPriority = 70 };
            LatentStyleExceptionInfo latentStyleExceptionInfo188 = new LatentStyleExceptionInfo() { Name = "Colorful Shading Accent 1", UiPriority = 71 };
            LatentStyleExceptionInfo latentStyleExceptionInfo189 = new LatentStyleExceptionInfo() { Name = "Colorful List Accent 1", UiPriority = 72 };
            LatentStyleExceptionInfo latentStyleExceptionInfo190 = new LatentStyleExceptionInfo() { Name = "Colorful Grid Accent 1", UiPriority = 73 };
            LatentStyleExceptionInfo latentStyleExceptionInfo191 = new LatentStyleExceptionInfo() { Name = "Light Shading Accent 2", UiPriority = 60 };
            LatentStyleExceptionInfo latentStyleExceptionInfo192 = new LatentStyleExceptionInfo() { Name = "Light List Accent 2", UiPriority = 61 };
            LatentStyleExceptionInfo latentStyleExceptionInfo193 = new LatentStyleExceptionInfo() { Name = "Light Grid Accent 2", UiPriority = 62 };
            LatentStyleExceptionInfo latentStyleExceptionInfo194 = new LatentStyleExceptionInfo() { Name = "Medium Shading 1 Accent 2", UiPriority = 63 };
            LatentStyleExceptionInfo latentStyleExceptionInfo195 = new LatentStyleExceptionInfo() { Name = "Medium Shading 2 Accent 2", UiPriority = 64 };
            LatentStyleExceptionInfo latentStyleExceptionInfo196 = new LatentStyleExceptionInfo() { Name = "Medium List 1 Accent 2", UiPriority = 65 };
            LatentStyleExceptionInfo latentStyleExceptionInfo197 = new LatentStyleExceptionInfo() { Name = "Medium List 2 Accent 2", UiPriority = 66 };
            LatentStyleExceptionInfo latentStyleExceptionInfo198 = new LatentStyleExceptionInfo() { Name = "Medium Grid 1 Accent 2", UiPriority = 67 };
            LatentStyleExceptionInfo latentStyleExceptionInfo199 = new LatentStyleExceptionInfo() { Name = "Medium Grid 2 Accent 2", UiPriority = 68 };
            LatentStyleExceptionInfo latentStyleExceptionInfo200 = new LatentStyleExceptionInfo() { Name = "Medium Grid 3 Accent 2", UiPriority = 69 };
            LatentStyleExceptionInfo latentStyleExceptionInfo201 = new LatentStyleExceptionInfo() { Name = "Dark List Accent 2", UiPriority = 70 };
            LatentStyleExceptionInfo latentStyleExceptionInfo202 = new LatentStyleExceptionInfo() { Name = "Colorful Shading Accent 2", UiPriority = 71 };
            LatentStyleExceptionInfo latentStyleExceptionInfo203 = new LatentStyleExceptionInfo() { Name = "Colorful List Accent 2", UiPriority = 72 };
            LatentStyleExceptionInfo latentStyleExceptionInfo204 = new LatentStyleExceptionInfo() { Name = "Colorful Grid Accent 2", UiPriority = 73 };
            LatentStyleExceptionInfo latentStyleExceptionInfo205 = new LatentStyleExceptionInfo() { Name = "Light Shading Accent 3", UiPriority = 60 };
            LatentStyleExceptionInfo latentStyleExceptionInfo206 = new LatentStyleExceptionInfo() { Name = "Light List Accent 3", UiPriority = 61 };
            LatentStyleExceptionInfo latentStyleExceptionInfo207 = new LatentStyleExceptionInfo() { Name = "Light Grid Accent 3", UiPriority = 62 };
            LatentStyleExceptionInfo latentStyleExceptionInfo208 = new LatentStyleExceptionInfo() { Name = "Medium Shading 1 Accent 3", UiPriority = 63 };
            LatentStyleExceptionInfo latentStyleExceptionInfo209 = new LatentStyleExceptionInfo() { Name = "Medium Shading 2 Accent 3", UiPriority = 64 };
            LatentStyleExceptionInfo latentStyleExceptionInfo210 = new LatentStyleExceptionInfo() { Name = "Medium List 1 Accent 3", UiPriority = 65 };
            LatentStyleExceptionInfo latentStyleExceptionInfo211 = new LatentStyleExceptionInfo() { Name = "Medium List 2 Accent 3", UiPriority = 66 };
            LatentStyleExceptionInfo latentStyleExceptionInfo212 = new LatentStyleExceptionInfo() { Name = "Medium Grid 1 Accent 3", UiPriority = 67 };
            LatentStyleExceptionInfo latentStyleExceptionInfo213 = new LatentStyleExceptionInfo() { Name = "Medium Grid 2 Accent 3", UiPriority = 68 };
            LatentStyleExceptionInfo latentStyleExceptionInfo214 = new LatentStyleExceptionInfo() { Name = "Medium Grid 3 Accent 3", UiPriority = 69 };
            LatentStyleExceptionInfo latentStyleExceptionInfo215 = new LatentStyleExceptionInfo() { Name = "Dark List Accent 3", UiPriority = 70 };
            LatentStyleExceptionInfo latentStyleExceptionInfo216 = new LatentStyleExceptionInfo() { Name = "Colorful Shading Accent 3", UiPriority = 71 };
            LatentStyleExceptionInfo latentStyleExceptionInfo217 = new LatentStyleExceptionInfo() { Name = "Colorful List Accent 3", UiPriority = 72 };
            LatentStyleExceptionInfo latentStyleExceptionInfo218 = new LatentStyleExceptionInfo() { Name = "Colorful Grid Accent 3", UiPriority = 73 };
            LatentStyleExceptionInfo latentStyleExceptionInfo219 = new LatentStyleExceptionInfo() { Name = "Light Shading Accent 4", UiPriority = 60 };
            LatentStyleExceptionInfo latentStyleExceptionInfo220 = new LatentStyleExceptionInfo() { Name = "Light List Accent 4", UiPriority = 61 };
            LatentStyleExceptionInfo latentStyleExceptionInfo221 = new LatentStyleExceptionInfo() { Name = "Light Grid Accent 4", UiPriority = 62 };
            LatentStyleExceptionInfo latentStyleExceptionInfo222 = new LatentStyleExceptionInfo() { Name = "Medium Shading 1 Accent 4", UiPriority = 63 };
            LatentStyleExceptionInfo latentStyleExceptionInfo223 = new LatentStyleExceptionInfo() { Name = "Medium Shading 2 Accent 4", UiPriority = 64 };
            LatentStyleExceptionInfo latentStyleExceptionInfo224 = new LatentStyleExceptionInfo() { Name = "Medium List 1 Accent 4", UiPriority = 65 };
            LatentStyleExceptionInfo latentStyleExceptionInfo225 = new LatentStyleExceptionInfo() { Name = "Medium List 2 Accent 4", UiPriority = 66 };
            LatentStyleExceptionInfo latentStyleExceptionInfo226 = new LatentStyleExceptionInfo() { Name = "Medium Grid 1 Accent 4", UiPriority = 67 };
            LatentStyleExceptionInfo latentStyleExceptionInfo227 = new LatentStyleExceptionInfo() { Name = "Medium Grid 2 Accent 4", UiPriority = 68 };
            LatentStyleExceptionInfo latentStyleExceptionInfo228 = new LatentStyleExceptionInfo() { Name = "Medium Grid 3 Accent 4", UiPriority = 69 };
            LatentStyleExceptionInfo latentStyleExceptionInfo229 = new LatentStyleExceptionInfo() { Name = "Dark List Accent 4", UiPriority = 70 };
            LatentStyleExceptionInfo latentStyleExceptionInfo230 = new LatentStyleExceptionInfo() { Name = "Colorful Shading Accent 4", UiPriority = 71 };
            LatentStyleExceptionInfo latentStyleExceptionInfo231 = new LatentStyleExceptionInfo() { Name = "Colorful List Accent 4", UiPriority = 72 };
            LatentStyleExceptionInfo latentStyleExceptionInfo232 = new LatentStyleExceptionInfo() { Name = "Colorful Grid Accent 4", UiPriority = 73 };
            LatentStyleExceptionInfo latentStyleExceptionInfo233 = new LatentStyleExceptionInfo() { Name = "Light Shading Accent 5", UiPriority = 60 };
            LatentStyleExceptionInfo latentStyleExceptionInfo234 = new LatentStyleExceptionInfo() { Name = "Light List Accent 5", UiPriority = 61 };
            LatentStyleExceptionInfo latentStyleExceptionInfo235 = new LatentStyleExceptionInfo() { Name = "Light Grid Accent 5", UiPriority = 62 };
            LatentStyleExceptionInfo latentStyleExceptionInfo236 = new LatentStyleExceptionInfo() { Name = "Medium Shading 1 Accent 5", UiPriority = 63 };
            LatentStyleExceptionInfo latentStyleExceptionInfo237 = new LatentStyleExceptionInfo() { Name = "Medium Shading 2 Accent 5", UiPriority = 64 };
            LatentStyleExceptionInfo latentStyleExceptionInfo238 = new LatentStyleExceptionInfo() { Name = "Medium List 1 Accent 5", UiPriority = 65 };
            LatentStyleExceptionInfo latentStyleExceptionInfo239 = new LatentStyleExceptionInfo() { Name = "Medium List 2 Accent 5", UiPriority = 66 };
            LatentStyleExceptionInfo latentStyleExceptionInfo240 = new LatentStyleExceptionInfo() { Name = "Medium Grid 1 Accent 5", UiPriority = 67 };
            LatentStyleExceptionInfo latentStyleExceptionInfo241 = new LatentStyleExceptionInfo() { Name = "Medium Grid 2 Accent 5", UiPriority = 68 };
            LatentStyleExceptionInfo latentStyleExceptionInfo242 = new LatentStyleExceptionInfo() { Name = "Medium Grid 3 Accent 5", UiPriority = 69 };
            LatentStyleExceptionInfo latentStyleExceptionInfo243 = new LatentStyleExceptionInfo() { Name = "Dark List Accent 5", UiPriority = 70 };
            LatentStyleExceptionInfo latentStyleExceptionInfo244 = new LatentStyleExceptionInfo() { Name = "Colorful Shading Accent 5", UiPriority = 71 };
            LatentStyleExceptionInfo latentStyleExceptionInfo245 = new LatentStyleExceptionInfo() { Name = "Colorful List Accent 5", UiPriority = 72 };
            LatentStyleExceptionInfo latentStyleExceptionInfo246 = new LatentStyleExceptionInfo() { Name = "Colorful Grid Accent 5", UiPriority = 73 };
            LatentStyleExceptionInfo latentStyleExceptionInfo247 = new LatentStyleExceptionInfo() { Name = "Light Shading Accent 6", UiPriority = 60 };
            LatentStyleExceptionInfo latentStyleExceptionInfo248 = new LatentStyleExceptionInfo() { Name = "Light List Accent 6", UiPriority = 61 };
            LatentStyleExceptionInfo latentStyleExceptionInfo249 = new LatentStyleExceptionInfo() { Name = "Light Grid Accent 6", UiPriority = 62 };
            LatentStyleExceptionInfo latentStyleExceptionInfo250 = new LatentStyleExceptionInfo() { Name = "Medium Shading 1 Accent 6", UiPriority = 63 };
            LatentStyleExceptionInfo latentStyleExceptionInfo251 = new LatentStyleExceptionInfo() { Name = "Medium Shading 2 Accent 6", UiPriority = 64 };
            LatentStyleExceptionInfo latentStyleExceptionInfo252 = new LatentStyleExceptionInfo() { Name = "Medium List 1 Accent 6", UiPriority = 65 };
            LatentStyleExceptionInfo latentStyleExceptionInfo253 = new LatentStyleExceptionInfo() { Name = "Medium List 2 Accent 6", UiPriority = 66 };
            LatentStyleExceptionInfo latentStyleExceptionInfo254 = new LatentStyleExceptionInfo() { Name = "Medium Grid 1 Accent 6", UiPriority = 67 };
            LatentStyleExceptionInfo latentStyleExceptionInfo255 = new LatentStyleExceptionInfo() { Name = "Medium Grid 2 Accent 6", UiPriority = 68 };
            LatentStyleExceptionInfo latentStyleExceptionInfo256 = new LatentStyleExceptionInfo() { Name = "Medium Grid 3 Accent 6", UiPriority = 69 };
            LatentStyleExceptionInfo latentStyleExceptionInfo257 = new LatentStyleExceptionInfo() { Name = "Dark List Accent 6", UiPriority = 70 };
            LatentStyleExceptionInfo latentStyleExceptionInfo258 = new LatentStyleExceptionInfo() { Name = "Colorful Shading Accent 6", UiPriority = 71 };
            LatentStyleExceptionInfo latentStyleExceptionInfo259 = new LatentStyleExceptionInfo() { Name = "Colorful List Accent 6", UiPriority = 72 };
            LatentStyleExceptionInfo latentStyleExceptionInfo260 = new LatentStyleExceptionInfo() { Name = "Colorful Grid Accent 6", UiPriority = 73 };
            LatentStyleExceptionInfo latentStyleExceptionInfo261 = new LatentStyleExceptionInfo() { Name = "Subtle Emphasis", UiPriority = 19, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo262 = new LatentStyleExceptionInfo() { Name = "Intense Emphasis", UiPriority = 21, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo263 = new LatentStyleExceptionInfo() { Name = "Subtle Reference", UiPriority = 31, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo264 = new LatentStyleExceptionInfo() { Name = "Intense Reference", UiPriority = 32, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo265 = new LatentStyleExceptionInfo() { Name = "Book Title", UiPriority = 33, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo266 = new LatentStyleExceptionInfo() { Name = "Bibliography", UiPriority = 37, SemiHidden = true, UnhideWhenUsed = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo267 = new LatentStyleExceptionInfo() { Name = "TOC Heading", UiPriority = 39, SemiHidden = true, UnhideWhenUsed = true, PrimaryStyle = true };
            LatentStyleExceptionInfo latentStyleExceptionInfo268 = new LatentStyleExceptionInfo() { Name = "Plain Table 1", UiPriority = 41 };
            LatentStyleExceptionInfo latentStyleExceptionInfo269 = new LatentStyleExceptionInfo() { Name = "Plain Table 2", UiPriority = 42 };
            LatentStyleExceptionInfo latentStyleExceptionInfo270 = new LatentStyleExceptionInfo() { Name = "Plain Table 3", UiPriority = 43 };
            LatentStyleExceptionInfo latentStyleExceptionInfo271 = new LatentStyleExceptionInfo() { Name = "Plain Table 4", UiPriority = 44 };
            LatentStyleExceptionInfo latentStyleExceptionInfo272 = new LatentStyleExceptionInfo() { Name = "Plain Table 5", UiPriority = 45 };
            LatentStyleExceptionInfo latentStyleExceptionInfo273 = new LatentStyleExceptionInfo() { Name = "Grid Table Light", UiPriority = 40 };
            LatentStyleExceptionInfo latentStyleExceptionInfo274 = new LatentStyleExceptionInfo() { Name = "Grid Table 1 Light", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo275 = new LatentStyleExceptionInfo() { Name = "Grid Table 2", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo276 = new LatentStyleExceptionInfo() { Name = "Grid Table 3", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo277 = new LatentStyleExceptionInfo() { Name = "Grid Table 4", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo278 = new LatentStyleExceptionInfo() { Name = "Grid Table 5 Dark", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo279 = new LatentStyleExceptionInfo() { Name = "Grid Table 6 Colorful", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo280 = new LatentStyleExceptionInfo() { Name = "Grid Table 7 Colorful", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo281 = new LatentStyleExceptionInfo() { Name = "Grid Table 1 Light Accent 1", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo282 = new LatentStyleExceptionInfo() { Name = "Grid Table 2 Accent 1", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo283 = new LatentStyleExceptionInfo() { Name = "Grid Table 3 Accent 1", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo284 = new LatentStyleExceptionInfo() { Name = "Grid Table 4 Accent 1", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo285 = new LatentStyleExceptionInfo() { Name = "Grid Table 5 Dark Accent 1", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo286 = new LatentStyleExceptionInfo() { Name = "Grid Table 6 Colorful Accent 1", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo287 = new LatentStyleExceptionInfo() { Name = "Grid Table 7 Colorful Accent 1", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo288 = new LatentStyleExceptionInfo() { Name = "Grid Table 1 Light Accent 2", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo289 = new LatentStyleExceptionInfo() { Name = "Grid Table 2 Accent 2", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo290 = new LatentStyleExceptionInfo() { Name = "Grid Table 3 Accent 2", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo291 = new LatentStyleExceptionInfo() { Name = "Grid Table 4 Accent 2", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo292 = new LatentStyleExceptionInfo() { Name = "Grid Table 5 Dark Accent 2", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo293 = new LatentStyleExceptionInfo() { Name = "Grid Table 6 Colorful Accent 2", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo294 = new LatentStyleExceptionInfo() { Name = "Grid Table 7 Colorful Accent 2", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo295 = new LatentStyleExceptionInfo() { Name = "Grid Table 1 Light Accent 3", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo296 = new LatentStyleExceptionInfo() { Name = "Grid Table 2 Accent 3", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo297 = new LatentStyleExceptionInfo() { Name = "Grid Table 3 Accent 3", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo298 = new LatentStyleExceptionInfo() { Name = "Grid Table 4 Accent 3", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo299 = new LatentStyleExceptionInfo() { Name = "Grid Table 5 Dark Accent 3", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo300 = new LatentStyleExceptionInfo() { Name = "Grid Table 6 Colorful Accent 3", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo301 = new LatentStyleExceptionInfo() { Name = "Grid Table 7 Colorful Accent 3", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo302 = new LatentStyleExceptionInfo() { Name = "Grid Table 1 Light Accent 4", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo303 = new LatentStyleExceptionInfo() { Name = "Grid Table 2 Accent 4", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo304 = new LatentStyleExceptionInfo() { Name = "Grid Table 3 Accent 4", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo305 = new LatentStyleExceptionInfo() { Name = "Grid Table 4 Accent 4", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo306 = new LatentStyleExceptionInfo() { Name = "Grid Table 5 Dark Accent 4", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo307 = new LatentStyleExceptionInfo() { Name = "Grid Table 6 Colorful Accent 4", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo308 = new LatentStyleExceptionInfo() { Name = "Grid Table 7 Colorful Accent 4", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo309 = new LatentStyleExceptionInfo() { Name = "Grid Table 1 Light Accent 5", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo310 = new LatentStyleExceptionInfo() { Name = "Grid Table 2 Accent 5", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo311 = new LatentStyleExceptionInfo() { Name = "Grid Table 3 Accent 5", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo312 = new LatentStyleExceptionInfo() { Name = "Grid Table 4 Accent 5", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo313 = new LatentStyleExceptionInfo() { Name = "Grid Table 5 Dark Accent 5", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo314 = new LatentStyleExceptionInfo() { Name = "Grid Table 6 Colorful Accent 5", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo315 = new LatentStyleExceptionInfo() { Name = "Grid Table 7 Colorful Accent 5", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo316 = new LatentStyleExceptionInfo() { Name = "Grid Table 1 Light Accent 6", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo317 = new LatentStyleExceptionInfo() { Name = "Grid Table 2 Accent 6", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo318 = new LatentStyleExceptionInfo() { Name = "Grid Table 3 Accent 6", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo319 = new LatentStyleExceptionInfo() { Name = "Grid Table 4 Accent 6", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo320 = new LatentStyleExceptionInfo() { Name = "Grid Table 5 Dark Accent 6", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo321 = new LatentStyleExceptionInfo() { Name = "Grid Table 6 Colorful Accent 6", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo322 = new LatentStyleExceptionInfo() { Name = "Grid Table 7 Colorful Accent 6", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo323 = new LatentStyleExceptionInfo() { Name = "List Table 1 Light", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo324 = new LatentStyleExceptionInfo() { Name = "List Table 2", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo325 = new LatentStyleExceptionInfo() { Name = "List Table 3", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo326 = new LatentStyleExceptionInfo() { Name = "List Table 4", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo327 = new LatentStyleExceptionInfo() { Name = "List Table 5 Dark", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo328 = new LatentStyleExceptionInfo() { Name = "List Table 6 Colorful", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo329 = new LatentStyleExceptionInfo() { Name = "List Table 7 Colorful", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo330 = new LatentStyleExceptionInfo() { Name = "List Table 1 Light Accent 1", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo331 = new LatentStyleExceptionInfo() { Name = "List Table 2 Accent 1", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo332 = new LatentStyleExceptionInfo() { Name = "List Table 3 Accent 1", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo333 = new LatentStyleExceptionInfo() { Name = "List Table 4 Accent 1", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo334 = new LatentStyleExceptionInfo() { Name = "List Table 5 Dark Accent 1", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo335 = new LatentStyleExceptionInfo() { Name = "List Table 6 Colorful Accent 1", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo336 = new LatentStyleExceptionInfo() { Name = "List Table 7 Colorful Accent 1", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo337 = new LatentStyleExceptionInfo() { Name = "List Table 1 Light Accent 2", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo338 = new LatentStyleExceptionInfo() { Name = "List Table 2 Accent 2", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo339 = new LatentStyleExceptionInfo() { Name = "List Table 3 Accent 2", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo340 = new LatentStyleExceptionInfo() { Name = "List Table 4 Accent 2", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo341 = new LatentStyleExceptionInfo() { Name = "List Table 5 Dark Accent 2", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo342 = new LatentStyleExceptionInfo() { Name = "List Table 6 Colorful Accent 2", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo343 = new LatentStyleExceptionInfo() { Name = "List Table 7 Colorful Accent 2", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo344 = new LatentStyleExceptionInfo() { Name = "List Table 1 Light Accent 3", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo345 = new LatentStyleExceptionInfo() { Name = "List Table 2 Accent 3", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo346 = new LatentStyleExceptionInfo() { Name = "List Table 3 Accent 3", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo347 = new LatentStyleExceptionInfo() { Name = "List Table 4 Accent 3", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo348 = new LatentStyleExceptionInfo() { Name = "List Table 5 Dark Accent 3", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo349 = new LatentStyleExceptionInfo() { Name = "List Table 6 Colorful Accent 3", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo350 = new LatentStyleExceptionInfo() { Name = "List Table 7 Colorful Accent 3", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo351 = new LatentStyleExceptionInfo() { Name = "List Table 1 Light Accent 4", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo352 = new LatentStyleExceptionInfo() { Name = "List Table 2 Accent 4", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo353 = new LatentStyleExceptionInfo() { Name = "List Table 3 Accent 4", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo354 = new LatentStyleExceptionInfo() { Name = "List Table 4 Accent 4", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo355 = new LatentStyleExceptionInfo() { Name = "List Table 5 Dark Accent 4", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo356 = new LatentStyleExceptionInfo() { Name = "List Table 6 Colorful Accent 4", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo357 = new LatentStyleExceptionInfo() { Name = "List Table 7 Colorful Accent 4", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo358 = new LatentStyleExceptionInfo() { Name = "List Table 1 Light Accent 5", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo359 = new LatentStyleExceptionInfo() { Name = "List Table 2 Accent 5", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo360 = new LatentStyleExceptionInfo() { Name = "List Table 3 Accent 5", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo361 = new LatentStyleExceptionInfo() { Name = "List Table 4 Accent 5", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo362 = new LatentStyleExceptionInfo() { Name = "List Table 5 Dark Accent 5", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo363 = new LatentStyleExceptionInfo() { Name = "List Table 6 Colorful Accent 5", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo364 = new LatentStyleExceptionInfo() { Name = "List Table 7 Colorful Accent 5", UiPriority = 52 };
            LatentStyleExceptionInfo latentStyleExceptionInfo365 = new LatentStyleExceptionInfo() { Name = "List Table 1 Light Accent 6", UiPriority = 46 };
            LatentStyleExceptionInfo latentStyleExceptionInfo366 = new LatentStyleExceptionInfo() { Name = "List Table 2 Accent 6", UiPriority = 47 };
            LatentStyleExceptionInfo latentStyleExceptionInfo367 = new LatentStyleExceptionInfo() { Name = "List Table 3 Accent 6", UiPriority = 48 };
            LatentStyleExceptionInfo latentStyleExceptionInfo368 = new LatentStyleExceptionInfo() { Name = "List Table 4 Accent 6", UiPriority = 49 };
            LatentStyleExceptionInfo latentStyleExceptionInfo369 = new LatentStyleExceptionInfo() { Name = "List Table 5 Dark Accent 6", UiPriority = 50 };
            LatentStyleExceptionInfo latentStyleExceptionInfo370 = new LatentStyleExceptionInfo() { Name = "List Table 6 Colorful Accent 6", UiPriority = 51 };
            LatentStyleExceptionInfo latentStyleExceptionInfo371 = new LatentStyleExceptionInfo() { Name = "List Table 7 Colorful Accent 6", UiPriority = 52 };

            latentStyles1.Append(latentStyleExceptionInfo1);
            latentStyles1.Append(latentStyleExceptionInfo2);
            latentStyles1.Append(latentStyleExceptionInfo3);
            latentStyles1.Append(latentStyleExceptionInfo4);
            latentStyles1.Append(latentStyleExceptionInfo5);
            latentStyles1.Append(latentStyleExceptionInfo6);
            latentStyles1.Append(latentStyleExceptionInfo7);
            latentStyles1.Append(latentStyleExceptionInfo8);
            latentStyles1.Append(latentStyleExceptionInfo9);
            latentStyles1.Append(latentStyleExceptionInfo10);
            latentStyles1.Append(latentStyleExceptionInfo11);
            latentStyles1.Append(latentStyleExceptionInfo12);
            latentStyles1.Append(latentStyleExceptionInfo13);
            latentStyles1.Append(latentStyleExceptionInfo14);
            latentStyles1.Append(latentStyleExceptionInfo15);
            latentStyles1.Append(latentStyleExceptionInfo16);
            latentStyles1.Append(latentStyleExceptionInfo17);
            latentStyles1.Append(latentStyleExceptionInfo18);
            latentStyles1.Append(latentStyleExceptionInfo19);
            latentStyles1.Append(latentStyleExceptionInfo20);
            latentStyles1.Append(latentStyleExceptionInfo21);
            latentStyles1.Append(latentStyleExceptionInfo22);
            latentStyles1.Append(latentStyleExceptionInfo23);
            latentStyles1.Append(latentStyleExceptionInfo24);
            latentStyles1.Append(latentStyleExceptionInfo25);
            latentStyles1.Append(latentStyleExceptionInfo26);
            latentStyles1.Append(latentStyleExceptionInfo27);
            latentStyles1.Append(latentStyleExceptionInfo28);
            latentStyles1.Append(latentStyleExceptionInfo29);
            latentStyles1.Append(latentStyleExceptionInfo30);
            latentStyles1.Append(latentStyleExceptionInfo31);
            latentStyles1.Append(latentStyleExceptionInfo32);
            latentStyles1.Append(latentStyleExceptionInfo33);
            latentStyles1.Append(latentStyleExceptionInfo34);
            latentStyles1.Append(latentStyleExceptionInfo35);
            latentStyles1.Append(latentStyleExceptionInfo36);
            latentStyles1.Append(latentStyleExceptionInfo37);
            latentStyles1.Append(latentStyleExceptionInfo38);
            latentStyles1.Append(latentStyleExceptionInfo39);
            latentStyles1.Append(latentStyleExceptionInfo40);
            latentStyles1.Append(latentStyleExceptionInfo41);
            latentStyles1.Append(latentStyleExceptionInfo42);
            latentStyles1.Append(latentStyleExceptionInfo43);
            latentStyles1.Append(latentStyleExceptionInfo44);
            latentStyles1.Append(latentStyleExceptionInfo45);
            latentStyles1.Append(latentStyleExceptionInfo46);
            latentStyles1.Append(latentStyleExceptionInfo47);
            latentStyles1.Append(latentStyleExceptionInfo48);
            latentStyles1.Append(latentStyleExceptionInfo49);
            latentStyles1.Append(latentStyleExceptionInfo50);
            latentStyles1.Append(latentStyleExceptionInfo51);
            latentStyles1.Append(latentStyleExceptionInfo52);
            latentStyles1.Append(latentStyleExceptionInfo53);
            latentStyles1.Append(latentStyleExceptionInfo54);
            latentStyles1.Append(latentStyleExceptionInfo55);
            latentStyles1.Append(latentStyleExceptionInfo56);
            latentStyles1.Append(latentStyleExceptionInfo57);
            latentStyles1.Append(latentStyleExceptionInfo58);
            latentStyles1.Append(latentStyleExceptionInfo59);
            latentStyles1.Append(latentStyleExceptionInfo60);
            latentStyles1.Append(latentStyleExceptionInfo61);
            latentStyles1.Append(latentStyleExceptionInfo62);
            latentStyles1.Append(latentStyleExceptionInfo63);
            latentStyles1.Append(latentStyleExceptionInfo64);
            latentStyles1.Append(latentStyleExceptionInfo65);
            latentStyles1.Append(latentStyleExceptionInfo66);
            latentStyles1.Append(latentStyleExceptionInfo67);
            latentStyles1.Append(latentStyleExceptionInfo68);
            latentStyles1.Append(latentStyleExceptionInfo69);
            latentStyles1.Append(latentStyleExceptionInfo70);
            latentStyles1.Append(latentStyleExceptionInfo71);
            latentStyles1.Append(latentStyleExceptionInfo72);
            latentStyles1.Append(latentStyleExceptionInfo73);
            latentStyles1.Append(latentStyleExceptionInfo74);
            latentStyles1.Append(latentStyleExceptionInfo75);
            latentStyles1.Append(latentStyleExceptionInfo76);
            latentStyles1.Append(latentStyleExceptionInfo77);
            latentStyles1.Append(latentStyleExceptionInfo78);
            latentStyles1.Append(latentStyleExceptionInfo79);
            latentStyles1.Append(latentStyleExceptionInfo80);
            latentStyles1.Append(latentStyleExceptionInfo81);
            latentStyles1.Append(latentStyleExceptionInfo82);
            latentStyles1.Append(latentStyleExceptionInfo83);
            latentStyles1.Append(latentStyleExceptionInfo84);
            latentStyles1.Append(latentStyleExceptionInfo85);
            latentStyles1.Append(latentStyleExceptionInfo86);
            latentStyles1.Append(latentStyleExceptionInfo87);
            latentStyles1.Append(latentStyleExceptionInfo88);
            latentStyles1.Append(latentStyleExceptionInfo89);
            latentStyles1.Append(latentStyleExceptionInfo90);
            latentStyles1.Append(latentStyleExceptionInfo91);
            latentStyles1.Append(latentStyleExceptionInfo92);
            latentStyles1.Append(latentStyleExceptionInfo93);
            latentStyles1.Append(latentStyleExceptionInfo94);
            latentStyles1.Append(latentStyleExceptionInfo95);
            latentStyles1.Append(latentStyleExceptionInfo96);
            latentStyles1.Append(latentStyleExceptionInfo97);
            latentStyles1.Append(latentStyleExceptionInfo98);
            latentStyles1.Append(latentStyleExceptionInfo99);
            latentStyles1.Append(latentStyleExceptionInfo100);
            latentStyles1.Append(latentStyleExceptionInfo101);
            latentStyles1.Append(latentStyleExceptionInfo102);
            latentStyles1.Append(latentStyleExceptionInfo103);
            latentStyles1.Append(latentStyleExceptionInfo104);
            latentStyles1.Append(latentStyleExceptionInfo105);
            latentStyles1.Append(latentStyleExceptionInfo106);
            latentStyles1.Append(latentStyleExceptionInfo107);
            latentStyles1.Append(latentStyleExceptionInfo108);
            latentStyles1.Append(latentStyleExceptionInfo109);
            latentStyles1.Append(latentStyleExceptionInfo110);
            latentStyles1.Append(latentStyleExceptionInfo111);
            latentStyles1.Append(latentStyleExceptionInfo112);
            latentStyles1.Append(latentStyleExceptionInfo113);
            latentStyles1.Append(latentStyleExceptionInfo114);
            latentStyles1.Append(latentStyleExceptionInfo115);
            latentStyles1.Append(latentStyleExceptionInfo116);
            latentStyles1.Append(latentStyleExceptionInfo117);
            latentStyles1.Append(latentStyleExceptionInfo118);
            latentStyles1.Append(latentStyleExceptionInfo119);
            latentStyles1.Append(latentStyleExceptionInfo120);
            latentStyles1.Append(latentStyleExceptionInfo121);
            latentStyles1.Append(latentStyleExceptionInfo122);
            latentStyles1.Append(latentStyleExceptionInfo123);
            latentStyles1.Append(latentStyleExceptionInfo124);
            latentStyles1.Append(latentStyleExceptionInfo125);
            latentStyles1.Append(latentStyleExceptionInfo126);
            latentStyles1.Append(latentStyleExceptionInfo127);
            latentStyles1.Append(latentStyleExceptionInfo128);
            latentStyles1.Append(latentStyleExceptionInfo129);
            latentStyles1.Append(latentStyleExceptionInfo130);
            latentStyles1.Append(latentStyleExceptionInfo131);
            latentStyles1.Append(latentStyleExceptionInfo132);
            latentStyles1.Append(latentStyleExceptionInfo133);
            latentStyles1.Append(latentStyleExceptionInfo134);
            latentStyles1.Append(latentStyleExceptionInfo135);
            latentStyles1.Append(latentStyleExceptionInfo136);
            latentStyles1.Append(latentStyleExceptionInfo137);
            latentStyles1.Append(latentStyleExceptionInfo138);
            latentStyles1.Append(latentStyleExceptionInfo139);
            latentStyles1.Append(latentStyleExceptionInfo140);
            latentStyles1.Append(latentStyleExceptionInfo141);
            latentStyles1.Append(latentStyleExceptionInfo142);
            latentStyles1.Append(latentStyleExceptionInfo143);
            latentStyles1.Append(latentStyleExceptionInfo144);
            latentStyles1.Append(latentStyleExceptionInfo145);
            latentStyles1.Append(latentStyleExceptionInfo146);
            latentStyles1.Append(latentStyleExceptionInfo147);
            latentStyles1.Append(latentStyleExceptionInfo148);
            latentStyles1.Append(latentStyleExceptionInfo149);
            latentStyles1.Append(latentStyleExceptionInfo150);
            latentStyles1.Append(latentStyleExceptionInfo151);
            latentStyles1.Append(latentStyleExceptionInfo152);
            latentStyles1.Append(latentStyleExceptionInfo153);
            latentStyles1.Append(latentStyleExceptionInfo154);
            latentStyles1.Append(latentStyleExceptionInfo155);
            latentStyles1.Append(latentStyleExceptionInfo156);
            latentStyles1.Append(latentStyleExceptionInfo157);
            latentStyles1.Append(latentStyleExceptionInfo158);
            latentStyles1.Append(latentStyleExceptionInfo159);
            latentStyles1.Append(latentStyleExceptionInfo160);
            latentStyles1.Append(latentStyleExceptionInfo161);
            latentStyles1.Append(latentStyleExceptionInfo162);
            latentStyles1.Append(latentStyleExceptionInfo163);
            latentStyles1.Append(latentStyleExceptionInfo164);
            latentStyles1.Append(latentStyleExceptionInfo165);
            latentStyles1.Append(latentStyleExceptionInfo166);
            latentStyles1.Append(latentStyleExceptionInfo167);
            latentStyles1.Append(latentStyleExceptionInfo168);
            latentStyles1.Append(latentStyleExceptionInfo169);
            latentStyles1.Append(latentStyleExceptionInfo170);
            latentStyles1.Append(latentStyleExceptionInfo171);
            latentStyles1.Append(latentStyleExceptionInfo172);
            latentStyles1.Append(latentStyleExceptionInfo173);
            latentStyles1.Append(latentStyleExceptionInfo174);
            latentStyles1.Append(latentStyleExceptionInfo175);
            latentStyles1.Append(latentStyleExceptionInfo176);
            latentStyles1.Append(latentStyleExceptionInfo177);
            latentStyles1.Append(latentStyleExceptionInfo178);
            latentStyles1.Append(latentStyleExceptionInfo179);
            latentStyles1.Append(latentStyleExceptionInfo180);
            latentStyles1.Append(latentStyleExceptionInfo181);
            latentStyles1.Append(latentStyleExceptionInfo182);
            latentStyles1.Append(latentStyleExceptionInfo183);
            latentStyles1.Append(latentStyleExceptionInfo184);
            latentStyles1.Append(latentStyleExceptionInfo185);
            latentStyles1.Append(latentStyleExceptionInfo186);
            latentStyles1.Append(latentStyleExceptionInfo187);
            latentStyles1.Append(latentStyleExceptionInfo188);
            latentStyles1.Append(latentStyleExceptionInfo189);
            latentStyles1.Append(latentStyleExceptionInfo190);
            latentStyles1.Append(latentStyleExceptionInfo191);
            latentStyles1.Append(latentStyleExceptionInfo192);
            latentStyles1.Append(latentStyleExceptionInfo193);
            latentStyles1.Append(latentStyleExceptionInfo194);
            latentStyles1.Append(latentStyleExceptionInfo195);
            latentStyles1.Append(latentStyleExceptionInfo196);
            latentStyles1.Append(latentStyleExceptionInfo197);
            latentStyles1.Append(latentStyleExceptionInfo198);
            latentStyles1.Append(latentStyleExceptionInfo199);
            latentStyles1.Append(latentStyleExceptionInfo200);
            latentStyles1.Append(latentStyleExceptionInfo201);
            latentStyles1.Append(latentStyleExceptionInfo202);
            latentStyles1.Append(latentStyleExceptionInfo203);
            latentStyles1.Append(latentStyleExceptionInfo204);
            latentStyles1.Append(latentStyleExceptionInfo205);
            latentStyles1.Append(latentStyleExceptionInfo206);
            latentStyles1.Append(latentStyleExceptionInfo207);
            latentStyles1.Append(latentStyleExceptionInfo208);
            latentStyles1.Append(latentStyleExceptionInfo209);
            latentStyles1.Append(latentStyleExceptionInfo210);
            latentStyles1.Append(latentStyleExceptionInfo211);
            latentStyles1.Append(latentStyleExceptionInfo212);
            latentStyles1.Append(latentStyleExceptionInfo213);
            latentStyles1.Append(latentStyleExceptionInfo214);
            latentStyles1.Append(latentStyleExceptionInfo215);
            latentStyles1.Append(latentStyleExceptionInfo216);
            latentStyles1.Append(latentStyleExceptionInfo217);
            latentStyles1.Append(latentStyleExceptionInfo218);
            latentStyles1.Append(latentStyleExceptionInfo219);
            latentStyles1.Append(latentStyleExceptionInfo220);
            latentStyles1.Append(latentStyleExceptionInfo221);
            latentStyles1.Append(latentStyleExceptionInfo222);
            latentStyles1.Append(latentStyleExceptionInfo223);
            latentStyles1.Append(latentStyleExceptionInfo224);
            latentStyles1.Append(latentStyleExceptionInfo225);
            latentStyles1.Append(latentStyleExceptionInfo226);
            latentStyles1.Append(latentStyleExceptionInfo227);
            latentStyles1.Append(latentStyleExceptionInfo228);
            latentStyles1.Append(latentStyleExceptionInfo229);
            latentStyles1.Append(latentStyleExceptionInfo230);
            latentStyles1.Append(latentStyleExceptionInfo231);
            latentStyles1.Append(latentStyleExceptionInfo232);
            latentStyles1.Append(latentStyleExceptionInfo233);
            latentStyles1.Append(latentStyleExceptionInfo234);
            latentStyles1.Append(latentStyleExceptionInfo235);
            latentStyles1.Append(latentStyleExceptionInfo236);
            latentStyles1.Append(latentStyleExceptionInfo237);
            latentStyles1.Append(latentStyleExceptionInfo238);
            latentStyles1.Append(latentStyleExceptionInfo239);
            latentStyles1.Append(latentStyleExceptionInfo240);
            latentStyles1.Append(latentStyleExceptionInfo241);
            latentStyles1.Append(latentStyleExceptionInfo242);
            latentStyles1.Append(latentStyleExceptionInfo243);
            latentStyles1.Append(latentStyleExceptionInfo244);
            latentStyles1.Append(latentStyleExceptionInfo245);
            latentStyles1.Append(latentStyleExceptionInfo246);
            latentStyles1.Append(latentStyleExceptionInfo247);
            latentStyles1.Append(latentStyleExceptionInfo248);
            latentStyles1.Append(latentStyleExceptionInfo249);
            latentStyles1.Append(latentStyleExceptionInfo250);
            latentStyles1.Append(latentStyleExceptionInfo251);
            latentStyles1.Append(latentStyleExceptionInfo252);
            latentStyles1.Append(latentStyleExceptionInfo253);
            latentStyles1.Append(latentStyleExceptionInfo254);
            latentStyles1.Append(latentStyleExceptionInfo255);
            latentStyles1.Append(latentStyleExceptionInfo256);
            latentStyles1.Append(latentStyleExceptionInfo257);
            latentStyles1.Append(latentStyleExceptionInfo258);
            latentStyles1.Append(latentStyleExceptionInfo259);
            latentStyles1.Append(latentStyleExceptionInfo260);
            latentStyles1.Append(latentStyleExceptionInfo261);
            latentStyles1.Append(latentStyleExceptionInfo262);
            latentStyles1.Append(latentStyleExceptionInfo263);
            latentStyles1.Append(latentStyleExceptionInfo264);
            latentStyles1.Append(latentStyleExceptionInfo265);
            latentStyles1.Append(latentStyleExceptionInfo266);
            latentStyles1.Append(latentStyleExceptionInfo267);
            latentStyles1.Append(latentStyleExceptionInfo268);
            latentStyles1.Append(latentStyleExceptionInfo269);
            latentStyles1.Append(latentStyleExceptionInfo270);
            latentStyles1.Append(latentStyleExceptionInfo271);
            latentStyles1.Append(latentStyleExceptionInfo272);
            latentStyles1.Append(latentStyleExceptionInfo273);
            latentStyles1.Append(latentStyleExceptionInfo274);
            latentStyles1.Append(latentStyleExceptionInfo275);
            latentStyles1.Append(latentStyleExceptionInfo276);
            latentStyles1.Append(latentStyleExceptionInfo277);
            latentStyles1.Append(latentStyleExceptionInfo278);
            latentStyles1.Append(latentStyleExceptionInfo279);
            latentStyles1.Append(latentStyleExceptionInfo280);
            latentStyles1.Append(latentStyleExceptionInfo281);
            latentStyles1.Append(latentStyleExceptionInfo282);
            latentStyles1.Append(latentStyleExceptionInfo283);
            latentStyles1.Append(latentStyleExceptionInfo284);
            latentStyles1.Append(latentStyleExceptionInfo285);
            latentStyles1.Append(latentStyleExceptionInfo286);
            latentStyles1.Append(latentStyleExceptionInfo287);
            latentStyles1.Append(latentStyleExceptionInfo288);
            latentStyles1.Append(latentStyleExceptionInfo289);
            latentStyles1.Append(latentStyleExceptionInfo290);
            latentStyles1.Append(latentStyleExceptionInfo291);
            latentStyles1.Append(latentStyleExceptionInfo292);
            latentStyles1.Append(latentStyleExceptionInfo293);
            latentStyles1.Append(latentStyleExceptionInfo294);
            latentStyles1.Append(latentStyleExceptionInfo295);
            latentStyles1.Append(latentStyleExceptionInfo296);
            latentStyles1.Append(latentStyleExceptionInfo297);
            latentStyles1.Append(latentStyleExceptionInfo298);
            latentStyles1.Append(latentStyleExceptionInfo299);
            latentStyles1.Append(latentStyleExceptionInfo300);
            latentStyles1.Append(latentStyleExceptionInfo301);
            latentStyles1.Append(latentStyleExceptionInfo302);
            latentStyles1.Append(latentStyleExceptionInfo303);
            latentStyles1.Append(latentStyleExceptionInfo304);
            latentStyles1.Append(latentStyleExceptionInfo305);
            latentStyles1.Append(latentStyleExceptionInfo306);
            latentStyles1.Append(latentStyleExceptionInfo307);
            latentStyles1.Append(latentStyleExceptionInfo308);
            latentStyles1.Append(latentStyleExceptionInfo309);
            latentStyles1.Append(latentStyleExceptionInfo310);
            latentStyles1.Append(latentStyleExceptionInfo311);
            latentStyles1.Append(latentStyleExceptionInfo312);
            latentStyles1.Append(latentStyleExceptionInfo313);
            latentStyles1.Append(latentStyleExceptionInfo314);
            latentStyles1.Append(latentStyleExceptionInfo315);
            latentStyles1.Append(latentStyleExceptionInfo316);
            latentStyles1.Append(latentStyleExceptionInfo317);
            latentStyles1.Append(latentStyleExceptionInfo318);
            latentStyles1.Append(latentStyleExceptionInfo319);
            latentStyles1.Append(latentStyleExceptionInfo320);
            latentStyles1.Append(latentStyleExceptionInfo321);
            latentStyles1.Append(latentStyleExceptionInfo322);
            latentStyles1.Append(latentStyleExceptionInfo323);
            latentStyles1.Append(latentStyleExceptionInfo324);
            latentStyles1.Append(latentStyleExceptionInfo325);
            latentStyles1.Append(latentStyleExceptionInfo326);
            latentStyles1.Append(latentStyleExceptionInfo327);
            latentStyles1.Append(latentStyleExceptionInfo328);
            latentStyles1.Append(latentStyleExceptionInfo329);
            latentStyles1.Append(latentStyleExceptionInfo330);
            latentStyles1.Append(latentStyleExceptionInfo331);
            latentStyles1.Append(latentStyleExceptionInfo332);
            latentStyles1.Append(latentStyleExceptionInfo333);
            latentStyles1.Append(latentStyleExceptionInfo334);
            latentStyles1.Append(latentStyleExceptionInfo335);
            latentStyles1.Append(latentStyleExceptionInfo336);
            latentStyles1.Append(latentStyleExceptionInfo337);
            latentStyles1.Append(latentStyleExceptionInfo338);
            latentStyles1.Append(latentStyleExceptionInfo339);
            latentStyles1.Append(latentStyleExceptionInfo340);
            latentStyles1.Append(latentStyleExceptionInfo341);
            latentStyles1.Append(latentStyleExceptionInfo342);
            latentStyles1.Append(latentStyleExceptionInfo343);
            latentStyles1.Append(latentStyleExceptionInfo344);
            latentStyles1.Append(latentStyleExceptionInfo345);
            latentStyles1.Append(latentStyleExceptionInfo346);
            latentStyles1.Append(latentStyleExceptionInfo347);
            latentStyles1.Append(latentStyleExceptionInfo348);
            latentStyles1.Append(latentStyleExceptionInfo349);
            latentStyles1.Append(latentStyleExceptionInfo350);
            latentStyles1.Append(latentStyleExceptionInfo351);
            latentStyles1.Append(latentStyleExceptionInfo352);
            latentStyles1.Append(latentStyleExceptionInfo353);
            latentStyles1.Append(latentStyleExceptionInfo354);
            latentStyles1.Append(latentStyleExceptionInfo355);
            latentStyles1.Append(latentStyleExceptionInfo356);
            latentStyles1.Append(latentStyleExceptionInfo357);
            latentStyles1.Append(latentStyleExceptionInfo358);
            latentStyles1.Append(latentStyleExceptionInfo359);
            latentStyles1.Append(latentStyleExceptionInfo360);
            latentStyles1.Append(latentStyleExceptionInfo361);
            latentStyles1.Append(latentStyleExceptionInfo362);
            latentStyles1.Append(latentStyleExceptionInfo363);
            latentStyles1.Append(latentStyleExceptionInfo364);
            latentStyles1.Append(latentStyleExceptionInfo365);
            latentStyles1.Append(latentStyleExceptionInfo366);
            latentStyles1.Append(latentStyleExceptionInfo367);
            latentStyles1.Append(latentStyleExceptionInfo368);
            latentStyles1.Append(latentStyleExceptionInfo369);
            latentStyles1.Append(latentStyleExceptionInfo370);
            latentStyles1.Append(latentStyleExceptionInfo371);

            Style style1 = new Style() { Type = StyleValues.Paragraph, StyleId = "a", Default = true };
            StyleName styleName1 = new StyleName() { Val = "Normal" };
            PrimaryStyle primaryStyle1 = new PrimaryStyle();

            style1.Append(styleName1);
            style1.Append(primaryStyle1);

            Style style2 = new Style() { Type = StyleValues.Character, StyleId = "a0", Default = true };
            StyleName styleName2 = new StyleName() { Val = "Default Paragraph Font" };
            UIPriority uIPriority1 = new UIPriority() { Val = 1 };
            SemiHidden semiHidden1 = new SemiHidden();
            UnhideWhenUsed unhideWhenUsed1 = new UnhideWhenUsed();

            style2.Append(styleName2);
            style2.Append(uIPriority1);
            style2.Append(semiHidden1);
            style2.Append(unhideWhenUsed1);

            Style style3 = new Style() { Type = StyleValues.Table, StyleId = "a1", Default = true };
            StyleName styleName3 = new StyleName() { Val = "Normal Table" };
            UIPriority uIPriority2 = new UIPriority() { Val = 99 };
            SemiHidden semiHidden2 = new SemiHidden();
            UnhideWhenUsed unhideWhenUsed2 = new UnhideWhenUsed();

            StyleTableProperties styleTableProperties1 = new StyleTableProperties();
            TableIndentation tableIndentation1 = new TableIndentation() { Width = 0, Type = TableWidthUnitValues.Dxa };

            TableCellMarginDefault tableCellMarginDefault1 = new TableCellMarginDefault();
            TopMargin topMargin1 = new TopMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };
            TableCellLeftMargin tableCellLeftMargin1 = new TableCellLeftMargin() { Width = 108, Type = TableWidthValues.Dxa };
            BottomMargin bottomMargin1 = new BottomMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };
            TableCellRightMargin tableCellRightMargin1 = new TableCellRightMargin() { Width = 108, Type = TableWidthValues.Dxa };

            tableCellMarginDefault1.Append(topMargin1);
            tableCellMarginDefault1.Append(tableCellLeftMargin1);
            tableCellMarginDefault1.Append(bottomMargin1);
            tableCellMarginDefault1.Append(tableCellRightMargin1);

            styleTableProperties1.Append(tableIndentation1);
            styleTableProperties1.Append(tableCellMarginDefault1);

            style3.Append(styleName3);
            style3.Append(uIPriority2);
            style3.Append(semiHidden2);
            style3.Append(unhideWhenUsed2);
            style3.Append(styleTableProperties1);

            Style style4 = new Style() { Type = StyleValues.Numbering, StyleId = "a2", Default = true };
            StyleName styleName4 = new StyleName() { Val = "No List" };
            UIPriority uIPriority3 = new UIPriority() { Val = 99 };
            SemiHidden semiHidden3 = new SemiHidden();
            UnhideWhenUsed unhideWhenUsed3 = new UnhideWhenUsed();

            style4.Append(styleName4);
            style4.Append(uIPriority3);
            style4.Append(semiHidden3);
            style4.Append(unhideWhenUsed3);

            Style style5 = new Style() { Type = StyleValues.Table, StyleId = "a3" };
            StyleName styleName5 = new StyleName() { Val = "Table Grid" };
            BasedOn basedOn1 = new BasedOn() { Val = "a1" };
            UIPriority uIPriority4 = new UIPriority() { Val = 39 };
            Rsid rsid4 = new Rsid() { Val = "004F2046" };

            StyleParagraphProperties styleParagraphProperties1 = new StyleParagraphProperties();
            SpacingBetweenLines spacingBetweenLines16 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            styleParagraphProperties1.Append(spacingBetweenLines16);

            StyleTableProperties styleTableProperties2 = new StyleTableProperties();

            TableBorders tableBorders2 = new TableBorders();
            TopBorder topBorder2 = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder2 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            InsideHorizontalBorder insideHorizontalBorder2 = new InsideHorizontalBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            InsideVerticalBorder insideVerticalBorder2 = new InsideVerticalBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableBorders2.Append(topBorder2);
            tableBorders2.Append(leftBorder2);
            tableBorders2.Append(bottomBorder2);
            tableBorders2.Append(rightBorder2);
            tableBorders2.Append(insideHorizontalBorder2);
            tableBorders2.Append(insideVerticalBorder2);

            styleTableProperties2.Append(tableBorders2);

            style5.Append(styleName5);
            style5.Append(basedOn1);
            style5.Append(uIPriority4);
            style5.Append(rsid4);
            style5.Append(styleParagraphProperties1);
            style5.Append(styleTableProperties2);

            styles1.Append(docDefaults1);
            styles1.Append(latentStyles1);
            styles1.Append(style1);
            styles1.Append(style2);
            styles1.Append(style3);
            styles1.Append(style4);
            styles1.Append(style5);

            styleDefinitionsPart1.Styles = styles1;
        }

        // Generates content of customXmlPart1.
        private void GenerateCustomXmlPart1Content(CustomXmlPart customXmlPart1)
        {
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(customXmlPart1.GetStream(System.IO.FileMode.Create), System.Text.Encoding.UTF8);
            writer.WriteRaw("<b:Sources SelectedStyle=\"\\APASixthEditionOfficeOnline.xsl\" StyleName=\"APA\" Version=\"6\" xmlns:b=\"http://schemas.openxmlformats.org/officeDocument/2006/bibliography\" xmlns=\"http://schemas.openxmlformats.org/officeDocument/2006/bibliography\"></b:Sources>\r\n");
            writer.Flush();
            writer.Close();
        }

        // Generates content of customXmlPropertiesPart1.
        private void GenerateCustomXmlPropertiesPart1Content(CustomXmlPropertiesPart customXmlPropertiesPart1)
        {
            Ds.DataStoreItem dataStoreItem1 = new Ds.DataStoreItem() { ItemId = "{33A19745-D1A9-49C7-9956-C8CB2FB2C1E6}" };
            dataStoreItem1.AddNamespaceDeclaration("ds", "http://schemas.openxmlformats.org/officeDocument/2006/customXml");

            Ds.SchemaReferences schemaReferences1 = new Ds.SchemaReferences();
            Ds.SchemaReference schemaReference1 = new Ds.SchemaReference() { Uri = "http://schemas.openxmlformats.org/officeDocument/2006/bibliography" };

            schemaReferences1.Append(schemaReference1);

            dataStoreItem1.Append(schemaReferences1);

            customXmlPropertiesPart1.DataStoreItem = dataStoreItem1;
        }

        // Generates content of imagePart3.
        private void GenerateImagePart3Content(ImagePart imagePart3)
        {
            System.IO.Stream data = GetBinaryDataStream(imagePart3Data);
            imagePart3.FeedData(data);
            data.Close();
        }

        // Generates content of imagePart4.
        private void GenerateImagePart4Content(ImagePart imagePart4)
        {
            System.IO.Stream data = GetBinaryDataStream(imagePart4Data);
            imagePart4.FeedData(data);
            data.Close();
        }

        // Generates content of themePart1.
        private void GenerateThemePart1Content(ThemePart themePart1)
        {
            A.Theme theme1 = new A.Theme() { Name = "Тема Office" };
            theme1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            A.ThemeElements themeElements1 = new A.ThemeElements();

            A.ColorScheme colorScheme1 = new A.ColorScheme() { Name = "Стандартная" };

            A.Dark1Color dark1Color1 = new A.Dark1Color();
            A.SystemColor systemColor1 = new A.SystemColor() { Val = A.SystemColorValues.WindowText, LastColor = "000000" };

            dark1Color1.Append(systemColor1);

            A.Light1Color light1Color1 = new A.Light1Color();
            A.SystemColor systemColor2 = new A.SystemColor() { Val = A.SystemColorValues.Window, LastColor = "FFFFFF" };

            light1Color1.Append(systemColor2);

            A.Dark2Color dark2Color1 = new A.Dark2Color();
            A.RgbColorModelHex rgbColorModelHex1 = new A.RgbColorModelHex() { Val = "44546A" };

            dark2Color1.Append(rgbColorModelHex1);

            A.Light2Color light2Color1 = new A.Light2Color();
            A.RgbColorModelHex rgbColorModelHex2 = new A.RgbColorModelHex() { Val = "E7E6E6" };

            light2Color1.Append(rgbColorModelHex2);

            A.Accent1Color accent1Color1 = new A.Accent1Color();
            A.RgbColorModelHex rgbColorModelHex3 = new A.RgbColorModelHex() { Val = "5B9BD5" };

            accent1Color1.Append(rgbColorModelHex3);

            A.Accent2Color accent2Color1 = new A.Accent2Color();
            A.RgbColorModelHex rgbColorModelHex4 = new A.RgbColorModelHex() { Val = "ED7D31" };

            accent2Color1.Append(rgbColorModelHex4);

            A.Accent3Color accent3Color1 = new A.Accent3Color();
            A.RgbColorModelHex rgbColorModelHex5 = new A.RgbColorModelHex() { Val = "A5A5A5" };

            accent3Color1.Append(rgbColorModelHex5);

            A.Accent4Color accent4Color1 = new A.Accent4Color();
            A.RgbColorModelHex rgbColorModelHex6 = new A.RgbColorModelHex() { Val = "FFC000" };

            accent4Color1.Append(rgbColorModelHex6);

            A.Accent5Color accent5Color1 = new A.Accent5Color();
            A.RgbColorModelHex rgbColorModelHex7 = new A.RgbColorModelHex() { Val = "4472C4" };

            accent5Color1.Append(rgbColorModelHex7);

            A.Accent6Color accent6Color1 = new A.Accent6Color();
            A.RgbColorModelHex rgbColorModelHex8 = new A.RgbColorModelHex() { Val = "70AD47" };

            accent6Color1.Append(rgbColorModelHex8);

            A.Hyperlink hyperlink1 = new A.Hyperlink();
            A.RgbColorModelHex rgbColorModelHex9 = new A.RgbColorModelHex() { Val = "0563C1" };

            hyperlink1.Append(rgbColorModelHex9);

            A.FollowedHyperlinkColor followedHyperlinkColor1 = new A.FollowedHyperlinkColor();
            A.RgbColorModelHex rgbColorModelHex10 = new A.RgbColorModelHex() { Val = "954F72" };

            followedHyperlinkColor1.Append(rgbColorModelHex10);

            colorScheme1.Append(dark1Color1);
            colorScheme1.Append(light1Color1);
            colorScheme1.Append(dark2Color1);
            colorScheme1.Append(light2Color1);
            colorScheme1.Append(accent1Color1);
            colorScheme1.Append(accent2Color1);
            colorScheme1.Append(accent3Color1);
            colorScheme1.Append(accent4Color1);
            colorScheme1.Append(accent5Color1);
            colorScheme1.Append(accent6Color1);
            colorScheme1.Append(hyperlink1);
            colorScheme1.Append(followedHyperlinkColor1);

            A.FontScheme fontScheme1 = new A.FontScheme() { Name = "Стандартная" };

            A.MajorFont majorFont1 = new A.MajorFont();
            A.LatinFont latinFont1 = new A.LatinFont() { Typeface = "Calibri Light", Panose = "020F0302020204030204" };
            A.EastAsianFont eastAsianFont1 = new A.EastAsianFont() { Typeface = "" };
            A.ComplexScriptFont complexScriptFont1 = new A.ComplexScriptFont() { Typeface = "" };
            A.SupplementalFont supplementalFont1 = new A.SupplementalFont() { Script = "Jpan", Typeface = "ＭＳ ゴシック" };
            A.SupplementalFont supplementalFont2 = new A.SupplementalFont() { Script = "Hang", Typeface = "맑은 고딕" };
            A.SupplementalFont supplementalFont3 = new A.SupplementalFont() { Script = "Hans", Typeface = "宋体" };
            A.SupplementalFont supplementalFont4 = new A.SupplementalFont() { Script = "Hant", Typeface = "新細明體" };
            A.SupplementalFont supplementalFont5 = new A.SupplementalFont() { Script = "Arab", Typeface = "Times New Roman" };
            A.SupplementalFont supplementalFont6 = new A.SupplementalFont() { Script = "Hebr", Typeface = "Times New Roman" };
            A.SupplementalFont supplementalFont7 = new A.SupplementalFont() { Script = "Thai", Typeface = "Angsana New" };
            A.SupplementalFont supplementalFont8 = new A.SupplementalFont() { Script = "Ethi", Typeface = "Nyala" };
            A.SupplementalFont supplementalFont9 = new A.SupplementalFont() { Script = "Beng", Typeface = "Vrinda" };
            A.SupplementalFont supplementalFont10 = new A.SupplementalFont() { Script = "Gujr", Typeface = "Shruti" };
            A.SupplementalFont supplementalFont11 = new A.SupplementalFont() { Script = "Khmr", Typeface = "MoolBoran" };
            A.SupplementalFont supplementalFont12 = new A.SupplementalFont() { Script = "Knda", Typeface = "Tunga" };
            A.SupplementalFont supplementalFont13 = new A.SupplementalFont() { Script = "Guru", Typeface = "Raavi" };
            A.SupplementalFont supplementalFont14 = new A.SupplementalFont() { Script = "Cans", Typeface = "Euphemia" };
            A.SupplementalFont supplementalFont15 = new A.SupplementalFont() { Script = "Cher", Typeface = "Plantagenet Cherokee" };
            A.SupplementalFont supplementalFont16 = new A.SupplementalFont() { Script = "Yiii", Typeface = "Microsoft Yi Baiti" };
            A.SupplementalFont supplementalFont17 = new A.SupplementalFont() { Script = "Tibt", Typeface = "Microsoft Himalaya" };
            A.SupplementalFont supplementalFont18 = new A.SupplementalFont() { Script = "Thaa", Typeface = "MV Boli" };
            A.SupplementalFont supplementalFont19 = new A.SupplementalFont() { Script = "Deva", Typeface = "Mangal" };
            A.SupplementalFont supplementalFont20 = new A.SupplementalFont() { Script = "Telu", Typeface = "Gautami" };
            A.SupplementalFont supplementalFont21 = new A.SupplementalFont() { Script = "Taml", Typeface = "Latha" };
            A.SupplementalFont supplementalFont22 = new A.SupplementalFont() { Script = "Syrc", Typeface = "Estrangelo Edessa" };
            A.SupplementalFont supplementalFont23 = new A.SupplementalFont() { Script = "Orya", Typeface = "Kalinga" };
            A.SupplementalFont supplementalFont24 = new A.SupplementalFont() { Script = "Mlym", Typeface = "Kartika" };
            A.SupplementalFont supplementalFont25 = new A.SupplementalFont() { Script = "Laoo", Typeface = "DokChampa" };
            A.SupplementalFont supplementalFont26 = new A.SupplementalFont() { Script = "Sinh", Typeface = "Iskoola Pota" };
            A.SupplementalFont supplementalFont27 = new A.SupplementalFont() { Script = "Mong", Typeface = "Mongolian Baiti" };
            A.SupplementalFont supplementalFont28 = new A.SupplementalFont() { Script = "Viet", Typeface = "Times New Roman" };
            A.SupplementalFont supplementalFont29 = new A.SupplementalFont() { Script = "Uigh", Typeface = "Microsoft Uighur" };
            A.SupplementalFont supplementalFont30 = new A.SupplementalFont() { Script = "Geor", Typeface = "Sylfaen" };

            majorFont1.Append(latinFont1);
            majorFont1.Append(eastAsianFont1);
            majorFont1.Append(complexScriptFont1);
            majorFont1.Append(supplementalFont1);
            majorFont1.Append(supplementalFont2);
            majorFont1.Append(supplementalFont3);
            majorFont1.Append(supplementalFont4);
            majorFont1.Append(supplementalFont5);
            majorFont1.Append(supplementalFont6);
            majorFont1.Append(supplementalFont7);
            majorFont1.Append(supplementalFont8);
            majorFont1.Append(supplementalFont9);
            majorFont1.Append(supplementalFont10);
            majorFont1.Append(supplementalFont11);
            majorFont1.Append(supplementalFont12);
            majorFont1.Append(supplementalFont13);
            majorFont1.Append(supplementalFont14);
            majorFont1.Append(supplementalFont15);
            majorFont1.Append(supplementalFont16);
            majorFont1.Append(supplementalFont17);
            majorFont1.Append(supplementalFont18);
            majorFont1.Append(supplementalFont19);
            majorFont1.Append(supplementalFont20);
            majorFont1.Append(supplementalFont21);
            majorFont1.Append(supplementalFont22);
            majorFont1.Append(supplementalFont23);
            majorFont1.Append(supplementalFont24);
            majorFont1.Append(supplementalFont25);
            majorFont1.Append(supplementalFont26);
            majorFont1.Append(supplementalFont27);
            majorFont1.Append(supplementalFont28);
            majorFont1.Append(supplementalFont29);
            majorFont1.Append(supplementalFont30);

            A.MinorFont minorFont1 = new A.MinorFont();
            A.LatinFont latinFont2 = new A.LatinFont() { Typeface = "Calibri", Panose = "020F0502020204030204" };
            A.EastAsianFont eastAsianFont2 = new A.EastAsianFont() { Typeface = "" };
            A.ComplexScriptFont complexScriptFont2 = new A.ComplexScriptFont() { Typeface = "" };
            A.SupplementalFont supplementalFont31 = new A.SupplementalFont() { Script = "Jpan", Typeface = "ＭＳ 明朝" };
            A.SupplementalFont supplementalFont32 = new A.SupplementalFont() { Script = "Hang", Typeface = "맑은 고딕" };
            A.SupplementalFont supplementalFont33 = new A.SupplementalFont() { Script = "Hans", Typeface = "宋体" };
            A.SupplementalFont supplementalFont34 = new A.SupplementalFont() { Script = "Hant", Typeface = "新細明體" };
            A.SupplementalFont supplementalFont35 = new A.SupplementalFont() { Script = "Arab", Typeface = "Arial" };
            A.SupplementalFont supplementalFont36 = new A.SupplementalFont() { Script = "Hebr", Typeface = "Arial" };
            A.SupplementalFont supplementalFont37 = new A.SupplementalFont() { Script = "Thai", Typeface = "Cordia New" };
            A.SupplementalFont supplementalFont38 = new A.SupplementalFont() { Script = "Ethi", Typeface = "Nyala" };
            A.SupplementalFont supplementalFont39 = new A.SupplementalFont() { Script = "Beng", Typeface = "Vrinda" };
            A.SupplementalFont supplementalFont40 = new A.SupplementalFont() { Script = "Gujr", Typeface = "Shruti" };
            A.SupplementalFont supplementalFont41 = new A.SupplementalFont() { Script = "Khmr", Typeface = "DaunPenh" };
            A.SupplementalFont supplementalFont42 = new A.SupplementalFont() { Script = "Knda", Typeface = "Tunga" };
            A.SupplementalFont supplementalFont43 = new A.SupplementalFont() { Script = "Guru", Typeface = "Raavi" };
            A.SupplementalFont supplementalFont44 = new A.SupplementalFont() { Script = "Cans", Typeface = "Euphemia" };
            A.SupplementalFont supplementalFont45 = new A.SupplementalFont() { Script = "Cher", Typeface = "Plantagenet Cherokee" };
            A.SupplementalFont supplementalFont46 = new A.SupplementalFont() { Script = "Yiii", Typeface = "Microsoft Yi Baiti" };
            A.SupplementalFont supplementalFont47 = new A.SupplementalFont() { Script = "Tibt", Typeface = "Microsoft Himalaya" };
            A.SupplementalFont supplementalFont48 = new A.SupplementalFont() { Script = "Thaa", Typeface = "MV Boli" };
            A.SupplementalFont supplementalFont49 = new A.SupplementalFont() { Script = "Deva", Typeface = "Mangal" };
            A.SupplementalFont supplementalFont50 = new A.SupplementalFont() { Script = "Telu", Typeface = "Gautami" };
            A.SupplementalFont supplementalFont51 = new A.SupplementalFont() { Script = "Taml", Typeface = "Latha" };
            A.SupplementalFont supplementalFont52 = new A.SupplementalFont() { Script = "Syrc", Typeface = "Estrangelo Edessa" };
            A.SupplementalFont supplementalFont53 = new A.SupplementalFont() { Script = "Orya", Typeface = "Kalinga" };
            A.SupplementalFont supplementalFont54 = new A.SupplementalFont() { Script = "Mlym", Typeface = "Kartika" };
            A.SupplementalFont supplementalFont55 = new A.SupplementalFont() { Script = "Laoo", Typeface = "DokChampa" };
            A.SupplementalFont supplementalFont56 = new A.SupplementalFont() { Script = "Sinh", Typeface = "Iskoola Pota" };
            A.SupplementalFont supplementalFont57 = new A.SupplementalFont() { Script = "Mong", Typeface = "Mongolian Baiti" };
            A.SupplementalFont supplementalFont58 = new A.SupplementalFont() { Script = "Viet", Typeface = "Arial" };
            A.SupplementalFont supplementalFont59 = new A.SupplementalFont() { Script = "Uigh", Typeface = "Microsoft Uighur" };
            A.SupplementalFont supplementalFont60 = new A.SupplementalFont() { Script = "Geor", Typeface = "Sylfaen" };

            minorFont1.Append(latinFont2);
            minorFont1.Append(eastAsianFont2);
            minorFont1.Append(complexScriptFont2);
            minorFont1.Append(supplementalFont31);
            minorFont1.Append(supplementalFont32);
            minorFont1.Append(supplementalFont33);
            minorFont1.Append(supplementalFont34);
            minorFont1.Append(supplementalFont35);
            minorFont1.Append(supplementalFont36);
            minorFont1.Append(supplementalFont37);
            minorFont1.Append(supplementalFont38);
            minorFont1.Append(supplementalFont39);
            minorFont1.Append(supplementalFont40);
            minorFont1.Append(supplementalFont41);
            minorFont1.Append(supplementalFont42);
            minorFont1.Append(supplementalFont43);
            minorFont1.Append(supplementalFont44);
            minorFont1.Append(supplementalFont45);
            minorFont1.Append(supplementalFont46);
            minorFont1.Append(supplementalFont47);
            minorFont1.Append(supplementalFont48);
            minorFont1.Append(supplementalFont49);
            minorFont1.Append(supplementalFont50);
            minorFont1.Append(supplementalFont51);
            minorFont1.Append(supplementalFont52);
            minorFont1.Append(supplementalFont53);
            minorFont1.Append(supplementalFont54);
            minorFont1.Append(supplementalFont55);
            minorFont1.Append(supplementalFont56);
            minorFont1.Append(supplementalFont57);
            minorFont1.Append(supplementalFont58);
            minorFont1.Append(supplementalFont59);
            minorFont1.Append(supplementalFont60);

            fontScheme1.Append(majorFont1);
            fontScheme1.Append(minorFont1);

            A.FormatScheme formatScheme1 = new A.FormatScheme() { Name = "Стандартная" };

            A.FillStyleList fillStyleList1 = new A.FillStyleList();

            A.SolidFill solidFill3 = new A.SolidFill();
            A.SchemeColor schemeColor11 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };

            solidFill3.Append(schemeColor11);

            A.GradientFill gradientFill1 = new A.GradientFill() { RotateWithShape = true };

            A.GradientStopList gradientStopList1 = new A.GradientStopList();

            A.GradientStop gradientStop1 = new A.GradientStop() { Position = 0 };

            A.SchemeColor schemeColor12 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };
            A.LuminanceModulation luminanceModulation1 = new A.LuminanceModulation() { Val = 110000 };
            A.SaturationModulation saturationModulation1 = new A.SaturationModulation() { Val = 105000 };
            A.Tint tint1 = new A.Tint() { Val = 67000 };

            schemeColor12.Append(luminanceModulation1);
            schemeColor12.Append(saturationModulation1);
            schemeColor12.Append(tint1);

            gradientStop1.Append(schemeColor12);

            A.GradientStop gradientStop2 = new A.GradientStop() { Position = 50000 };

            A.SchemeColor schemeColor13 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };
            A.LuminanceModulation luminanceModulation2 = new A.LuminanceModulation() { Val = 105000 };
            A.SaturationModulation saturationModulation2 = new A.SaturationModulation() { Val = 103000 };
            A.Tint tint2 = new A.Tint() { Val = 73000 };

            schemeColor13.Append(luminanceModulation2);
            schemeColor13.Append(saturationModulation2);
            schemeColor13.Append(tint2);

            gradientStop2.Append(schemeColor13);

            A.GradientStop gradientStop3 = new A.GradientStop() { Position = 100000 };

            A.SchemeColor schemeColor14 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };
            A.LuminanceModulation luminanceModulation3 = new A.LuminanceModulation() { Val = 105000 };
            A.SaturationModulation saturationModulation3 = new A.SaturationModulation() { Val = 109000 };
            A.Tint tint3 = new A.Tint() { Val = 81000 };

            schemeColor14.Append(luminanceModulation3);
            schemeColor14.Append(saturationModulation3);
            schemeColor14.Append(tint3);

            gradientStop3.Append(schemeColor14);

            gradientStopList1.Append(gradientStop1);
            gradientStopList1.Append(gradientStop2);
            gradientStopList1.Append(gradientStop3);
            A.LinearGradientFill linearGradientFill1 = new A.LinearGradientFill() { Angle = 5400000, Scaled = false };

            gradientFill1.Append(gradientStopList1);
            gradientFill1.Append(linearGradientFill1);

            A.GradientFill gradientFill2 = new A.GradientFill() { RotateWithShape = true };

            A.GradientStopList gradientStopList2 = new A.GradientStopList();

            A.GradientStop gradientStop4 = new A.GradientStop() { Position = 0 };

            A.SchemeColor schemeColor15 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };
            A.SaturationModulation saturationModulation4 = new A.SaturationModulation() { Val = 103000 };
            A.LuminanceModulation luminanceModulation4 = new A.LuminanceModulation() { Val = 102000 };
            A.Tint tint4 = new A.Tint() { Val = 94000 };

            schemeColor15.Append(saturationModulation4);
            schemeColor15.Append(luminanceModulation4);
            schemeColor15.Append(tint4);

            gradientStop4.Append(schemeColor15);

            A.GradientStop gradientStop5 = new A.GradientStop() { Position = 50000 };

            A.SchemeColor schemeColor16 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };
            A.SaturationModulation saturationModulation5 = new A.SaturationModulation() { Val = 110000 };
            A.LuminanceModulation luminanceModulation5 = new A.LuminanceModulation() { Val = 100000 };
            A.Shade shade1 = new A.Shade() { Val = 100000 };

            schemeColor16.Append(saturationModulation5);
            schemeColor16.Append(luminanceModulation5);
            schemeColor16.Append(shade1);

            gradientStop5.Append(schemeColor16);

            A.GradientStop gradientStop6 = new A.GradientStop() { Position = 100000 };

            A.SchemeColor schemeColor17 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };
            A.LuminanceModulation luminanceModulation6 = new A.LuminanceModulation() { Val = 99000 };
            A.SaturationModulation saturationModulation6 = new A.SaturationModulation() { Val = 120000 };
            A.Shade shade2 = new A.Shade() { Val = 78000 };

            schemeColor17.Append(luminanceModulation6);
            schemeColor17.Append(saturationModulation6);
            schemeColor17.Append(shade2);

            gradientStop6.Append(schemeColor17);

            gradientStopList2.Append(gradientStop4);
            gradientStopList2.Append(gradientStop5);
            gradientStopList2.Append(gradientStop6);
            A.LinearGradientFill linearGradientFill2 = new A.LinearGradientFill() { Angle = 5400000, Scaled = false };

            gradientFill2.Append(gradientStopList2);
            gradientFill2.Append(linearGradientFill2);

            fillStyleList1.Append(solidFill3);
            fillStyleList1.Append(gradientFill1);
            fillStyleList1.Append(gradientFill2);

            A.LineStyleList lineStyleList1 = new A.LineStyleList();

            A.Outline outline6 = new A.Outline() { Width = 6350, CapType = A.LineCapValues.Flat, CompoundLineType = A.CompoundLineValues.Single, Alignment = A.PenAlignmentValues.Center };

            A.SolidFill solidFill4 = new A.SolidFill();
            A.SchemeColor schemeColor18 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };

            solidFill4.Append(schemeColor18);
            A.PresetDash presetDash1 = new A.PresetDash() { Val = A.PresetLineDashValues.Solid };
            A.Miter miter1 = new A.Miter() { Limit = 800000 };

            outline6.Append(solidFill4);
            outline6.Append(presetDash1);
            outline6.Append(miter1);

            A.Outline outline7 = new A.Outline() { Width = 12700, CapType = A.LineCapValues.Flat, CompoundLineType = A.CompoundLineValues.Single, Alignment = A.PenAlignmentValues.Center };

            A.SolidFill solidFill5 = new A.SolidFill();
            A.SchemeColor schemeColor19 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };

            solidFill5.Append(schemeColor19);
            A.PresetDash presetDash2 = new A.PresetDash() { Val = A.PresetLineDashValues.Solid };
            A.Miter miter2 = new A.Miter() { Limit = 800000 };

            outline7.Append(solidFill5);
            outline7.Append(presetDash2);
            outline7.Append(miter2);

            A.Outline outline8 = new A.Outline() { Width = 19050, CapType = A.LineCapValues.Flat, CompoundLineType = A.CompoundLineValues.Single, Alignment = A.PenAlignmentValues.Center };

            A.SolidFill solidFill6 = new A.SolidFill();
            A.SchemeColor schemeColor20 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };

            solidFill6.Append(schemeColor20);
            A.PresetDash presetDash3 = new A.PresetDash() { Val = A.PresetLineDashValues.Solid };
            A.Miter miter3 = new A.Miter() { Limit = 800000 };

            outline8.Append(solidFill6);
            outline8.Append(presetDash3);
            outline8.Append(miter3);

            lineStyleList1.Append(outline6);
            lineStyleList1.Append(outline7);
            lineStyleList1.Append(outline8);

            A.EffectStyleList effectStyleList1 = new A.EffectStyleList();

            A.EffectStyle effectStyle1 = new A.EffectStyle();
            A.EffectList effectList1 = new A.EffectList();

            effectStyle1.Append(effectList1);

            A.EffectStyle effectStyle2 = new A.EffectStyle();
            A.EffectList effectList2 = new A.EffectList();

            effectStyle2.Append(effectList2);

            A.EffectStyle effectStyle3 = new A.EffectStyle();

            A.EffectList effectList3 = new A.EffectList();

            A.OuterShadow outerShadow1 = new A.OuterShadow() { BlurRadius = 57150L, Distance = 19050L, Direction = 5400000, Alignment = A.RectangleAlignmentValues.Center, RotateWithShape = false };

            A.RgbColorModelHex rgbColorModelHex11 = new A.RgbColorModelHex() { Val = "000000" };
            A.Alpha alpha1 = new A.Alpha() { Val = 63000 };

            rgbColorModelHex11.Append(alpha1);

            outerShadow1.Append(rgbColorModelHex11);

            effectList3.Append(outerShadow1);

            effectStyle3.Append(effectList3);

            effectStyleList1.Append(effectStyle1);
            effectStyleList1.Append(effectStyle2);
            effectStyleList1.Append(effectStyle3);

            A.BackgroundFillStyleList backgroundFillStyleList1 = new A.BackgroundFillStyleList();

            A.SolidFill solidFill7 = new A.SolidFill();
            A.SchemeColor schemeColor21 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };

            solidFill7.Append(schemeColor21);

            A.SolidFill solidFill8 = new A.SolidFill();

            A.SchemeColor schemeColor22 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };
            A.Tint tint5 = new A.Tint() { Val = 95000 };
            A.SaturationModulation saturationModulation7 = new A.SaturationModulation() { Val = 170000 };

            schemeColor22.Append(tint5);
            schemeColor22.Append(saturationModulation7);

            solidFill8.Append(schemeColor22);

            A.GradientFill gradientFill3 = new A.GradientFill() { RotateWithShape = true };

            A.GradientStopList gradientStopList3 = new A.GradientStopList();

            A.GradientStop gradientStop7 = new A.GradientStop() { Position = 0 };

            A.SchemeColor schemeColor23 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };
            A.Tint tint6 = new A.Tint() { Val = 93000 };
            A.SaturationModulation saturationModulation8 = new A.SaturationModulation() { Val = 150000 };
            A.Shade shade3 = new A.Shade() { Val = 98000 };
            A.LuminanceModulation luminanceModulation7 = new A.LuminanceModulation() { Val = 102000 };

            schemeColor23.Append(tint6);
            schemeColor23.Append(saturationModulation8);
            schemeColor23.Append(shade3);
            schemeColor23.Append(luminanceModulation7);

            gradientStop7.Append(schemeColor23);

            A.GradientStop gradientStop8 = new A.GradientStop() { Position = 50000 };

            A.SchemeColor schemeColor24 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };
            A.Tint tint7 = new A.Tint() { Val = 98000 };
            A.SaturationModulation saturationModulation9 = new A.SaturationModulation() { Val = 130000 };
            A.Shade shade4 = new A.Shade() { Val = 90000 };
            A.LuminanceModulation luminanceModulation8 = new A.LuminanceModulation() { Val = 103000 };

            schemeColor24.Append(tint7);
            schemeColor24.Append(saturationModulation9);
            schemeColor24.Append(shade4);
            schemeColor24.Append(luminanceModulation8);

            gradientStop8.Append(schemeColor24);

            A.GradientStop gradientStop9 = new A.GradientStop() { Position = 100000 };

            A.SchemeColor schemeColor25 = new A.SchemeColor() { Val = A.SchemeColorValues.PhColor };
            A.Shade shade5 = new A.Shade() { Val = 63000 };
            A.SaturationModulation saturationModulation10 = new A.SaturationModulation() { Val = 120000 };

            schemeColor25.Append(shade5);
            schemeColor25.Append(saturationModulation10);

            gradientStop9.Append(schemeColor25);

            gradientStopList3.Append(gradientStop7);
            gradientStopList3.Append(gradientStop8);
            gradientStopList3.Append(gradientStop9);
            A.LinearGradientFill linearGradientFill3 = new A.LinearGradientFill() { Angle = 5400000, Scaled = false };

            gradientFill3.Append(gradientStopList3);
            gradientFill3.Append(linearGradientFill3);

            backgroundFillStyleList1.Append(solidFill7);
            backgroundFillStyleList1.Append(solidFill8);
            backgroundFillStyleList1.Append(gradientFill3);

            formatScheme1.Append(fillStyleList1);
            formatScheme1.Append(lineStyleList1);
            formatScheme1.Append(effectStyleList1);
            formatScheme1.Append(backgroundFillStyleList1);

            themeElements1.Append(colorScheme1);
            themeElements1.Append(fontScheme1);
            themeElements1.Append(formatScheme1);
            A.ObjectDefaults objectDefaults1 = new A.ObjectDefaults();
            A.ExtraColorSchemeList extraColorSchemeList1 = new A.ExtraColorSchemeList();

            A.ExtensionList extensionList1 = new A.ExtensionList();

            A.Extension extension1 = new A.Extension() { Uri = "{05A4C25C-085E-4340-85A3-A5531E510DB2}" };

            OpenXmlUnknownElement openXmlUnknownElement3 = OpenXmlUnknownElement.CreateOpenXmlUnknownElement("<thm15:themeFamily xmlns:thm15=\"http://schemas.microsoft.com/office/thememl/2012/main\" name=\"Office Theme\" id=\"{62F939B6-93AF-4DB8-9C6B-D6C7DFDC589F}\" vid=\"{4A3C46E8-61CC-4603-A589-7422A47A8E4A}\" />");

            extension1.Append(openXmlUnknownElement3);

            extensionList1.Append(extension1);

            theme1.Append(themeElements1);
            theme1.Append(objectDefaults1);
            theme1.Append(extraColorSchemeList1);
            theme1.Append(extensionList1);

            themePart1.Theme = theme1;
        }

        // Generates content of webSettingsPart1.
        private void GenerateWebSettingsPart1Content(WebSettingsPart webSettingsPart1)
        {
            WebSettings webSettings1 = new WebSettings() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15" } };
            webSettings1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            webSettings1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            webSettings1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            webSettings1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            webSettings1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");
            OptimizeForBrowser optimizeForBrowser1 = new OptimizeForBrowser();
            AllowPNG allowPNG1 = new AllowPNG();

            webSettings1.Append(optimizeForBrowser1);
            webSettings1.Append(allowPNG1);

            webSettingsPart1.WebSettings = webSettings1;
        }

        // Generates content of fontTablePart1.
        private void GenerateFontTablePart1Content(FontTablePart fontTablePart1)
        {
            Fonts fonts1 = new Fonts() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15" } };
            fonts1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            fonts1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            fonts1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            fonts1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            fonts1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");

            Font font1 = new Font() { Name = "Calibri" };
            Panose1Number panose1Number1 = new Panose1Number() { Val = "020F0502020204030204" };
            FontCharSet fontCharSet1 = new FontCharSet() { Val = "CC" };
            FontFamily fontFamily1 = new FontFamily() { Val = FontFamilyValues.Swiss };
            Pitch pitch1 = new Pitch() { Val = FontPitchValues.Variable };
            FontSignature fontSignature1 = new FontSignature() { UnicodeSignature0 = "E00002FF", UnicodeSignature1 = "4000ACFF", UnicodeSignature2 = "00000001", UnicodeSignature3 = "00000000", CodePageSignature0 = "0000019F", CodePageSignature1 = "00000000" };

            font1.Append(panose1Number1);
            font1.Append(fontCharSet1);
            font1.Append(fontFamily1);
            font1.Append(pitch1);
            font1.Append(fontSignature1);

            Font font2 = new Font() { Name = "Times New Roman" };
            Panose1Number panose1Number2 = new Panose1Number() { Val = "02020603050405020304" };
            FontCharSet fontCharSet2 = new FontCharSet() { Val = "CC" };
            FontFamily fontFamily2 = new FontFamily() { Val = FontFamilyValues.Roman };
            Pitch pitch2 = new Pitch() { Val = FontPitchValues.Variable };
            FontSignature fontSignature2 = new FontSignature() { UnicodeSignature0 = "E0002AFF", UnicodeSignature1 = "C0007841", UnicodeSignature2 = "00000009", UnicodeSignature3 = "00000000", CodePageSignature0 = "000001FF", CodePageSignature1 = "00000000" };

            font2.Append(panose1Number2);
            font2.Append(fontCharSet2);
            font2.Append(fontFamily2);
            font2.Append(pitch2);
            font2.Append(fontSignature2);

            Font font3 = new Font() { Name = "Arial" };
            Panose1Number panose1Number3 = new Panose1Number() { Val = "020B0604020202020204" };
            FontCharSet fontCharSet3 = new FontCharSet() { Val = "CC" };
            FontFamily fontFamily3 = new FontFamily() { Val = FontFamilyValues.Swiss };
            Pitch pitch3 = new Pitch() { Val = FontPitchValues.Variable };
            FontSignature fontSignature3 = new FontSignature() { UnicodeSignature0 = "E0002AFF", UnicodeSignature1 = "C0007843", UnicodeSignature2 = "00000009", UnicodeSignature3 = "00000000", CodePageSignature0 = "000001FF", CodePageSignature1 = "00000000" };

            font3.Append(panose1Number3);
            font3.Append(fontCharSet3);
            font3.Append(fontFamily3);
            font3.Append(pitch3);
            font3.Append(fontSignature3);

            Font font4 = new Font() { Name = "Calibri Light" };
            Panose1Number panose1Number4 = new Panose1Number() { Val = "020F0302020204030204" };
            FontCharSet fontCharSet4 = new FontCharSet() { Val = "CC" };
            FontFamily fontFamily4 = new FontFamily() { Val = FontFamilyValues.Swiss };
            Pitch pitch4 = new Pitch() { Val = FontPitchValues.Variable };
            FontSignature fontSignature4 = new FontSignature() { UnicodeSignature0 = "A00002EF", UnicodeSignature1 = "4000207B", UnicodeSignature2 = "00000000", UnicodeSignature3 = "00000000", CodePageSignature0 = "0000019F", CodePageSignature1 = "00000000" };

            font4.Append(panose1Number4);
            font4.Append(fontCharSet4);
            font4.Append(fontFamily4);
            font4.Append(pitch4);
            font4.Append(fontSignature4);

            fonts1.Append(font1);
            fonts1.Append(font2);
            fonts1.Append(font3);
            fonts1.Append(font4);

            fontTablePart1.Fonts = fonts1;
        }

        private void SetPackageProperties(OpenXmlPackage document)
        {
            document.PackageProperties.Creator = "Мельников Юрий";
            document.PackageProperties.Title = "";
            document.PackageProperties.Subject = "";
            document.PackageProperties.Keywords = "";
            document.PackageProperties.Description = "";
            document.PackageProperties.Revision = "1";
            document.PackageProperties.Created = System.Xml.XmlConvert.ToDateTime("2019-03-20T09:45:00Z", System.Xml.XmlDateTimeSerializationMode.RoundtripKind);
            document.PackageProperties.Modified = System.Xml.XmlConvert.ToDateTime("2019-03-20T09:46:00Z", System.Xml.XmlDateTimeSerializationMode.RoundtripKind);
            document.PackageProperties.LastModifiedBy = "Мельников Юрий";
        }

        #region Binary Data
        private string imagePart1Data = "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";

        private string imagePart2Data = "iVBORw0KGgoAAAANSUhEUgAAAHkAAAB3CAMAAAFnKS+dAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAB4UExURf///+Pj43x6euzs7KWjpH99fZqZmaCentLR0SMfIOHg4GhmZygkJVZTVGBdXX16e4B+fu3t7To2N3p4eE5LS6Khoff391pXWPLx8YmHhyQgIZeVlZyamvHw8EE9PpSSkvb29tfW1vn5+fz8/NjX1+Dg4OLh4QAAAHKiRPAAAAAodFJOU////////////////////////////////////////////////////wC+qi4YAAAACXBIWXMAACHVAAAh1QEEnLSdAAAC6klEQVRYR+2ZW3ObQAyFeci4MJnYcfzQ2g/tTMfu//+Jlc7RgrAXsAGThupLHHTZs1rWsFxS3ENVVfqp1GKAH/6ODTSddgX4mRbQThmwyINoB6/y5wSL3bxKn+IfmR/2ty2/7q/Dv24/pn/4mhqB6CDVUSmnFGNa7cPhgLxsU/7nbsd0XVfy8JjH4EDKbwfyQ/qePJpI/+AJ/WM7Yfxo0s4jBGCL0+4foU+nTAPBVkimDvKeNI4cjeHQgXnavfPQkTS2mi6KfVIz1kpLdyPSv2GMVdN4Uuf0sBVoIidMTtNA7EcmnUa+ujS9dhrfwOT0l6asTgndLVmIzKtBtDBHYQBwZggCsoy1sMbmCXOrzVGohilnVVvdLLhGXk3JvWoecWCE2hxlcbUbOS6MS9Y2RxmhJgikS3riMTVqO56uNkfhyGHKavKomrVhLq6OkS+kJgj0jRxYANyo+2oDr96XDQhczpeyvMAsLoqZ5cV+LRB8Ndx1/89VwED0zRzhOwLADh2FB4A5NSY2T/wp4uofqixPKofDvgl8IADTifUpZvf+ggBAA56FBP5bY3oxTAca3Cn2zQCCIRZCLIQ4geA0MRheSWA6ECQhTgyLzVHgx+EphDiBYIiF/0EM3ErSIQZbBIBFlPvEw7ePfZWniGerbM9zCm9cz/JIV/KmH1F2Kc9y9lR3ZrMgWD97PMDd4E6BFwt1Yw2VjYXafLNsC/+qxsGzEfDdTTetd/Hu5G/wi0jD9Mq24JB8ZbfqNixSOfbZuHkZeIOrzKsJwItCkq98unlreYWvnL+a5Cu7K13HPj+rMv4BQRauPDjbM1R2/ytZuHLscyJmuyZmu5eY7Rwx24l5KztmuCeZXtnNdp4V7vO6Z/u42fyyH3eYu33+2NQcLSTMUPmRs2qJO8D8SvJ5957rrhyznYjZronZ7iVmO0fMdmLeyo78PclDlR1O1zBceew+O6a/fRxZeezbxxlm+8vt87O+52C1FMVfY7j5iurQwqAAAAAASUVORK5CYII=";

        private string imagePart3Data = "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";

        private string imagePart4Data = "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";

        private System.IO.Stream GetBinaryDataStream(string base64String)
        {
            return new System.IO.MemoryStream(System.Convert.FromBase64String(base64String));
        }

        #endregion



    }
}