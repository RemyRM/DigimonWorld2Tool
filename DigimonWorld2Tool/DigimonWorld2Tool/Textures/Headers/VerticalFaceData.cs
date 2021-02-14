using System.IO;
using DigimonWorld2MapTool.Utility;

namespace DigimonWorld2Tool.Textures.Headers
{
    class VerticalFaceData
    {
        public readonly byte[] VertexIDs = new byte[4]; // This is the ID of the vertex inside of the Vertex array 
        public readonly byte[] VertexColour = new byte[4];// Not 100% on this, it seems like some kind of per vertex Colour/Shading?
        public readonly Vector2[] TexturePlaneOffset = new Vector2[4]; // 4 points that create a plane over the texture sheet to apply on the face
        public readonly byte Unknown1;
        public readonly byte Unknown2;
        public readonly byte Unknown3;
        public readonly byte Unknown4;

        public VerticalFaceData(ref BinaryReader reader)
        {
            for (int i = 0; i < VertexIDs.Length; i++)
                VertexIDs[i] = reader.ReadByte();

            for (int i = 0; i < VertexColour.Length; i++)
                VertexColour[i] = reader.ReadByte();

            for (int i = 0; i < TexturePlaneOffset.Length; i++)
                TexturePlaneOffset[i] = new Vector2(reader.ReadByte(), reader.ReadByte());

            Unknown1 = reader.ReadByte();
            Unknown2 = reader.ReadByte();
            Unknown3 = reader.ReadByte();
            Unknown4 = reader.ReadByte();
        }
    }
}
