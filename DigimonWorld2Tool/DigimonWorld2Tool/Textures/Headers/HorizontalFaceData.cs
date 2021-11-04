using DigimonWorld2Tool.Utility;
using System.IO;

namespace DigimonWorld2Tool.Textures.Headers
{
    class HorizontalFaceData
    {
        public readonly byte[] VertexIDs = new byte[3]; // This is the ID of the vertex inside of the Vertex array 
        public readonly byte[] VertexColour = new byte[3];// Not 100% on this, it seems like some kind of per vertex Colour/Shading?
        public readonly Vector2[] TexturePlaneOffset = new Vector2[3]; // 4 points that create a plane over the texture sheet to apply on the face
        public readonly byte Unknown1;
        public readonly byte Unknown2;
        public readonly byte Unknown3;
        public readonly byte Unknown4;

        public HorizontalFaceData(ref BinaryReader reader)
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
