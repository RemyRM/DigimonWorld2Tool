namespace DigimonWorld2MapVisualizer.Files
{
    public class DungFile
    {
        public readonly string Filename;
        public readonly string DomainName;
        public readonly byte DomainIDDecimal;
        public readonly byte Data4000IDDecimal;

        public DungFile(string filename, string domainName, byte domainIDDecimal, byte data4000IDDecimal)
        {
            this.Filename = filename + ".BIN";
            this.DomainName = domainName;
            this.DomainIDDecimal = domainIDDecimal;
            this.Data4000IDDecimal = data4000IDDecimal;
        }

        public override string ToString()
        {
            return string.Format("Domain Name: {0, -14} ID: {1,-2} Filename: {2, -8}", DomainName, DomainIDDecimal, Filename);
        }
    }
}
