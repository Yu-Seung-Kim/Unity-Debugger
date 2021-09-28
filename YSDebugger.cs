/*
 제목: 디버그 (V0.2.2)
 날짜: 2021년 09월 28일
 작성: 김유승 (inspire156@gmail.com)
 사용법:
	using YSDebugger;
 
    string mode = "console" or "disk";
    string savePath = "D:"; 
    string saveFileName = "Result try";
    int rowCount = 2; // row에 출력할 개수. 
    
    YSDebug ysDebug = new YSDebug(mode, savePath, saveFileName, rowCount);
    or
    YSDebug ysDebug = new YSDebug(mode, rowCount);
    
 기능: 
    현재 시간 출력 > 시:분:초
    Print(value)    > console 모드시 콘솔창에 value 값을 출력.
                    > disk 모드시 txt 파일로 value 값 저장.
    Break()     > break point 설정.
 */

using System;
using System.IO;
using System.Text;

namespace YSDebugger
{
    class YSDebug : PrintDebug
    {
        public YSDebug(string mode = "console", string savePath = "D:", string saveFileName = "Result try", int rowCount = 2)
        {
            DisplayModeProcess(mode, savePath, saveFileName, rowCount);
        }

        public YSDebug(string mode = "console", int rowCount = 2)
        {
            string savePath = "D:";
            string saveFileName = "Result try";
            DisplayModeProcess(mode, savePath, saveFileName, rowCount);
        }

        // Print value.
        public void Print(bool boolValue) { PrintProcess(boolValue.ToString()); }

        public void Print(byte byteValue) { PrintProcess(byteValue.ToString()); }

        public void Print(short shortValue) { PrintProcess(shortValue.ToString()); }

        public void Print(int intValue) { PrintProcess(intValue.ToString()); }

        public void Print(long longValue) { PrintProcess(longValue.ToString()); }

        public void Print(ushort ushortValue) { PrintProcess(ushortValue.ToString()); }

        public void Print(uint uintValue) { PrintProcess(uintValue.ToString()); }

        public void Print(ulong ulongValue) { PrintProcess(ulongValue.ToString()); }

        public void Print(float floatValue) { PrintProcess(floatValue.ToString()); }

        public void Print(double doubleValue) { PrintProcess(doubleValue.ToString()); }

        public void Print(decimal decimalValue) { PrintProcess(decimalValue.ToString()); }

        public void Print(char charValue) { PrintProcess(charValue.ToString()); }

        public void Print(string stringValue) {
            if(stringValue == null)
            {
                stringValue = "NULL";
            }
            PrintProcess(stringValue.ToString()); 
        }

        public void Print(Object objectValue) { PrintProcess(objectValue.ToString()); }

        // Set break point.
        public void Break() { isBreakPoint = true; }
    }

    class PrintDebug
    {
        protected bool isBreakPoint = false;

        private string displayMode;
        private string saveResultPath;
        private string fileName;
        private const string folderName = "YSDebug Result";

        private const string console = "console";
        private const string disk = "disk";
        private const string consoleAndDisk = "cd";

        private bool isLineChange = true;
        private int tapCount = 0;
        private int maxStringLen = 24;
        private int limitTapCount;

        protected void DisplayModeProcess(string mode, string savePath, string saveFileName, int rowCount)
        {
            SetLimitTapCount(rowCount);
            switch (mode)
            {
                case console:
                    SetDisplayMode(console);
                    break;
                case disk:
                    fileName = saveFileName;
                    SetDisplayMode(disk);
                    DiskSetting(savePath);
                    break;
                case consoleAndDisk:
                    fileName = saveFileName;
                    SetDisplayMode(consoleAndDisk);
                    DiskSetting(savePath);
                    break;
            }
        }

        private void SetLimitTapCount(int rowCount) { this.limitTapCount = rowCount - 1; }

    
        private void SetDisplayMode(string mode) { this.displayMode = mode; }

        private void DiskSetting(string savePath)
        {
            string folderPath = GetFolderPath(savePath);
            CreateFolder(folderPath);
            SetSaveResultPath(folderPath);
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
                this.saveResultPath = GetSaveResultPath(folderPath, tryCount);
                FileInfo fileInfo = new FileInfo(this.saveResultPath);
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

        private string GetSaveResultPath(string folderPath, int tryCount)
        {
            string resultFile = folderPath + "\\" + fileName;
            string resultTryCount = " = " + tryCount.ToString();
            string resultFileFormat = ".txt";
            string resultPath = resultFile + resultTryCount + resultFileFormat;
            return resultPath;
        }

        protected void PrintProcess(string valueString)
        {
            if (isBreakPoint) return;

            valueString = GetTimeString(valueString);
            valueString = GetAligedString(valueString);
            switch (displayMode)
            {
                case console:
                    PrintConsole(valueString);
                    break;
                case disk:
                    PrintDisk(valueString);
                    break;
                case consoleAndDisk:
                    PrintConsole(valueString);
                    PrintDisk(valueString);
                    break;
            }
        }

        private string GetTimeString(string valueString)
        {
            string attachedTimeString = GetNowTime() + valueString;
            return attachedTimeString;
        }

        private string GetNowTime()
        {
            string nowTime = DateTime.Now.ToString("HH:mm:ss");
            nowTime = "[" + nowTime + "] ";
            return nowTime;
        }

        private string GetAligedString(string valueString)
        {
            ConfigLineChangeable(valueString);
            string aligedString;
            if (this.isLineChange)
            {
                aligedString = valueString + "\n";
            }
            else
            {
                aligedString = valueString + "\t";
            }
            return aligedString;
        }

        private void ConfigLineChangeable(string valueString)
        {
            if (IsLineChangeable(valueString))
            {
                SetLineChangeOn();
                InitTapCount();
            }
            else
            {
                if (IsLimitTapCount())
                {
                    SetLineChangeOn();
                    InitTapCount();
                }
                else
                {
                    SetLineChangeOff();
                    IncreaseTapCount();
                }
            }
        }

        private bool IsLineChangeable(string valueString)
        {
            int stringLen = valueString.Length;
            return (stringLen > this.maxStringLen);
        }

        private void SetLineChangeOn() { this.isLineChange = true; }

        private bool IsLimitTapCount() { return (this.tapCount >= this.limitTapCount); }

        private void InitTapCount() { this.tapCount = 0; }

        private void SetLineChangeOff() { this.isLineChange = false; }

        private void IncreaseTapCount() { this.tapCount++; }

        private void PrintConsole(string valueString) { Console.Write(valueString); }

        private void PrintDisk(string valueString) { File.AppendAllText(this.saveResultPath, valueString, Encoding.Default); }
    }
}
