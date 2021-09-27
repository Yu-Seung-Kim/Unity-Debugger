# Unity-Debugger

 제목: 디버그 (V0.2.1)
 날짜: 2021년 09월 27일
 작성: 김유승 (inspire156@gmail.com)
 
 사용법:
 
	using YSDebugger;
 
    string mode = "console" or "disk";
    string savePath = "D:"; use default.
    
    YSDebug ysDebug = new YSDebug(mode, savePath, saveFileName, rowCount);
    or
    YSDebug ysDebug = new YSDebug(mode, rowCount);
    
    ysDebug.Print("hello world");
    ysDebug.Break();
    
 기능: 
 
    현재 시간 출력 > 시:분:초
    Print(value)    > console 모드시 콘솔창에 value 값을 출력.
                    > disk 모드시 txt 파일로 value 값 저장.
    Break()     > break point 설정.
