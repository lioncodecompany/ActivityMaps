using System.IO;
using System.Reflection;

namespace ActivityMaps.AzureStorage
{
    public static class Convert
    {
        public static byte[] ToByteArray(string resource)
        {
            
			using (FileStream fs = new FileStream(resource, FileMode.Open, FileAccess.Read))
			{
				// Create a byte array of file stream length
				byte[] bytes = System.IO.File.ReadAllBytes(resource);
				//Read block of bytes from stream into the byte array
				fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
				//Close the File Stream
				fs.Close();
				return bytes; //return the byte data
			}

	
        }
    }
}
