using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace PocketZone.Space
{
    public static class SaveSystem
    {
        const string saveFileName = "save.dat";
        const string nonSerializedFileName = "save.txt";
        public static void SaveProgress(ProgressData progressData)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + saveFileName;
            FileStream stream = new FileStream(path, FileMode.Create);

            ProgressData _progressData = new ProgressData(progressData);
            formatter.Serialize(stream, _progressData);
            stream.Close();

            string _NS_path = Application.persistentDataPath + "/" + nonSerializedFileName;
            //FileStream _NS_stream = new FileStream(_NS_path, FileMode.Create);
            StreamWriter streamWriter =  File.CreateText(_NS_path);
            streamWriter.Write(_progressData);
            streamWriter.Close();
            //_NS_stream.Close();
        }

        public static ProgressData LoadProgressData()
        {
            string path = Application.persistentDataPath + "/" + saveFileName;
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                ProgressData progress = formatter.Deserialize(stream) as ProgressData;
                stream.Close();
                return progress;
            }
            else
            {
                return null;
            }
        }

        public static void ClearSaveData()
        {
            string path = Application.persistentDataPath + "/" + saveFileName;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}