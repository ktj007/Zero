﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// 샘플 클라이언트 프로그램
/// 
/// -NetClient라이브러리 프로젝트를 참조하여 서버에 접속하는 방법을 보여줍니다
/// 
/// -동일한 방법으로 unity 3d에서도 NetClient를 사용하여 서버에 접속할 수 있습니다
/// 
/// 
/// </summary>
namespace Client
{
    class Program
    {
        static NetClient.NetManager net;


        static void Main(string[] args)
        {
            net = new NetClient.NetManager();


            net.InfoHandler = (txt) =>
            {
                Console.WriteLine(txt);
            };

            net.JoinHandler = () =>
            {
                Console.WriteLine("서버입장");
            };

            net.LeaveHandler = () =>
            {
                Console.WriteLine("서버퇴장");
            };

            net.JoinFailedHandler = () =>
            {
                Console.WriteLine("서버에 접속실패");
            };

            net.InternalHandler = (txt) =>
            {
                Console.WriteLine("내부오류발생: {0}", txt);
            };


            // 비동기 방식으로 명령어를 입력받습니다
            var ret = ReadLineAsync();


            // 프로그램 반복 실행을 위해...
            bool run_program = true;


            Console.WriteLine("프로그램 시작:  /c 접속   /q 해제   /exit 종료");


            while (run_program)
            {
                if (ret.IsCompleted)
                {
                    switch (ret.Result)
                    {
                        case "/c":
                            Console.WriteLine("접속 시도");
                            net.Connect("127.0.0.1", 29999);
                            break;

                        case "/q":
                            Console.WriteLine("접속 해제 시도");
                            net.Disconnect();
                            break;

                        case "/exit":
                            // 콘솔 프로그램을 종료합니다
                            run_program = false;
                            break;

                        default:
                            break;
                    }

                    if (run_program)
                        ret = ReadLineAsync();
                }

                net.Update();
                System.Threading.Thread.Sleep(1);
            }

            net.Destroy();
            System.Threading.Thread.Sleep(1000 * 2);
        }

        static public async Task<string> ReadLineAsync()
        {
            var line = await Task.Run(() => Console.ReadLine());
            return line;
        }
    }
}
