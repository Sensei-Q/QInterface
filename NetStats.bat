:: NetStats.bat v1.0 (c) 2022 Sensei (aka 'Q')
:: A batch script for Windows that waits a specified number of seconds and records network interfaces.
::
@echo off
SET QROOT=[fill-me]
SET LOG=[file-me]

:: Delay in seconds.
SET DELAY=1

:: Set to 0 to have an infinite loop.
:: Set to >0 to have an finite loop.
SET COUNT=0

ECHO Recording to the %LOG%..
IF %COUNT%==0 ECHO Infinite loop. Use Ctrl-C to break it.
:loop
   %QROOT%QInterface >>%LOG%
   IF %COUNT%==0 GOTO next
   SET /A COUNT-=1
   IF %COUNT%==0 GOTO end
:next
   "%QROOT%QTimeout" %DELAY%
GOTO loop
:end
