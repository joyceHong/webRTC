using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class writeEvent
    {
          // string strPath = @"C:\";
        string strPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        static object lockMe = new object();

        public void writeToFile(string strWrittingWord)
        {
            try
            {

                if (!Directory.Exists(strPath))
                    System.IO.Directory.CreateDirectory(strPath);

                FileStream fs = new FileStream(strPath + "\\CooperServiceErrorLog.txt",
                                    FileMode.OpenOrCreate, FileAccess.Write);

                fileStreamToWrite(fs, strWrittingWord,true);

                //lock (lockMe)
                //{

                //    StreamWriter m_streamWriter = new StreamWriter(fs);
                //    m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                //    m_streamWriter.WriteLine(
                //       "[" +DateTime.Now.ToShortDateString() + " " +
                //       DateTime.Now.ToShortTimeString() + "]"+ strWrittingWord );
                //    m_streamWriter.Flush();
                //    m_streamWriter.Close();
                //}

                //if (!File.Exists(strPath))
                //{
                //    //建立檔案
                //    FileStream fs = File.Create(strPath);
                //}

                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(strPath, true))
                //{
                //    file.WriteLine(DateTime.Now.ToShortTimeString() + ":" + strWrittingWord);
                //}
            }
            catch (Exception ex)
            {
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(strPath, true))
                //{
                //    file.WriteLine(ex.Message);
                //}
            }
        }

        public void writeToFile( string FileName,string FilePath,string WrittingWord)
        {
            try
            {
                if (!Directory.Exists(FilePath))
                    System.IO.Directory.CreateDirectory(FilePath);

                FileStream fs = new FileStream(FilePath + "\\" + FileName,
                                    FileMode.OpenOrCreate, FileAccess.Write);

                fileStreamToWrite(fs, WrittingWord,false);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
          
        }


        public void fileStreamToWrite(FileStream fs, string strWrittingWord,bool bolWriteTime)
        {
            try
            {
                lock (lockMe)
                {

                    StreamWriter m_streamWriter = new StreamWriter(fs,Encoding.UTF8);
                    m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                    string strBeforeWrite = "";

                    if (bolWriteTime == true)
                    {
                        strBeforeWrite += "[" + DateTime.Now.ToShortDateString() + " " +
                           DateTime.Now.ToShortTimeString() + "]" + strWrittingWord;
                    }
                    else
                    {
                        strBeforeWrite += strWrittingWord;
                    }

                    m_streamWriter.WriteLine(strBeforeWrite);               

                    m_streamWriter.Flush();
                    m_streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    
}
