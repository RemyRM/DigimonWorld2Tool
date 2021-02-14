using System.IO;
using DigimonWorld2Tool.Utility;

namespace DigimonWorld2Tool.Textures.Headers
{
    class ModelBodyPartHeader
    {
        private enum FaceType
        {
            Vertical,
            Horizontal
        }

        public readonly VerticalFaceData[] VerticalFacesData;
        public int VerticalFacesNullByte { get; private set; }
        public int VerticalFaceCount { get; private set; }
        public short VerticalVertexAllignmentByte { get; private set; }
        public byte[] VerticalVertexPaddingBytes {get; private set; }

        public int HorizontalFacesNullByte { get; private set; }
        public int HorizontalFaceCount { get; private set; }
        public short HorizontalVertexAllignmentByte { get; private set; }
        public byte[] HorizontalVertexPaddingBytes { get; private set; }

        public readonly HorizontalFaceData[] HorizontalFacesData;

        public readonly Vector3[] VerticalFaceVertexData;
        public readonly Vector3[] HorizontalFaceVertexData;


        public ModelBodyPartHeader(ref BinaryReader reader)
        {
            VerticalFacesData = GetVerticalFaceData(ref reader);
            HorizontalFacesData = GetHorizontalFaceData(ref reader);
            VerticalFaceVertexData = GetVertexData(ref reader, FaceType.Vertical);
            HorizontalFaceVertexData = GetVertexData(ref reader, FaceType.Horizontal);
        }

        /// <summary>
        /// Get the data from the header used for rendering the vertical (perpendicular to the floor) faces
        /// </summary>
        /// <returns>Array containing the header data for the vertical faces</returns>
        private VerticalFaceData[] GetVerticalFaceData(ref BinaryReader reader)
        {
            VerticalFacesNullByte = reader.ReadInt32();
            if (VerticalFacesNullByte != 0x00)
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"First byte in vertical face data was not null, found {VerticalFacesNullByte}");
                return null;
            }

            VerticalFaceCount = reader.ReadInt32();
            if(VerticalFaceCount < 0 || VerticalFaceCount > 255)
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"Vertical face count was invalid, found {VerticalFaceCount}");
                return null;
            }

            var faceData = new VerticalFaceData[VerticalFaceCount];
            for (int i = 0; i < VerticalFaceCount; i++)
            {
                faceData[i] = new VerticalFaceData(ref reader);
            }

            return faceData;
        }

        /// <summary>
        /// Get the data from the header used for rendering the horizontal (parallel to the floor) faces
        /// </summary>
        /// <returns>Array containing the header data for the horizontal faces</returns>
        private HorizontalFaceData[] GetHorizontalFaceData(ref BinaryReader reader)
        {
            int HorizontalFacesNullByte = reader.ReadInt32();
            if (HorizontalFacesNullByte != 0x00)
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"First byte in vertical face data was not null, found {HorizontalFacesNullByte}");
                return null;
            }

            HorizontalFaceCount = reader.ReadInt32();
            if (HorizontalFaceCount < 0 || HorizontalFaceCount > 255)
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"Vertical face count was invalid, found {HorizontalFaceCount}");
                return null;
            }

            var faceData = new HorizontalFaceData[HorizontalFaceCount];
            for (int i = 0; i < HorizontalFaceCount; i++)
            {
                faceData[i] = new HorizontalFaceData(ref reader);
            }

            return faceData;
        }

        private Vector3[] GetVertexData(ref BinaryReader reader, FaceType faceType)
        {
            int vertexCount = reader.ReadInt32();
            short allignmentByte = reader.ReadInt16();
            if(allignmentByte != 0x00)
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"Allignment byte in GetVertexData was not null, found {allignmentByte}");
                return null;
            }

            Vector3[] vertexData = new Vector3[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                // It looks like every second byte of vertex data is the sign, since it is always either 0x00 or 0xFF
                short xValue = reader.ReadInt16();
                short yValue = reader.ReadInt16();
                short zValue = reader.ReadInt16();

                vertexData[i] = new Vector3(xValue, yValue, zValue);
            }

            long allignmentByteCount = reader.BaseStream.Position % 4;

            if (allignmentByteCount > 0)
            {
                switch (faceType)
                {
                    case FaceType.Vertical:
                        VerticalVertexAllignmentByte = allignmentByte;
                        VerticalVertexPaddingBytes = new byte[allignmentByteCount];
                        break;
                    case FaceType.Horizontal:
                        HorizontalVertexAllignmentByte = allignmentByte;
                        HorizontalVertexPaddingBytes = new byte[allignmentByteCount];
                        break;
                }

                for (int i = 0; i < allignmentByteCount; i++)
                {
                    byte wordAllignmentByte = reader.ReadByte();
                    if(wordAllignmentByte != 0x00)
                        DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"Word allignment byte was not 0x00, found {wordAllignmentByte}");

                    switch (faceType)
                    {
                        case FaceType.Vertical:
                            VerticalVertexPaddingBytes[i] = wordAllignmentByte;
                            break;
                        case FaceType.Horizontal:
                            HorizontalVertexPaddingBytes[i] = wordAllignmentByte;
                            break;
                    }
                }
            }

            return vertexData;
        }
    }
}
