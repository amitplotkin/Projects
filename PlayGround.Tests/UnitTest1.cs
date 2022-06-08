using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace PlayGround.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test_CreateBibaryFile()
        {
            FileStream fs = new FileStream("test.bin", FileMode.Create);
            IList<string> myList = new List<string>();
            myList.Add("amit");
            myList.Add("Plotkin");
            myList.Add("39");
            for (int i = 0; i < 1000000; i++)
            {
                myList.Add(Guid.NewGuid().ToString());
            }
            
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, myList);
            fs.Close();
        }

        [Fact]
        public void Test_ReadBinaryFile()
        {

            // Declare the hashtable reference.
            IList<string> myList = new List<string>();

            // Open the file containing the data that you want to deserialize.
            FileStream fs = new FileStream("test.bin", FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and
                // assign the reference to the local variable.
                myList = (IList<string>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }

            // display the key/value pairs.
            foreach (string de in myList)
            {
                Console.WriteLine($"{de}");
            }
        }
    }
}
