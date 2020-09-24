using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Dependencies
{
    public interface IFileLauncher
    {
        void Open(string filePath);
        void Open(byte[] byteArray, string name);

        string SaveFile(byte[] byteArray, string name);

        string RetrivePathForPDF(string name);
    }
}
