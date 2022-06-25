using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DigimonWorld2Tool.FileFormats
{
    /// <summary>
    /// This file format is based off of the FBX 7.4.0 project file specification
    /// </summary>
    class FBX
    {
        public StringBuilder FBXFileInfo { get; private set; }
        private Random rnd = new Random();

        private GeometryInfo[] Geometries { get; set; }

        public FBX(ModelFile model, string inputFileName, string outputFileName)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;


            FBXFileInfo = new StringBuilder();

            AddProjectFileInformation();
            AddFBXHeaderExtension(outputFileName);
            AddFBXGlobalSettings();
            AddDocumentDescription();
            AddDocumentReferences();
            AddObjectDefinitions(1); //model.Header.BoneCount
            AddObjectProperties(model);
            AddObjectConnections(model);
        }

        private void AddProjectFileInformation()
        {
            FBXFileInfo.AppendLine("; FBX 7.4.0 project file");
            FBXFileInfo.AppendLine("; ----------------------------------------------------");
            FBXFileInfo.AppendLine("");
        }

        private void AddFBXHeaderExtension(string outputFileName)
        {
            string tabIndent = "";
            FBXFileInfo.AppendLine($"FBXHeaderExtension: {{");

            tabIndent = "\t";
            FBXFileInfo.AppendLine($"{tabIndent}FBXHeaderVersion: 1003");
            FBXFileInfo.AppendLine($"{tabIndent}FBXVersion: 7400");
            FBXFileInfo.AppendLine($"{tabIndent} CreationTimeStamp: {{");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}Version: 1000");
            FBXFileInfo.AppendLine($"{tabIndent}Year: {DateTime.Now.Year}");
            FBXFileInfo.AppendLine($"{tabIndent}Month: {DateTime.Now.Month}");
            FBXFileInfo.AppendLine($"{tabIndent}Day: {DateTime.Now.Day}");
            FBXFileInfo.AppendLine($"{tabIndent}Hour: {DateTime.Now.Hour}");
            FBXFileInfo.AppendLine($"{tabIndent}Minute: {DateTime.Now.Minute}");
            FBXFileInfo.AppendLine($"{tabIndent}Second: {DateTime.Now.Second}");
            FBXFileInfo.AppendLine($"{tabIndent}Millisecond: {DateTime.Now.Millisecond}");

            tabIndent = "\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");
            FBXFileInfo.AppendLine($"{tabIndent}Creator: \"FBX SDK / FBX Plugins version 2020.0.1\"");
            FBXFileInfo.AppendLine($"{tabIndent}SceneInfo: \"SceneInfo::GlobalInfo\", \"UserData\" {{");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}Type: \"UserData\"");
            FBXFileInfo.AppendLine($"{tabIndent}Version: 100");
            FBXFileInfo.AppendLine($"{tabIndent}MetaData: {{");

            tabIndent = "\t\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}Version: 100");
            FBXFileInfo.AppendLine($"{tabIndent}Title: \"\"");
            FBXFileInfo.AppendLine($"{tabIndent}Subject: \"\"");
            FBXFileInfo.AppendLine($"{tabIndent}Author: \"RemyRm\"");
            FBXFileInfo.AppendLine($"{tabIndent}Keywords: \"\"");
            FBXFileInfo.AppendLine($"{tabIndent}Revision: \"\"");
            FBXFileInfo.AppendLine($"{tabIndent}Comment: \"This FBX was generated using the Digimon World 2 Tool, found at https://github.com/RemyRM/DigimonWorld2Tool \"");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");
            FBXFileInfo.AppendLine($"{tabIndent}Properties70: {{");

            tabIndent = "\t\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}P: \"DocumentUrl\", \"KString\", \"Url\", \"\", \"{outputFileName}\"");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"SrcDocumentUrl\", \"KString\", \"Url\", \"\", \"{outputFileName}\"");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Original\", \"Compound\", \"\", \"\"");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Original | DateTime_GMT\", \"DateTime\", \"\", \"\", \"{DateTime.Now:G}\"");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Original | FileName\", \"KString\", \"\", \"\", \"{outputFileName}\"");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"LastSaved\", \"Compound\", \"\", \"\"");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"LastSaved | DateTime_GMT\", \"DateTime\", \"\", \"\", \"{DateTime.Now:G}\"");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");

            tabIndent = "\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");

            tabIndent = "";
            FBXFileInfo.AppendLine($"{tabIndent}}}");
        }

        private void AddFBXGlobalSettings()
        {
            string tabIndent = "";
            FBXFileInfo.AppendLine($"{tabIndent}GlobalSettings: {{");

            tabIndent = "\t";
            FBXFileInfo.AppendLine($"{tabIndent}Version: 1000");
            FBXFileInfo.AppendLine($"{tabIndent}Properties70: {{");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}P: \"UpAxis\", \"int\", \"Integer\", \"\",1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"UpAxisSign\", \"int\", \"Integer\", \"\",1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"FrontAxis\", \"int\", \"Integer\", \"\",2");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"FrontAxisSign\", \"int\", \"Integer\", \"\",1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"CoordAxis\", \"int\", \"Integer\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"CoordAxisSign\", \"int\", \"Integer\", \"\",1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"OriginalUpAxis\", \"int\", \"Integer\", \"\",2");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"OriginalUpAxisSign\", \"int\", \"Integer\", \"\",1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"UnitScaleFactor\", \"double\", \"Number\", \"\",0.1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"OriginalUnitScaleFactor\", \"double\", \"Number\", \"\",0.1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"AmbientColor\", \"ColorRGB\", \"Color\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"DefaultCamera\", \"KString\", \"\", \"\", \"Producer Perspective\"");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TimeMode\", \"enum\", \"\", \"\",6");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TimeProtocol\", \"enum\", \"\", \"\",2");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"SnapOnFrameMode\", \"enum\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TimeSpanStart\", \"KTime\", \"Time\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TimeSpanStop\", \"KTime\", \"Time\", \"\",153953860000");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"CustomFrameRate\", \"double\", \"Number\", \"\",-1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TimeMarker\", \"Compound\", \"\", \"\"");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"CurrentTimeMarker\", \"int\", \"Integer\", \"\",-1");

            tabIndent = "\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");

            tabIndent = "";
            FBXFileInfo.AppendLine($"{tabIndent}}}");
            FBXFileInfo.AppendLine("");
        }

        private void AddDocumentDescription()
        {
            string tabIndent = "";
            FBXFileInfo.AppendLine($"{tabIndent}; Documents description");
            FBXFileInfo.AppendLine($"{tabIndent};------------------------------------------------------------------");
            FBXFileInfo.AppendLine($"");
            FBXFileInfo.AppendLine($"{tabIndent}Documents: {{");

            tabIndent = "\t";
            FBXFileInfo.AppendLine($"{tabIndent}Count: 1");
            FBXFileInfo.AppendLine($"{tabIndent}Document: {rnd.Next(0, int.MaxValue)}, \"\", \"Scene\" {{");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}Properties70: {{");

            tabIndent = "\t\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}P: \"SourceObject\", \"object\", \"\", \"\"");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ActiveAnimStackName\", \"KString\", \"\", \"\", \"\"");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");
            FBXFileInfo.AppendLine($"{tabIndent}RootNode: 0");

            tabIndent = "\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");

            tabIndent = "";
            FBXFileInfo.AppendLine($"{tabIndent}}}");
            FBXFileInfo.AppendLine($"");
        }

        private void AddDocumentReferences()
        {
            string tabIndent = "";
            FBXFileInfo.AppendLine($"{tabIndent}; Document References");
            FBXFileInfo.AppendLine($"{tabIndent};------------------------------------------------------------------");
            FBXFileInfo.AppendLine($"");
            FBXFileInfo.AppendLine($"{tabIndent}References: {{");
            FBXFileInfo.AppendLine($"{tabIndent}}}");
            FBXFileInfo.AppendLine($"");
        }

        private void AddObjectDefinitions(int subModelCount)
        {
            string tabIndent = "";
            FBXFileInfo.AppendLine($"{tabIndent}; Object definitions");
            FBXFileInfo.AppendLine($"{tabIndent};------------------------------------------------------------------");
            FBXFileInfo.AppendLine($"");
            FBXFileInfo.AppendLine($"{tabIndent}Definitions: {{");

            tabIndent = "\t";
            FBXFileInfo.AppendLine($"{tabIndent}Version: 100");
            FBXFileInfo.AppendLine($"{tabIndent}Count: {subModelCount * 2 + 1}"); //This count is always 2 * the amount of models (1 model node, 1 Geometry node per model + GlobalSettings node)
            FBXFileInfo.AppendLine($"{tabIndent}ObjectType: \"GlobalSettings\" {{");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}Count: 1");
            FBXFileInfo.AppendLine($"{tabIndent}}}");

            tabIndent = "\t";
            FBXFileInfo.AppendLine($"{tabIndent}ObjectType: \"Model\" {{");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}Count: {subModelCount}"); // Count is equal to the amount of sub models (or bones) in the rig
            FBXFileInfo.AppendLine($"{tabIndent}PropertyTemplate: \"FbxNode\" {{");

            tabIndent = "\t\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}Properties70: {{");

            tabIndent = "\t\t\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}P: \"QuaternionInterpolate\", \"enum\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationOffset\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationPivot\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingOffset\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingPivot\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TranslationActive\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TranslationMin\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TranslationMax\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TranslationMinX\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TranslationMinY\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TranslationMinZ\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TranslationMaxX\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TranslationMaxY\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"TranslationMaxZ\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationOrder\", \"enum\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationSpaceForLimitOnly\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationStiffnessX\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationStiffnessY\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationStiffnessZ\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"AxisLen\", \"double\", \"Number\", \"\",10");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"PreRotation\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"PostRotation\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationActive\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationMin\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationMax\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationMinX\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationMinY\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationMinZ\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationMaxX\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationMaxY\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationMaxZ\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"InheritType\", \"enum\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingActive\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingMin\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingMax\", \"Vector3D\", \"Vector\", \"\",1,1,1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingMinX\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingMinY\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingMinZ\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingMaxX\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingMaxY\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingMaxZ\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"GeometricTranslation\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"GeometricRotation\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"GeometricScaling\", \"Vector3D\", \"Vector\", \"\",1,1,1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MinDampRangeX\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MinDampRangeY\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MinDampRangeZ\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MaxDampRangeX\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MaxDampRangeY\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MaxDampRangeZ\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MinDampStrengthX\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MinDampStrengthY\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MinDampStrengthZ\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MaxDampStrengthX\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MaxDampStrengthY\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"MaxDampStrengthZ\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"PreferedAngleX\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"PreferedAngleY\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"PreferedAngleZ\", \"double\", \"Number\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"LookAtProperty\", \"object\", \"\", \"\"");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"UpVectorProperty\", \"object\", \"\", \"\"");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Show\", \"bool\", \"\", \"\",1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"NegativePercentShapeSupport\", \"bool\", \"\", \"\",1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"DefaultAttributeIndex\", \"int\", \"Integer\", \"\",-1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Freeze\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"LODBox\", \"bool\", \"\", \"\",0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Lcl Translation\", \"Lcl Translation\", \"\", \"A\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Lcl Rotation\", \"Lcl Rotation\", \"\", \"A\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Lcl Scaling\", \"Lcl Scaling\", \"\", \"A\",1,1,1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Visibility\", \"Visibility\", \"\", \"A\",1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Visibility Inheritance\", \"Visibility Inheritance\", \"\", \"\",1");

            tabIndent = "\t\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");

            tabIndent = "\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");
            FBXFileInfo.AppendLine($"{tabIndent}ObjectType: \"Geometry\" {{");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}Count: {subModelCount}"); // Count is equal to the amount of sub models (bones) in the rig
            FBXFileInfo.AppendLine($"{tabIndent}PropertyTemplate: \"FbxMesh\" {{");

            tabIndent = "\t\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}Properties70: {{");

            tabIndent = "\t\t\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Color\", \"ColorRGB\", \"Color\", \"\",0.8,0.8,0.8");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"BBoxMin\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"BBoxMax\", \"Vector3D\", \"Vector\", \"\",0,0,0");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Primary Visibility\", \"bool\", \"\", \"\",1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Casts Shadows\", \"bool\", \"\", \"\",1");
            FBXFileInfo.AppendLine($"{tabIndent}P: \"Receive Shadows\", \"bool\", \"\", \"\",1");

            tabIndent = "\t\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");

            tabIndent = "\t\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");

            tabIndent = "\t";
            FBXFileInfo.AppendLine($"{tabIndent}}}");

            tabIndent = "";
            FBXFileInfo.AppendLine($"{tabIndent}}}");
            FBXFileInfo.AppendLine($"");
        }

        private void AddObjectProperties(ModelFile model)
        {
            string tabIndent = "";
            FBXFileInfo.AppendLine($"; Object properties");
            FBXFileInfo.AppendLine($";----------------------------------------------------");
            FBXFileInfo.AppendLine($"");
            FBXFileInfo.AppendLine($"{tabIndent}Objects: {{");

            //Add the Geometry nodes
            Geometries = new GeometryInfo[1]; //model.Header.BoneCount
            for (int i = 0; i < Geometries.Length; i++)
            {
                //i++;
                var geometry = new GeometryInfo(rnd.Next(0, int.MaxValue), rnd.Next(0, int.MaxValue), i, model.VertexData[i]);

                tabIndent = "\t";
                FBXFileInfo.AppendLine($"{tabIndent}Geometry: {geometry.GeometryID}, \"Geometry::\", \"Mesh\" {{");

                tabIndent = "\t\t";
                FBXFileInfo.AppendLine($"{tabIndent}Properties70: {{");

                #region Color
                tabIndent = "\t\t\t";
                //FBX uses normalized colour values so we need to divide the colour by 255
                FBXFileInfo.AppendLine($"{tabIndent}P: \"Color\", \"ColorRGB\", \"Color\" \"\", {(double)geometry.MeshColour.R / 255},{(double)geometry.MeshColour.G / 255},{(double)geometry.MeshColour.B / 255},");

                tabIndent = "\t\t";
                FBXFileInfo.AppendLine($"{tabIndent}}}");
                #endregion

                #region Vertices
                FBXFileInfo.AppendLine($"{tabIndent}Vertices: *{geometry.Vertices.Length * 3} {{");//Length is equal to the amount of vertices in the geometry * 3, as every vertex has an XYZ component

                tabIndent = "\t\t\t";

                StringBuilder verticesString = new StringBuilder();
                for (int j = 0; j < geometry.Vertices.Length; j++)
                {
                    verticesString.Append("\n");

                    if (j != 0)
                        verticesString.Append(",");

                    verticesString.Append($"{geometry.Vertices[j].X},");
                    verticesString.Append($"{geometry.Vertices[j].Y},");
                    verticesString.Append($"{geometry.Vertices[j].Z}");
                }

                FBXFileInfo.AppendLine($"{tabIndent}a: {verticesString}");
                tabIndent = "\t\t";
                FBXFileInfo.AppendLine($"{tabIndent}}}");
                #endregion

                #region PolygonVertexIndex
                //The polygon vertex index can be either quad or tri, the last vertex is negated to indicate that it is the ending vertex.
                //FBXFileInfo.AppendLine($"{tabIndent}PolygonVertexIndex: *{(model.PrimitiveData[i].QuadCount * 4) + (model.PrimitiveData[i].TrisCount * 3)} {{"); //Length is equal to the total amount of points found in all the quads and tris added
                //FBXFileInfo.AppendLine($"{tabIndent}PolygonVertexIndex: *{model.PrimitiveData[i].TrisCount * 3} {{"); //Length is equal to the total amount of points found in all the quads and tris added
                FBXFileInfo.AppendLine($"{tabIndent}PolygonVertexIndex: *{model.PrimitiveData[i].QuadCount * 4} {{"); //Length is equal to the total amount of points found in all the quads and tris added
                //FBXFileInfo.AppendLine($"{tabIndent}PolygonVertexIndex: *{4} {{"); //Length is equal to the total amount of points found in all the quads and tris added

                tabIndent = "\t\t\t";

                StringBuilder polygonVertexIndexString = new StringBuilder();


                for (int j = 0; j < model.PrimitiveData[i].QuadCount; j++)
                {
                    polygonVertexIndexString.Append("\n");

                    PrimitiveQuad quad = model.PrimitiveData[i].QuadsData[j];

                    if (j != 0)
                        polygonVertexIndexString.Append(",");

                    List<byte> sortedVertexIds = quad.VertexIds.ToList();
                    sortedVertexIds.Sort();
                    byte[] orderedVertices = sortedVertexIds.ToArray();

                    var vertX = model.VertexData[i].Vertecis[sortedVertexIds[0]].X;


                    if (vertX == 0)
                        //If the first Vertex is 0 we follow the order (0,1,3,2)
                        orderedVertices = new byte[] { sortedVertexIds[0], sortedVertexIds[1], sortedVertexIds[3], sortedVertexIds[2] };
                    else if (vertX < 0)
                        //If the X of the first vertex is smaller than 0 we need to go the anti clockwise route
                        orderedVertices = new byte[] { sortedVertexIds[0], sortedVertexIds[2], sortedVertexIds[3], sortedVertexIds[1] };
                    //orderedVertices = quad.GetVerticesAntiClockWise();
                    else if (vertX > 0)
                        //If the vertex.x is higher than 0 we need to just sort it
                        // RIGHT
                        orderedVertices = new byte[] { sortedVertexIds[0], sortedVertexIds[1], sortedVertexIds[2], sortedVertexIds[3] };


                    for (int k = 0; k < orderedVertices.Length; k++)
                    {
                        if (k != 0)
                            polygonVertexIndexString.Append(",");

                        if (k == orderedVertices.Length - 1)
                            //We need to negate the last vertex ID to tell the FBX interpreter that it is the last vertex for the quad or tri
                            polygonVertexIndexString.Append($"{~orderedVertices[k]}");
                        else
                            polygonVertexIndexString.Append($"{orderedVertices[k]}");
                    }
                }

                //polygonVertexIndexString.Append(",");

                ////Then we add all the tris
                //for (int j = 0; j < model.PrimitiveData[i].TrisCount; j++)
                //{
                //    PrimitiveTri tri = model.PrimitiveData[i].TrisData[j];


                //    for (int k = tri.VertexIds.Length - 1; k >= 0 ; k--)
                //    {
                //        if (k == 0)
                //            //We need to negate the last vertex ID to tell the FBX interpreter that it is the last vertex for the quad or tri
                //            polygonVertexIndexString.Append($"{~tri.VertexIds[k]}");
                //        else
                //            polygonVertexIndexString.Append($"{tri.VertexIds[k]},");
                //    }

                //        polygonVertexIndexString.Append(",");
                //}

                FBXFileInfo.AppendLine($"{tabIndent}a: {polygonVertexIndexString}");

                #region edges
                //To come, optional for FBX format
                #endregion

                #region LayerElementNormal
                //To come, optional for FBX format
                #endregion

                #region LayerElementUV
                //To come, optional for FBX format
                #endregion

                #region Layer
                //To come, optional for FBX format
                #endregion

                tabIndent = "\t\t";
                FBXFileInfo.AppendLine($"{tabIndent}}}");
                #endregion

                tabIndent = "\t";
                FBXFileInfo.AppendLine($"{tabIndent}}}");

                //i--;
                Geometries[i] = geometry;
            }

            //Add the Model data
            for (int i = 0; i < Geometries.Length; i++)
            {
                FBXFileInfo.AppendLine($"{tabIndent}Model: {Geometries[i].ModelID}, \"Model::{Geometries[i].Name}\", \"Mesh\" {{");
                tabIndent = "\t\t";
                FBXFileInfo.AppendLine($"{tabIndent}Version: 232");
                FBXFileInfo.AppendLine($"{tabIndent}Properties70: {{");

                tabIndent = "\t\t\t";
                FBXFileInfo.AppendLine($"{tabIndent}P: \"PreRotation\", \"Vector3D\", \"Vector\", \"\",0,0,0");
                FBXFileInfo.AppendLine($"{tabIndent}P: \"RotationActive\", \"bool\", \"\", \"\",1");
                FBXFileInfo.AppendLine($"{tabIndent}P: \"InheritType\", \"enum\", \"\", \"\",1");
                FBXFileInfo.AppendLine($"{tabIndent}P: \"ScalingMax\", \"Vector3D\", \"Vector\", \"\",0,0,0");
                FBXFileInfo.AppendLine($"{tabIndent}P: \"DefaultAttributeIndex\", \"int\", \"Integer\", \"\",0");
                FBXFileInfo.AppendLine($"{tabIndent}P: \"Lcl Translation\", \"Lcl Translation\", \"\", \"A\",0.0334704555571079,0,-0.0116531997919083"); //No idea what these 3 values mean
                FBXFileInfo.AppendLine($"{tabIndent}P: \"MaxHandle\", \"int\", \"Integer\", \"UH\",1"); //This might need to be incremented

                tabIndent = "\t\t";
                FBXFileInfo.AppendLine($"{tabIndent}}}");

                FBXFileInfo.AppendLine($"{tabIndent}Shading: T");
                FBXFileInfo.AppendLine($"{tabIndent}Culling: \"CullingOff\"");

                tabIndent = "\t";
                FBXFileInfo.AppendLine($"{tabIndent}}}");
            }

            tabIndent = "";
            FBXFileInfo.AppendLine($"{tabIndent}}}");
            FBXFileInfo.AppendLine($"{tabIndent}");
        }



        private void AddObjectConnections(ModelFile model)
        {
            string tabIndent = "";
            FBXFileInfo.AppendLine($"{tabIndent}; Object connections");
            FBXFileInfo.AppendLine($"{tabIndent};------------------------------------------------------------------");
            FBXFileInfo.AppendLine($"{tabIndent}");
            FBXFileInfo.AppendLine($"{tabIndent}Connections: {{");
            FBXFileInfo.AppendLine($"{tabIndent}");

            tabIndent = "\t";
            //First every Model::<name> gets linked to the Model::RootNode using the Model ID
            for (int i = 0; i < Geometries.Length; i++)
            {
                FBXFileInfo.AppendLine($"{tabIndent};Model::{Geometries[i].Name}, Model::RootNode");
                FBXFileInfo.AppendLine($"{tabIndent}C: \"OO\",{Geometries[i].ModelID},0");
                FBXFileInfo.AppendLine($"{tabIndent}");
            }

            //Then every Geometry gets linked to the Model using the ModelID and then the Geometry ID
            for (int i = 0; i < Geometries.Length; i++)
            {
                FBXFileInfo.AppendLine($"{tabIndent};Geometry::, Model::{Geometries[i].Name}");
                FBXFileInfo.AppendLine($"{tabIndent}C: \"OO\",{Geometries[i].GeometryID},{Geometries[i].ModelID}");
                FBXFileInfo.AppendLine($"{tabIndent}");
            }

            tabIndent = "";
            FBXFileInfo.AppendLine($"{tabIndent}}}");
        }
    }

    struct GeometryInfo
    {
        public int GeometryID { get; private set; }
        public int ModelID { get; private set; }
        public string Name { get; private set; }
        public Color MeshColour { get; private set; }
        public FbxVertex[] Vertices { get; private set; }

        public GeometryInfo(int geometryID, int modelID, int boneID, VertexData vertexData)
        {
            GeometryID = geometryID;
            ModelID = modelID;
            Name = $"Bone{boneID:D3}";
            MeshColour = Color.Gray;
            Vertices = new FbxVertex[vertexData.Vertecis.Length];
            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new FbxVertex(vertexData.Vertecis[i].X, vertexData.Vertecis[i].Y, vertexData.Vertecis[i].Z);
            //Vertices[i] = new FbxVertex(vertexData.Vertecis[i].X, vertexData.Vertecis[i].Y, -vertexData.Vertecis[i].Z);
            //Vertices[i] = new FbxVertex(vertexData.Vertecis[i].X, vertexData.Vertecis[i].Y, vertexData.Vertecis[i].Z);

        }
    }

    struct FbxVertex
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public FbxVertex(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
