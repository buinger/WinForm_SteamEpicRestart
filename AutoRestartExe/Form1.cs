using Microsoft.Win32;
using System.Diagnostics;

namespace AutoRestartExe
{
    public partial class Form1 : Form
    {
        string[] lastDates;
        string steamPath = "";
        string epicPath = "";
        public Form1()
        {
            InitializeComponent();
            string targetData = LoadDataFromFile();
            //Debug.WriteLine("----------------------!!!!!!!!!");
            if (targetData != "")
            {
                lastDates = targetData.Split('*');
                string[] datas = lastDates;


                runTime_Input.Text = datas[1];
                waitTime_Input.Text = datas[2];

                if (datas[5] == "False")
                {
                    proName_Input.Text = datas[0];
                    gameIdToogle.Checked = false;
                    gameId_Input.Text = datas[3];
                }
                else
                {
                    proName_Input.Text = datas[6];
                    gameIdToogle.Checked = true;
                    gameId_Input.Text = datas[4];
                }
            }


        }


        void StartGame(string id)
        {
            if (gameIdToogle.Checked == false)
            {
                // 构造 Steam 游戏启动参数
                string steamParams = "-applaunch " + id;
                // 启动 Steam 并启动游戏
                Process.Start(steamPath, steamParams);

            }
            else
            {
                // 设置要启动的游戏的应用程序ID或URL scheme
                string gameAppIdOrURL = "com.epicgames.launcher://apps/" + id + "?action=launch&silent=true";

                // 使用Process启动Epic Games客户端，并传递游戏应用程序ID或URL scheme作为参数
                ProcessStartInfo startInfo = new ProcessStartInfo(epicPath, gameAppIdOrURL);
                Process.Start(startInfo);

            }
        }


        Task jianKong;
        bool jianKongSwitch = false;


        async Task DoJianKong(int rSecond, int wSecond)
        {
            infoBox.Text = "开始监控";
            proName_Input.ReadOnly = true;
            gameId_Input.ReadOnly = true;
            runTime_Input.ReadOnly = true;
            waitTime_Input.ReadOnly = true;
            gameIdToogle.Enabled = false;

            jianKongSwitch = true;
            startButton.Text = "正在监控(点击停止)";
            startButton.ForeColor = Color.Green;

            int fullPass = 0, runPass = 0, waitPass = 0;
            while (jianKongSwitch)
            {
                await Task.Delay(1000);
                fullPass += 1;

                //infoBox.Text = "运行时长:" + fullPass + "秒\n";
                //infoBox.Text = "";
                if (CheckIsRun())
                {
                    infoBox.Text = "游戏运行中\n";
                    runPass += 1;


                    infoBox.Text += "运行时长:" + GetTimeStr(runPass) + "\n";


                    infoBox.Text += "还剩" + GetTimeStr(rSecond - runPass) + "关闭游戏";
                    if (runPass >= rSecond)
                    {
                        runPass = 0;
                        waitPass = 0;
                        KillPro();
                    }
                }
                else
                {
                    //if (runPass != 0)
                    //{
                    //    infoBox.Text += "检测到游戏奔溃，正在启动。。。";
                    //    StartGame(gameId_Input.Text);
                    //    runPass = 0;
                    //    waitPass = 0;
                    //    await Task.Delay(3000);
                    //}
                    //else
                    //{
                    infoBox.Text = "游戏已关闭\n";
                    waitPass += 1;


                    infoBox.Text += "休息时长:" + GetTimeStr(waitPass) + "\n";


                    infoBox.Text += "还剩" + GetTimeStr(wSecond - waitPass) + "开启游戏";
                    if (waitPass >= wSecond)
                    {
                        runPass = 0;
                        waitPass = 0;
                        KillPro();
                        StartGame(gameId_Input.Text);
                    }
                    //}

                }



            }
            infoBox.Text = "";
        }


        string GetTimeStr(int fullSec)
        {
            int min = fullSec / 60;
            int sec = fullSec % 60;
            if (min == 0)
            {
                return sec + "秒";
            }
            else
            {
                return min + "分" + sec + "秒";
            }

        }

        bool CheckIsRun()
        {
            Process[] processes = Process.GetProcessesByName(proName_Input.Text);
            bool isRun = processes.Length >= 1;

            return isRun;
        }

        void KillPro()
        {
            Process[] processes = Process.GetProcessesByName(proName_Input.Text);
            foreach (var item in processes)
            {
                item.Kill();
            }
        }


        void EndJianKong()
        {
            jianKongSwitch = false;
            startButton.Text = "开始监控";
            startButton.ForeColor = Color.Black;
            proName_Input.ReadOnly = false;
            gameId_Input.ReadOnly = false;
            runTime_Input.ReadOnly = false;
            waitTime_Input.ReadOnly = false;
            gameIdToogle.Enabled = true;
        }






        void CheckAndDoJianKong()
        {

            RemeberGameIdAndName();

            if (jianKong != null)
            {
                if (!jianKong.IsCompleted)
                {
                    MessageBox.Show("监控进程还在运行，请稍后重试");
                    return;
                }
            }
            int rTime, wTime;
            if (!int.TryParse(runTime_Input.Text, out rTime))
            {
                MessageBox.Show("运行时间填写错误，请检查");
                return;
            }

            if (!int.TryParse(waitTime_Input.Text, out wTime))
            {
                MessageBox.Show("休息时间填写错误，请检查");
                return;
            }



            if (proName_Input.Text != "")
            {

                if (gameIdToogle.Checked == false)
                {
                    if (!SteamGameCheck())
                    {
                        return;
                    }
                }
                else
                {
                    if (!EpicGameCheck())
                    {
                        return;
                    }
                }



                if (CheckIsRun())
                {
                    jianKong = DoJianKong(rTime * 60, wTime * 60);
                }
                else
                {
                    MessageBox.Show(proName_Input.Text + ".exe：未运行，请运行后再点击开始");
                    return;
                }

            }
            else
            {
                MessageBox.Show("进程名不能为空");
                return;
            }


            void RemeberGameIdAndName()
            {
                if (gameIdToogle.Checked == true)
                {
                    lastDates[6] = proName_Input.Text;
                    lastDates[4] = gameId_Input.Text;
                }
                else
                {
                    lastDates[0] = proName_Input.Text;
                    lastDates[3] = gameId_Input.Text;
                }
            }

            bool SteamGameCheck()
            {

                if (steamPath == "")
                {
                    Process[] processes = Process.GetProcessesByName("Steam");
                    if (processes.Length == 0)
                    {
                        MessageBox.Show("未检测到steam程序");
                        return false;
                    }
                    steamPath = processes[0]?.MainModule?.FileName;
                }

                Process[] allGamePro = Process.GetProcessesByName(proName_Input.Text);
                if (allGamePro.Length != 0)
                {
                    Process steamGameProcess = allGamePro[0];
                    // 假设 Steam 游戏进程的启动路径包含 "steamapps\common"
                    string fullName = "";
                    if (steamGameProcess.MainModule != null)
                    {
                        fullName = steamGameProcess.MainModule.FileName != null ? steamGameProcess.MainModule.FileName : "";
                    }
                    if (fullName == "")
                    {
                        MessageBox.Show("未找到游戏的路径，请检查游戏名是否正确");
                        return false;
                    }


                    //if (!fullName.Contains("steamapps\\common"))
                    //{

                    //    MessageBox.Show(proName_Input.Text + ".exe：不是steam游戏，无法监控");
                    //    return false;
                    //}
                }
                else
                {
                    MessageBox.Show(proName_Input.Text + ".exe：未运行，请运行后在点击开始");
                    return false;
                }

                return true;
            }



            bool EpicGameCheck()
            {
                // 如果 Epic Games 客户端路径为空，则尝试查找并设置
                if (epicPath == "")
                {
                    Process[] processes = Process.GetProcessesByName("EpicGamesLauncher");
                    if (processes.Length == 0)
                    {
                        MessageBox.Show("未检测到 Epic Games 客户端程序");
                        return false;
                    }
                    epicPath = processes[0]?.MainModule?.FileName;
                }

                // 获取指定游戏进程
                Process[] allGamePro = Process.GetProcessesByName(proName_Input.Text);

                if (allGamePro.Length != 0)
                {
                    Process epicGameProcess = allGamePro[0];
                    // 假设 Epic Games 游戏进程的启动路径包含 "Epic Games"
                    string fullName = "";
                    if (epicGameProcess.MainModule != null)
                    {
                        fullName = epicGameProcess.MainModule.FileName != null ? epicGameProcess.MainModule.FileName : "";
                    }
                    if (fullName == "")
                    {
                        MessageBox.Show("未找到游戏的路径，请检查游戏名是否正确");
                        return false;
                    }

                    //if (!fullName.Contains("Epic Games"))
                    //{
                    //    Debug.WriteLine(fullName);
                    //    MessageBox.Show(proName_Input.Text + ".exe：不是 Epic Games 游戏，无法监控");
                    //    return false;
                    //}
                }
                else
                {
                    MessageBox.Show(proName_Input.Text + ".exe：未运行，请运行后再点击开始");
                    return false;
                }

                return true;
            }

        }


        private void StartButton_Click(object sender, EventArgs e)
        {
            if (startButton.Text != "正在监控(点击停止)")
            {
                CheckAndDoJianKong();
            }
            else
            {
                EndJianKong();
            }

        }



        //"默认数据.txt"
        private string GetDataFilePath()
        {
            // 获取当前运行程序的路径，并拼接数据文件名
            return Path.Combine(Application.StartupPath, "配置数据.txt");
        }

        private void SaveDataToFile(string data)
        {
            try
            {
                // 获取数据文件路径
                string filePath = GetDataFilePath();

                // 如果文件不存在，则创建文件
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close(); // 创建文件并立即关闭，以允许其他进程访问
                }

                // 将数据写入文件
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(data);
                }
                //MessageBox.Show("数据保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存数据时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string LoadDataFromFile()
        {
            try
            {
                // 获取数据文件路径
                string filePath = GetDataFilePath();

                // 如果文件不存在，则返回空字符串
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close(); // 创建文件并立即关闭，以允许其他进程访问
                    SaveDataToFile("*****False*");
                }

                // 从文件中读取数据
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (gameIdToogle.Checked == false)
            {
                SaveDataToFile(lastDates[0] + "*" + runTime_Input.Text + "*" + waitTime_Input.Text + "*" + gameId_Input.Text + "*" + lastDates[4] + "*" + false + "*" + lastDates[6]);
            }
            else
            {
                SaveDataToFile(lastDates[0] + "*" + runTime_Input.Text + "*" + waitTime_Input.Text + "*" + lastDates[3] + "*" + gameId_Input.Text + "*" + true + "*" + lastDates[6]);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (gameIdToogle.Checked == true)
            {
                gameIdToogle.Text = "Epic游戏ID";
                gameId_Input.Text = lastDates[4];
                proName_Input.Text = lastDates[6];
            }
            else
            {
                gameIdToogle.Text = "Steam游戏ID";
                gameId_Input.Text = lastDates[3];
                proName_Input.Text = lastDates[0];
            }
        }
    }
}