/*
 제목: 디버그 (V0.1)
 날짜: 2021년 09월 23일
 작성: 김유승 (inspire156@gmail.com)
 사용법:
	using YSDebugger;
 
    string mode = "console" or "disk";
    string savePath = "D:"; use default.
    YSDebug ysDebug = new YSDebug(mode, savePath);
    
 기능: 
    현재 시간 출력 > 시:분:초
    Print(value)    > console 모드시 콘솔창에 value 값을 출력.
                    > disk 모드시 txt 파일로 value 값 저장.
 */

using System;
using System.IO;
using System.Text;

namespace YSDebugger
{
    public class YSDebug
    {
        private string displayMode;
        private string saveResultPath;
        private string fileName;

        private const string console = "console";
        private const string disk = "disk";
        private const string folderName = "YSDebug Result";

        public YSDebug(string mode = "console", string savePath = "D:", string saveFileName = "Result try")
        {
            if (mode == console)
            {
                SetDisplayMode(mode);
            }
            else if (mode == disk)
            {
                fileName = saveFileName;
                SetDisplayMode(mode);
                diskSetting(savePath);
            }
        }

        private void diskSetting(string savePath)
        {
            string folderPath = GetFolderPath(savePath);
            CreateFolder(folderPath);
            SetSaveResultPath(folderPath);
        }

        private void SetDisplayMode(string mode)
        {
            displayMode = mode;
        }

        private string GetFolderPath(string savePath)
        {
            string folderPath = savePath + "\\" + folderName;
            return folderPath;
        }

        private void CreateFolder(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            if (di.Exists == false)
            {
                di.Create();
            }
        }

        private void SetSaveResultPath(string folderPath)
        {
            int tryCount = 0;
            while (true)
            {
                saveResultPath = folderPath + "\\" + fileName + " = " + tryCount.ToString() + ".txt";
                FileInfo fileInfo = new FileInfo(saveResultPath);
                if (fileInfo.Exists)
                {
                    tryCount++;
                }
                else
                {
                    return;
                }
            }
        }

        private void GetSaveResultPath(string folderPath, int tryCount)
        {
            string resultFile = folderPath + "\\" + fileName;
            string resultTryCount = " = " + tryCount.ToString();
            string resultFileFormat = ".txt";
            string resultPath = resultFile + resultTryCount + resultFileFormat;
        }

        // Print on console.
        public void Print(bool value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(byte value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(short value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(int value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(long value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(ushort value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(uint value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(ulong value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(float value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(double value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(decimal value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(char value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(string value)
        {
            PrintProcess(value.ToString());
        }

        public void Print(Object value)
        {
            PrintProcess(value.ToString());
        }


        private void PrintProcess(string stringValue)
        {
            stringValue = GetNowTime() + stringValue;
            if (displayMode == console)
            {
                PrintConsole(stringValue);
            }
            else if (displayMode == disk)
            {
                PrintDisk(stringValue);
            }
        }

        private string GetNowTime()
        {
            string nowTime = DateTime.Now.ToString("HH:mm:ss");
            nowTime = "[" + nowTime + "] ";
            return nowTime;
        }

        private void PrintConsole(string stringValue)
        {
            Console.WriteLine(stringValue);
        }

        private void PrintDisk(string stringValue)
        {
            stringValue = stringValue + "\n";
            File.AppendAllText(saveResultPath, stringValue, Encoding.Default);
        }

        public void Break()
        {
            Environment.Exit(1);
        }
    }
}
